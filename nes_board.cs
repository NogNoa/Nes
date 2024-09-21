using uint6 = byte;
using uint11 = ushort;
using uint14 = ushort;
using System.Net.Sockets;
using System.ComponentModel;

class NesBoard:ICpuBus
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
        this.cartridge_port = new CartridgePort(this);   
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
    public void Interrupt_request()
    {
        this.cpu.Interrupt_request();
    }
    
    public byte Ppu_Latch(byte data, bool latch_enable)
    {
        return ppu_address_buffer.Access(data, latch_enable);
    }
    public byte Ppu_access(byte da_duplex, uint6 hi_address, bool latch_enable, ReadWrite? readWrite)
    {
        byte vram_data_bus = da_duplex;
        (bool chip_ebable_vram, byte? data, uint11 vram_address)? cart_status;
        uint14 address = (ushort) (( hi_address << 8) | Ppu_Latch(da_duplex, latch_enable));
        cart_status = cartridge_port.Ppu_Access(address, da_duplex, readWrite);
        if (cart_status != null)
        {
            vram_data_bus |= cart_status.data;
            da_duplex = (byte) (da_duplex & );
            Ppu_Latch(da_duplex, latch_enable);
        }
        return da_duplex;
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

class RAM : ICpuBus
{
    private readonly byte[] value;

    public RAM()
    {
        value = new byte[0x800];
    }
    public byte Access(uint11 address, byte value, ReadWrite readWrite)
    {
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