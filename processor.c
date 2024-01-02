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

uint8_t* stack = p6502.memory + 0x100 

void 
stack_push(void)
{
	stack[p6502.reg.SP--] = p6502.reg.A;
}
void 
stack_pull(void)
{
	p6502.reg.A = stack[++p6502.reg.SP];
}

void
upgrade_flags(uint8_t mask)
{
	if (mask & 2)
		{p6502.reg.P.flags.z = !p6502.reg.A;}
	if (mask & 8)
		{p6502.reg.P.flags.z = (p6502.reg.A < 0);}
}