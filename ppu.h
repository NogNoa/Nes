/* PpuControl_2000 */
uint8_t PPU_base_nametable = 0; // 0..4 λn. 0x2000 + 0x400*n
bool VRAM_adress_increment = 0;
bool sprite_pattern_table = 0; // λn. 0x1000*n
bool BG_pattern_table = 0; // λn. 0x1000*n

/* PpuStatus_2002 */
bool inVblank = 0;

/* PpuMask_2001 */
bool sprite_show_left = 0;
bool BG_show_left = 0;