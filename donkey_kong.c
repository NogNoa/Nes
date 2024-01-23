#include <stdint.h>
#include "ppu.h"

static void wait_2_frames(void)
{
	BG_pattern_table = 1;
	Vblank_on_Nmi = 0;
	int8_t var_10 = 10;
	int8_t var_11 = 6;
	x_scroll =
	int8_t var_12 =
	int8_t var_13 = 0;
	sprite_show_left =
	BG_show_left = 1;
	/* $12 and $13 are cleared again*/
}

void main(void)
{
	BG_pattern_table = 1;
	Vblank_on_Nmi = 0;
	while (!inVblank);
	/* clear $0000..$07FF except $1==FF*/
	wait_2_frames();
}
