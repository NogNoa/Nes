class cpu_6502
{
    byte A;
    byte X;
    byte Y;
    byte SP;
    byte P;
    uint16 PC;

    public uint16 address;
    public byte data;
    

    byte bit_set(bool value, byte the_byte, byte power_o_2)
    {
        return (value) ? the_byte | power_o_2 : the_byte & ~power_o_2;
    }

    bool carry
    {   get => (bool) (this.P & 1);
        set {this.P = bit_set(value, this.P, 1);}
    }
    bool zero
    {   get => (bool) (this.P & 0b10);
        set {this.P = bit_set(value, this.P, 0b10);}
    }
    bool interrupt_disable 
    {   get => (bool) (this.P & 0b100);
        set {this.P = bit_set(value, this.P, 0b100);}
    }
    bool mode_decimal 
    {   get => (bool) (this.P & 0b1000);
        set {this.P = bit_set(value, this.P, 0b1000);}
    }
    bool overflow 
    {   get => (bool) (this.P & 0b100000);
        set {this.P = bit_set(value, this.P, 0b100000);}
    }
    bool negative 
    {   get => (bool) (this.P & 0b1000000);
        set {this.P = bit_set(value, this.P, 0b1000000);}
    }


    private enum interrupt_vector {NMI=0xFFFA, RST=0xFFFC, IRQ=0xFFFE};

    void interrupt(uint16 vector)
    {   this.push((byte)(this.PC >> 8));
        this.push((byte)(this.PC));
        this.push(this.P);
        this.interrupt_disable = true;
        this.PC = vector;
    }

    public void interrupt_request() 
    {   if (this.interrupt_disable)
        {   interrupt(interrupt_vector.IRQ);
        }
    }

    public void nonmaskable_interrupt() 
    {   interrupt(interrupt_vector.NMI);
    }

    public void reset()
    {   this.interrupt_disable = true;
        this.PC = interrupt_vector.RST;
    }
    
    public void set_overflow() {this.overflow = true;}
}