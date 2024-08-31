#include <stdint.h>
#include <stdlib.h>

#define 	P_c 0b1
#define 	P_z 0b10
#define 	P_i 0b100
#define 	P_d 0b1000
#define 	P_b 0b10000
//			    0b100000
#define 	P_v 0b1000000
#define 	P_n 0b10000000


struct rgistr 
{	
	uint16_t PC;
	uint8_t A;
	uint8_t X;
	uint8_t Y;
	uint8_t SP;
	uint8_t P;
};


static struct _p6502
{
	struct rgistr reg;
	uint8_t* memory;
	uint8_t* stack;
} p6502;

struct _p6502
processor_memory_init(struct _p6502 p, uint8_t* memory)
{
	p.memory = memory;
	p.stack = p6502.memory + 0x100;
	return p;
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
post_op_update(uint8_t result)
{
	p6502.reg.P &=  ~(P_z | P_n);
	p6502.reg.P |= (result ? 0 : P_z) 
	            | ((result < 0) ? P_n : 0);
}
