using uint3 = byte;
using uint6 = byte;

class Ppu : ICpuAccessible
{
    private byte Ppu_control;
    private byte Ppu_mask;
    private byte Ppu_status;
    private byte Cpu_Oam_address;
    private byte Ppu_scroll;
    private byte Cpu_Vram_address;

    private readonly RAM Vram, Oam;
    private readonly IPpuBus Bus;

    public Ppu(IPpuBus Bus, RAM Vram)
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
        => Access((uint3) (address & ((1 << 3) - 1)), value, readWrite);
    
    private byte Access(uint3 address, byte data, ReadWrite readWrite)
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
                    {Cpu_Oam_address =data;}
                break;
            case 4:
                {return Oam.Access(Cpu_Oam_address, data, readWrite);}
            case 5:
                if (readWrite == ReadWrite.WRITE)
                    {Ppu_scroll = data;}
                break;
            case 6:
                if (readWrite == ReadWrite.WRITE)
                    {Cpu_Vram_address =data;}
                break;
            case 7:
                {return Vram.Access(Cpu_Vram_address,data, readWrite);}
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
        {   Latch(lo_address);
            return Bus.Ppu_access(0, hi_address, true, ReadWrite.READ);
        }
    private void Write(byte lo_address, uint6 hi_address, byte data)
        {   Latch(lo_address);
            Bus.Ppu_access(data, hi_address, true, ReadWrite.WRITE);
        }
    private void Latch(byte lo_address)
    {Bus.Ppu_access(lo_address, 0, false, null);}
}