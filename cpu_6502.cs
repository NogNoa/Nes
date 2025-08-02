using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography;

internal class CPU6502(ICpuAccessible bus)
{
    private byte A;
    private byte X;
    private byte Y;
    private byte SP;
    private byte P;
    private ushort PC;
    private bool φ0;
    private byte _data = 0xFF;
    public bool φ1 {get => !φ0;}
    private readonly ICpuAccessible bus = bus;

    private static byte Bit_set(bool value, byte the_byte, byte power_o_2) => 
        (byte)(value ? the_byte | power_o_2 : the_byte & ~power_o_2);

    private bool Carry
    {
        get => (P & 1) != 0;
        set { P = Bit_set(value, P, 1); }
    }

    private bool Zero
    {
        get => (P & 0b10) != 0;
        set { P = Bit_set(value, P, 0b10); }
    }

    private bool Interrupt_disable
    {
        get => (P & 0b100) != 0;
        set { P = Bit_set(value, P, 0b100); }
    }

    private bool Mode_decimal
    {
        get => (P & 0b1000) != 0;
        set { P = Bit_set(value, P, 0b1000); }
    }

    private bool Overflow
    {
        get => (P & 0b100000) != 0;
        set {if (!value) {P = Bit_set(false, P, 0b100000);} }
    }

    private bool Negative
    {
        get => (P & 0b1000000) != 0;
        set { P = Bit_set(value, P, 0b1000000); }
    }

    private enum Interrupt_vector { NMI = 0xFFFA, RST = 0xFFFC, IRQ = 0xFFFE };

    private void Interrupt(Interrupt_vector vector, bool isSoft)
    {
        Push(PC);
        Push((byte)(P |
                    (isSoft ? 0x30 : 0x20)));
        this.Interrupt_disable = true;
        PC = (ushort) ((Read((ushort) (vector + 1)) << 8) | Read((ushort)vector));
        --PC;
    }

    private void Interrupt(Interrupt_vector vector) => Interrupt(vector, false);

    public void Interrupt_request()
    {
        if (this.Interrupt_disable)
        {
            Interrupt(Interrupt_vector.IRQ);
        }
    }

    public void Nonmaskable_interrupt()
    {
        Interrupt(Interrupt_vector.NMI);
    }

    public void Reset()
    {
        this.Interrupt_disable = true;
        PC = (ushort)Interrupt_vector.RST;
    }

    public void Set_overflow() { this.Overflow = true; }

    private void Push(byte value)
    {
        ushort stack_adr = (ushort)(0x100 | SP--);
        this.Write(stack_adr, value);
    }

    private void Push(ushort value)
    {
        Push((byte)(value >> 8));
        Push((byte)value);
    }
    
    private byte Pull() =>
        this.Read((ushort)(0x100 | SP++));
        
    private ushort Pull16()
    {
        byte temp = Pull();
        return (ushort) (Pull() << 8 | temp);
    }

    private byte Read(ushort address)
        => this._data = this.bus.Cpu_Access(address, this._data, ReadWrite.READ);

    private void Write(ushort address, byte value)
    {
        this.bus.Cpu_Access(address, this._data = value, ReadWrite.WRITE);
    }

    public bool φ2(ushort address, byte value, ReadWrite readWrite)
    {   this._data = this.bus.Cpu_Access(address, value, readWrite);
        return !φ1;
    }
    public bool φ2(ushort address, ReadWrite readWrite)
        =>  φ2(address, this._data, readWrite);
    
