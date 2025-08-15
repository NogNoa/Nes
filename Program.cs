using System;

class Program
{
    static void Main(string[] args)
    {
        // Initialize RAM for internal and video RAM
        RAM iram = new RAM();
        RAM vram = new RAM();

        // Create a dummy controller array
        Controller[] controllers = new Controller[2] { new Controller(), new Controller() };

        // Create the NES board (which initializes CPU, PPU, CartridgePort, etc.)
        NesBoard nesBoard = new();

        // Optionally, create a dummy cartridge and load it
        Nrom dummyCartridge = new("Dummy Game", "DUMMY01", Mirroring.Horizontal);
        nesBoard.Load_Cartridge(dummyCartridge);

        // The chips are now initialized and connected
        Console.WriteLine("NES chips initialized.");
    }
}