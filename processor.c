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

struct _p6502
{
	struct rgistr reg;
	uint8_t memory[0x10000];
	uint8_t* stack;
} p6502;

void
p6502_init(struct _p6502 p6502)
{
	p6502.memory = malloc(0x10000); //0x40K
	p6502.stack = p6502.memory + 0x100;
	return p6502;
}

void 
stack_push(void)
{
	p6502.stack[p6502.reg.SP--] = p6502.reg.A;
}
void 
stack_pull(void)
{
	p6502.reg.A = p6502.stack[++p6502.reg.SP];
}

void
post_op_update(uint8_t mask, uint8_t result)
{
	if (mask & 2)
		{p6502.reg.P.flags.z = !result;}
	if (mask & 256)
		{p6502.reg.P.flags.n = (result < 0);}
}
