using uint3 = byte;
using uint6 = byte;
using uint11 = ushort;
using uint14 = ushort;

class NesBoard : ICpuBus, IPpuBus, ICartridgeBus
{
    private readonly CPU2403 cpu;
    private readonly static AddressDecoder address_decoder = new AddressDecoder();
    private readonly RAM iram, vram;
    private readonly CartridgePort cartridge_port;
    private readonly ICpuAccessible?[] cpu_recipients;
    private readonly static ushort[] masks =
            [((1 << 11) - 1), ((1 << 3) - 1), 0, 0, ((1 << 15) - 1)];
    private readonly Ppu ppu;
    private readonly Buffer ppu_address_buffer;
    readonly Controller[] controllers; //len 2

    public NesBoard()
    {
        this.iram = new RAM();
        this.vram = new RAM();
        this.cartridge_port = new CartridgePort(this);
        this.ppu = new Ppu(this, vram);
        this.cpu_recipients = [iram, ppu, null, null, cartridge_port];
        this.controllers = new Controller[2];
        this.cpu = new CPU2403(this);
        this.ppu_address_buffer = new Buffer();
    }

    public byte Cpu_Access(ushort full_address, byte value, ReadWrite readWrite)
    {
        byte back;
        ushort address = full_address;
        int recipient_index = address_decoder.Decode(full_address);
        ICpuAccessible destination = cpu_recipients[recipient_index] ??
                                     new DeadEnd();
        address &= masks[recipient_index];
        if (destination == cartridge_port)
        {
            back = cartridge_port.Cpu_Access(address, value, readWrite);
        }
        else
        {
            full_address &= masks[4];
            back = destination.Cpu_Access(address, value, readWrite);
            cartridge_port.Cpu_Access(full_address, value, readWrite, false);
        }
        return back;
    }
    public void Nonmaskable_interrupt() => cpu.Nonmaskable_interrupt();
    public void Interrupt_request() => cpu.Interrupt_request();

    private byte Ppu_Latch(byte data, bool latch_enable)
     => ppu_address_buffer.Access(data, latch_enable);

    public byte Ppu_access(byte vram_data_bus, uint6 hi_address, bool latch_enable, bool write, bool read)
    {
        if (write || read)
        {
            uint14 address = (ushort)((hi_address << 8) | Ppu_Latch(vram_data_bus, latch_enable));
            vram_data_bus &= cartridge_port.Ppu_Access(address, vram_data_bus, write, read);
        }
        Ppu_Latch(vram_data_bus, latch_enable);
        return vram_data_bus;
    }
    public byte Access_Vram(uint11 address, byte value, ReadWrite readWrite)
     => vram.Access(address, value, readWrite, true);

    public byte Get_controller(byte index, uint3 outsig)
    {
        return this.controllers[index].get_buttons(outsig);
    }

    public void Load_Cartridge(Cartridge cartridge)
    { cartridge_port.Load_Cartridge(cartridge);

    }

}

class AddressDecoder
{
    public int Decode(ushort address)
        => (new int[] {4, (address >> 13) & 0b11})[address >> 15];
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
    public byte Access(uint11 address, byte value, ReadWrite wrt_enabl, bool out_enabl)
    {   /*the very fact of the call indicate chip_select low*/
        if (wrt_enabl == ReadWrite.WRITE) {this.value[address] = value;}
        return out_enabl ? this.value[address]: value;
    }
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite) 
    {   
        return Access(address, value, readWrite, true);
    }
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