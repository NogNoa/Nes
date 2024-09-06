
public enum ReadWrite {WRITE, READ}
interface IBus
{
    byte Access(ushort address, byte value, ReadWrite readWrite);
}

class Controller {public byte get_buttons(byte outsig){return 0;}}


class CartridgePort : IBus
{
    Object? Cartridge;
    public byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
    public byte Whisper(ushort address, byte value, ReadWrite readWrite)
    {
        throw new NotImplementedException();
    }
}

