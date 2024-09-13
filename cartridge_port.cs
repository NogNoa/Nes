using System.Diagnostics;

public enum Mirroring {Horizontal, Vertical}

internal abstract class Cartridge(string Name, string Game_id, string Pcb_class, int Mapper_id)
{
    public string Name = Name;
    public string Game_id = Game_id; 
    public string Pcb_class = Pcb_class;
    public int Mapper_id = Mapper_id;
    required internal CartridgePort bus;
    private byte[]? chr_rom;
    private byte[]? prg_rom;

    abstract internal byte Cpu_Access(ushort address, byte value, ReadWrite readWrite);

    abstract internal byte Ppu_Access(ushort address, byte data, ReadWrite readWrite);

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

    internal override byte Ppu_Access(ushort address, byte data, ReadWrite readWrite)
    {
        bool ciram_ce = ((address & (1 << 13)) == 0); // not ppu_a13
        bool ciram_a10 = (1 & ((this.Mirroring == Mirroring.Vertical) ? (address >> 10) : (address >> 11))) == 1;
        throw new NotImplementedException();
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

    internal byte Ppu_Access(ushort address, byte data, ReadWrite readWrite)
    {
        return Cartridge?.Ppu_Access(address, data, readWrite) ?? data;
    }
    internal void Interrupt_request()
    {
        bus.Interrupt_request();
    }
}

