class CPU2403: ICpuBus
{
    readonly CPU6502 cpu;
    readonly NesBoard bus;

    
    public CPU2403(NesBoard bus) 
    {
        this.bus = bus;
        this.cpu = new CPU6502(this);
    }

    public bool M2(ushort address, byte value, ReadWrite readWrite)
    {   Cpu_Access(address, value, readWrite); 
        return !cpu.φ1;
    }
    public bool M2(ushort address, ReadWrite readWrite)
        =>   cpu.φ2(address, readWrite);

    public void Interrupt_request() {cpu.Interrupt_request();}
    public void Nonmaskable_interrupt() {cpu.Nonmaskable_interrupt();}
    public void Reset() {cpu.Reset();}
    public void Set_overflow() {cpu.Set_overflow();}

    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite) => 
        this.bus.Cpu_Access(address, value, readWrite);

    private byte Get_controller(byte index, byte outsig)
    {
        return this.bus.Get_controller(index, outsig);
    }
    // object audio()
}