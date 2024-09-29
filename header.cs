using uint6 = byte;
using uint11 = ushort;

public enum ReadWrite {WRITE, READ}

interface IAccessible
{
    byte Access(uint11 address, byte value, ReadWrite? readWrite);
}
interface ICpuAccessible
{
    byte Cpu_Access(ushort address, byte value, ReadWrite readWrite);
}

interface ICpuBus: ICpuAccessible
{   
    public byte Get_controller(byte index, byte outsig);
}
interface IPpuBus
{
    void Nonmaskable_interrupt();
    byte Ppu_access(byte vram_data_bus, uint6 hi_address, bool latch_enable, ReadWrite? readWrite);
}

interface ICartridgeBus
{
     byte Access_Vram(uint11 address, byte value, ReadWrite readWrite);
     void Interrupt_request();
}

class Controller {public byte get_buttons(byte outsig){return 0;}}


