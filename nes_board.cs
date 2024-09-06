class NesBoard:IBus
{
    readonly CPU2403 cpu;
    readonly AddressDecoder address_decoder;
    private RAM ram;
    private readonly Cartridge cartridge_port;
    private readonly Ppu ppu;

    public NesBoard()
    {
        this.ram = new RAM();
        this.cartridge_port = new Cartridge();   
        this.ppu = new Ppu();
        this.address_decoder = new AddressDecoder([ram, ppu, new DeadEnd(), new DeadEnd(), cartridge_port]);
        this.cpu = new CPU2403(this, new Controller[2]);
    } 

    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        byte back = address_decoder.Access(address, value, readWrite);
        cartridge_port.Whisper((ushort)(address & ((1 << 15) - 1)), value, readWrite);
        return back;
    }

}

class AddressDecoder:IBus
{
    readonly IBus[] recipients;

    public AddressDecoder(IBus[] recipients) {
        this.recipients = recipients;
    }
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        return (new IBus[] {recipients[4], recipients[..4][(address >> 13) & 0b11]})
        [address >> 15].Access(address, value, readWrite);
    }
}

class DeadEnd: IBus
{
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {return value;}
}