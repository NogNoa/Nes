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
        PC = Read16((ushort)vector);
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

    private ushort Read16(ushort address)
        =>(ushort) ((Read((ushort)(address + 1)) << 8) | Read(address));

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
                back.Source = 'M';
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
                back.Source = 'M';
            }
        /* if !AF and !(adrs_group & 1) we don't really care 
           since everything is implied (arity 1) Immediate or relative (arity 2)
           which are both also treated as implied.
           Except JSR abs which we will treat personnaly now.
           */
        // if the X index is alrady an operand, we'll treat IndX as IndY
        if (!XF && !AF)
        {
            if (oper_group == 4)
            {
                back.Source = 'Y';
                back.Dest = 'M';
            }
            else if (5 == oper_group || oper_group == 6)
            {
                back.Dest = 'Y';
            }
            else if (oper_group == 7)
            {
                back.Dest = 'X';
            }
            if ((oper_group & 6) == 6) //6,7
            {
                back.Operation = "compare";
            }
            if (adrs_group == 6)
            {
                switch (oper_group >> 1)
                {
                    case 0: back.Dest = 'C'; break;
                    case 1: back.Dest = 'I'; break;
                    case 2: back.Dest = 'V'; break;
                    case 3: back.Dest = 'D'; break;
                }
                back.Source = (oper_group & 1).ToString()[0]; //even -> clear; odd -> set;
                if (inst == 0x98) { back.Source = 'Y'; back.Dest = 'A'; }
            }
            else if (adrs_group == 4)
            {
                back.Dest = 'E';
                switch (oper_group >> 1)
                {
                    case 0: back.Source = 'N'; break;
                    case 1: back.Source = 'V'; break;
                    case 2: back.Source = 'C'; break;
                    case 3: back.Source = 'Z'; break;
                }
                back.Operation = ((oper_group & 1) == 1) ? "branch if" : "branch nif";

            }
            if (oper_group < 4)
            {   if (adrs_group == 3)
                {
                    if ((oper_group & 2) == 2) //(0,2,3) (0,3,3)
                    {
                        back.Dest = 'E';
                        back.Operation = "jmp";
                        if (oper_group == 3)
                        { back.addressing = Instruct.Addressing.DRef; }
                    }
                }
                else if (adrs_group == 2)
                {
                    char argument = ((oper_group & 2) == 0) ? 'P' : 'A';
                    if ((oper_group & 1) == 0)
                    {
                        back.Operation = "push";
                        back.Source = argument;
                    }
                    else
                    {
                        back.Operation = "pull";
                        back.Dest = argument;
                    }
                }
                else if (adrs_group == 0)
                {
                    back.Dest = 'E';
                    switch (oper_group)
                    {
                        case 0:
                            back.Operation = "brk";
                            break;
                        case 1:
                            back.Operation = "call";
                            back.addressing = Instruct.Addressing.Dir;
                            back.Length = 3;
                            back.Source = 'M';
                            break;
                        case 2:
                            back.Operation = "ret int";
                            break;
                        case 3:
                            back.Operation = "ret sub";
                            break;
                    }
                }
                if (oper_group == 1 && (adrs_group & 5) == 1)
                {
                    back.Dest = 'P';
                    back.Operation = "bit";
                }
            }
        }
        else if (AF)
        {
            back.Dest = 'A';
            back.Source = (adrs_group != 2) ? 'M' : 'N';
            back.Operation = (oper_group == 0) ? "or" :
                             (oper_group == 1) ? "and" :
                             (oper_group == 2) ? "xor" :
                             (oper_group == 3) ? "adc" :
                             (oper_group == 4) ? "store" :
                             (oper_group == 5) ? "load" :
                             (oper_group == 6) ? "cmp" :
                             (oper_group == 7) ? "sbc" :
                             throw new InvalidDataException();
        }
        else if (XF)
        {
            ;
        }
        //post_op
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
            if (operation.PostOp)
                Post_op_update(operand);
            if (operation.Operation != "cmp")
            { this.Write(operation.Dest, operand); }
        }

        private byte Read(char? src)
        {
            src = (src != null) ? char.ToUpper((char)src) : null;
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
                case '1':
                    return 1;
                case '0':
                    return 0;
                case null:
                    return Read(this.operation.Dest);
                default:
                    throw new Exception();
            }
        }
        private void Write(char dst, byte data)
        {   dst = char.ToUpper(dst);
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
                    byte ind_reg = (operation.Dest == 'X' || operation.Source == 'X') ? parent.Y : parent.X;
                    back = GetAddress(Instruct.Addressing.Dir, arity) + ind_reg;
                    break;
                case Instruct.Addressing.IndY:
                    back = GetAddress(Instruct.Addressing.Dir, arity) + parent.Y;
                    break;
                case Instruct.Addressing.DRef:
                    back = GetAddress(Instruct.Addressing.Dir, arity);
                    back = parent.Read16((ushort)back);
                    break;
                case Instruct.Addressing.XDRef:
                    back = GetAddress(Instruct.Addressing.IndX, arity);
                    back = parent.Read16((ushort)back);
                    break;
                case Instruct.Addressing.DRefY:
                    back = GetAddress(Instruct.Addressing.DRef, arity) + parent.Y;
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
                case "compare":
                case "cmp":
                    temp = operation.Dest - operand;
                    parent.Carry = temp >= 0;
                    return (byte)temp;
                case "bit":
                    parent.Negative = (operand & 0x80) != 0;
                    parent.Overflow = (operand & 0x40) != 0;
                    parent.Zero = (operand & parent.A) == 0;
                    return operand;
                case "jump":
                case "jmp":
                    parent.PC = --address;
                    return (byte)address;
                case "branch if":
                    return Branch(true);
                case "branch nif":
                    return Branch(false);
                case "break":
                case "brk":
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
                ++operation.Cycles; 
                int temp = parent.PC + relop - 1;
                if (temp < 0)
                {
                    ++operation.Cycles;
                    parent.PC -= 0x100;
                }
                else if (temp > 0x100)
                {
                    ++operation.Cycles;
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
