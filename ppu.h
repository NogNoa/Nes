#include <stdbool.h>
#include <stdint.h>
#include "rp2a0.h"

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

void PpuControl_write(uint8_t call);

/* PpuMask_2001 */
bool isGreyscale;
bool sprite_show_left;
bool BG_show_left;
bool sprite_show;
bool BG_show;
bool RedEmphasize;
bool GreenEmphasize;
bool BlueEmphasize;

void PpuMask_write(uint8_t call);

/* PpuStatus_2002 */
bool Sprite_overflow;
bool Sprite_0_Hit;
bool inVblank;

uint8_t PpuStatusRead(void);

/* Oam_2003..2005*/
void OamAddress(uint8_t);
void OamWrite(uint8_t);
uint8_t OamRead();


/* PpuScroll_2005*/
uint8_t x_scroll;

void PpuScrollWrite(uint8_t);

/* Vram_2006...2008*/
void VramAddress(uint8_t);
void VramWrite(uint8_t);
uint8_t VramRead();


uint8_t ppu_cs(uint8_t address, uint8_t data, enum RW rw);
void ppu_render(void);

#endif