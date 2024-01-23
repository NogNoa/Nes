#include <stdbool.h>
#include <stdint.h>

#ifndef PPU_REGISTERS
#define PPU_REGISTERS

typedef enum  {slave, master} ppu_role;

/* PpuControl_2000 */
uint8_t PPU_base_nametable; // 0..4 λn. 0x2000 + 0x400*n ---- --10
bool VRAM_adress_increment; // λn. 1 << 0x20 ** n		 ---- -2--
bool Sprite_pattern_table; // λn. 0x1000*n				 ---- 3---
bool BG_pattern_table; // λn. 0x1000*n					 ---4 ----
bool Sprite_size; // vert(px) = λn. 8*n					 --5- ----
ppu_role EXT_pins;			//							 -6-- ----
bool Vblank_on_Nmi;			//							 7--- ----

/* PpuMask_2001 */
bool isGreyscale;
bool sprite_show_left;
bool BG_show_left;
bool sprite_show;
bool BG_show;
bool RedEmphasize;
bool GreenEmphasize;
bool BlueEmphasize;

/* PpuStatus_2002 */
bool Sprite_overflow;
bool Sprite_0_Hit;
bool inVblank;

#endif