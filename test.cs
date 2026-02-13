using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using Microsoft.VisualBasic;

class Logger
{
    public struct Step(ushort pc, byte[] bytes, string assembly, Dictionary<string, byte> regs, int cycles)
    {
        readonly public UInt16 pc = pc;
        readonly public byte[] bytes = bytes;
        readonly public string assembly = assembly;
        public Dictionary<string, byte> regs = regs;
        public int cycles = cycles;
        static string Line {get=>"";}
    }
    private readonly int[] tabstops = [6, 16, 48];
    private readonly string[] gold;
    private int line_index = 0;

    public string CurrentLine {get=> gold[line_index];}

    public Logger()
    {
        gold = File.ReadAllLines(@"D:\Projects\Code\C--#\Nes\docs\golden.log");

    }

    public Step NextStep()
    {
        return LineProcess(gold[line_index++]);
    }
    
    public Step LineProcess(string line)
    {
        string strPc = line[..tabstops[0]].TrimEnd();
        string strBytes = line[tabstops[0]..tabstops[1]].TrimEnd();
        string Asm = line[tabstops[1]..tabstops[2]].TrimEnd();
        string stReg = line[tabstops[2]..].TrimEnd();

        var pc = ushort.Parse(strPc, NumberStyles.HexNumber);
        byte[] bytes = [.. strBytes.Split(' ').
                           Select((s) => byte.Parse(s, NumberStyles.HexNumber))];
        var reg_pairs = stReg.Split(' ').Select((s) => s.Split(':'));
        int cycles = int.Parse(reg_pairs.Last()[1], NumberStyles.None);
        var reg = reg_pairs.Select(couple => (couple[0], byte.Parse(couple[1], NumberStyles.HexNumber))).ToDictionary();
        reg.Remove("CYC");
        return new Step(pc, bytes, Asm, reg, cycles);
    }

    public string StepExpose(Step step)
    => step.pc.ToString("X4").PadRight(tabstops[0]) +
        string.Join(" ", step.bytes.Select(b => b.ToString("X2"))).
          PadRight(tabstops[1] - tabstops[0]) +
        step.assembly.PadRight(tabstops[2] - tabstops[1]) +
        string.Join(" ", step.regs.Select(pair => 
          $"{pair.Key}:{pair.Value:X2}")) + 
          $" CYC:{step.cycles:D}";
}

class TestBoard: ICpuAccessible
{
    byte[] opcodes;
    ushort pc;
    int cycles;
    readonly Logger logger = new();
    readonly RAM iram  = new();
    readonly CPU6502 cpu;
    public byte Cpu_Access(ushort address, byte value, ReadWrite readWrite)
    {
        if (readWrite == ReadWrite.READ) 
        {   if (pc <= address && address < pc + opcodes.Length)
                {return opcodes[address - pc];}
            else if (address == 0xfffc) {return 0x00;}
            else if (address == 0xfffd) {return 0xC0;}
        }
        return iram.Cpu_Access(address, value, readWrite);
    }

    public TestBoard()
    {
        cpu = new(this);
        cycles = 0;
        opcodes = [];
        cpu.Reset();
    }

    public void Execute()
    {   var step = logger.NextStep();
        pc = step.pc;
        opcodes = step.bytes;
        cycles += cpu.Execute();
        Check(cpu, logger.LineProcess(logger.CurrentLine));
    }
    public bool Check(CPU6502 cpu, Logger.Step step)
    {
        bool back = new List<string> { "A", "X", "Y", "P", "SP" }.
          Aggregate(true, (bl, r) => bl & cpu.CompareRegister(r, step.regs[r]));
        // back &= cycles == step.cycles;
        back &= cpu.CompareRegister("PCL", (byte) step.pc) &&
                cpu.CompareRegister("PCH", (byte) (step.pc >> 8));
        return back;
    }

    public void Run()
    {
        string old_line = logger.CurrentLine;
        string new_line;
        while (true)
        {
            try
                {Execute();}
            catch (Exception e)
            {   new_line = logger.CurrentLine;
                Console.WriteLine($"While running:\n{old_line}\n{new_line}");
                Console.WriteLine(e.Message);
                break;
            }
            try
            {new_line = logger.CurrentLine;}
            catch (IndexOutOfRangeException)
                {break;}
            if (!Check(cpu, logger.LineProcess(new_line)))
            {
                var cpu_reg = cpu.registersExpose();
                cpu_reg.Remove("PCL"); cpu_reg.Remove("PCH");
                var yours = logger.LineProcess(new_line);
                yours.regs = cpu_reg;
                yours.cycles = cycles;
                Console.WriteLine($"Golden:\n{old_line}\n{new_line}\nYours:\n{logger.StepExpose(yours)}");
                break;
            }
            old_line = new_line;
        }
    }

}