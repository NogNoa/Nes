class CPU2403: IBus
{
    readonly CPU6502 cpu;
    readonly IBus bus;
    readonly Controller[] controller; //len 2
    
    public CPU2403(IBus bus, Controller[] controller) 
    {
        this.bus = bus;
        this.controller = controller;
        this.cpu = new CPU6502(this);
    }

    public bool M2(ushort address, byte value, ReadWrite readWrite)
    {   Access(address, value, readWrite); 
        return !cpu.φ1;
    }
    public bool M2(ushort address, ReadWrite readWrite)
        =>   cpu.φ2(address, readWrite);

    public void Interrupt_request() {cpu.Interrupt_request();}
    public void Nonmaskable_interrupt() {cpu.Nonmaskable_interrupt();}
    public void Reset() {cpu.Reset();}
    public void Set_overflow() {cpu.Set_overflow();}

    public byte Access(ushort address, byte value, ReadWrite readWrite) => 
        this.bus.Access(address, value, readWrite);

    private byte Get_controller(byte index, byte outsig)
    {
        outsig &= (1 << 3) - 1;
        return this.controller[index].get_buttons(outsig);
    }
    // object audio()
}