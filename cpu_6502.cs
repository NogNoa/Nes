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
    private byte _data;
    public bool φ1 {get => !φ0;}
    private readonly ICpuAccessible bus = bus;

    private class Instruct
    {
        public int Arity {get; init;}
        public int Cycles {get; init;}

        

        public enum Addressing {Implied};
        public delegate void Microcode();
        public Addressing addressing{get; init;}
        public Microcode[] steps = [];

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
        get => (P & 0b1000) != 0;
        set { P = Bit_set(value, P, 0b1000); }
    }

    private bool Overflow
    {
        get => (P & 0b100000) != 0;
        set { P = Bit_set(value, P, 0b100000); }
    }

    private bool Negative
    {
        get => (P & 0b1000000) != 0;
        set { P = Bit_set(value, P, 0b1000000); }
    }

    private enum Interrupt_vector { NMI = 0xFFFA, RST = 0xFFFC, IRQ = 0xFFFE };

    private void Interrupt(Interrupt_vector vector)
    {
        Push((byte)(PC >> 8));
        Push((byte) PC);
        Push(P);
        this.Interrupt_disable = true;
        PC = (ushort)vector;
    }

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

    private byte Pull() => 
        this.Read((ushort)(0x100 | SP++));

    private byte Read(ushort address)
        =>  this.bus.Cpu_Access(address, this._data, ReadWrite.READ);

    private void Write(ushort address, byte value)
    {
        this._data = value;
        this.bus.Cpu_Access(address, value, ReadWrite.WRITE);
    }

    public bool φ2(ushort address, byte value, ReadWrite readWrite)
    {   this._data = this.bus.Cpu_Access(address, value, readWrite);
        return !φ1;
    }
    public bool φ2(ushort address, ReadWrite readWrite)
        =>  φ2(address, this._data, readWrite);

    private void Post_op_update(byte result)
    {   this.Zero = (result == 0);
        this.Negative = (result < 0);
    }
    
    Instruct decode_instrcution(byte inst)
    {
        bool AF = (inst & 1) != 0;
        bool XF = (inst & 2) != 0;
        byte adrs_group = (byte)((inst >> 2) & 7);
        byte oper_group = (byte)(inst >> 5);
        if (!AF && !XF && (adrs_group & 4) == 0)
        {   

        }
        return new Instruct();
    }
    private class execution_unit 
    {
        private int T = 0;
        private byte opcode;
        Instruct operation;
        private CPU6502 parent;

        static readonly Instruct nop = 
            new() { Arity=1, Cycles=2, addressing=Instruct.Addressing.Implied};
        static readonly Instruct clc =
            new() { Arity=1, Cycles=2, addressing=Instruct.Addressing.Implied, steps=[()=>parent.Carry = 0]};

        public execution_unit(CPU6502 parent)
        {
            this.parent = parent;
            this.operation = nop;
        }
        public void step()
        {   if (T == 0)
            {   opcode = parent.Read(parent.PC++);
                operation = parent.decode_instrcution(opcode);
            }
            else
            {
                //execute step
            }
            if (++T >= operation.Cycles) {T=0;}
        }
    }
}

/* todo:next probably want to make clock 2 an event for both decoder and cartridge */
