using System.Diagnostics;
using uint14 = ushort;
using uint11 = ushort;

public enum Mirroring {Horizontal, Vertical}

internal abstract class Cartridge(string Name, string Game_id, string Pcb_class, int Mapper_id)
{
    public string Name = Name;
    public string Game_id = Game_id; 
    public string Pcb_class = Pcb_class;
    public int Mapper_id = Mapper_id;
    required internal CartridgePort bus;
    protected byte[]? chr_rom;
    protected byte[]? prg_rom;

    abstract internal byte Cpu_Access(ushort address, byte value, ReadWrite readWrite);

    abstract internal (bool, byte?, uint11) Ppu_Access(ushort address, byte data, ReadWrite? readWrite);

    public void Interrupt_request()
    {
        bus?.Interrupt_request();
    }
}

class Nrom: Cartridge
{
    readonly Mirroring Mirroring;
    public Nrom(string Name, string Game_id, Mirroring mirroring) : base(Name, Game_id, "Nrom", 0) 
    {this.Mirroring = mirroring;}

    internal override (bool, byte?, uint11) Ppu_Access(uint14 address, byte data, ReadWrite? readWrite)
    {
        uint11 ciram_address = (uint11) ((this.Mirroring == Mirroring.Vertical) ? 
                               (address &  (1 << 10) - 1) : 
                               (address &  (1 <<  9) - 1) | ((address &  1 << 11) >> 1));   
        bool ciram_ce = ((address & (1 << 13)) == 0); // not ppu_a13
        bool chr_cs = !ciram_ce;
        byte? chr_data;
        if (readWrite == ReadWrite.READ && chr_cs) {chr_data = chr_rom?[address];}
        else {chr_data = null;}
        return (ciram_ce, chr_data, ciram_address);
    }
    public new void Interrupt_request(){;}

    internal override byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
}

class CartridgePort : ICpuBus
{
    Cartridge? _cartridge;
    public Cartridge? Cartridge {get => _cartridge;}
    private NesBoard bus;

    private bool ROMSEL;

    public CartridgePort(NesBoard bus)
    {
        this.bus = bus;
    }

    public void Load_Cartridge(Cartridge cartridge)
    {
        this._cartridge = cartridge;
        _cartridge.bus = this;
    }
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        ROMSEL = true;
        Debug.Assert(address <  1 << 15);
        return Cartridge?.Cpu_Access((ushort)(address |  1 << 15), value, 
                                     readWrite) ?? value;
    }
    public byte? Whisper(ushort address, byte value, ReadWrite readWrite)
    {
        if (ROMSEL) 
        {   ROMSEL = false; 
            return null;
        }
        Debug.Assert(address <  1 << 15);
        return Cartridge?.Cpu_Access(address, value, readWrite);
    }

    internal (bool, byte?, uint11)? Ppu_Access(ushort address, byte data, ReadWrite? readWrite)
    {
        return Cartridge?.Ppu_Access(address, data, readWrite) ?? null;
    }
    internal void Interrupt_request()
    {
        bus.Interrupt_request();
    }
}

