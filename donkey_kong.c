#include <stdint.h>

uint8_t PPU_base_nametable; // 0..4 λn. 0x2000 + 0x400*n
bool VRAM_adress_increment;
bool Sprite_pattern_table; // λn. 0x1000*n
bool BG_pattern_table; // λn. 0x1000*n
bool inVblank;

void reset(void)
{
	PPU_base_nametable = 
	VRAM_adress_increment =
	Sprite_pattern_table =
	0;
	BG_pattern_table = 1
	while (not inVblank);
	
}