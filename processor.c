#include <stdint.h>

struct rgistr 
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
			unsigned 
			c: 1,
			z: 1,
			i: 1,
			d: 1,
			b: 1,
			_: 1,
			v: 1,
			n: 1;
		} flags;
	} P;
};

struct
{
	struct rgistr reg;
	uint8_t memory[0x10000];
} p6502;

static void 
stack_push(void)
{
	p6502.memory[0x100 | p6502.reg.SP--] = p6502.reg.A;
}
static void 
stack_pull(void)
{
	p6502.reg.A = p6502.memory[0x100 | ++p6502.reg.SP];
}