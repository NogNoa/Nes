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
        NesBoard nesBoard = new NesBoard();

        // Optionally, create a dummy cartridge and load it
        Nrom dummyCartridge = new Nrom("Dummy Game", "DUMMY01", Mirroring.Horizontal);
        nesBoard
            .GetType()
            .GetField("cartridge_port", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(nesBoard) is CartridgePort port
            ? port.Load_Cartridge(dummyCartridge)
            : throw new Exception("Failed to access cartridge port.");

        // The chips are now initialized and connected
        Console.WriteLine("NES chips initialized.");
    }
}