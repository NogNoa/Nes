class cpu_2403
{
    CPU6502 cpu;
    public ushort address
    {   get => cpu.address;}
    public byte data
    {   get => cpu.data;}

    public CPU6502.ReadWrite read_write
    {   get => cpu.read_write;}

    public void interrupt_request => cpu.interrupt_request();
    public void nonmaskable_interrupt => cpu.nonmaskable_interrupt();
    public void reset => cpu.reset();
    public void set_overflow => cpu.set_overflow();

    access

}