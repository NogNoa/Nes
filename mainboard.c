#include <stdint.h>
#include <stdbool.h>

void adress_decoder(uint16_t address)
{
    uint8_t control = address >> 13;
    if (control & 0b100)
        {return romsel(address & (('\1' << 15) - 1));}
    else
    {   if (control & 0b10)
            {return;}
        else if (control & 1)
            {return ppu_cs(address & (('\1' << 3) - 1));}
        else
            {return cpu_ram_cs(address & (('\1' << 11) - 1));}
    }
}

uint8_t demultiplexer_74139(bool enable, uint8_t address)
{   // address is only 2-bit
    uint8_t back = ('\1' << 4) -1;
    if (enable) {return back;}
    else {return (back ^ ('\1' << address)) & back;}
}