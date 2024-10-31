using uint3 = byte;
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
        byte back;
        ICpuAccessible destination = address_decoder.Decode(address);
        if (destination == cartridge_port)
        {   back = cartridge_port.Cpu_Access(address, value, readWrite); 
            
        }
        else 
        {   back = destination.Cpu_Access(address, value, readWrite);
            cartridge_port.Cpu_Access(address, value, readWrite, false);
        }
        return back;
    }
    public void Nonmaskable_interrupt() => cpu.Nonmaskable_interrupt();
    public void Interrupt_request() => cpu.Interrupt_request();
    
    private byte Ppu_Latch(byte data, bool latch_enable)
     => ppu_address_buffer.Access(data, latch_enable);

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
     => vram.Access(address, value, readWrite);
    
    public byte Get_controller(byte index, uint3 outsig)
    {
        return this.controllers[index].get_buttons(outsig);
    }
}

class AddressDecoder
{
    readonly ICpuAccessible?[] recipients;
    readonly ushort[] masks;

    public AddressDecoder(ICpuAccessible?[] recipients, ushort[] masks) {
        this.recipients = recipients;
        this.masks = masks;
    }

    public ICpuAccessible Decode(ushort address)
    {
        int index = (new int[] {4, (address >> 13) & 0b11})
        [address >> 15];
        return recipients[index] ?? new DeadEnd();
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
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite) 
        => Access((uint11)(address & ((1 << 11) - 1)), value, readWrite);
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