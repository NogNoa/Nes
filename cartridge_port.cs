using System.ComponentModel;
using System.Diagnostics;
using uint11 = ushort;
using uint14 = ushort;
using uint15 = ushort;


public enum Mirroring { Horizontal, Vertical }

internal abstract class Cartridge(string Name, string Game_id, string Pcb_class, int Mapper_id)
{
    public string Name = Name;
    public string Game_id = Game_id;
    public string Pcb_class = Pcb_class;
    public int Mapper_id = Mapper_id;
    internal CartridgePort? bus;
    protected byte[] chr_rom = [];
    protected byte[] prg_rom = [];

    const int PRGBlock = 0x4000;
    const int CHRBlock = 0x2000;

    abstract internal byte Prg_Access(ushort address, byte value, ReadWrite readWrite, bool romsel);

    abstract internal byte Chr_Access(ushort address, byte data, bool ppu_write, bool ppu_read);

    public void Interrupt_request()
    {
        bus?.Interrupt_request();
    }

    abstract internal bool Ciram_CS(ushort address, bool ppu_write, bool ppu_read);

    abstract internal bool Ciram_A10(ushort address, bool ppu_write, bool ppu_read);

    abstract internal bool Chr_CS(ushort address, bool ppu_write, bool ppu_read);

    public static Cartridge From_file(string name, BinaryReader rom)
    {
        byte[] header = rom.ReadBytes(0x10);
        string magic = System.Text.Encoding.ASCII.GetString(header.AsSpan(0, 4));
        if (magic != "NES\x1a")
        { throw new NotACartridge(); }
        int prg_sz = header[4];
        int chr_sz = header[5];
        Mirroring mirroring = ((header[6] & 1) == 1) ? Mirroring.Horizontal : Mirroring.Vertical;
        bool isTrainer = (header[6] & 4) == 4;
        int mapper = (header[6] & 0xf0) >> 4;
        mapper |= header[7] & 0xf0;
        mapper |= (header[8] & 0x0f) << 8;
        prg_sz |= (header[9] & 0x0f) << 8;
        chr_sz |= (header[9] & 0xf0) << 4;
        prg_sz *= PRGBlock;
        chr_sz *= CHRBlock;
        Cartridge back;
        switch (mapper)
        {
            case 0:
                back = (prg_sz == 0x8000) ? new Nrom(name, "?", mirroring) :
                       (prg_sz == 0x4000) ? new Nrom_128K(name, "?", mirroring) :
                        throw new NotACartridge();
                break;
            default:
                throw new NotACartridge();
        }
        if (isTrainer)
            rom.ReadBytes(0x200);
        back.prg_rom = rom.ReadBytes(prg_sz);
        back.chr_rom = rom.ReadBytes(chr_sz);
        return back;
    }
}

[Serializable]
internal class NotACartridge : Exception{}

class Nrom: Cartridge
{
    readonly Mirroring Mirroring;

    /* byte[0x2000] chr_rom;
       byte[0x8000] prg_rom;
    */

    public Nrom(string Name, string Game_id, Mirroring mirroring) : base(Name, Game_id, "Nrom", 0)
    { this.Mirroring = mirroring; }

    internal override byte Chr_Access(uint14 address, byte data, bool _, bool ppu_read)
    {
        return ppu_read ? chr_rom[address] : data;
    }
    public new void Interrupt_request(){;}

    internal override byte Prg_Access(ushort address, byte data, ReadWrite readWrite, bool romsel)
    {
        return romsel ? prg_rom[address] : data;
    }

    internal override bool Ciram_CS(ushort address, bool _, bool __)
    {
        return (address & (1 << 13)) == 0;
    }

    internal override bool Ciram_A10(ushort address, bool _, bool __)
    {
        return  (address & (1 << ((this.Mirroring == Mirroring.Vertical) ? 10 : 9))) != 0;
    }

    internal override bool Chr_CS(ushort address, bool _, bool __)
    {
        return (address & (1 << 13)) != 0; //!ciram_cs
    }
}

class Nrom_128K(string Name, string Game_id, Mirroring mirroring) : Nrom(Name, Game_id, mirroring)
{
    /* byte[0x2000] chr_rom;
       byte[0x4000] prg_rom;
    */
    internal override byte Prg_Access(ushort address, byte data, ReadWrite readWrite, bool romsel)
    {
        address &= 1<<14 - 1;
        return romsel ? prg_rom[address] : data;
    }
}

class CartridgePort : ICpuAccessible
{
    Cartridge? _cartridge;
    public Cartridge? Cartridge {get => _cartridge;}
    private readonly ICartridgeBus bus;

    public CartridgePort(ICartridgeBus bus)
    {
        this.bus = bus;
    }

    public void Load_Cartridge(Cartridge cartridge)
    {
        this._cartridge = cartridge;
        _cartridge.bus = this;
    }
    public byte Cpu_Access(uint15 address, byte value, ReadWrite readWrite, bool romsel)
        => Cartridge?.Prg_Access(address, value, 
                                 readWrite, romsel) ?? value;
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {   return Cpu_Access(address, value,readWrite, false);
    }

    internal byte Ppu_Access(ushort address, byte data, bool ppu_write, bool ppu_read)
    {
        byte back = 0xFF;
        Cartridge cartridge;
        if (this.Cartridge == null) {return back;}
        else {cartridge = (Cartridge) this.Cartridge;}
        if (cartridge.Ciram_CS(address, ppu_write, ppu_read))
        {   uint11 vram_address = (uint11) ((address &  (1 <<  9) - 1) | ((cartridge.Ciram_A10(address, ppu_write, ppu_read)? 1 : 0) << 10));
            back &= bus.Access_Vram(vram_address, data, ppu_write ? ReadWrite.WRITE: ReadWrite.READ);   
        }
        if (cartridge.Chr_CS(address, ppu_write, ppu_read))
        {   back &= cartridge?.Chr_Access(address, data, ppu_write, ppu_read) ?? 0xFF;
        }
        return back;
    }
    internal void Interrupt_request()
    {
        bus.Interrupt_request();
    }
}

