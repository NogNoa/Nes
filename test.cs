using System.ComponentModel;
using System.Globalization;
using System.Runtime.Intrinsics.X86;

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