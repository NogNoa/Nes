
public enum ReadWrite {WRITE, READ}
interface Ibus
{
    byte access(ushort address, byte value, ReadWrite readWrite);
}

class Controller {get_buttons(){;}}