using System.Diagnostics;

public class Instruct
    {
        public int Arity {get; set;} = 1;
        public int Cycles {get; set;} = 2;

        

        public enum Addressing {Impl, Dir, Rel, IndX, IndY, XDRef, DRefY};
        // Immediate is included in implied
        // Dir is direct. 
        // Ind is indexed. 
        // DRef is dereferenced i.e. indirect
        public struct Microcode
        {   //Source and dest could only be a name of a register, with M for memory
            public char? Source {get; set;}    //null: source=dest
            public char Dest {get; set;}       //
            public string? Operation {get; set;} //null: = mov
            public byte? Operand {get; set;}    //only a direct or address

        }
        public Addressing addressing{get; set;} = Addressing.Impl;
        public List<Microcode> steps = [];
        

    }

