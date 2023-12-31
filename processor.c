#include <stdint.h>

struct register 
{	
	uint16_t PC;
	uint8_t A;
	uint8_t X;
	uint8_t Y;
	uint8_t SP;
	union 
	{
		uint8_t full;
		struct {
		c: 1;
		z: 1;
		i: 1;
		d: 1;
		b: 1;
		_: 1;
		v: 1;
		n: 1;
		} flags;
	} P;
};

struct
{
	struct register reg;
	uint8_t[0x10000] memory;
} p6502;

static void 
stack_push(void)
{
	p6502.memory[0x100 | p6502.SP--] = p6502.A;
}
static void 
stack_pull(void)
{
	p6502.A = p6502.memory[0x100 | ++p6502.SP];
}