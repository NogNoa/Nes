using System.ComponentModel;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using Microsoft.VisualBasic;

class Logger
{
    public readonly struct Step(short pc, byte[] bytes, string assembly, Dictionary<string, byte> regs)
    {
        readonly Int16 pc;
        readonly byte[] bytes;
        readonly string assembly;
        readonly Dictionary<string, byte> regs;
        static string Line {get=>"";}
    }
    private readonly int[] tabstops = [6, 16, 48];
    private readonly FileStream gold;
    private int line_index = 0;

    public Logger()
    {
        gold = File.Open("./docs/golden.log", FileMode.Open);

    }
    ~Logger()
    {
        gold.Close();
    }

    public string readLine()
    {
        return File.ReadLines(gold);
    }
    public Step LineProcess(string line)
    {
        string strPc = line[..tabstops[0]].TrimEnd();
        string strBytes = line[tabstops[0]..tabstops[1]].TrimEnd();
        string Asm = line[tabstops[1]..tabstops[2]].TrimEnd();
        string stReg = line[tabstops[2]..].TrimEnd();

        var pc = short.Parse(strPc, NumberStyles.HexNumber);
        byte[] bytes = [.. strBytes.Split(' ').
                           Select((s) => byte.Parse(s, NumberStyles.HexNumber))];
        Dictionary<string, byte> reg = stReg.Split(' ').Select((s) => s.Split(':')).Select(couple => (couple[0], byte.Parse(couple[1], NumberStyles.HexNumber))).ToDictionary();
        ;
        return new Step(pc, bytes, Asm, reg);
    }
}

class TestBoard: ICpuAccessible
{
    byte[] opcodes;
    short pc;
    Logger logger = new();
    readonly RAM iram  = new();
    readonly CPU6502 cpu;
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        if (readWrite == ReadWrite.READ && pc <= address && address < pc + opcodes.Length)
            {return opcodes[address - pc];}
        return iram.Cpu_Access(address, value, readWrite);
    }

    public TestBoard()
    {
        cpu = new(this);
        cpu.Reset();
    }

    public static void Execute()
    {
        
    }

}