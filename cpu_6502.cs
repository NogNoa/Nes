class cpu_6502
{
    uint8 A;
    uint8 X;
    uint8 Y;
    uint8 SP;
    uint8 P;
    uint16 PC;

    bool carry
    {   get => (bool) (this.P & 1);
        set {(value) ? this.P | 1 : this.P & -2;}
    }
    bool zero
    {   get => (bool) (this.P & 2);
        set {(value) ? this.P | 2 : this.P & -3;}
    }
    bool interrupt_disable 
    {   get => (bool) (this.P & 4);
        set {(value) ? this.P | 4 : this.P & -5;}
    }
    bool mode_decimal 
    {   get => (bool) (this.P & 8);
        set {(value) ? this.P | 8 : this.P & -9;}
    }
    bool overflow => (bool) (this.P & 0b100000);
    bool negative => (bool) (this.P & 0b1000000);


    private enum interrupt_vector {NMI=0xFFFA, RST=0xFFFC, IRQ=0xFFFE};

    void interrupt_request() 
    {   if (this.interrupt_disable)
        {   this.push((uint8)(this.PC >> 8));
            this.push((uint8)(this.PC));
            this.push(this.P);
            this.interrupt_disable = true; 
            this.push(this.A);
            PC = interrupt_vector.IRQ;
        }
    }

    void nonmaskable_interrupt() 
    {   this.push(this.P);
        this.push(this.A);
        PC = interrupt_vector.NMI;
    }
}