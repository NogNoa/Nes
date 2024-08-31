class cpu_2403
{
    CPU6502 cpu;
    public ushort address
    {   get => cpu.address;}
    public byte data
    {   get => cpu.data;}

    public CPU6502.ReadWrite read_write
    {   get => cpu.read_write;}
}