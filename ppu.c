#include "ppu.h"

static uint8_t vOamAddress = 0;
static uint16_t vVramAddress = 0;
static bool w = 0;

void PpuControl_write(uint8_t call)
{   PPU_base_nametable = call & 0b11;
    VRAM_adress_increment = (call >> 2) & 1;
    Sprite_pattern_table = (call >> 3) & 1;
    BG_pattern_table = (call >> 4) & 1;
    Sprite_size = (call >>5) & 1;
    EXT_pins = (call >> 6) & 1; 
    Vblank_on_Nmi = (call >> 7) & 1; 
}

void PpuMask_write(uint8_t call)
{   isGreyscale = call & 1;
    sprite_show_left = (call >> 1) & 1;
    BG_show_left = (call >> 2) & 1;
    sprite_show = (call >> 3) & 1;
    BG_show = (call >> 4) & 1;
    RedEmphasize = (call >>5) & 1;
    GreenEmphasize = (call >> 6) & 1; 
    BlueEmphasize = (call >> 7) & 1; 
}

uint8_t PpuStatusRead(void)
{
    uint8_t back;
    w = 0;
    back = inVblank << 1;
    back = (back | Sprite_0_Hit) << 1;
    back = (back | Sprite_overflow) << 5;
    return back;
}

void OamAddress(uint8_t address)
{
    vOamAddress = address;
}

// stubs start
void OamWrite(uint8_t call)
{
    ;
}
uint8_t OamRead()
{
    return vOamAddress;
}
//stabs end

void PpuScrollWrite(uint8_t call)
{
    if (w){ y_scroll = call;}
    else {x_scroll = call;}
    w = !w;
}

void VramAddress(uint8_t call)
{
        vVramAddress &= ((1 << 8) - 1) << (8 * w);
        vVramAddress |= ((uint16_t) call) << (8 * !w);
    w = !w;
}

//stabs start
void VramWrite(uint8_t call)
{
    ;
}

uint8_t VramRead()
{
    return vVramAddress;
}
//stabs end

uint8_t ppu_cs(uint8_t address, uint8_t data, enum RW rw)
{
    switch (address & 7)
    {
    case 0:
        if (rw == WRITE)
            {PpuControl_write(data);}
        break;
    case 1:
        if (rw == WRITE)
            {PpuMask_write(data);}
        break;
    case 2:
        if (rw == READ)
            {return PpuStatusRead();}
        break;
    case 3:
        if (rw == WRITE)
            {OamAddress(data);}
        break;
    case 4:
        if (rw == WRITE)
            {OamWrite(data);}
        else
            {return OamRead();}
        break;
    case 5:
        if (rw == WRITE)
            {PpuScrollWrite(data);}
        break;
    case 6:
        if (rw == WRITE)
            {VramAddress(data);}
        break;
    case 7:
        if (rw == WRITE)
            {VramWrite(data);}
        else
            {return VramRead();}
        break;
    default:
        break;
    }
    return data;
}

//stab
void ppu_render(void);