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
                back.Arity = ((adrs_group >> 1) & 1) +  2;
                //1, 5->2 zeropaged   ; 3, 7->3 absolute; 
            }
        else if (AF) 
            {   switch (adrs_group >> 1)
                {   case 0: back.addressing = Instruct.Addressing.XDRef; 
                            back.Arity = 2; 
                            break;//0->X-indirect;
                                  //2->implied
                    case 2: back.addressing = Instruct.Addressing.DRefY; 
                            back.Arity = 2; 
                            break;//4->indirect-Y;
                    case 3: back.addressing = Instruct.Addressing.IndY;
                            back.Arity = 3;
                            break;//6->indexed-Y;
                }
            }
        /* if !AF and !(adrs_group & 1) we don't really care 
           since everything is implied (arity 1) Immediate or relative (arity 2)
           immediate is also treated as implied, and relative is implicit from the
           opperation.
           */
        if (inst == 0x20) {back.Arity = 3;} // JSR abs
        // if the X index is alrady an operand, we'll treat IndX as IndY
        if (!XF && !AF && adrs_group == 6)
        {
            Instruct.Microcode flagop = new();
            switch (oper_group >> 1)
            {   case 0: flagop.Dest = 'C'; break;
                case 1: flagop.Dest = 'I'; break;
                case 2: flagop.Dest = 'V'; break;
                case 3: flagop.Dest = 'D'; break;
            }
            flagop.Operand = (byte) (oper_group & 1); //even -> clear; odd -> set;
            back.steps.Add(flagop);
        }
        return back;
    }
    private class execution_unit 
    {
        private int T = 0;
        private byte opcode;
        Instruct operation;
        private CPU6502 parent;

        static readonly Instruct clc = new() {steps= [new Instruct.Microcode() {Dest='C', Operand=0}]}; 

        public execution_unit(CPU6502 parent)
        {
            this.parent = parent;
            this.operation = new();
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
