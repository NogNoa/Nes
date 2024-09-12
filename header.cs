using uint6 = byte;

public enum ReadWrite {WRITE, READ}
interface ICpuBus
{
    byte Cpu_Access(ushort address, byte value, ReadWrite readWrite);
}

class Controller {public byte get_buttons(byte outsig){return 0;}}


