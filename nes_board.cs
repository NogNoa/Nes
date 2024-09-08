class NesBoard:ICpuBus, IPpuBus
{
    private readonly CPU2403 cpu;
    private readonly AddressDecoder address_decoder;
    private readonly RAM iram, vram;
    private readonly CartridgePort cartridge_port;
    private readonly Ppu ppu;
    private readonly Buffer ppu_address_buffer;

    public NesBoard()
    {
        this.iram = new RAM();
        this.vram = new RAM();
        this.cartridge_port = new CartridgePort();   
        this.ppu = new Ppu(this, vram);
        ushort[] masks = [((1 << 11) - 1), ((1 << 3) - 1), 0, 0,  ((1 << 15) - 1)];
        this.address_decoder = new AddressDecoder([iram, ppu, null, null, cartridge_port], masks);
        this.cpu = new CPU2403(this, new Controller[2]);
        this.ppu_address_buffer = new Buffer();
    } 

    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        byte back = address_decoder.Cpu_Access(address, value, readWrite);
        byte? whisp = cartridge_port.Whisper((ushort)(address & ((1 << 15) - 1)), value, readWrite);
        return whisp ?? back;
    }
    public void Nonmaskable_interrupt()
    {
        this.cpu.Nonmaskable_interrupt();
    }

    public byte Ppu_Read(ushort address)
    {
        throw new NotImplementedException();
    }

    public byte Ppu_Write(ushort address, byte value)
    {
        throw new NotImplementedException();
    }
}

class AddressDecoder:ICpuBus
{
    readonly ICpuBus?[] recipients;
    readonly ushort[] masks;

    public AddressDecoder(ICpuBus?[] recipients, ushort[] masks) {
        this.recipients = recipients;
        this.masks = masks;
    }
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        int index = (new int[] {4, (address >> 13) & 0b11})
        [address >> 15];
        return (recipients[index] ?? new DeadEnd()).Cpu_Access((ushort)(address & masks[index]), value, readWrite);
    }
}

class DeadEnd: ICpuBus
{
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {return value;}
}

class RAM : ICpuBus, IPpuBus
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
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
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

    public byte Ppu_Read(ushort address)
    {
        throw new NotImplementedException();
    }

    public byte Ppu_Write(ushort address, byte value)
    {
        throw new NotImplementedException();
    }
}

class Buffer
{
    private byte _in = 0;
    private byte _out = 0;
    public bool isOpen = true;

    public byte Read()
    {
        if (isOpen){ return _in;}
        else {return _out;}
    }
    public byte Write(byte data, bool? isOpen)
    {   this.isOpen = isOpen ?? this.isOpen;
        _in = data;
        if (this.isOpen) {_out = data;}
        return data;
    }
}