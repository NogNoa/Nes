using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using Microsoft.VisualBasic;

class Logger
{
    public readonly struct Step(short pc, byte[] bytes, string assembly, Dictionary<string, byte> regs)
    {
        readonly public Int16 pc;
        readonly public byte[] bytes;
        readonly public string assembly;
        readonly public Dictionary<string, byte> regs;
        static string Line {get=>"";}
    }
    private readonly int[] tabstops = [6, 16, 48];
    private readonly string[] gold;
    private int line_index = 0;

    public Logger()
    {
        gold = File.ReadAllLines(@"D:\Projects\Code\C--#\Nes\docs\golden.log");

    }

    public string readLine()
    {
        return gold[line_index++];
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
    readonly Logger logger = new();
    readonly RAM iram  = new();
    readonly CPU6502 cpu;
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        if (readWrite == ReadWrite.READ) 
        {   if (pc <= address && address < pc + opcodes?.Length)
                {return opcodes[address - pc];}
            else if (address == 0xfffc) {return 0x00;}
            else if (address == 0xfffd) {return 0xC0;}
        }
        return iram.Cpu_Access(address, value, readWrite);
    }

    public TestBoard()
    {
        cpu = new(this);
        cpu.Reset();
    }

    public void Execute()
    {   var step = logger.LineProcess(logger.readLine());
        pc = step.pc;
        opcodes = step.bytes;
        cpu.Execute();
    }

}