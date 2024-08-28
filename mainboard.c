#include <stdint.h>
#include <stdbool.h>
#include <rp2a0.h>

void adress_decoder(uint16_t address, uint8_t data, enum RW rw)
{
    // CPU_M2 is implied by the call
    uint8_t control = address >> 13;
    if (control & 0b100)
        {return romsel(address & (('\1' << 15) - 1), data, rw);}
    else if (control & 0b10)
        {return;}
    else if (control & 1)
        {return ppu_cs(address & (('\1' << 3) - 1), data, rw);}
    else
        {return cpu_ram_cs(address & (('\1' << 11) - 1), data, rw);}
}

uint8_t demultiplexer_74139(bool enable, uint8_t address)
{   // address is only 2-bit
    uint8_t back = ('\1' << 4) -1;
    if (enable) {return back;}
    else {return (back ^ ('\1' << address)) & back;}
}