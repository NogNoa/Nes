#include <stdint.h>

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


struct _p6502mem
{
	struct rgistr reg;
	union 
	{	uint8_t self[0x200];
		struct 
		{	uint8_t zero[0x100];
			uint8_t stack[0x100];
		} pages;
	} lowmem;
	uint8_t *highmem;
};

void
p6502_init(struct _p6502mem p6502)
{
	// p6502.memory = malloc(0x10000); //0x40K //already allocated statically
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
post_op_update(uint8_t result)
{
	p6502.reg.P &=  ~(P_z | P_n);
	p6502.reg.P |= (result ? 0 : P_z) 
	            | ((result < 0) ? P_n : 0);
}
