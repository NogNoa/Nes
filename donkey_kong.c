#include <stdint.h>
#include "ppu.h"


void main(void)
{
	BG_pattern_table = 1;
	while (not inVblank);
	func_87e7();
}

void func_87e7()
{
	BG_pattern_table =
	sprite_show_left =
	BG_show_left =
	1;
}