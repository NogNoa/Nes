class NesBoard:IBus
{
    readonly CPU2403 cpu;
    readonly Demultiplxer[] address_decoder;
    public NesBoard()
    {
        this.cpu = new CPU2403(this, new Controller[2]);
        this.address_decoder = new Demultiplxer[2];
        this.address_decoder[1] = new Demultiplxer(new RAM(), new Cartridge(), new DeadEnd(), new DeadEnd());
        this.address_decoder[0] = new Demultiplxer(new DeadEnd(), new Ppu(), new DeadEnd(), address_decoder[1]);
    }

    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {return 0;}

}

class Demultiplxer:IBus
{
    readonly IBus[] recipients;

    public Demultiplxer(IBus r0, IBus r1, IBus r2, IBus r3)
    {
        this.recipients = [r0, r1, r2, r3];
    }
    public byte Access(byte index, ushort address, byte value, ReadWrite readWrite)
    {
        return recipients[index].Access(address, value, readWrite);
    }
}

class DeadEnd: IBus
{
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {return value;}
}