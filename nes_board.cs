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
        ushort[] masks = [((1 << 11) - 1), ((1 << 3) - 1), 0, 0,  ((1 << 15) - 1)];
        this.address_decoder = new AddressDecoder([ram, ppu, new DeadEnd(), new DeadEnd(), cartridge_port], masks);
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
    readonly ushort[] masks;

    public AddressDecoder(IBus[] recipients, ushort[] masks) {
        this.recipients = recipients;
        this.masks = masks;
    }
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        int index = (new int[] {4, new int[] {0,1,2,3}[(address >> 13) & 0b11]})
        [address >> 15];
        return recipients[index].Access((ushort)(address & masks[index]), value, readWrite);
    }
}

class DeadEnd: IBus
{
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {return value;}
}