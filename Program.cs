using System;

class Program
{
    static void Main(string[] args)
    {

        NesBoard nes = new();

        BinaryReader file = new(new FileStream(@"<path>\Donkey Kong (Japan).nes", FileMode.Open));
        Nrom dk = (Nrom) Cartridge.From_file("Donkey Kong (Japan)", file);
        nes.Load_Cartridge(dk);

        
    }
}