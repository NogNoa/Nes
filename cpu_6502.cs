class cpu_6502
{
    byte A;
    byte X;
    byte Y;
    byte SP;
    byte P;
    ushort PC;
    bool φ0;

    public ushort address;
    public byte data;
    public bool φ1 => !φ0;
    public bool φ2;

    public enum ReadWrite {WRITE, READ}

    public ReadWrite read_write;


    byte bit_set(bool value, byte the_byte, byte power_o_2)
    {
        return (byte)((value) ? the_byte | power_o_2 : the_byte & ~power_o_2);
    }

    bool carry
    {
        get => (this.P & 1) != 0;
        set { this.P = bit_set(value, this.P, 1); }
    }
    bool zero
    {
        get => (this.P & 0b10) != 0;
        set { this.P = bit_set(value, this.P, 0b10); }
    }
    bool interrupt_disable
    {
        get => (this.P & 0b100) != 0;
        set { this.P = bit_set(value, this.P, 0b100); }
    }
    bool mode_decimal
    {
        get => (this.P & 0b1000) != 0;
        set { this.P = bit_set(value, this.P, 0b1000); }
    }
    bool overflow
    {
        get => (this.P & 0b100000) != 0;
        set { this.P = bit_set(value, this.P, 0b100000); }
    }
    bool negative
    {
        get => (this.P & 0b1000000) != 0;
        set { this.P = bit_set(value, this.P, 0b1000000); }
    }


    private enum interrupt_vector { NMI = 0xFFFA, RST = 0xFFFC, IRQ = 0xFFFE };

    void interrupt(interrupt_vector vector)
    {
        this.push((byte)(this.PC >> 8));
        this.push((byte)(this.PC));
        this.push(this.P);
        this.interrupt_disable = true;
        this.PC = (ushort)vector;
    }

    public void interrupt_request()
    {
        if (this.interrupt_disable)
        {
            interrupt(interrupt_vector.IRQ);
        }
    }

    public void nonmaskable_interrupt()
    {
        interrupt(interrupt_vector.NMI);
    }

    public void reset()
    {
        this.interrupt_disable = true;
        this.PC = (ushort)interrupt_vector.RST;
    }

    public void set_overflow() { this.overflow = true; }

    void push(byte value)
    {
        this.write(SP--, value);
    }
    byte pull()
    {
        return this.read(SP++, this.data);
    }


    void write(ushort address, byte value) => 
        access(address, value, WRITE);
    void read(ushort address, byte value) => 
        access(address, value, READ);
    void access(ushort address, byte value, ReadWrite readWrite)
    {
        this.address = address;
        this.data = value;
        this.read_write = readWrite
        //this.bus.access(address, value, readWrite);
    }
}