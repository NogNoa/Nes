#include <stdint.h>
#include "ppu.h"

static void func_87e7(void)
{
	int8_t PpuScrollDuppleX, PpuScrollDuppleY;
	int8_t PpuControlDupple = 0x10;
	PpuControl_write(PpuControlDupple);
	int8_t PpuMaskDupple = 6;
	PpuMask_write(PpuMaskDupple);
	PpuScrollDuppleX =
	PpuScrollDuppleY = 0;
	PpuScrollWrite(0);
	PpuScrollWrite(0);
}

static void wait_frame(void)
{
	PpuControl_write(0x10);
	while (!inVblank) ppu_render();
}

void main(void)
{
	wait_frame();
	/* clear $0000..$07FF except $1==FF*/
	func_87e7();
}
