class NesBoard:IBus
{
    private readonly CPU2403 cpu;
    private readonly AddressDecoder address_decoder;
    private readonly RAM iram, vram;
    private readonly CartridgePort cartridge_port;
    private readonly Ppu ppu;

    public NesBoard()
    {
        this.iram = new RAM();
        this.vram = new RAM();
        this.cartridge_port = new CartridgePort();   
        this.ppu = new Ppu(vram);
        ushort[] masks = [((1 << 11) - 1), ((1 << 3) - 1), 0, 0,  ((1 << 15) - 1)];
        this.address_decoder = new AddressDecoder([iram, ppu, null, null, cartridge_port], masks);
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
    readonly IBus?[] recipients;
    readonly ushort[] masks;

    public AddressDecoder(IBus?[] recipients, ushort[] masks) {
        this.recipients = recipients;
        this.masks = masks;
    }
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        int index = (new int[] {4, (address >> 13) & 0b11})
        [address >> 15];
        return (recipients[index] ?? new DeadEnd()).Access((ushort)(address & masks[index]), value, readWrite);
    }
}

class DeadEnd: IBus
{
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {return value;}
}

class RAM : IBus
{
    private readonly byte[] value;

    public RAM()
    {
        value = new byte[0x800];
    }
    public void Write(ushort address, byte value)
    {
        this.value[address] = value;
    }
    public byte Read(ushort address)
    {
        return this.value[address];
    }
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        switch (readWrite)
        {
            case ReadWrite.WRITE:
                Write(address, value);
                return value;
            case ReadWrite.READ:
                return Read(address);
            default:
                throw new NotSupportedException();
        }
    }
}