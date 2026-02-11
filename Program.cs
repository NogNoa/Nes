using System;

class Program
{
    static void Main(string[] args)
    {

        NesBoard nes = new();

        BinaryReader file = new(new FileStream(args[0], FileMode.Open));
        Nrom dk = (Nrom) Cartridge.From_file("Donkey Kong (Japan)", file);
        nes.Load_Cartridge(dk);
        // nes.Run();
        new Logger().LineProcess("C000  4C F5 C5  JMP $C5F5                       A:00 X:00 Y:00 P:24 SP:FD CYC:7");
        
    }
}