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
    public ushort Address
    {   get => cpu.Address;}
    public byte Data
    {   get => cpu.Data;}

    public bool M2
    {   get => cpu.φ2;}

    public ReadWrite Read_write
    {   get => cpu.read_write;}

    public void Interrupt_request() {cpu.Interrupt_request();}
    public void Nonmaskable_interrupt() {cpu.Nonmaskable_interrupt();}
    public void Reset() {cpu.Reset();}
    public void Set_overflow() {cpu.Set_overflow();}

    public byte access(ushort address, byte value, ReadWrite readWrite) => 
        this.bus.access(address, value, readWrite);

    private byte Get_controller(byte index, byte outsig)
    {
        outsig &= (1 << 3) - 1;
        return this.controller[index].get_buttons(outsig);
    }
    // object audio()
}