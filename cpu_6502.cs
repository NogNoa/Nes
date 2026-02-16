using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;

internal class CPU6502
{
    private byte A;
    private byte X;
    private byte Y;
    private byte SP;
    private byte P = 0x20; //6th bit passive high
    private ushort PC;
    private bool φ0 = false;
    private byte _data = 0xFF;
    public bool φ1 {get => !φ0;}
    private readonly ICpuAccessible bus;

    private readonly execution_unit exu;

    public CPU6502(ICpuAccessible bus)
    {
        this.bus = bus;
        this.exu = new execution_unit(this);
    }

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
        // get => (P & 0b1000) != 0;
        set { P = Bit_set(value, P, 0b1000); }
    }

    private bool Overflow
    {
        get => (P & 0b100_0000) != 0;
        set {if (!value) {P = Bit_set(false, P, 0b100_0000);} }
    }

    private bool Negative
    {
        get => (P & 0b1000_0000) != 0;
        set { P = Bit_set(value, P, 0b1000_0000); }
    }

    public bool CompareRegister(string reg, byte val)
    {   reg = reg.ToUpper();  
        return (reg switch {
            "A" => A,
            "X" => X,
            "Y" => Y,
            "P" => P,
            "SP" => SP,
            "PCL" => (byte) PC,
            "PCH" => PC >> 8,
            _ => null 
        }
        ) == val;
    }

    public Dictionary<string, byte> registersExpose()
        => new Dictionary<string, byte> { 
            { "A", A }, 
            { "X", X }, 
            { "Y", Y }, 
            { "P", P }, 
            { "SP", SP }, 
            { "PCL", (byte) PC }, 
            { "PCH", (byte) (PC >> 8) },
        };

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
        {   Interrupt(Interrupt_vector.IRQ); }
    }

    public void Nonmaskable_interrupt()
    {
        Interrupt(Interrupt_vector.NMI);
    }

    public void Reset()
    {
        this.Interrupt_disable = true;
        SP -= 3;
        PC = Read16((ushort)Interrupt_vector.RST);
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
        this.Read((ushort)(0x100 | ++SP));
        
    private ushort Pull16()
    {
        byte temp = Pull();
        return (ushort) (Pull() << 8 | temp);
    }

    private byte Read(ushort address)
        => this._data = this.bus.Cpu_Access(address, this._data, ReadWrite.READ);

    private ushort Read16(ushort address)
        =>(ushort) ((Read((ushort)(address + 1)) << 8) | Read(address));
    private ushort BuggyRead16(ushort address)
        =>(ushort) ((Read((ushort)((address & 0xff00) | (byte)(address + 1))) << 8) | Read(address));

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

    public int Execute()
    {
        return exu.Execute();
    }

    private class execution_unit(CPU6502 parent)
    {
        private int Cycles;
        private readonly CPU6502 parent = parent;
        private ushort address;

        // static readonly Instruct clc = new() { Dest = 'C', Source = '0', Operation = "move", };
        // static readonly Instruct cmp = new() { Dest = 'A', Source = 'M', Operation = "cmp", };


        private byte Fetch_prg() => parent.Read(parent.PC++);
        public int Execute()
        {   byte opcode = Fetch_prg();
            Instruct operation = Instruct.Decode_instrcution(opcode);
            Cycles = operation.Cycles;
            if (operation.addressing != Instruct.Addressing.Impl)
            {   address = GetAddress(operation.addressing, operation.Length); }
#pragma warning disable CS8629 // Nullable value type may be null.
            byte operand = this.Read(operation.Source) ?? Read(operation.Dest).Value;
#pragma warning restore CS8629
            operand = this.Operate(operation, operand);
            if (operation.PostOp)
                Post_op_update(operand);
            if (operation.Operation != "cmp")
            { this.Write(operation.Dest, operand); }
            Debug.Print(operation.Format());
            return Cycles;
        }

        private byte? Read(char? src)
        {   src = (src != null) ? char.ToUpper((char)src) : null;
            switch (src)
            {   case 'M':
                    return parent.Read(address);
                case 'O':
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
                case 'N':
                    return Fetch_prg();
                case 'P':
                    return parent.P;
                case 'E':
                    return (byte)parent.PC;
                case '1':
                    return 1;
                case '0':
                    return 0;
                case null:
                    return null;
                default:
                    throw new Exception();
            }
        }
        private void Write(char dst, byte data)
        {   dst = char.ToUpper(dst);
            switch (dst)
            {   case 'M':
                    parent.Write(address, data);
                    break;
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
                case 'N':
                    parent.Negative = data == 1;
                    break;
                case 'I':
                    parent.Interrupt_disable = data == 1;
                    break;
                case 'E':
                    parent.PC &= 0xff00;
                    parent.PC |= data;
                    break;
                case 'P':
                    parent.P = data;
                    break;
                default:
                    throw new Exception();
            }
        }

        private ushort GetAddress(Instruct.Addressing addressing, int arity)
        {   int back = 0;
            int bas;
            switch (addressing)
            {   case Instruct.Addressing.Dir:
                    {   for (int i = 0; i < arity - 1; ++i)
                        {   back |= Fetch_prg() << (8 * i); }
                        break;
                    }
                case Instruct.Addressing.IndX:
                    bas = GetAddress(Instruct.Addressing.Dir, arity);
                    back = bas + parent.X;
                    if ((bas & 0xff00) != (back & 0xff00)) {Cycles += 1;}
                    break;
                case Instruct.Addressing.IndY:
                    bas = GetAddress(Instruct.Addressing.Dir, arity);
                    back = bas  + parent.Y;
                    if ((bas & 0xff00) != (back & 0xff00)) {Cycles += 1;}
                    break;
                case Instruct.Addressing.DRef:
                    back = GetAddress(Instruct.Addressing.Dir, arity - 1);
                    back = parent.BuggyRead16((ushort)back);
                    break;
                case Instruct.Addressing.XDRef:
                    back = GetAddress(Instruct.Addressing.IndX, arity -1);
                    back = parent.BuggyRead16((ushort)back);
                    break;
                case Instruct.Addressing.DRefY:
                    bas = GetAddress(Instruct.Addressing.DRef, arity);
                    back = bas + parent.Y;
                    if ((bas & 0xff00) != (back & 0xff00)) {Cycles += 1;}
                    break;
                default:
                    throw new Exception();
            }
            return  (arity < 3) ? (byte) back : (ushort)back;
        }

        private byte Operate(Instruct operation, byte operand)
        {   int temp;
            switch (operation.Operation)
            {   case "load":
                case "store":
                case "transfer":
                case "move":
                case "mov":
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
                    temp = parent.P;
                    temp &= ~0xC2;
                    temp |= operand & 0xC0;
                    temp |= ((operand & parent.A) == 0) ? 2 : 0;
                    return (byte) temp;
                case "jump":
                case "jmp":
                    parent.PC &= 0x00ff;
                    parent.PC |= (ushort) (address & 0xff00);
                    return (byte) address;
#pragma warning disable CS8629 // Nullable value type may be null.
                case "branch if":
                    return Branch(true, operation.Source.Value, operand);
                case "branch nif":
                    return Branch(false, operation.Source.Value, operand);
#pragma warning restore CS8629
                case "break":
                case "brk":
                    parent.Interrupt(Interrupt_vector.NMI, isSoft: true);
                    return (byte)--address;
                case "call":
                    parent.Push(parent.PC);
                    parent.PC = address;
                    return (byte)address;
                case "ret int":
                    parent.P = parent.Pull();
                    goto case "ret sub";
                case "ret sub":
                    address = parent.Pull16();
                    goto case "jmp";
                default:
                    throw new Exception();
            }
        }

        private byte Branch(bool ifSet, char Source, byte operand)
        {   bool source = (Source == 'N') ? parent.Negative :
                        (Source == 'V') ? parent.Overflow :
                        (Source == 'C') ? parent.Carry :
                        (Source == 'Z') ? parent.Zero :
                        throw new Exception();
            sbyte relop;
            unchecked
            {   relop = (sbyte)operand;
            }
            if (source == ifSet)
            {   ++Cycles;
                int temp =  (byte) parent.PC + relop;
                if (temp < 0)
                {   ++Cycles;
                    parent.PC -= 0x100;
                }
                else if (temp > 0x100)
                {   ++Cycles;
                    parent.PC += 0x100;
                }
                return (byte)temp;
            }
            else
            { return (byte)parent.PC; }
        }

        private void Post_op_update(byte result)
        {   parent.Zero = result == 0;
            parent.Negative = (result & 0x80) != 0;
        }

        private void Overflow_update(int result)
        {   parent.Overflow = -0x80 > result || result > 0x7F; }

    }
}

/* todo:next probably want to make clock 2 an event for both decoder and cartridge */
