using uint6 = byte;

public enum ReadWrite {WRITE, READ}
interface ICpuBus
{
    byte Cpu_Access(ushort address, byte value, ReadWrite readWrite);
}
interface IPpuBus
{
    byte Ppu_Read(byte lo_address, uint6 hi_address, bool latch_enable);
    byte Ppu_Write(byte value, uint6 hi_address, bool latch_enable);
}

class Controller {public byte get_buttons(byte outsig){return 0;}}


