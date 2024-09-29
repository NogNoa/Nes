using uint6 = byte;
using uint11 = ushort;
using uint14 = ushort;

class NesBoard:ICpuBus, IPpuBus, ICartridgeBus
{
    private readonly CPU2403 cpu;
    private readonly AddressDecoder address_decoder;
    private readonly RAM iram, vram;
    private readonly CartridgePort cartridge_port;
    private readonly Ppu ppu;
    private readonly Buffer ppu_address_buffer;
    readonly Controller[] controllers; //len 2

    public NesBoard()
    {
        this.iram = new RAM();
        this.vram = new RAM();
        this.cartridge_port = new CartridgePort(this);   
        this.ppu = new Ppu(this, vram);
        ushort[] masks = [((1 << 11) - 1), ((1 << 3) - 1), 0, 0,  ((1 << 15) - 1)];
        this.address_decoder = new AddressDecoder([iram, ppu, null, null, cartridge_port], masks);
        this.controllers = new Controller[2];
        this.cpu = new CPU2403(this);
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
    public void Interrupt_request()
    {
        this.cpu.Interrupt_request();

    }
    
    private byte Ppu_Latch(byte data, bool latch_enable)
    {
        return ppu_address_buffer.Access(data, latch_enable);
    }
    public byte Ppu_access(byte vram_data_bus, uint6 hi_address, bool latch_enable, ReadWrite? readWrite)
    {
        if (readWrite != null)
        {   uint14 address = (ushort) (( hi_address << 8) | Ppu_Latch(vram_data_bus, latch_enable));
            vram_data_bus |= cartridge_port.Ppu_Access(address, vram_data_bus, (ReadWrite) readWrite);
        }
        Ppu_Latch(vram_data_bus, latch_enable);
        return vram_data_bus;
    }
    public byte Access_Vram(uint11 address, byte value, ReadWrite readWrite)
    {
        return vram.Access(address, value, readWrite);
    }
    public byte Get_controller(byte index, byte outsig)
    {
        outsig &= (1 << 3) - 1;
        return this.controllers[index].get_buttons(outsig);
    }
}

class AddressDecoder:ICpuAccessible
{
    readonly ICpuAccessible?[] recipients;
    readonly ushort[] masks;

    public AddressDecoder(ICpuAccessible?[] recipients, ushort[] masks) {
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

class DeadEnd: ICpuAccessible
{
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {return value;}
}

class RAM : ICpuAccessible, IAccessible
{
    private readonly byte[] value;

    public RAM()
    {
        value = new byte[0x800];
    }
    public byte Access(uint11 address, byte value, ReadWrite? readWrite)
    {
        if (readWrite == null) {return 0;}
        if (readWrite == ReadWrite.WRITE) {this.value[address] = value;}
        return this.value[address];
    }
    public byte Cpu_Access(uint11 address, byte value, ReadWrite readWrite) 
        => Access(address, value, readWrite);
}

class Buffer
{
    private byte _in = 0;
    public byte Access(byte source, bool isOpen)
    {   
        if (isOpen){_in = source;}
        return _in;
    }
}