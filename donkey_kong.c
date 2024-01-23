#include <stdint.h>
#include "ppu.h"

void wait_2_frames()
{
	BG_pattern_table =
	sprite_show_left =
	BG_show_left =
	1;
}

void main(void)
{
	BG_pattern_table = 1;
	while (not inVblank);
	wait_2_frames();
}

