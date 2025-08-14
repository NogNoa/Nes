using System.Diagnostics;

public class Instruct
{
    public int Length { get; set; } = 1;
    public int Cycles { get; set; } = 2;

    public enum Addressing { Impl, Dir, IndX, IndY, DRef, XDRef, DRefY };
    // Immediate and Rel are included in implied
    // Dir is direct. 
    // Ind is indexed. 
    // DRef is dereferenced i.e. indirect

    //Source and dest could only be a name of a register, with M for memory
    public char? Source { get; set; }    //null: source=dest
    public char Dest { get; set; }       //
    public string? Operation { get; set; } //null: = mov
    public Addressing addressing { get; set; } = Addressing.Impl;
    public bool PostOp { get; set; } = true;

    public static Instruct decode_instrcution(byte inst)
    {
        Instruct back = new();
        bool AF = (inst & 1) != 0; // Accumulator function
        bool XF = (inst & 2) != 0; /* X-register function
                                      Y-register function is implied 
                                      by nither of the above */
        byte adrs_group = (byte)((inst >> 2) & 7);
        byte oper_group = (byte)(inst >> 5);
        if ((adrs_group & 1) == 1)
            {   back.addressing = (adrs_group >> 2 == 0) ? 
                    Instruct.Addressing.Dir : 
                    Instruct.Addressing.IndX;
                // 1, 3->Dir; 5, 7->Indexed-X;
                back.Length = ((adrs_group >> 1) & 1) +  2;
                //1, 5->2 zeropaged   ; 3, 7->3 absolute; 
                back.Source = 'M';
            }
        else if (AF) 
            {   switch (adrs_group >> 1)
                {   case 0: back.addressing = Instruct.Addressing.XDRef; 
                            back.Length = 2; 
                            break;//0->X-indirect;
                                  //2->implied
                    case 2: back.addressing = Instruct.Addressing.DRefY; 
                            back.Length = 2; 
                            break;//4->indirect-Y;
                    case 3: back.addressing = Instruct.Addressing.IndY;
                            back.Length = 3;
                            break;//6->indexed-Y;
                }
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
            if (oper_group == 4)
            {
                back.Source = 'Y';
                back.Dest = 'M';
            }
            else if (5 == oper_group || oper_group == 6)
            {
                back.Dest = 'Y';
            }
            else if (oper_group == 7)
            {
                back.Dest = 'X';
            }
            if ((oper_group & 6) == 6) //6,7
            {
                back.Operation = "compare";
            }
            if (adrs_group == 6)
            {
                switch (oper_group >> 1)
                {
                    case 0: back.Dest = 'C'; break;
                    case 1: back.Dest = 'I'; break;
                    case 2: back.Dest = 'V'; break;
                    case 3: back.Dest = 'D'; break;
                }
                back.Source = (oper_group & 1).ToString()[0]; //even -> clear; odd -> set;
                if (inst == 0x98) { back.Source = 'Y'; back.Dest = 'A'; }
            }
            else if (adrs_group == 4)
            {
                back.Dest = 'E';
                switch (oper_group >> 1)
                {
                    case 0: back.Source = 'N'; break;
                    case 1: back.Source = 'V'; break;
                    case 2: back.Source = 'C'; break;
                    case 3: back.Source = 'Z'; break;
                }
                back.Operation = ((oper_group & 1) == 1) ? "branch if" : "branch nif";

            }
            if (oper_group < 4)
            {
                if (adrs_group == 3)
                {
                    if ((oper_group & 2) == 2) //(0,2,3) (0,3,3)
                    {
                        back.Dest = 'E';
                        back.Operation = "jmp";
                        if (oper_group == 3)
                        { back.addressing = Instruct.Addressing.DRef; }
                    }
                }
                else if (adrs_group == 2)
                {
                    char argument = ((oper_group & 2) == 0) ? 'P' : 'A';
                    if ((oper_group & 1) == 0)
                    {
                        back.Operation = "push";
                        back.Source = argument;
                    }
                    else
                    {
                        back.Operation = "pull";
                        back.Dest = argument;
                    }
                }
                else if (adrs_group == 0)
                {
                    back.Dest = 'E';
                    switch (oper_group)
                    {
                        case 0:
                            back.Operation = "brk";
                            break;
                        case 1:
                            back.Operation = "call";
                            back.addressing = Instruct.Addressing.Dir;
                            back.Length = 3;
                            back.Source = 'M';
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
                {
                    back.Dest = 'P';
                    back.Operation = "bit";
                }
            }
        }
        else if (AF)
        {
            back.Dest = 'A';
            back.Source = (adrs_group != 2) ? 'M' : 'N';
            back.Operation = (oper_group == 0) ? "or" :
                             (oper_group == 1) ? "and" :
                             (oper_group == 2) ? "xor" :
                             (oper_group == 3) ? "adc" :
                             (oper_group == 4) ? "store" :
                             (oper_group == 5) ? "load" :
                             (oper_group == 6) ? "cmp" :
                             (oper_group == 7) ? "sbc" :
                             throw new InvalidDataException();
        }
        else if (XF)
        {
            if (oper_group == 0)
            {
                back.Operation = "asl";
            }
            else if (oper_group == 1)
            {
                back.Operation = "rol";
            }
            else if (oper_group == 2)
            {
                back.Operation = "lsr";
            }
            else if (oper_group == 3)
            {
                back.Operation = "ror";
            }
            else if (oper_group == 4)
            {
                back.Operation = "store";
            }

        }
        //post_op
            return back;
    }


}

