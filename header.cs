
public enum ReadWrite {WRITE, READ}
interface IBus
{
    byte Access(ushort address, byte value, ReadWrite readWrite);
}

class Controller {public byte get_buttons(byte outsig){return 0;}}
class RAM : IBus
{
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
}


class Cartridge : IBus
{
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
}
class Ppu : IBus
{
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
}
