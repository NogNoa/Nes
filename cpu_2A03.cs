class CPU2403: IBus
{
    CPU6502 cpu;
    IBus bus;

    Controller[] controller; //len 2

    public CPU2403(IBus bus, Controller[] controller) 
    {
        this.bus = bus;
        this.controller = controller;
        this.cpu = new CPU6502(this);
    }
    public ushort address
    {   get => cpu.address;}
    public byte data
    {   get => cpu.data;}

    public ReadWrite read_write
    {   get => cpu.read_write;}

    public void interrupt_request() {cpu.interrupt_request();}
    public void nonmaskable_interrupt() {cpu.nonmaskable_interrupt();}
    public void reset() {cpu.reset();}
    public void set_overflow() {cpu.set_overflow();}

    public byte access(ushort address, byte value, ReadWrite readWrite) => 
        this.bus.access(address, value, readWrite);
    byte get_controller(byte index, byte outsig)
    {
        outsig &= (1 << 3) - 1;
        return this.controller[index].get_buttons(outsig);
    }
    // object audio()
}