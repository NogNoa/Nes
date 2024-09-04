class NesBoard:IBus
{
    readonly CPU2403 cpu;

    byte Access(ushort address, byte value, ReadWrite readWrite)
    {return 0;}

}

class Demultiplxer
{
    readonly IBus[] recipients;

    Demultiplxer(IBus[] recipients)
    {
        this.recipients = recipients;
    }
    byte Access(ushort address, byte value, ReadWrite readWrite)
    {
        recipients[]
    }
}