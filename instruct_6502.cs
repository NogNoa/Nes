using System.Data;
using System.Diagnostics;

public class Instruct
{
    private static readonly HashSet<string> NonNZOps =
    [
        "branch", "bit", "jmp", "call", "push", "ret int", "ret sub"
    ];

    public int Length { get; set; } = 1;
    public int Cycles { get; set; } = 2;

    public enum Addressing { Impl, Dir, IndX, IndY, DRef, XDRef, DRefY };
    // Immediate and Rel are included in implied
    // Dir is direct. 
    // Ind is indexed. 
    // DRef is dereferenced i.e. indirect
    // Jump abs -> immediete[2], DRef -> abs

    //Source and dest could be:
    //  a name of a register
    //  S: for SP
    //  E: for PC (from Execute)
    //  M: for memory
    //  O: Source Imediate (from Operand)
    //  \0: null. Not using either a source or a dest
    //  0,1: constants (0 is ascii 30 != \0)
    //  Flags: themselves and on read also an Imediate
    //  Writing to O,0,1 also fails silently. Which is used by $89 NOP .
    public char Source { get; set; } = '\0';
    public char Dest { get; set; } = '\0';
    public string Operation { get; set; } = "mov";
    public Addressing addressing { get; set; } = Addressing.Impl;
    public bool PostOp { get; set; } = true;

