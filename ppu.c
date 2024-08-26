#include "ppu.h"

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