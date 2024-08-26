#include <cstdint>

void memory_mapper(uint16_t address)
{
    if (address & (1s << 15))
        {return romsel(address & ((1s << 15) - 1));}
    else
    {    if (address & (1s << 14))
            {return;}
        else if (address & (1s << 13))
            {return ppu_cs(address & ((1s << 3) - 1));}
        else
            {return cpu_ram_cs(address & ((1s << 11) - 1));}
    }
}