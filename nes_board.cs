class NesBoard:IBus
{
    readonly CPU2403 cpu;
    readonly AddressDecoder address_decoder;
    private RAM ram;
    private readonly Cartridge cartridge;
    private readonly Ppu ppu;

    public NesBoard()
    {
        this.ram = new RAM();
        this.cartridge = new Cartridge();   
        this.ppu = new Ppu();
        this.address_decoder = new AddressDecoder([this.ram, this.ppu, new DeadEnd(), new DeadEnd(), this.cartridge]);
        this.cpu = new CPU2403(this.address_decoder, new Controller[2]);
    } 

    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {return 0;}

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