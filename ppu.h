#include <stdbool.h>
#include <stdint.h>

#ifndef PPU_REGISTERS
#define PPU_REGISTERS

typedef enum  {slave, master} ppu_role;

/* PpuControl_2000 */
uint8_t PPU_base_nametable; // 0..4 λn. 0x2000 + 0x400*n
bool VRAM_adress_increment; // λn. 1 << 0x20 ** n
bool Sprite_pattern_table; // λn. 0x1000*n
bool BG_pattern_table; // λn. 0x1000*n
bool Sprite_size; // vert(px) = λn. 8*n
ppu_role EXT_pins;
bool Vblank_on_Nmi;

/* PpuStatus_2002 */
bool inVblank;

/* PpuMask_2001 */
bool sprite_show_left;
bool BG_show_left;

#endif