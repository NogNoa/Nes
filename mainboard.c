#include <stdint.h>
#include <stdbool.h>

void adress_decoder(uint16_t address)
{
    if (address & ('\1' << 15))
        {return romsel(address & (('\1' << 15) - 1));}
    else
    {   if (address & ('\1' << 14))
            {return;}
        else if (address & ('\1' << 13))
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