    public static Instruct Decode_instruction(byte inst)
    {
        /*  the 6502 opcodes are made of 3 fields aaab_bbcc
            We break c into 2 flags: AF and XF;
            and assign b as adrs_group and a as oper_group.
            The function' broad-strokes structure is one 
            if-else tree which goes AF->XF->adrs_group -> oper_group.
            But again, this is just in broad-strokes, 
            as the 6502 endcoding is only broad-strokes Orthogonal.
        */
        Instruct back = new();
        bool AF = (inst & 1) != 0; // Accumulator function
        bool XF = (inst & 2) != 0; /* X-register function
                                      both being false implies 
                                      Y-register function*/
        byte adrs_group = (byte)((inst >> 2) & 7);
        byte oper_group = (byte)(inst >> 5);
        if ((adrs_group & 1) == 1)
        {   back.addressing = (adrs_group >> 2 == 0) ? 
                Addressing.Dir : 
                Addressing.IndX;
            // 1, 3->Dir; 5, 7->Indexed-X;
            back.Length = ((adrs_group >> 1) & 1) + 2;
            //1, 5->2 zeropaged   ; 3, 7->3 absolute; 
            back.Source = 'M';
        }
        /* if !AF and !(adrs_group & 1) we don't really care 
           since everything is implied (arity 1) Immediate or relative (arity 2)
           which are both also treated as implied.
           Except JSR abs which we will treat personnaly now.
           */
        // if the X index is alrady an operand, we'll treat IndX as IndY
        if (AF)
        {
            if ((adrs_group & 1) == 0)
            {   (back.addressing, back.Length) = (adrs_group >> 1) switch
                {
                    0 => (Addressing.XDRef, 3), //0->X-indirect;
                    1 => (back.addressing, back.Length), //2->implied
                    2 => (Addressing.DRefY, 3), //4->indirect-Y;
                    3 => (Addressing.IndY, 3), //6->indexed-Y;
                    _ => throw new InvalidDataException()
                };
            };
            back.Operation = oper_group switch
            {   0 => "or",
                1 => "and",
                2 => "xor",
                3 => "adc",
                4 => "store",
                5 => "load",
                6 => "cmp",
                7 => "sbc",
                _ => throw new InvalidDataException()
            };
            char patner = (adrs_group == 2) ? 'O' : 'M';
            if (back.Operation == "store") 
            {   back.Dest = patner;  
                back.Source = 'A'; 
            }
            else
            {   back.Dest = 'A';
                back.Source = patner;
            }
        }
        else  // !AF
        {   if (adrs_group == 0 && (oper_group & 4) == 4)
                    {   back.Source = 'O';
                        back.Length = 2;
                    }
            if (!XF)
            {   if ((oper_group & 4) == 0 && (adrs_group & 4) == 0)
                {   if (adrs_group == 0)
                    {   back.Dest = 'E';
                        switch (oper_group)
                        {   case 0:
                                back.Operation = "brk";
                                break;
                            case 1:
                                back.Operation = "call";
                                back.addressing = Addressing.Dir;
                                back.Length = 3;
                                back.Source = '\0';
                                break;
                            case 2:
                                back.Operation = "ret int";
                                break;
                            case 3:
                                back.Operation = "ret sub";
                                break;
                        }
                    }
                    else if (adrs_group == 2)
                    {   char argument = ((oper_group & 2) == 0) ? 'P' : 'A';
                        if ((oper_group & 1) == 0)
                        {   back.Operation = "push";
                            back.Source = argument;
                            back.Dest = '\0';
                        }
                        else
                        {   back.Operation = "pull";
                            back.Dest = argument;
                        }
                    }
                    else if (adrs_group == 3 & (oper_group & 2) == 2)
                    {  //(0,2,3) (0,3,3)
                        back.Source = '\0';
                        back.Dest = 'E';
                        back.Operation = "jmp";
                        if (oper_group == 3)
                        {   back.addressing = Addressing.DRef;
                            back.Length = 4;
                        }
                    }
                    else if ((adrs_group & 3) == 1 && oper_group == 1 )
                    {   //(0,1,1) (0,1,3)
                        back.Dest = 'P';
                        back.Operation = "bit";
                    }
                } 
                else  //((oper_group & 4) == 4 || (adrs_group & 4) == 4)
                {   if (adrs_group == 4)
                    {
                        back.Operation = "branch";
                        back.Length = 2;
                        back.Source = (oper_group >> 1) switch
                        {
                            0 => 'N',
                            1 => 'V',
                            2 => 'C',
                            3 => 'Z',
                            _ => throw new InvalidDataException()
                        }; 
                        back.Dest = (oper_group & 1).ToString()[0];
                    }
                    else if (adrs_group == 6)
                    {   char def_src = (oper_group & 1).ToString()[0]; //even -> clear; odd -> set;
                        (back.Dest, back.Source) = (oper_group >> 1, oper_group & 1) switch
                        {
                            (0, _) => ('C', def_src),
                            (1, _) => ('I', def_src),
                            (3, _) => ('D', def_src),
                            (2, 0) => ('A', 'Y'),
                            (2, 1) => ('V', '0'), 
                            _ => throw new InvalidDataException()
                        };
                    }                    
                    else  // (adrs_group not in (4, 6))
                    {   if (oper_group == 4)
                        {   back.Source = 'Y';
                            if (adrs_group == 2)
                            {   back.Operation = "dec";
                                back.Dest = 'Y';
                            }
                            else
                            {back.Dest = 'M';}
                        }
                        else {
                            back.Dest =  oper_group switch
                            {   5 or 6 => 'Y',
                                7 => 'X',
                                _ => throw new InvalidDataException()
                            };
                        }
                        if ((oper_group & 6) == 6)  //6,7
                        {   
                            if (adrs_group == 2)
                            {   back.Operation = "inc";
                                back.Source = back.Dest;
                            }
                            else if (adrs_group != 6)
                            {   back.Operation = "cmp";}
                        }
                    }
                }
            }
            else  // // !AF && XF
            {   (back.Dest, back.Source) = (adrs_group, oper_group) switch
                {   (var ag, _) when (ag & 1) == 1 => ('M', back.Source),
                    (2, 6) => ('X', 'X'),
                    (2, 7) => ('\0', '0'), //NOP = write 0+1 to nowhere don't update NZ
                    (2, _) => ('A', 'A'),
                    (6, 4) => ('S', 'X'),
                    (_, 4) => (back.Dest, 'X'),
                    (0, 5) => ('X', 'O'),
                    (6, 5) => ('X', 'S'),
                    (_, 5) => ('X', back.Source),
                    _ => (back.Dest, back.Source)
                };
                if (back.addressing == Addressing.IndX && (back.Dest == 'X' || back.Source == 'X'))
                    {back.addressing = Addressing.IndY;}
                back.Operation = oper_group switch
                {   0 => "asl",
                    1 => "rol",
                    2 => "lsr",
                    3 => "ror",
                    4 => "store",
                    5 => "load",
                    6 => "dec",
                    7 => "inc",
                    _ => throw new InvalidDataException()
                };
            }
        }
        if (NonNZOps.Contains(back.Operation) ||
            back.Source == '0' || back.Source == '1' ||
            back.Dest == 'S')
            { back.PostOp = false; }
        //todo: cycles
        return back;
    }

    public string Format()
    {
        string source = (Source == '\0') ? "Null" : Source.ToString();
        string dest = (Dest == '\0') ? "Null" : Dest.ToString();
        return $"{Operation}: {addressing}; {dest} <- {source}";
    }
}

/*
all
    if AG odd
    if AF
        AG
        OG
    else
        if AG = 0
        AG
        if not XF
            if OG &4 = 0 && AG & 4 = 0
                ...
            else
                AG: 4
                AG: 6
                else
                    if OG &6 = 6
                    else
        else
            OG: 0
            OG: 1
            OG: 2
            OG: 3
            else
                AG: 2
                AG: 6
                else:
                    OG: 4
                    OG: 5
                    OG: 6
                    OG: 7
*/