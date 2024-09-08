
public enum ReadWrite {WRITE, READ}
interface ICpuBus
{
    byte Cpu_Access(ushort address, byte value, ReadWrite readWrite);
}
interface IPpuBus
{
    byte Ppu_Read(ushort address);
    byte Ppu_Write(ushort address, byte value);
}

class Controller {public byte get_buttons(byte outsig){return 0;}}


