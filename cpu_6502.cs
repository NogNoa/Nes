class cpu_6502
{
    uint8 A;
    uint8 X;
    uint8 Y;
    uint8 SP;
    uint8 P;
    uint16 PC;

    bool carry => (bool) (this.P & 1);
    bool zero => (bool) (this.P & 0b10);
    bool interrupt_disable => (bool) (this.P & 0b100);
    bool mode_decimal => (bool) (this.P & 0b1000);
    bool overflow => (bool) (this.P & 0b100000);
    bool negative => (bool) (this.P & 0b1000000);


    private enum interrupt_vector {NMI=0xFFFA, RST=0xFFFC, IRQ=0xFFFE};

    void interrupt_request() 
    {   if (this.interrupt_disable != 0)
        {   this.push(this.P);
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