using System.Diagnostics.Contracts;

class Cartridge(string Name, string Game_id, string Pcb_class, int Mapper_id)
{
    public string Name = Name;
    public string Game_id = Game_id; 
    public string Pcb_class = Pcb_class;
    public int Mapper_id = Mapper_id;
}

class CartridgePort : IBus
{
    Cartridge? _cartridge;
    public Cartridge? Cartridge {get => _cartridge;}

    private bool ROMSEL;

    public void Load_Cartridge(Cartridge cartridge)
    {
        this._cartridge = cartridge;
    }
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        ROMSEL = true;
        throw new NotImplementedException();
        if (readWrite == ReadWrite.READ)
            {return 0;}
        else
            {return value;}
    }
    public byte? Whisper(ushort address, byte value, ReadWrite readWrite)
    {
        if (ROMSEL) 
        {   ROMSEL = false; 
            return null;
        }
        throw new NotImplementedException();
    }
}