    Instruct decode_instrcution(byte inst)
    {
        Instruct back = new();
        bool AF = (inst & 1) != 0; // Accumulator function
        bool XF = (inst & 2) != 0; /* X-register function
                                      Y-register function is implied 
                                      by nither of the above */
        byte adrs_group = (byte)((inst >> 2) & 7);
        byte oper_group = (byte)(inst >> 5);
        if ((adrs_group & 1) == 1)
            {   back.addressing = (adrs_group >> 2 == 0) ? 
                    Instruct.Addressing.Dir : 
                    Instruct.Addressing.IndX;
                // 1, 3->Dir; 5, 7->Indexed-X;
                back.Length = ((adrs_group >> 1) & 1) +  2;
                //1, 5->2 zeropaged   ; 3, 7->3 absolute; 
            }
        else if (AF) 
            {   switch (adrs_group >> 1)
                {   case 0: back.addressing = Instruct.Addressing.XDRef; 
                            back.Length = 2; 
                            break;//0->X-indirect;
                                  //2->implied
                    case 2: back.addressing = Instruct.Addressing.DRefY; 
                            back.Length = 2; 
                            break;//4->indirect-Y;
                    case 3: back.addressing = Instruct.Addressing.IndY;
                            back.Length = 3;
                            break;//6->indexed-Y;
                }
            }
        /* if !AF and !(adrs_group & 1) we don't really care 
           since everything is implied (arity 1) Immediate or relative (arity 2)
           immediate is also treated as implied, and relative is implicit from the
           opperation.
           */
        if (inst == 0x20) {back.Length = 3;} // JSR abs
        // if the X index is alrady an operand, we'll treat IndX as IndY
        if (!XF && !AF)
        { if (adrs_group == 6)
            {
                Instruct.Microcode flagop = new();
                switch (oper_group >> 1)
                {   case 0: flagop.Dest = 'C'; break;
                    case 1: flagop.Dest = 'I'; break;
                    case 2: flagop.Dest = 'V'; break;
                    case 3: flagop.Dest = 'D'; break;
                }
                back.Operand = (byte) (oper_group & 1); //even -> clear; odd -> set;
                back.steps.Add(flagop);
            }
          if (oper_group == 4)
          { Instruct.Microcode strop = new();
            
          }
        }
        return back;
    }
    private class execution_unit(CPU6502 parent)
    {
        private byte opcode;
        Instruct operation = new();
        private readonly CPU6502 parent = parent;

        private ushort address;
        private byte operand;

        static readonly Instruct clc = new() { Dest = 'C', Source = 'X', Operation = "clear", };
        static readonly Instruct cmp = new() { Dest = 'A', Source = 'M', Operation = "cmp", };


        private byte Fetch_prg() => parent.Read(parent.PC++);
        public void Execute()
        {
            opcode = Fetch_prg();
            operation = parent.decode_instrcution(opcode);
            if (operation.addressing != Instruct.Addressing.Impl)
            {
                address = GetAddress(operation.addressing, operation.Length);
            }
            operand = this.Read(operation.Source);
            operand = this.Operate(operation);
            if (operation.Operation != "bit" &&
                operation.Operation != "branch if" &&
                operation.Operation != "branch nif" &&
                operation.Operation != "break" &&
                operation.Operation != "clear"
            )
                Post_op_update(operand);
            if (operation.Operation != "cmp")
            { this.Write(operation.Dest, operand); }
        }

        private byte Read(char? src)
        {
            switch (src)
            {
                case 'M':
                    return parent.Read(address);
                case 'N':
                    return Fetch_prg();
                case 'A':
                    return parent.A;
                case 'X':
                    return parent.X;
                case 'Y':
                    return parent.Y;
                case 'S':
                    return parent.SP;
                case 'C':
                case 'I':
                case 'V':
                case 'D':
                case 'B':
                case 'Z':
                    return parent.P;
                case 'E':
                    return (byte)parent.PC;
                case null:
                    return Read(this.operation.Dest);
                default:
                    throw new Exception();
            }
        }
        private void Write(char dst, byte data)
        {
            switch (dst)
            {
                case 'M':
                    parent.Write(address, data);
                    break;
                case 'N':
                case 'O':
                    break;
                case 'A':
                    parent.A = data;
                    break;
                case 'X':
                    parent.X = data;
                    break;
                case 'Y':
                    parent.Y = data;
                    break;
                case 'S':
                    parent.SP = data;
                    break;
                case 'C':
                    parent.Carry = data == 1;
                    break;
                case 'V':
                    parent.Overflow = data == 1;
                    break;
                case 'D':
                    parent.Mode_decimal = data == 1;
                    break;
                case 'E':
                    parent.PC &= 0xff00;
                    parent.PC |= data;
                    break;
                default:
                    throw new Exception();
            }
        }

