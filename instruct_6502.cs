using System.Data;
using System.Diagnostics;

public class Instruct
{
    private static readonly HashSet<string> NonNZOps =
    [
        "branch if", "branch nif", "bit", "jmp", "call", "push", "ret int", "ret sub"
    ];

    public int Length { get; set; } = 1;
    public int Cycles { get; set; } = 2;

    public enum Addressing { Impl, Dir, IndX, IndY, DRef, XDRef, DRefY };
    // Immediate and Rel are included in implied
    // Dir is direct. 
    // Ind is indexed. 
    // DRef is dereferenced i.e. indirect
    // Jump abs -> immediete[2], DRef -> abs

    //Source and dest could only be a name of a register, with M for memory
    public char Source { get; set; }    //null: source=dest
    public char Dest { get; set; }       //
    public string Operation { get; set; } = "mov";
    public Addressing addressing { get; set; } = Addressing.Impl;
    public bool PostOp { get; set; } = true;

    public static Instruct Decode_instruction(byte inst)
    {
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
        if (!XF && !AF)
        {   
            if ((oper_group & 6) == 6) //6,7
            {   back.Operation = (adrs_group == 6) ? 
                                 "store" : 
                                 "cmp";
            }
            if (adrs_group == 6)
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
            else if (adrs_group == 4)
            {
                back.Dest = 'E';
                back.Length = 2;
                back.Source = (oper_group >> 1) switch
                {
                    0 => 'N',
                    1 => 'V',
                    2 => 'C',
                    3 => 'Z',
                    _ => throw new InvalidDataException()
                };
                back.Operation = ((oper_group & 1) == 1) ? "branch if" : "branch nif";
            }
            else
            {   switch (oper_group)
                {   case 4:
                        back.Source = 'Y';
                        back.Dest = 'M';
                        break;
                    case 5:
                    case 6:
                        back.Dest = 'Y'; break;
                    case 7:
                        back.Dest = 'X'; break;
                } 
            }
            if (oper_group < 4)
            {   if (adrs_group == 3)
                {   if ((oper_group & 2) == 2) //(0,2,3) (0,3,3)
                    {   back.Dest = 'E';
                        back.Operation = "jmp";
                        back.Source = '\0';
                        if (oper_group == 3)
                        {   back.addressing = Addressing.DRef;
                            back.Length = 4;
                        }
                    }
                }
                else if (adrs_group == 2)
                {   char argument = ((oper_group & 2) == 0) ? 'P' : 'A';
                    if ((oper_group & 1) == 0)
                    {   back.Operation = "push";
                        back.Source = argument;
                        back.Dest = 'O';
                    }
                    else
                    {   back.Operation = "pull";
                        back.Dest = argument;
                    }
                }
                else if (adrs_group == 0)
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
                if (oper_group == 1 && (adrs_group & 5) == 1)
                {   back.Dest = 'P';
                    back.Operation = "bit";
                }
            }
            else if (adrs_group == 0 && oper_group >= 4)
            {   back.Source = 'O';
                back.Length = 2;
            }
        }
        else if (AF)
        {
            (back.addressing, back.Length) = adrs_group switch
            {
                0 => (Addressing.XDRef, 3), //0->X-indirect;
                                            //2->implied
                4 => (Addressing.DRefY, 3), //4->indirect-Y;
                6 => (Addressing.IndY, 3), //6->indexed-Y;
                _ => (back.addressing, back.Length)
            };
            back.Dest = 'A';
            back.Source = (adrs_group != 2) ? 'M' : 'O';
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
            if (back.Operation == "store") { back.Dest = back.Source;  back.Source = 'A'; }
        }
        else if (XF)
        {   (back.Dest, back.Source) = (adrs_group, oper_group) switch
            {   (var ag, _) when (ag & 1) == 1 => ('M', back.Source),
                (2, 6) => ('X', 'X'),
                (2, 7) => ('O', '0'), //NOP = write 0+1 to nowhere don't update NZ
                (2, _) => ('A', 'A'),
                (6, 4) => ('S', 'X'),
                (_, 4) => (back.Dest, 'X'),
                (0, 5) => ('X', 'O'),
                (6, 5) => ('X', 'S'),
                (_, 5) => ('X', back.Source),
                _ => (back.Dest, back.Source)
            };
            if (back.Source == 'O') {back.Length = 2;}
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
        if (NonNZOps.Contains(back.Operation) ||
            back.Source == '0' || back.Source == '1' ||
            back.Dest == 'S')
        { back.PostOp = false; }
        if (back.addressing == Addressing.IndX && (back.Dest == 'X' || back.Source == 'X'))
        {back.addressing = Addressing.IndY;}
        //todo: cycles
        return back;
    }

    public string Format()
    {
        return $"{Operation}: {addressing}; {Dest} <- {Source}";
    }
}

