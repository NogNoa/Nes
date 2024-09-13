using System.Diagnostics;

class Cartridge(string Name, string Game_id, string Pcb_class, int Mapper_id): ICpuBus
{
    public string Name = Name;
    public string Game_id = Game_id; 
    public string Pcb_class = Pcb_class;
    public int Mapper_id = Mapper_id;

    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
    internal byte Ppu_Access(ushort address, byte da_duplex, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
}

class CartridgePort : ICpuBus
{
    Cartridge? _cartridge;
    public Cartridge? Cartridge {get => _cartridge;}

    private bool ROMSEL;

    public void Load_Cartridge(Cartridge cartridge)
    {
        this._cartridge = cartridge;
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

    internal byte Ppu_Access(ushort address, byte da_duplex, ReadWrite readWrite)
    {
        return Cartridge?.Ppu_Access(address, da_duplex, readWrite) ?? da_duplex;
    }
}