        private ushort GetAddress(Instruct.Addressing addressing, int arity)
        {
            int back = 0;
            switch (addressing)
            {
                case Instruct.Addressing.Dir:
                    {
                        for (int i = 0; i < arity - 1; ++i)
                        {
                            back |= Fetch_prg() << (8 * i);
                        }
                        break;
                    }
                case Instruct.Addressing.IndX:
                    back = GetAddress(Instruct.Addressing.Dir, arity) + parent.X;
                    break;
                case Instruct.Addressing.IndY:
                    back = GetAddress(Instruct.Addressing.Dir, arity) + parent.Y;
                    break;
                case Instruct.Addressing.XDRef:
                    back = GetAddress(Instruct.Addressing.IndX, arity);
                    return parent.Read((ushort)back);
                case Instruct.Addressing.DRefY:
                    back = GetAddress(Instruct.Addressing.Dir, arity);
                    back = parent.Read((ushort)back) + parent.Y;
                    break;
                default:
                    throw new Exception();
            }
            return (ushort)back;
        }

        private byte Operate(Instruct operation)
        {
            int temp;
            switch (operation.Operation)
            {
                case "clear":
                case "clr":
                    return 0;
                case "set":
                    return 1;
                case "load":
                case "store":
                case "transfer":
                case "move":
                    return operand;
                case "push":
                    parent.Push(operand);
                    return operand;
                case "pull":
                    return parent.Pull();
                case "inc":
                    return ++operand; //side-effective option saves me the cast
                case "dec":
                    return --operand;
                case "asl":
                    parent.Carry = (operand & 0x80) != 0;
                    return (byte)(operand << 1);
                case "lsr":
                    parent.Carry = (operand & 1) != 0;
                    return (byte)(operand >> 1);
                case "rol":
                    temp = parent.Carry ? 1 : 0;
                    parent.Carry = (operand & 0x80) != 0;
                    return (byte)((operand << 1) | temp);
                case "ror":
                    temp = parent.Carry ? 0x80 : 0;
                    parent.Carry = (operand & 1) != 0;
                    return (byte)((operand >> 1) | (temp));
                case "or":
                    return (byte)(operand | parent.A);
                case "and":
                    return (byte)(operand & parent.A);
                case "xor":
                case "eor":
                    return (byte)(operand ^ parent.A);
                case "add":
                case "adc":
                    temp = operand + parent.A + (parent.Carry ? 1 : 0);
                    parent.Carry = temp > 0x100;
                    Overflow_update(temp);
                    return (byte)temp;
                case "sbc":
                case "sub":
                    temp = parent.A - operand + (parent.Carry ? 0 : -1);
                    parent.Carry = temp >= 0;
                    Overflow_update(temp);
                    return (byte)temp;
                case "cmp":
                    temp = operation.Dest - operand;
                    parent.Carry = temp >= 0;
                    return (byte)temp;
                case "bit":
                    parent.Negative = (operand & 0x80) != 0;
                    parent.Overflow = (operand & 0x40) != 0;
                    parent.Zero = (operand & parent.A) == 0;
                    return operand;
                case "jmp":
                    parent.PC = --address;
                    return (byte)address;
                case "branch if":
                    return Branch(true);
                case "branch nif":
                    return Branch(false);
                case "break":
                    parent.Interrupt(Interrupt_vector.NMI, isSoft:true);
                    return (byte)--address;
                case "call":
                    parent.Push(parent.PC);
                    parent.PC = --address;
                    return (byte)address;
                case "ret int":
                    parent.P = parent.Pull();
                    parent.PC = (ushort)(parent.Pull() - 1);
                    return (byte)parent.PC;
                case "ret sub":
                    parent.PC = (ushort)(parent.Pull() - 1);
                    return (byte)parent.PC;
                default:
                    throw new Exception();
            }
        }

        private byte Branch(bool ifSet)
        {
            bool source = (operation.Source == 'N') ? parent.Negative :
                        (operation.Source == 'V') ? parent.Overflow :
                        (operation.Source == 'C') ? parent.Carry :
                        (operation.Source == 'Z') ? parent.Zero :
                        throw new Exception();
            sbyte relop;
            unchecked
            {
                relop = (sbyte)operand;
            }
            if (source == ifSet)
            {
                int temp = parent.PC + relop - 1;
                if (temp < 0)
                {
                    parent.PC -= 0x100;
                }
                else if (temp > 0x100)
                {
                    parent.PC += 0x100;
                }
                return (byte)temp;
            }
            else
                {return (byte)parent.PC; }
        }

        private void Post_op_update(byte result)
        {
            parent.Zero = result == 0;
            parent.Negative = (operand & 0x80) != 0;
        }

        private void Overflow_update(int result)
        {
            parent.Overflow = -0x80 > result || result > 0x7F;
        }
        
    }
}

/* todo:next probably want to make clock 2 an event for both decoder and cartridge */
