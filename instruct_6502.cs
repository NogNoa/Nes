using System.Diagnostics;

public class Instruct
    {
        public int Arity {get; init;}
        public int Cycles {get; init;}

        

        public enum Addressing {Impl, Zpg, Abs, Rel, ZpgI, AbsI, XInd, IndY, Imm};
        public struct Microcode
        {   //Source and dest could only be a name of a register, with M for memory
            public char? Source {get; init;}    //null: source=dest
            public char Dest {get; init;}       //
            public string? Operation {get; init;} //null: = mov
            public byte? Operand {get; init;}    //only a direct or address

        }
        public Addressing addressing{get; set;} = Addressing.Impl;
        public Microcode[] steps = [];
        

    }

