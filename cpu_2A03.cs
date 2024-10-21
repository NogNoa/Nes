class CPU2403: ICpuAccessible
{
    readonly CPU6502 cpu;
    readonly ICpuBus bus;

    
    public CPU2403(ICpuBus bus) 
    {
        this.bus = bus;
        this.cpu = new CPU6502(this);
    }

    private bool M2(ushort address, byte value, ReadWrite readWrite)
    {   Cpu_Access(address, value, readWrite); 
        return !cpu.φ1;
    }
    private bool M2(ushort address, ReadWrite readWrite)
        =>   cpu.φ2(address, readWrite);

    public void Interrupt_request() {cpu.Interrupt_request();}
    public void Nonmaskable_interrupt() {cpu.Nonmaskable_interrupt();}
    public void Reset() {cpu.Reset();}
    public void Set_overflow() {cpu.Set_overflow();}

    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite) => 
        this.bus.Cpu_Access(address, value, readWrite);

    private byte Get_controller(byte index, byte outsig)
    {
        outsig &= 1<<3 - 1;
        return this.bus.Get_controller(index, outsig);
    }
    // object audio()
}