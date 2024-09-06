class Ppu : IBus
{
    private byte Ppu_control;
    private byte Ppu_mask;
    private byte Ppu_status;
    public byte Oam_address;
    private byte Ppu_scroll;
    public byte Vram_address;

    private readonly RAM Vram, Oam;

    public Ppu(RAM Vram)
    {
        this.Vram = Vram;
        this.Oam = new RAM();
    }
    
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        return Access((byte) address, value, readWrite);
    }
    public byte Access(byte address, byte data, ReadWrite readWrite)
    {
        switch (address)
        {
            case 0:
                if (readWrite == ReadWrite.WRITE)
                    {Ppu_control = data;}
                break;
            case 1:
                if (readWrite == ReadWrite.WRITE)
                    {Ppu_mask = data;}
                break;
            case 2:
                if (readWrite == ReadWrite.READ)
                    {return Ppu_status;}
                break;
            case 3:
                if (readWrite == ReadWrite.WRITE)
                    {Oam_address =data;}
                break;
            case 4:
                if (readWrite == ReadWrite.WRITE)
                    {Oam.Write(Oam_address,data);}
                else
                    {return Oam.Read(Oam_address);}
                break;
            case 5:
                if (readWrite == ReadWrite.WRITE)
                    {Ppu_scroll = data;}
                break;
            case 6:
                if (readWrite == ReadWrite.WRITE)
                    {Vram_address =data;}
                break;
            case 7:
                if (readWrite == ReadWrite.WRITE)
                    {Vram.Write(Vram_address,data);}
                else
                    {return Vram.Read(Vram_address);}
                break;
            default:
                break;
            }
            return data;
        }
}