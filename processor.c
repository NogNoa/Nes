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


typedef struct _p6502
{
	struct rgistr reg;
	uint8_t* memory;
	uint8_t* stack;
} p6502_t;

p6502_t *
p6502_init(void)
{
	p6502_t *p6502 = (p6502_t *) malloc(sizeof p6502);
	// p6502.memory = malloc(0x10000); //0x40K //already allocated statically
	(*p6502).stack = (*p6502).memory + 0x100;
	return p6502;
}

void 
stack_push(p6502_t p6502)
{
	p6502.stack[p6502.reg.SP--] = p6502.reg.A;
}
void 
stack_pull(p6502_t p6502)
{
	p6502.reg.A = p6502.stack[++p6502.reg.SP];
}

void
post_op_update(uint8_t result, p6502_t p6502)
{
	p6502.reg.P &=  ~(P_z | P_n);
	p6502.reg.P |= (result ? 0 : P_z) 
	            | ((result < 0) ? P_n : 0);
}
