using uint3 = byte;
using uint6 = byte;

class Ppu : ICpuBus
{
    private byte Ppu_control;
    private byte Ppu_mask;
    private byte Ppu_status;
    public byte Oam_address;
    private byte Ppu_scroll;
    public byte Vram_address;

    private readonly RAM Vram, Oam;
    private NesBoard Bus;

    public Ppu(NesBoard Bus, RAM Vram)
    {
        this.Bus = Bus;
        this.Vram = Vram;
        this.Oam = new RAM();
    }

    // uint8_t PPU_base_nametable
    //     {get => Ppu_control;
    //      set => Ppu_control = value;
    //     }
    // bool VRAM_adress_increment
    //     {get => (bool)((Ppu_control & 0x1) != 0);
    //      set => Ppu_control = Ppu_control & 1 | value & 1;
    //     }
    // bool Sprite_pattern_table
    //     {get => (bool)((Ppu_control & 0x1) != 0);
    //      set => Ppu_control = Ppu_control & 1 | value & 1;
    //     }
    // bool BG_pattern_table
    //     {get => (bool)((Ppu_control & 0x1) != 0);
    //      set => Ppu_control = Ppu_control & 1 | value & 1;
    //     }
    // bool Sprite_size
    //     {get => (bool)((Ppu_control & 0x1) != 0);
    //      set => Ppu_control = Ppu_control & 1 | value & 1;
    //     }
    // ppu_role EXT_pins
    //     {get => (bool)((Ppu_control & 0x1) != 0);
    //      set => Ppu_control = Ppu_control & 1 | value & 1;
    //     }
    // bool Vblank_on_Nmi
    //     {get => (bool)((Ppu_control & 0x1) != 0);
    //      set => Ppu_control = Ppu_control & 1 | value & 1;
    //     }
    
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        return Access((uint3) address, value, readWrite);
    }
    public byte Access(uint3 address, byte data, ReadWrite readWrite)
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
                throw new ArgumentException("PPU has only 3 lines of address", 
                                            nameof(address));
        }
        return data;
    }
    private void VBlank()
    {
        Bus.Nonmaskable_interrupt();
    }
    public void Reset()
    {;}
    private byte Read(byte lo_address, uint6 hi_address)
        {return Bus.Ppu_Read(lo_address, hi_address);}
    private void Write(byte lo_address, uint6 hi_address, byte data)
        {;}
    private void latch(){;}
}