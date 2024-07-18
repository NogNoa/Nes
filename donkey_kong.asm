--------NES RAM--------  

----------------         
--------unidentified block--------
-8000-

----------------         
--------reset--------  
-879E-:  
  SEI                      
  CLD                      
  LDA #$10                 
  STA PpuControl_2000      
  LDX #$FF                 
  TXS                      
  LDA PpuStatus_2002       
  AND #$80                 
  BEQ $C7A8                
  LDY #$07                 
  STY $01                  
  LDY #$00                 
  STY $00                  
  LDA #$00                 
  STA ($00),Y              
  DEY                      
  BNE $C7B9                
  DEC $01                  
  BPL $C7B9                
  JSR ppu_setup+0          
  LDA #$7F                 
  STA $0511                
  LDA #$18                 
  STA $51                  
  LDA #$01                 
  STA $4E                  
  STA $55                  
  LDA #$00                 
  STA $4F                  
  LDA $10                  
  EOR #$80                 
  STA PpuControl_2000      
  STA $10                  
  JSR $F4FC                
  JMP $C7E1                
--------ppu_setup()--------
-87E7-:
ppu_setup:               
  LDA #$10                 
  STA PpuControl_2000      
  STA $10                  
  LDA #$06                 
  STA PpuMask_2001         
  STA $11                  
  LDA #$00                 
  STA PpuScroll_2005       
  STA $12                  
  STA PpuScroll_2005       
  STA $13                  
  JSR $CBAA                
  JMP $CBB3                
--------sub start--------
  TAX                      
  LDA $C4A7,X              
  STA $00                  
  LDA $C4A8,X              
  STA $01                  
  JMP $F237                
--------sub start--------
  TAX                      
  LDA $C03C,X              
  STA $02                  
  LDA $C03D,X              
  STA $03                  
  JMP $F2E6                
--------unidentified block--------

----------------         
--------sub start--------
  TAX                      
  LDA $C03C,X              
  STA $04                  
  LDA $C03D,X              
  STA $05                  
  LDA $C044,X              
  STA $06                  
  LDA $C045,X              
  STA $07                  
  RTS                      
----------------         
--------sub start--------
  TAX                      
  LDA $C03C,X              
  STA $02                  
  LDA $C03D,X              
  STA $03                  
  RTS                      
----------------         
--------sub start--------
  TAX                      
  LDA $C03C,X              
  STA $08                  
  LDA $C03D,X              
  STA $09                  
  RTS                      
----------------         
--------nmi--------      
  PHA                      
  LDA $10                  
  AND #$7F                 
  STA PpuControl_2000      
  STA $10                  
  LDA #$00                 
  STA OamAddr_2003         
  LDA #$02                 
  STA SpriteDma_4014       
  LDA #$31                 
  STA $00                  
  LDA #$03                 
  STA $01                  
  JSR $F237                
  LDA #$00                 
  STA $0330                
  STA $0331                
  JSR $F51D                
  LDA $11                  
  EOR #$18                 
  STA PpuMask_2001         
  JSR $FA48                
  LDA $4E                  
  BNE $C8C1                
  LDA $4F                  
  BEQ $C8D4                
  LDA $9A                  
  BNE $C8A5                
  JSR $CE78                
  JMP $C8D7                
--------unidentified block--------

----------------         
  LDA $55                  
  BNE $C8CB                
  JSR $CA30                
  JMP $C8D7                
  JSR $C8F3                
  JSR $F4BB                
  JMP $C8D7                
  JSR $CAC9                
  LDA $0505                
  CMP #$01                 
  BNE $C8E8                
  LDA $51                  
  STA $00                  
  JSR $F24B                
  DEC $0505                
  LDA $10                  
  EOR #$80                 
  STA PpuControl_2000      
  STA $10                  
  PLA                      
  RTI                      
----------------         
--------sub start--------
  LDA $0102                
  BNE $C8FE                
  STA ApuStatus_4015       
  STA $0100                
  LDA $0518                
  BNE $C914                
  LDA #$80                 
  STA $FD                  
  LDA #$04                 
  STA $0518                
  LDA #$0F                 
  STA ApuStatus_4015       
  STA $0100                
  LDA $0510                
  BNE $C940                
  JSR $D196                
  LDA #$08                 
  JSR $C807                
  LDA $0511                
  STA $0200                
  LDA #$A2                 
  STA $0201                
  LDA #$00                 
  STA $0202                
  STA $58                  
  LDA #$38                 
  STA $0203                
  STA $0510                
  LDA #$20                 
  STA $44                  
  RTS                      
----------------         
  LDA $15                  
  AND #$20                 
  BNE $C95D                
  LDA $15                  
  AND #$10                 
  BNE $C98A                
  LDA #$00                 
  STA $0512                
  LDA $44                  
  BNE $C95C                
  LDA #$01                 
  STA $58                  
  JMP $C9B1                
  RTS                      
----------------         
  LDA #$40                 
  STA $44                  
  LDA $0512                
  BNE $C985                
  LDA #$40                 
  STA $35                  
  LDA $0200                
  CLC                      
  ADC #$10                 
  CMP #$BF                 
  BNE $C976                
  LDA #$7F                 
  STA $0200                
  STA $0511                
  INC $0512                
  LDA #$0A                 
  STA $0513                
  RTS                      
----------------         
--------unidentified block--------

----------------         
  STA $0514                
  LDX #$0A                 
  LDA #$00                 
  STA $24,X                
  DEX                      
  BNE $C991                
  LDA $0511                
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  SEC                      
  SBC #$07                 
  STA $50                  
  CMP #$02                 
  BMI $C9AD                
  LDA #$1C                 
  STA $51                  
  JMP $C9B1                
  LDA #$18                 
  STA $51                  
  LDA $50                  
  AND #$01                 
  ASL A                    
  TAX                      
  LDA $0507,X              
  STA $21                  
  LDA $0508,X              
  STA $22                  
  LDA #$0F                 
  STA $18                  
  LDA #$13                 
  STA $19                  
  LDA #$00                 
  STA $4E                  
  STA $0406                
  STA $0407                
  STA $4F                  
  STA $0510                
  STA $050B                
  STA $0512                
  LDA #$01                 
  STA $53                  
  STA $0400                
  STA $0401                
  LDA #$00                 
  STA $54                  
  STA $0402                
  STA $0403                
  LDA #$00                 
  STA $52                  
  STA $0408                
  STA $0409                
  STA $FC                  
  LDA #$03                 
  LDX $58                  
  BEQ $CA06                
  LDA #$01                 
  STA $55                  
  STA $0404                
  STA $0405                
  STA $040B                
  LDA $58                  
  BNE $CA26                
  LDA #$97                 
  STA $43                  
  LDA #$01                 
  STA $FD                  
  LDA #$0F                 
  STA ApuStatus_4015       
  STA $0100                
  RTS                      
----------------         
  DEC $0518                
  LDA #$75                 
  STA $43                  
  JMP $CBAA                
--------sub start--------
  JSR $F4BB                
  LDA $58                  
  BNE $CA4A                
  LDA $43                  
  CMP #$75                 
  BEQ $CA5A                
  CMP #$74                 
  BEQ $CA5F                
  CMP #$73                 
  BEQ $CA64                
  CMP #$5F                 
  BEQ $CA79                
  RTS                      
----------------         
  STA $55                  
  LDA #$00                 
  STA $58                  
  STA $0510                
--------sub start--------
  JSR $CBB3                
  JSR $CBAA                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDY #$00                 
  LDA $0053,Y              
  STA $0400,X              
  INX                      
  INX                      
  INY                      
  CPY #$03                 
  BNE $CABB                
  RTS                      
----------------         
--------sub start--------
  JSR $F4BB                
  LDA $53                  
  CMP #$01                 
  BEQ $CAD8                
  LDA $43                  
  CMP #$84                 
  BEQ $CB02                
  LDA $43                  
  CMP #$74                 
  BCS $CB18                
  CMP #$6F                 
  BEQ $CAE7                
  CMP #$64                 
  BEQ $CAFA                
  RTS                      
----------------         
  LDA $040B                
  BEQ $CAF6                
  LDA #$00                 
  STA $040B                
  DEC $55                  
  JSR $CBB9                
  JSR $CC30                
  RTS                      
----------------         
  LDA #$01                 
  STA $4F                  
  JSR $CC43                
  RTS                      
----------------         
--------unidentified block--------

----------------         
  JMP $CB1B                
  CMP #$7A                 
  BEQ $CB28                
  CMP #$75                 
  BEQ $CB30                
  CMP #$74                 
  BEQ $CB4C                
  RTS                      
----------------         
  DEC $43                  
  JSR $CA53                
  JMP $CBC6                
  JSR $CBB3                
  DEC $43                  
  LDA $58                  
  BNE $CB3D                
  LDA #$08                 
  STA $FD                  
  LDX $53                  
  DEX                      
  LDA $C608,X              
  STA $00                  
  LDA #$20                 
  STA $01                  
  JMP $EBBF                
  JSR $D196                
  LDX $53                  
  DEX                      
  TXA                      
  ASL A                    
  JSR $C807                
  LDA #$0A                 
  JSR $C807                
  LDA $51                  
  CMP #$1C                 
  BEQ $CB6F                
  LDA #$76                 
  STA $00                  
  LDA #$20                 
  STA $01                  
  LDA #$04                 
  JSR $C815                
  LDA #$01                 
  STA $0505                
  JSR $D02E                
  JSR $CBB9                
  LDA #$BC                 
  STA $00                  
  LDY $54                  
  INY                      
  JSR $F4D1                
  LDA #$00                 
  STA $2C                  
  LDA #$80                 
  DEY                      
  CPY #$04                 
  BPL $CB92                
  LDA $C207,Y              
  STA $2E                  
  LDA #$0D                 
  STA $45                  
  LDA #$02                 
  STA $00                  
  JSR $F24B                
  DEC $43                  
  LDA $58                  
  BEQ $CBA9                
  LDA #$73                 
  STA $43                  
  RTS                      
----------------         
--------sub start--------
  LDA #$00                 
  STA $04                  
  LDA #$FF                 
  JMP $F0A1                
--------sub start--------
  JSR $D196                
  JMP $F1C3                
--------sub start--------
  LDA #$B5                 
  STA $00                  
  LDA #$20                 
  STA $01                  
  LDY $55                  
  JMP $F4D1                
  LDA $58                  
  BNE $CBF0                
  LDA $51                  
  CMP #$1C                 
  BNE $CBF0                
  LDX $52                  
  LDA $53                  
  CMP $0400,X              
  BNE $CBF0                
  LDY #$00                 
  LDA $C6AA,Y              
  STA $0331,Y              
  BEQ $CBE7                
  INY                      
  JMP $CBDB                
--------unidentified block--------

----------------         
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $58                  
  BNE $CC1F                
  LDX $52                  
  LDA $0408,X              
  BNE $CC1F                
  TXA                      
  TAY                      
  CLC                      
  ASL A                    
  ASL A                    
  TAX                      
  LDA $25,X                
  CMP #$02                 
  BCC $CC1F                
  STA $0408,Y              
  INC $55                  
  JSR $CBB9                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA #$01                 
  STA $0505                
  JSR $D02E                
  LDA #$00                 
  STA $050B                
  JSR $CCBD                
  JMP $D7EE                
--------sub start--------
  LDA #$00                 
  TAX                      
  STA $59,X                
  STA $040D,X              
  INX                      
  CPX #$89                 
  BNE $CC46                
  LDA #$01                 
  STA $59                  
  STA $96                  
  STA $043E                
  STA $0451                
  STA $0452                
  STA $9F                  
  STA $0503                
  LDA #$04                 
  STA $97                  
  LDA #$58                 
  STA $043D                
  LDA #$20                 
  STA $A2                  
  LDA #$80                 
  STA $18                  
  LDA #$0A                 
  STA $34                  
  LDX $52                  
  JSR $CAB9                
  LDA #$BB                 
  STA $39                  
  LDA #$27                 
  STA $44                  
  LDA $53                  
  CMP #$01                 
  BEQ $CC95                
  CMP #$03                 
  BEQ $CCA2                
  LDA #$10                 
  STA $FC                  
  RTS                      
----------------         
  LDA #$38                 
  STA $36                  
  LDA #$40                 
  STA $43                  
  LDA #$02                 
  STA $FC                  
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $53                  
  SEC                      
  SBC #$01                 
  ASL A                    
  TAX                      
  LDA $C5A6,X              
  STA $09                  
  LDA $C5A7,X              
  STA $0A                  
  LDX #$00                 
  LDY #$00                 
  LDA ($09),Y              
  CMP #$FE                 
  BEQ $CCEF                
  STA $00,X                
  INY                      
  INX                      
  CPX #$05                 
  BNE $CCD2                
  STY $86                  
  LDA ($09),Y              
  JSR $F0A5                
  LDY $86                  
  INY                      
  LDX #$00                 
  JMP $CCD2                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $58                  
  BEQ $CE90                
  LDA $0102                
  BNE $CE87                
  STA ApuStatus_4015       
  STA $0100                
  LDA $15                  
  AND #$20                 
  BEQ $CE90                
  JMP $CF27                
  LDA $0516                
  BNE $CEAA                
  LDA $0517                
  BEQ $CE9E                
  DEC $0517                
  RTS                      
----------------         
  JSR $CC00                
  JSR $CFA4                
  LDA $9A                  
  CMP #$01                 
  BNE $CEAD                
  JMP $CF18                
  LDA $BF                  
  BEQ $CEB4                
  JMP $CF0F                
  LDA $96                  
  CMP #$FF                 
  BNE $CEBD                
  JMP $CF15                
  CMP #$08                 
  BEQ $CED2                
  CMP #$04                 
  BEQ $CED2                
  LDA $58                  
  BEQ $CECF                
  JSR $EBF3                
  JMP $CED2                
  JSR $D171                
  JSR $EB1F                
  JSR $EBCF                
  JSR $D03D                
  JSR $D1A0                
  JSR $EA76                
  JSR $E1FA                
  JSR $EE88                
  LDA $53                  
  CMP #$03                 
  BEQ $CEFD                
  CMP #$04                 
  BEQ $CF09                
  JSR $DA12                
  JSR $E1AF                
  JSR $EC38                
  JMP $CF18                
--------unidentified block--------

----------------         
  JSR $EE1B                
  JMP $CF18                
  JSR $D0BC                
  JSR $CF3E                
  LDA $0516                
  BNE $CF26                
  JSR $D048                
  JSR $F4BB                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $15                  
  AND #$10                 
  BEQ $CF8B                
  LDA $58                  
  BEQ $CF51                
  LDA #$00                 
  STA $58                  
  LDA $15                  
  JMP $C98A                
--------unidentified block--------

----------------         
  STA $0514                
  LDA $0517                
  BEQ $CF97                
  DEC $0517                
  RTS                      
----------------         
  LDA $0516                
  BNE $CF9D                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDX #$00                 
  LDY #$00                 
  LDA $41,X                
  BNE $CFB4                
  LDA #$FF                 
  STA $02C0,Y              
  STA $02C4,Y              
  INX                      
  INY                      
  INY                      
  INY                      
  INY                      
  INY                      
  INY                      
  INY                      
  INY                      
  CPX #$02                 
  BMI $CFA8                
  RTS                      
----------------         
--------sub start--------
  LDY #$00                 
  STY $0F                  
  JSR $D004                
  LDA $02C0,Y              
  CMP #$FF                 
  BNE $CFF5                
  LDA $05                  
  STA $02C3,Y              
  CLC                      
  ADC #$08                 
  STA $02C7,Y              
  LDA $06                  
  STA $02C0,Y              
  STA $02C4,Y              
  LDA $C604,X              
  STA $02C1,Y              
  LDA #$D4                 
  STA $02C5,Y              
  LDX $0F                  
  LDA #$03                 
  STA $41,X                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  TXA                      
  PHA                      
  TYA                      
  PHA                      
  LDA $58                  
  BNE $D026                
  LDA $52                  
  ORA #$18                 
  STA $01                  
  LDA $C600,X              
  STA $00                  
  LDA $05                  
  PHA                      
  LDA $06                  
  PHA                      
  JSR $F351                
--------unidentified block--------

----------------         
  JSR $D02E                
  PLA                      
  TAY                      
  PLA                      
  TAX                      
  RTS                      
----------------         
--------sub start--------
  LDA $0505                
  ORA #$01                 
  STA $0505                
  LDA #$F9                 
  STA $00                  
  JMP $F444                
--------sub start--------
  LDA $2E                  
  CMP #$10                 
  BPL $D047                
  LDA #$20                 
  STA $FC                  
  RTS                      
----------------         
--------sub start--------
  LDA $9A                  
  BNE $D08E                
  LDX $53                  
  CPX #$04                 
  BEQ $D05F                
  LDA $5A                  
  BEQ $D0BB                
  DEX                      
  LDA $C1FA,X              
  CMP $59                  
  BEQ $D070                
  RTS                      
----------------         
--------unidentified block--------

----------------         
  RTS                      
----------------         
--------sub start--------
  LDA #$00                 
  STA $FC                  
  LDA #$10                 
  JSR $D9E2                
  BEQ $D134                
  LDA $98                  
  CMP #$FF                 
  BEQ $D12C                
  LDA $98                  
  BNE $D0E0                
  LDA $58                  
  BNE $D0D9                
  LDA #$80                 
  STA $FE                  
  LDA #$40                 
  STA $3A                  
  INC $98                  
  RTS                      
----------------         
  LDA $3A                  
  BEQ $D0F4                
  CMP #$0E                 
  BCC $D134                
  LDA $58                  
  BNE $D0F0                
  LDA #$01                 
  STA $FF                  
  LDA #$00                 
  STA $3A                  
  LDA $0201                
  CMP #$6C                 
  BCS $D0FD                
  LDA #$6C                 
  CLC                      
  ADC #$04                 
  CMP #$7C                 
  BCC $D11B                
  INC $98                  
  LDA $98                  
  CMP #$05                 
  BEQ $D111                
  LDA #$6C                 
  JMP $D11B                
  LDA $58                  
  BEQ $D119                
  LDA #$7D                 
  STA $3A                  
  LDA #$7C                 
  STA $02                  
  JSR $EAFA                
  JSR $EAE6                
  JSR $F091                
  LDA $98                  
  CMP #$05                 
  BNE $D134                
  LDA #$FF                 
  STA $98                  
  LDA $3A                  
  BEQ $D135                
  RTS                      
----------------         
  LDX $52                  
  JSR $CAB9                
  LDA $55                  
  BNE $D147                
  LDA #$01                 
  STA $4E                  
  LDA #$87                 
  STA $43                  
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $52                  
  ASL A                    
  TAX                      
  LDA $15,X                
  AND #$0F                 
  STA $56                  
  BEQ $D185                
  LSR A                    
  LSR A                    
  BNE $D185                
  LDA $56                  
  STA $57                  
  LDA $96                  
  CMP #$01                 
  BNE $D195                
  LDA $15,X                
  AND #$80                 
  BEQ $D195                
  LDA #$04                 
  STA $96                  
  RTS                      
----------------         
--------sub start--------
  LDA $11                  
  AND #$E7                 
  STA PpuMask_2001         
  STA $11                  
  RTS                      
----------------         
--------sub start--------
  LDA $96                  
  CMP #$01                 
  BEQ $D1B7                
  CMP #$02                 
  BEQ $D1BF                
  CMP #$04                 
  BEQ $D1C2                
  CMP #$08                 
  BEQ $D1C5                
  CMP #$0A                 
  BEQ $D1C8                
  RTS                      
----------------         
  JSR $D1CB                
  LDA $96                  
  JMP $D1A6                
  JMP $D37A                
  JMP $D543                
--------unidentified block--------

----------------         
  JMP $D6C2                
--------sub start--------
  LDA $56                  
  CMP #$01                 
  BEQ $D1E1                
  CMP #$02                 
  BEQ $D1E1                
  CMP #$04                 
  BEQ $D1DE                
  CMP #$08                 
  BEQ $D1DE                
  RTS                      
----------------         
  JMP $D287                
  LDA #$DB                 
  STA $0A                  
  LDA #$36                 
  JSR $D9E4                
  BNE $D1EF                
  JMP $D271                
  JSR $D98C                
  BEQ $D1F5                
  RTS                      
----------------         
  LDA $56                  
  CMP #$02                 
  BEQ $D201                
  INC $0203                
  JMP $D204                
  DEC $0203                
  JSR $D2C7                
  STA $5A                  
  LDA $0200                
  JSR $E01B                
  STA $59                  
  JSR $D8E7                
  BEQ $D22F                
  LDX $53                  
  CPX #$01                 
  BNE $D223                
  CLC                      
  ADC $0200                
  STA $0200                
  JSR $D366                
  CMP #$00                 
  BEQ $D22F                
  LDA #$08                 
  STA $96                  
  RTS                      
----------------         
  LDA $9B                  
  BNE $D23A                
  LDA #$01                 
  STA $9B                  
  JMP $D271                
  LDA #$08                 
  STA $FF                  
  LDA #$00                 
  STA $9B                  
  LDA $97                  
  BEQ $D25E                
  CMP #$08                 
  BEQ $D269                
  LDA #$04                 
  STA $97                  
  LDA $85                  
  BEQ $D257                
  LDA #$00                 
  JMP $D259                
  LDA #$08                 
  STA $97                  
  JMP $D271                
  LDA #$04                 
  STA $97                  
  LDA #$00                 
  STA $85                  
  JMP $D271                
  LDA #$04                 
  STA $97                  
  LDA #$01                 
  STA $85                  
  JSR $EAFA                
  LDA $97                  
  STA $02                  
  JSR $EAE6                
  LDA $56                  
  CMP #$02                 
  BEQ $D284                
  JMP $F091                
  JMP $F097                
  JSR $EAFA                
  LDA #$86                 
  STA $02                  
  LDA #$C1                 
  STA $03                  
  JSR $EFFA                
  LDA $53                  
  SEC                      
  SBC #$01                 
  ASL A                    
  TAX                      
  LDA $C47B,X              
  STA $04                  
  LDA $C47C,X              
  STA $05                  
  LDA $C483,X              
  STA $06                  
  LDA $C484,X              
  STA $07                  
  JSR $D8A9                
  BEQ $D2C6                
  LDA $00                  
  SEC                      
  SBC #$04                 
  STA $A1                  
  LDA #$02                 
  STA $96                  
  LDA #$00                 
  STA $5B                  
  STA $5C                  
  RTS                      
----------------         
--------sub start--------
  JSR $EAFA                
  LDA $96                  
  CMP #$04                 
  BEQ $D2D9                
  CMP #$08                 
  BEQ $D2D9                
  LDA #$2C                 
  JMP $D2DB                
  LDA #$4A                 
  JSR $EFF7                
  LDA $53                  
  CMP #$01                 
  BEQ $D2EC                
  SEC                      
  SBC #$01                 
  ASL A                    
  TAX                      
  JMP $D2F9                
  LDA #$1A                 
  JSR $C831                
  JSR $D916                
  STA $0C                  
  JMP $D31F                
--------unidentified block--------

----------------         
  LDA $0C                  
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $53                  
  CMP #$01                 
  BEQ $D36F                
  JMP $D377                
  LDA #$1C                 
  JSR $C831                
  JMP $D8A9                
--------unidentified block--------

----------------         
  LDA $56                  
  CMP #$08                 
  BEQ $D38A                
  CMP #$04                 
  BEQ $D387                
  JMP $D4CB                
--------unidentified block--------

----------------         
  LDA $5A                  
  BEQ $D398                
  JSR $EAFA                
  DEC $01                  
  JSR $D506                
  BNE $D3C9                
  LDA #$24                 
  STA $0A                  
  LDA #$49                 
  JSR $D9E4                
  BNE $D3AB                
  LDA $0200                
  STA $01                  
  JMP $D4CB                
  JSR $D506                
  BEQ $D3E3                
  CMP #$02                 
  BNE $D3B7                
  JMP $D4CB                
  LDA $5B                  
  BEQ $D3CC                
  CLC                      
  ADC #$01                 
  CMP #$10                 
  BEQ $D3CE                
  BCC $D3CE                
  LDA #$10                 
  JMP $D3CE                
  JMP $D4CB                
  LDA #$01                 
  STA $5B                  
  TAX                      
  DEX                      
  LDA $C147,X              
  STA $02                  
  LDA #$00                 
  STA $5A                  
  STA $5C                  
  JSR $D4EA                
  JMP $D409                
  LDA $5C                  
  BEQ $D3F5                
  CLC                      
  ADC #$01                 
  CMP #$06                 
  BEQ $D3F7                
  BCC $D3F7                
  LDA #$01                 
  JMP $D3F7                
  LDA #$02                 
  STA $5C                  
  TAX                      
  DEX                      
  LDA $C159,X              
  STA $02                  
  LDA #$00                 
  STA $5A                  
  STA $5B                  
  JSR $D4EA                
  LDA $A1                  
  STA $00                  
  STA $0203                
  JSR $EAEA                
  LDA #$00                 
  STA $04                  
  LDA $02                  
  CMP #$54                 
  BEQ $D422                
  LDA #$00                 
  JMP $D428                
  LDA #$24                 
  STA $02                  
  LDA #$01                 
  JSR $F0A5                
  JMP $D4CB                
--------unidentified block--------

----------------         
  JSR $D2C7                
  STA $5A                  
  BEQ $D4E9                
  LDA $0200                
  CLC                      
  ADC #$08                 
  JSR $E01B                
  STA $59                  
  LDA #$01                 
  STA $96                  
  LDA #$00                 
  STA $5C                  
  STA $5B                  
  STA $85                  
  RTS                      
----------------         
--------sub start--------
  LDA $0200                
  SEC                      
  SBC #$01                 
  STA $01                  
  JMP $D4FD                
--------unidentified block--------

----------------         
  AND #$06                 
  BNE $D505                
  LDA #$08                 
  STA $FF                  
  RTS                      
----------------         
--------sub start--------
  JSR $EAFA                
  LDA #$2C                 
  JSR $EFF7                
  LDA $53                  
  SEC                      
  SBC #$01                 
  ASL A                    
  TAX                      
  LDA $C48B,X              
  STA $04                  
  LDA $C48C,X              
  STA $05                  
  LDA #$43                 
  STA $06                  
  LDA #$C1                 
  STA $07                  
  JSR $D8A9                
  STA $08                  
  LDA $53                  
  CMP #$01                 
  BNE $D540                
  LDA #$1E                 
  JSR $C831                
  JSR $D8A9                
  BEQ $D540                
  LDA #$02                 
  STA $08                  
  LDA $08                  
  RTS                      
----------------         
  LDA #$FF                 
  JSR $D9E2                
  CMP #$00                 
  BNE $D54D                
  RTS                      
----------------         
  LDA $94                  
  CMP #$F0                 
  BCC $D556                
  JMP $D609                
  JSR $D98C                
  BEQ $D56C                
  LDA $56                  
  CMP #$01                 
  BNE $D566                
  LDA #$02                 
  JMP $D568                
--------unidentified block--------

----------------         
  LDA $0200                
  STA $01                  
  LDA #$00                 
  JSR $EF81                
  LDA $01                  
  STA $0200                
  LDA $56                  
  CMP #$01                 
  BEQ $D588                
  CMP #$02                 
  BEQ $D59D                
  JMP $D5AF                
  LDA $9E                  
  BEQ $D596                
  INC $0203                
  LDA #$00                 
  STA $9E                  
  JMP $D5AF                
  LDA #$01                 
  STA $9E                  
  JMP $D5AF                
--------unidentified block--------

----------------         
  LDA $0203                
  STA $00                  
  JSR $D7FC                
  LDA $94                  
  BEQ $D5DE                
  LDA $01                  
  SEC                      
  SBC #$10                 
  CMP $95                  
  BCC $D5C8                
  LDA #$FF                 
  STA $95                  
  JSR $D2C7                
  STA $5A                  
  BEQ $D5ED                
  LDA $4B                  
  SEC                      
  SBC #$11                 
  STA $0200                
  LDA #$01                 
  STA $5A                  
  JMP $D5F2                
  LDA #$04                 
  STA $FF                  
  LDA #$01                 
  STA $94                  
  LDA $01                  
  STA $95                  
  JMP $D5ED                
  LDA #$28                 
  JMP $F07F                
  JSR $EAFA                
  LDA #$2C                 
  STA $02                  
  JSR $EAE6                
  LDA $57                  
  AND #$03                 
  LSR A                    
  JSR $F0A5                
  LDA #$F0                 
  STA $94                  
  RTS                      
----------------         
  INC $94                  
  LDA $94                  
  CMP #$F4                 
  BNE $D64B                
  LDA $95                  
  CMP #$FF                 
  BEQ $D63E                
  LDA #$04                 
  JSR $F07F                
  LDA #$00                 
  STA $042C                
  STA $94                  
  STA $95                  
  LDA #$01                 
  STA $96                  
  LDA $A0                  
  BEQ $D64B                
  LDA #$01                 
  STA $9F                  
  LDA #$4B                 
  STA $3F                  
  LDA #$0A                 
  STA $96                  
  LDA #$40                 
  STA $FC                  
  RTS                      
----------------         
--------unidentified block--------

----------------         
  RTS                      
----------------         
--------unidentified block--------

----------------         
  LDA $3F                  
  BNE $D6C9                
  JMP $D7BB                
  LDA #$DB                 
  STA $0A                  
  LDA #$36                 
  JSR $D9E4                
  BNE $D6D5                
  RTS                      
----------------         
  JSR $D98C                
  BNE $D6E4                
  LDA $56                  
  CMP #$01                 
  BEQ $D706                
  CMP #$02                 
  BEQ $D70C                
  LDA $A2                  
  ASL A                    
  STA $A2                  
  BEQ $D6EE                
  JMP $D74F                
  LDA #$20                 
  STA $A2                  
  LDA $9F                  
  BEQ $D6FA                
  CMP #$04                 
  BCC $D6FF                
  LDA #$02                 
  JMP $D701                
  LDA #$05                 
  STA $9F                  
  JMP $D74F                
  INC $0203                
  JMP $D70F                
  DEC $0203                
  JSR $D2C7                
  STA $5A                  
  LDA $0200                
  JSR $E01B                
  STA $59                  
  JSR $D8E7                
  BEQ $D73A                
  LDX $53                  
  CPX #$01                 
  BNE $D72E                
  CLC                      
  ADC $0200                
  STA $0200                
  JSR $D366                
  BEQ $D73A                
  LDA #$08                 
  STA $96                  
  JMP $D7BB                
  LDA #$08                 
  STA $FF                  
  LDA $9F                  
  BEQ $D74B                
  CMP #$06                 
  BCS $D74B                
  INC $9F                  
  JMP $D74F                
  LDA #$01                 
  STA $9F                  
  LDX $9F                  
  DEX                      
  LDA $C1A2,X              
  JSR $F07F                
  LDA $9F                  
  LSR A                    
  LSR A                    
  BEQ $D763                
  LDA #$00                 
  JMP $D765                
  LDA #$01                 
  BEQ $D782                
  LDA #$04                 
  CLC                      
  ADC $0203                
  STA $00                  
  LDA $0200                
  SEC                      
  SBC #$0E                 
  STA $01                  
  LDA #$21                 
  STA $03                  
  LDA #$F6                 
  STA $02                  
  JMP $D7A9                
  LDA $57                  
  CMP #$01                 
  BNE $D791                
  LDA #$0E                 
  CLC                      
  ADC $0203                
  JMP $D797                
  LDA $0203                
  SEC                      
  SBC #$0E                 
  STA $00                  
  LDA #$06                 
  CLC                      
  ADC $0200                
  STA $01                  
  LDA #$12                 
  STA $03                  
  LDA #$FA                 
  STA $02                  
  LDA $A0                  
  CMP #$01                 
  BEQ $D7B4                
  LDA #$D8                 
  JMP $D7B6                
  LDA #$D0                 
  STA $04                  
  JMP $F087                
  LDA #$12                 
  STA $03                  
  LDA $A0                  
  CMP #$01                 
  BEQ $D7CF                
  LDA #$00                 
  STA $0452                
  LDA #$D8                 
  JMP $D7D6                
  LDA #$00                 
  STA $0451                
  LDA #$D0                 
  STA $04                  
  JSR $F0A3                
  JSR $D7EE                
  LDA #$01                 
  STA $96                  
  LDA #$00                 
  STA $A0                  
  STA $9F                  
  LDA $0519                
  STA $FC                  
  RTS                      
----------------         
--------sub start--------
  LDA #$19                 
  STA $00                  
  LDA #$3F                 
  STA $01                  
  LDA #$4E                 
  JSR $C815                
  RTS                      
----------------         
--------sub start--------
  LDA $A0                  
  BEQ $D801                
  RTS                      
----------------         
  LDY $53                  
  CPY #$03                 
  BNE $D80A                
  JMP $D8A4                
  LDA $0203                
  CPY #$01                 
  BEQ $D81A                
  CMP #$88                 
  BEQ $D823                
  BCC $D823                
  JMP $D8A4                
  CMP #$28                 
  BEQ $D823                
  BCC $D823                
  JMP $D8A4                
  LDA $0200                
  CLC                      
  ADC #$08                 
  JSR $E01B                
  STA $59                  
  LDA $53                  
  SEC                      
  SBC #$01                 
  ASL A                    
  TAX                      
  LDA $59                  
  CMP $C1A8,X              
  BEQ $D845                
  INX                      
  CMP $C1A8,X              
  BEQ $D845                
  JMP $D8A4                
  TXA                      
  AND #$01                 
  BEQ $D863                
  LDA $0452                
  BNE $D852                
  JMP $D8A4                
--------unidentified block--------

----------------         
  LDA $0451                
  BNE $D86B                
  JMP $D8A4                
  LDA #$01                 
  STA $A0                  
  LDA $02D0                
  STA $01                  
  LDA $02D3                
  STA $00                  
  LDA #$2E                 
  JSR $EFF7                
  JSR $EAFA                
  LDA #$30                 
  JSR $C847                
  JSR $EFFE                
  BEQ $D8A4                
  LDA $FC                  
  STA $0519                
  LDA $53                  
  CMP #$04                 
  BNE $D8A3                
  LDA #$19                 
  STA $00                  
  LDA #$3F                 
  STA $01                  
  LDA #$46                 
  JSR $C815                
  RTS                      
----------------         
  LDA #$00                 
  STA $A0                  
  RTS                      
----------------         
--------sub start--------
  LDA #$F3                 
  STA $0B                  
  LDA #$00                 
  STA $86                  
  LDY #$00                 
  LDA ($04),Y              
  STA $00                  
  INY                      
  LDA ($04),Y              
  STA $01                  
  INY                      
  LDA ($04),Y              
  CLC                      
  ADC $06                  
  STA $02                  
  LDA $07                  
  ADC #$00                 
  STA $03                  
  STY $86                  
  JSR $F002                
  BNE $D8DD                
  LDY $86                  
  INY                      
  LDA ($04),Y              
  CMP #$FE                 
  BEQ $D8E2                
  JMP $D8B5                
  LDA #$01                 
  JMP $D8E4                
  LDA #$00                 
  STA $0C                  
  RTS                      
----------------         
--------sub start--------
  LDA $5A                  
  BNE $D913                
  LDA $59                  
  BEQ $D913                
  AND #$01                 
  BNE $D900                
  LDA $56                  
  CMP #$01                 
  BEQ $D910                
  CMP #$02                 
  BEQ $D90D                
  JMP $D913                
  LDA $56                  
  CMP #$01                 
  BEQ $D90D                
  CMP #$02                 
  BEQ $D910                
  JMP $D913                
  LDA #$FF                 
  RTS                      
----------------         
  LDA #$01                 
  RTS                      
----------------         
  LDA #$00                 
  RTS                      
----------------         
--------sub start--------
  LDA $0200                
  CLC                      
  ADC #$08                 
  JSR $E01B                
  STA $59                  
  CMP #$01                 
  BEQ $D934                
  LDX #$02                 
  LDA #$0C                 
  CPX $59                  
  BEQ $D937                
  CLC                      
  ADC #$06                 
  INX                      
  JMP $D929                
  SEC                      
  SBC #$01                 
  TAX                      
  LDA #$00                 
  STA $86                  
  LDA $C08C,X              
  STA $00                  
  INX                      
  LDA $C08C,X              
  STA $01                  
  INX                      
  LDA $C08C,X              
  CLC                      
  ADC $06                  
  STA $02                  
  LDA $07                  
  STA $03                  
  INX                      
  LDA $C08C,X              
  STA $08                  
  INX                      
  LDA $C08C,X              
  STA $09                  
  JSR $EFFE                
  BNE $D987                
  LDA $00                  
  CLC                      
  ADC $08                  
  STA $00                  
  DEC $01                  
  INC $86                  
  LDA $09                  
  CMP $86                  
  BNE $D960                
  INX                      
  LDA $C08C,X              
  CMP #$FE                 
  BEQ $D982                
  INX                      
  JMP $D938                
  LDA #$00                 
  JMP $D989                
  LDA #$01                 
  STA $5A                  
  RTS                      
----------------         
--------sub start--------
  LDA $56                  
  CMP #$01                 
  BEQ $D999                
  CMP #$02                 
  BEQ $D9AB                
  JMP $D9DF                
  LDA $53                  
  ASL A                    
  TAX                      
  DEX                      
  LDA $C1B4,X              
  CMP $0203                
  BEQ $D9DC                
  BCC $D9DC                
  JMP $D9DF                
  LDA $53                  
  ASL A                    
  TAX                      
  DEX                      
  DEX                      
  LDA $C1B4,X              
  CMP $0203                
  BCS $D9DC                
  LDA $53                  
  CMP #$04                 
  BEQ $D9DF                
  LDX $59                  
  CMP #$03                 
  BEQ $D9CC                
  CPX #$06                 
  BNE $D9DF                
  JMP $D9D0                
--------unidentified block--------

----------------         
  LDA #$00                 
  RTS                      
----------------         
--------sub start--------
  STA $0A                  
--------sub start--------
  STA $0B                  
  INC $88                  
  LDA $88                  
  CMP #$0F                 
  BCS $D9F1                
  JMP $D9F5                
  LDA #$00                 
  STA $88                  
  CMP #$08                 
  BCS $DA02                
  TAX                      
  LDA $C1BC,X              
  AND $0A                  
  JMP $DA0B                
  SEC                      
  SBC #$08                 
  TAX                      
  LDA $C1BC,X              
  AND $0B                  
  BEQ $DA0F                
  LDA #$01                 
  STA $BE                  
  RTS                      
----------------         
--------sub start--------
  JSR $E17B                
  LDA #$00                 
  STA $5D                  
  JSR $EFE4                
  LDA $0200,X              
  CMP #$FF                 
  BNE $DA39                
  LDA $36                  
  BNE $DA3C                
  LDA #$80                 
  LDX $5D                  
  STA $5E,X                
  LDA #$10                 
  STA $37                  
  JSR $EB10                
  LDA $C443,X              
  STA $36                  
  JSR $DA4B                
  LDA $5D                  
  CLC                      
  ADC #$01                 
  STA $5D                  
  CMP #$09                 
  BEQ $DA4A                
  JMP $DA19                
  RTS                      
----------------         
--------sub start--------
  LDX $5D                  
  LDA $5E,X                
  CMP #$80                 
  BEQ $DA7C                
  CMP #$81                 
  BEQ $DA7F                
  CMP #$01                 
  BEQ $DA82                
  CMP #$02                 
  BEQ $DA85                
  CMP #$C0                 
  BEQ $DA88                
  CMP #$C1                 
  BEQ $DA88                
  CMP #$C2                 
  BEQ $DA88                
  CMP #$08                 
  BEQ $DA8E                
  CMP #$10                 
  BEQ $DA91                
  CMP #$20                 
  BEQ $DA94                
  CMP #$40                 
  BEQ $DA97                
  RTS                      
----------------         
  JMP $DA9B                
  JMP $DB01                
  JMP $DB2D                
  JMP $DC31                
  LDA $0421,X              
  JMP $DD8C                
  JMP $DC6A                
  JMP $DCD1                
  JMP $DD33                
--------unidentified block--------

----------------         
  JSR $EFE4                
  LDA #$30                 
  STA $00                  
  LDA #$30                 
  STA $01                  
  LDA #$90                 
  STA $02                  
  STX $04                  
  JSR $EAF4                
  LDA $37                  
  BNE $DB00                
  LDA #$81                 
  LDX $5D                  
  STA $5E,X                
  LDA #$00                 
  STA $8A,X                
  LDA $AD                  
  BEQ $DAC4                
  JMP $DAD6                
  LDA $5D                  
  BNE $DB00                
  LDA #$C0                 
  LDX $5D                  
  STA $5E,X                
  LDA #$01                 
  STA $0421,X              
  JMP $DAF8                
  LDA $43                  
  BNE $DB00                
  LDA $5D                  
  BNE $DB00                
  LDA #$C0                 
  LDX $5D                  
  STA $5E,X                
  LDA $0421,X              
  CMP #$01                 
  BNE $DAF3                
  LDA #$03                 
  STA $0421,X              
  JMP $DAF8                
--------unidentified block--------

----------------         
  JSR $EB10                
  LDA $C44D,X              
  STA $43                  
  RTS                      
----------------         
  LDA #$55                 
  JSR $DFE5                
  BNE $DB22                
  JSR $EFE4                
  LDA #$4D                 
  STA $00                  
  LDA #$32                 
  STA $01                  
  LDA #$84                 
  STA $02                  
  STX $04                  
  JSR $EAF4                
  INC $0515                
  JMP $DB2C                
  LDX $5D                  
  LDA #$01                 
  STA $5E,X                
  LDA #$84                 
  STA $72,X                
  RTS                      
----------------         
  LDA #$FF                 
  JSR $DFE5                
  BNE $DB35                
  RTS                      
----------------         
  JSR $EFE4                
  PHA                      
  JSR $EB05                
  LDA $01                  
  JSR $E01B                
  LDY $5D                  
  STA $0068,Y              
  AND #$01                 
  BNE $DB4F                
  INC $00                  
  JMP $DB51                
  DEC $00                  
  LDA $00                  
  JSR $E063                
  STA $7D                  
  JSR $E04D                
  CLC                      
  ADC $01                  
  STA $01                  
  JSR $DBEF                
  LDX $5D                  
  LDA $72,X                
  JSR $EAED                
  PLA                      
  TAX                      
  JSR $F08F                
  LDA $00                  
  JSR $E0BF                
  BEQ $DBAD                
  JSR $EB10                
  LDA $C448,X              
  AND $19                  
  BNE $DBAD                
  LDX $5D                  
  LDA $68,X                
  TAX                      
  DEX                      
  LDA $7E,X                
  CMP #$04                 
  BCS $DBAD                
  LDA $96                  
  CMP #$02                 
  BNE $DBA4                
  LDX $04                  
  LDA $0200,X              
  CMP $0200                
  BCS $DBA4                
  CLC                      
  ADC #$0F                 
  CMP $0200                
  BCS $DBAD                
  LDA #$02                 
  LDX $5D                  
  STA $5E,X                
  DEC $68,X                
  RTS                      
----------------         
  LDA $00                  
  JSR $E09D                
  BEQ $DBB7                
  JMP $DBE8                
  JSR $DF41                
  LDX $5D                  
  LDA $68,X                
  CMP #$01                 
  BNE $DBEE                
  JSR $DFC4                
  LDA $00                  
  CMP #$20                 
  BEQ $DBCE                
  BCC $DBCE                
  RTS                      
----------------         
  LDA #$03                 
  STA $02                  
  LDA #$04                 
  STA $03                  
  JSR $F09D                
  LDA #$01                 
  STA $AD                  
  LDA #$00                 
  LDX $5D                  
  STA $68,X                
  LDA #$80                 
  STA $FE                  
  RTS                      
----------------         
  LDX $5D                  
  LDA #$08                 
  STA $5E,X                
  RTS                      
----------------         
--------sub start--------
  LDX $5D                  
  INC $040D,X              
  LDA $040D,X              
  CMP #$06                 
  BCS $DBFC                
  RTS                      
----------------         
  LDA #$00                 
  STA $040D,X              
  LDA $68,X                
  AND #$01                 
  BEQ $DC1C                
  LDA $72,X                
  CLC                      
  ADC #$04                 
  CMP #$80                 
  BCC $DC17                
  CMP #$90                 
  BCS $DC17                
  JMP $DC2E                
  LDA #$80                 
  JMP $DC2E                
  LDA $72,X                
  SEC                      
  SBC #$04                 
  CMP #$80                 
  BCC $DC2C                
  CMP #$90                 
  BCS $DC2C                
  JMP $DC2E                
  LDA #$8C                 
  STA $72,X                
  RTS                      
----------------         
  LDA #$55                 
  JSR $DFE5                
  BEQ $DC69                
  JSR $EFE4                
  STX $04                  
  JSR $EB05                
  INC $01                  
  LDY $5D                  
  LDA $0072,Y              
  CMP #$90                 
  BNE $DC50                
  LDA #$94                 
  JMP $DC52                
  LDA #$90                 
  STA $02                  
  LDX $5D                  
  STA $72,X                
  JSR $EAF4                
  LDA $01                  
  LDX $5D                  
  CMP $A3,X                
  BNE $DC69                
  LDX $5D                  
  LDA #$01                 
  STA $5E,X                
  RTS                      
----------------         
  LDA #$FF                 
  JSR $DFE5                
  BNE $DC72                
  RTS                      
----------------         
  JSR $EFE4                
  STX $04                  
  JSR $EB05                
  INC $01                  
  LDA $01                  
  AND #$01                 
  BEQ $DC91                
  LDX $5D                  
  LDA $68,X                
  AND #$01                 
  BEQ $DC8F                
  DEC $00                  
  JMP $DC91                
  INC $00                  
  JSR $DBEF                
  LDX $5D                  
  LDA $72,X                
  STA $02                  
  JSR $EAF4                
  LDA #$32                 
  JSR $C853                
  LDA $01                  
  JSR $E123                
  BEQ $DCD0                
  LDX $5D                  
  LDA #$10                 
  STA $5E,X                
  JSR $E141                
  BEQ $DCBD                
  LDA $19                  
  AND #$01                 
  BEQ $DCBD                
  JMP $DCCA                
  LDX $5D                  
  LDA $68,X                
  TAX                      
  DEX                      
  LDA $7E,X                
  CMP #$04                 
  BCS $DCCA                
  RTS                      
----------------         
  LDX $5D                  
  LDA #$20                 
  STA $5E,X                
  RTS                      
----------------         
  LDA #$77                 
  JSR $DFE5                
  BNE $DCD9                
  RTS                      
----------------         
  JSR $EFE4                
  STX $04                  
  JSR $EB05                
  LDA $01                  
  JSR $E01B                
  LDX $5D                  
  STA $68,X                
  AND #$01                 
  BNE $DD01                
  INC $00                  
  LDA $00                  
  LDX #$00                 
  CMP $C3FC,X              
  BEQ $DD14                
  INX                      
  CPX #$0B                 
  BEQ $DD26                
  JMP $DCF4                
  DEC $00                  
  LDA $00                  
  LDX #$00                 
  CMP $C412,X              
  BEQ $DD14                
  INX                      
  CPX #$0B                 
  BEQ $DD26                
  JMP $DD07                
  LDA $01                  
  CLC                      
  ADC $C407,X              
  STA $01                  
  CPX #$0A                 
  BNE $DD26                
  LDX $5D                  
  LDA #$01                 
  STA $5E,X                
  JSR $DBEF                
  LDX $5D                  
  LDA $72,X                
  STA $02                  
  JSR $EAF4                
  RTS                      
----------------         
  LDA #$55                 
  JSR $DFE5                
  BNE $DD3B                
  RTS                      
----------------         
  JSR $EFE4                
  STX $04                  
  JSR $EB05                
  LDA $01                  
  JSR $E01B                
  LDX $5D                  
  STA $68,X                
  AND #$01                 
  BNE $DD61                
  DEC $00                  
  LDA $01                  
  CMP #$14                 
  BNE $DD5A                
  DEC $01                  
  LDA $00                  
  BNE $DD74                
  JMP $DD80                
  INC $00                  
  LDA $01                  
  CMP #$EC                 
  BNE $DD6B                
  DEC $01                  
  LDA $00                  
  CMP #$F4                 
  BNE $DD74                
  JMP $DD80                
  JSR $DBEF                
  LDX $5D                  
  LDA $72,X                
  STA $02                  
  JMP $EAF4                
  LDA #$22                 
  JSR $F0A1                
  LDA #$00                 
  LDX $5D                  
  STA $68,X                
  RTS                      
----------------         
  STA $07                  
  LDX $5D                  
  LDA $5E,X                
  CMP #$C2                 
  BNE $DD99                
  JMP $DE83                
  CMP #$C1                 
  BEQ $DDD8                
  LDA $07                  
  CMP #$02                 
  BEQ $DDAC                
  CMP #$03                 
  BEQ $DDB1                
  LDA #$34                 
  JMP $DDB3                
--------unidentified block--------

----------------         
  LDA #$38                 
  JSR $C853                
  JSR $EFE4                
  STX $04                  
  LDA $0200,X              
  JSR $E123                
  LDY $0A                  
  CPY #$04                 
  BNE $DDCA                
  JMP $DE74                
  CMP #$00                 
  BEQ $DDD8                
  LDX $5D                  
  LDA #$01                 
  STA $8A,X                
  LDA #$C1                 
  STA $5E,X                
  JSR $EFE4                
  STX $04                  
  LDX $5D                  
  LDA $5E,X                
  CMP #$C1                 
  BNE $DE14                
  LDA #$20                 
  JSR $DFE5                
  BNE $DDF6                
  LDX $04                  
  LDA $0200,X              
  STA $01                  
  JMP $DE28                
  LDX $5D                  
  LDA #$C0                 
  STA $5E,X                
  LDA $07                  
  CMP #$03                 
  BNE $DE11                
  LDA $0417,X              
  BEQ $DE0C                
  LDA #$00                 
  JMP $DE0E                
  LDA #$01                 
  STA $0417,X              
  JMP $DE1B                
  LDA #$FF                 
  JSR $DFE5                
  BEQ $DE86                
  LDX $04                  
  LDA #$01                 
  CLC                      
  ADC $0200,X              
  STA $01                  
  JSR $DE87                
  INX                      
  INX                      
  INX                      
  LDA $07                  
  CMP #$02                 
  BNE $DE37                
  INC $0200,X              
  JMP $DE57                
  CMP #$03                 
  BNE $DE57                
  LDA $01                  
  AND #$01                 
  BEQ $DE57                
  LDY $5D                  
  LDA $0417,Y              
  BNE $DE51                
  INC $0200,X              
  INC $0200,X              
  JMP $DE57                
  DEC $0200,X              
  DEC $0200,X              
  LDA $0200,X              
  STA $00                  
  LDX $5D                  
  LDA $72,X                
  CMP #$90                 
  BNE $DE69                
  LDA #$94                 
  JMP $DE6B                
  LDA #$90                 
  STA $02                  
  LDX $5D                  
  STA $72,X                
  JMP $EAF4                
  LDA #$C2                 
  LDX $5D                  
  STA $5E,X                
  LDX $04                  
  LDA $0203,X              
  STA $042B                
  RTS                      
----------------         
  JSR $DEA6                
  RTS                      
----------------         
--------sub start--------
  LDA $07                  
  CMP #$01                 
  BNE $DEA5                
  LDY #$00                 
  LDA $01                  
  CMP $C41D,Y              
  BCC $DEA0                
  CMP $C420,Y              
  BCS $DEA0                
  INC $01                  
  JMP $DEA5                
  INY                      
  CPY #$03                 
  BNE $DE91                
  RTS                      
----------------         
--------sub start--------
  JSR $EFE4                
  STX $04                  
  JSR $EB05                
  DEC $00                  
  LDA $042B                
  SEC                      
  SBC #$01                 
  CMP $00                  
  BEQ $DEE9                
  SEC                      
  SBC #$01                 
  CMP $00                  
  BEQ $DEE9                
  SEC                      
  SBC #$01                 
  CMP $00                  
  BEQ $DEF3                
  SEC                      
  SBC #$08                 
  CMP $00                  
  BEQ $DEEE                
  SEC                      
  SBC #$01                 
  CMP $00                  
  BEQ $DEEE                
  SEC                      
  SBC #$01                 
  CMP $00                  
  BNE $DEFC                
  LDA #$01                 
  LDX $5D                  
  STA $5E,X                
  LDA #$00                 
  STA $0417,X              
  RTS                      
----------------         
  DEC $01                  
  JMP $DEFC                
  INC $01                  
  JMP $DEFC                
  LDX $5D                  
  LDA $0421,X              
  CMP #$01                 
  BEQ $DEDD                
  LDA #$84                 
  LDX $5D                  
  STA $72,X                
  STA $02                  
  JSR $EAF4                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $C0                  
  BEQ $DF46                
  RTS                      
----------------         
  LDA $96                  
  CMP #$0A                 
  BEQ $DF4D                
  RTS                      
----------------         
  LDA $59                  
  CMP #$03                 
  BEQ $DF56                
  JMP $DF73                
  LDX #$03                 
  LDA $7E,X                
  CMP #$05                 
  BCS $DF5F                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDX $5D                  
  LDA $68,X                
  CMP #$01                 
  BNE $DFE4                
  JSR $EFE4                
  LDA $0203,X              
  CMP #$30                 
  BCS $DFE4                
  LDA #$23                 
  STA $0202,X              
  STA $0206,X              
  STA $020A,X              
  STA $020E,X              
  RTS                      
----------------         
--------sub start--------
  STA $0A                  
  STA $0B                  
  LDX $5D                  
  INC $8A,X                
  LDA $8A,X                
  BMI $DFF8                
  CMP #$10                 
  BCS $DFF8                
  JMP $DFFC                
  LDA #$00                 
  STA $8A,X                
  CMP #$08                 
  BCS $E009                
  TAX                      
  LDA $C1BC,X              
  AND $0A                  
  JMP $E012                
  SEC                      
  SBC #$08                 
  TAX                      
  LDA $C1BC,X              
  AND $0B                  
  BEQ $E016                
  LDA #$01                 
  STA $0C                  
  LDA $0C                  
  RTS                      
----------------         
--------sub start--------
  STA $0A                  
  LDA $53                  
  SEC                      
  SBC #$01                 
  ASL A                    
  TAX                      
  LDA $C493,X              
  STA $08                  
  LDA $C494,X              
  STA $09                  
  LDY #$00                 
  LDA #$01                 
  STA $0B                  
  LDA ($08),Y              
  CMP #$FF                 
  BEQ $E046                
  CMP $0A                  
  BEQ $E04A                
  BCC $E04A                
  INC $0B                  
  INY                      
  JMP $E034                
--------unidentified block--------

----------------         
  LDA $0B                  
  RTS                      
----------------         
--------sub start--------
  LDX $5D                  
  LDA $5E,X                
  CMP #$01                 
  BNE $E05E                
  LDA $7D                  
  BNE $E05E                
  LDA #$01                 
  JMP $E060                
  LDA #$00                 
  STA $0C                  
  RTS                      
----------------         
--------sub start--------
  STA $0C                  
  LDX $5D                  
  LDA $68,X                
  CMP #$01                 
  BEQ $E082                
  CMP #$06                 
  BEQ $E082                
  LDX #$00                 
  LDA $C1C4,X              
  CMP $0C                  
  BEQ $E093                
  INX                      
  CPX #$09                 
  BEQ $E098                
  JMP $E073                
  LDX #$04                 
  LDA $C1C4,X              
  CMP $0C                  
  BEQ $E093                
  INX                      
  CPX #$09                 
  BEQ $E098                
  JMP $E084                
  LDA #$00                 
  JMP $E09A                
  LDA #$01                 
  STA $0B                  
  RTS                      
----------------         
--------sub start--------
  STA $0C                  
  LDX $5D                  
  LDA $68,X                
  AND #$01                 
  BEQ $E0AC                
  LDX #$00                 
  JMP $E0AE                
  LDX #$01                 
  LDA $C1CD,X              
  CMP $0C                  
  BEQ $E0BA                
  LDA #$00                 
  JMP $E0BC                
  LDA #$01                 
  STA $0B                  
  RTS                      
----------------         
--------sub start--------
  STA $0C                  
  LDX $5D                  
  LDA $68,X                
  CMP #$02                 
  BEQ $E0DC                
  CMP #$03                 
  BEQ $E0DC                
  CMP #$04                 
  BEQ $E0E2                
  CMP #$05                 
  BEQ $E0EE                
  CMP #$06                 
  BEQ $E0FA                
  JMP $E0FD                
  JSR $E102                
  JMP $E0FD                
  JSR $E102                
  LDY #$89                 
  CMP #$C4                 
  BEQ $E11A                
  JMP $E0FD                
  JSR $E102                
  LDY #$71                 
  CMP #$B4                 
  BEQ $E11A                
  JMP $E0FD                
  JSR $E102                
  LDA #$00                 
  JMP $E120                
--------sub start--------
  TAX                      
  DEX                      
  DEX                      
  LDA $0C                  
  LDY $C172,X              
  CMP $C177,X              
  BEQ $E118                
  LDY $C17C,X              
  CMP $C181,X              
  BEQ $E118                
  RTS                      
----------------         
  PLA                      
  PLA                      
  LDX $5D                  
  STY $A3,X                
  LDA #$01                 
  STA $0C                  
  RTS                      
----------------         
--------sub start--------
  STA $0B                  
  LDY #$00                 
  LDA ($08),Y              
  CMP #$FE                 
  BEQ $E13A                
  CMP $0B                  
  BEQ $E135                
  INY                      
  JMP $E127                
  LDA #$01                 
  JMP $E13C                
  LDA #$00                 
  STA $0C                  
  STY $0A                  
  RTS                      
----------------         
--------sub start--------
  LDX $5D                  
  LDA $68,X                
  SEC                      
  SBC $59                  
  BEQ $E14F                
  BMI $E14F                
  JMP $E154                
  LDA #$01                 
  JMP $E156                
  LDA #$00                 
  STA $0B                  
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA #$00                 
  LDY #$06                 
  STA $007E,Y              
  DEY                      
  BPL $E17F                
  LDY #$00                 
  LDA $0068,Y              
  BEQ $E194                
  TAX                      
  LDA $7E,X                
  CLC                      
  ADC #$01                 
  STA $7E,X                
  CPY #$09                 
  BEQ $E19C                
  INY                      
  JMP $E187                
  LDX $59                  
  CPX #$07                 
  BEQ $E1AE                
  INC $7E,X                
  LDA $96                  
  CMP #$0A                 
  BNE $E1AE                
  LDX $59                  
  INC $7E,X                
  RTS                      
----------------         
--------sub start--------
  LDA $AD                  
  BNE $E1B4                
  RTS                      
----------------         
  CMP #$01                 
  BNE $E1D4                
  LDA #$20                 
  STA $00                  
  LDA #$C0                 
  STA $01                  
  LDA #$FC                 
  STA $02                  
  LDA #$12                 
  STA $03                  
  LDA #$E0                 
  JSR $F08F                
  LDA #$02                 
  STA $AD                  
  JMP $E1F5                
  LDA $38                  
  BNE $E1F9                
  LDA #$03                 
  STA $AD                  
  LDX #$E1                 
  LDA $0200,X              
  CMP #$FC                 
  BEQ $E1EA                
  LDA #$FC                 
  JMP $E1EC                
  LDA #$FE                 
  STA $0200,X              
  CLC                      
  ADC #$01                 
  STA $0204,X              
  LDA #$10                 
  STA $38                  
  RTS                      
----------------         
--------sub start--------
  LDA #$00                 
  STA $AE                  
  JSR $EFEC                
  LDA $0200,X              
  CMP #$FF                 
  BNE $E23A                
  LDA $53                  
  CMP #$01                 
  BEQ $E215                
  CMP #$04                 
  BEQ $E228                
  JMP $E23A                
  LDA $40                  
  BNE $E23D                
  LDA $AD                  
  BEQ $E23D                
  CMP #$02                 
  BNE $E23D                
  LDA #$19                 
  STA $40                  
  JMP $E234                
--------unidentified block--------

----------------         
  LDA #$06                 
  LDX $AE                  
  STA $AF,X                
  JSR $E265                
  LDX $53                  
  DEX                      
  INC $AE                  
  LDA $AE                  
  CMP $C1F6,X              
  BEQ $E24C                
  JMP $E1FE                
  LDA $53                  
  CMP #$03                 
  BEQ $E264                
  LDA $3B                  
  BNE $E264                
  LDA #$00                 
  STA $D2                  
  STA $D3                  
  STA $D4                  
  STA $D5                  
  LDA #$BC                 
  STA $3B                  
  RTS                      
----------------         
--------sub start--------
  LDX $AE                  
  LDA $AF,X                
  AND #$0F                 
  BEQ $E2A7                
  CMP #$06                 
  BEQ $E2A4                
  CMP #$08                 
  BEQ $E2A4                
  CMP #$01                 
  BEQ $E2AA                
  CMP #$02                 
  BEQ $E2AF                
  CMP #$03                 
  BEQ $E2B6                
  LDA $53                  
  CMP #$03                 
  BEQ $E28D                
  JSR $E2CB                
  JMP $E295                
--------unidentified block--------

----------------         
  LDA $AF,X                
  CMP #$01                 
  BEQ $E29F                
  CMP #$02                 
  BNE $E2A1                
  STA $B3,X                
  JMP $E269                
  JMP $E54D                
  JMP $E30E                
  LDA #$00                 
  JMP $E2B1                
  LDA #$01                 
  STA $99                  
  JMP $E37D                
  LDA $53                  
  CMP #$01                 
  BNE $E2C8                
  JSR $E63B                
  LDX $AE                  
  LDA $AF,X                
  BNE $E2C8                
  JMP $E2A7                
  JMP $E430                
--------sub start--------
  LDX $AE                  
  LDA $D2,X                
  BNE $E2F2                
  LDA #$01                 
  STA $D2,X                
  LDA $AE                  
  CLC                      
  ADC #$01                 
  ASL A                    
  ASL A                    
  ASL A                    
  ASL A                    
  TAY                      
  LDA $0203,Y              
  CMP $0203                
  BCS $E2EE                
  LDA #$01                 
  STA $EC,X                
  JMP $E2F2                
  LDA #$02                 
  STA $EC,X                
  LDA $19,X                
  AND #$07                 
  STA $AF,X                
  TAY                      
  CMP #$04                 
  BCS $E300                
  JMP $E30B                
  LDY $EC,X                
  CMP #$07                 
  BCS $E309                
  JMP $E30B                
  LDY #$03                 
  STY $AF,X                
  RTS                      
----------------         
  LDA #$55                 
  STA $0A                  
  STA $0B                  
  JSR $E81B                
  BNE $E31A                
  RTS                      
----------------         
  JSR $EFEC                
  STX $04                  
  JSR $EB05                
  LDX $AE                  
  LDA $AF,X                
  CMP #$20                 
  BNE $E32F                
  LDA #$FF                 
  STA $AF,X                
  RTS                      
----------------         
  CMP #$10                 
  BEQ $E338                
  DEC $01                  
  JMP $E33A                
  INC $01                  
  LDA $04                  
  TAY                      
  INY                      
  LDA $0200,Y              
  LDX $53                  
  CPX #$04                 
  BEQ $E355                
  CMP #$9C                 
  BEQ $E350                
  LDA #$9C                 
  JMP $E360                
  LDA #$98                 
  JMP $E360                
--------unidentified block--------

----------------         
  JSR $EAED                
  LDX $AE                  
  LDA $B3,X                
  LSR A                    
  JSR $F0A5                
  LDX $AE                  
  LDA $AF,X                
  CMP #$10                 
  BEQ $E378                
  LDA #$10                 
  JMP $E37A                
  LDA #$20                 
  STA $AF,X                
  RTS                      
----------------         
  LDA #$55                 
  STA $0A                  
  STA $0B                  
  JSR $E81B                
  BNE $E389                
  RTS                      
----------------         
  JSR $EFEC                
  STX $04                  
  JSR $EB05                
  LDA $99                  
  BNE $E39A                
  INC $00                  
  JMP $E39C                
  DEC $00                  
  LDA $00                  
  AND #$0F                 
  CMP #$04                 
  BEQ $E3AB                
  CMP #$0C                 
  BEQ $E3AB                
  JMP $E3B0                
  INC $01                  
  JMP $E3C4                
  LDX $99                  
  CMP $C3E2,X              
  BEQ $E3BF                
  CMP $C3E4,X              
  BEQ $E3BF                
  JMP $E3C4                
  DEC $01                  
  JMP $E3D5                
  CMP #$04                 
  BEQ $E3CF                
  CMP #$0C                 
  BEQ $E3CF                
  JMP $E3D5                
  LDX $AE                  
  LDA #$FF                 
  STA $AF,X                
  LDY $99                  
  JSR $E6BA                
  BNE $E3E3                
  LDA #$00                 
  LDX $AE                  
  STA $AF,X                
  RTS                      
----------------         
  LDA $99                  
  BEQ $E402                
  LDA $00                  
  CMP #$0C                 
  BEQ $E3F2                
  BCC $E3FB                
  JMP $E402                
--------unidentified block--------

----------------         
  LDA $04                  
  TAY                      
  INY                      
  LDA $0200,Y              
  LDX $53                  
  CPX #$04                 
  BEQ $E41D                
  CMP #$9C                 
  BCS $E418                
  LDA #$9C                 
  JMP $E428                
  LDA #$98                 
  JMP $E428                
--------unidentified block--------

----------------         
  JSR $EAED                
  LDA $99                  
  JMP $F0A5                
  LDX $AE                  
  LDA $AF,X                
  LSR A                    
  LSR A                    
  LSR A                    
  TAX                      
  LDA $53                  
  CMP #$04                 
  BEQ $E44B                
  LDA $C3F4,X              
  STA $0A                  
  LDA $C3F5,X              
  STA $0B                  
  JMP $E460                
--------unidentified block--------

----------------         
  JSR $E81B                
  BNE $E466                
  RTS                      
----------------         
  JSR $EFEC                
  STX $04                  
  JSR $EB05                
  LDX $AE                  
  LDA $E8,X                
  BEQ $E482                
  CMP #$03                 
  BEQ $E47B                
  JMP $E482                
  LDA #$00                 
  STA $E8,X                
  JMP $E48F                
  LDA $01                  
  AND #$03                 
  BNE $E48F                
  LDA #$01                 
  INC $E8,X                
  JMP $E521                
  LDA $53                  
  CMP #$01                 
  BEQ $E4CA                
  JSR $E7B8                
--------unidentified block--------

----------------         
  LDX $AE                  
  LDA $AF,X                
  CMP #$13                 
  BEQ $E4D5                
  JMP $E4EB                
  INC $01                  
  LDA $AE                  
  ASL A                    
  TAX                      
  INX                      
  LDA $B9,X                
  CMP $01                  
  BNE $E4E8                
  LDA #$01                 
  LDX $AE                  
  STA $AF,X                
  JMP $E521                
  DEC $01                  
  LDX $AE                  
  CPX #$00                 
  BNE $E50E                
  LDX $AE                  
  LDA $E0,X                
  CMP #$02                 
  BEQ $E50E                
  LDA $AE                  
  ASL A                    
  TAX                      
  LDA $B9,X                
  CMP $01                  
  BNE $E521                
  LDA #$02                 
  LDX $AE                  
  STA $AF,X                
  JMP $E521                
  LDA $AE                  
  ASL A                    
  TAX                      
  LDA $B9,X                
  CLC                      
  ADC #$0D                 
  CMP $01                  
  BNE $E521                
  LDA #$13                 
  LDX $AE                  
  STA $AF,X                
  LDA $04                  
  TAY                      
  INY                      
  LDA $0200,Y              
  LDX $53                  
  CPX #$04                 
  BEQ $E53C                
  CMP #$9C                 
  BCS $E537                
  LDA #$9C                 
  JMP $E547                
  LDA #$98                 
  JMP $E547                
--------unidentified block--------

----------------         
  JSR $EAED                
  JMP $F097                
  LDX $AE                  
  LDA $AF,X                
  CMP #$06                 
  BEQ $E55D                
  CMP #$08                 
  BEQ $E55A                
  RTS                      
----------------         
  JMP $E5B4                
  LDA $53                  
  CMP #$01                 
  BEQ $E568                
  CMP #$04                 
  BEQ $E579                
  RTS                      
----------------         
  LDA #$20                 
  STA $00                  
  LDA #$B8                 
  STA $01                  
  LDX $AE                  
  LDA #$08                 
  STA $AF,X                
  JMP $E5A7                
--------unidentified block--------

----------------         
  LDA #$98                 
  JSR $EAED                
  JSR $EFEC                
  STA $04                  
  JMP $F091                
  JSR $EFEC                
  STX $04                  
  JSR $EB05                
  LDA $0201,X              
  JSR $EAED                
  LDA $53                  
  CMP #$01                 
  BEQ $E5C9                
  RTS                      
----------------         
  INC $00                  
  LDA $00                  
  CMP #$2C                 
  BEQ $E5D3                
  BCC $E5FA                
  INC $01                  
  LDA $01                  
  CMP #$C5                 
  BNE $E5FA                
  LDA #$00                 
  LDX $AE                  
  STA $AF,X                
  DEC $00                  
  LDA $00                  
  CMP #$68                 
  BCS $E5EE                
  INC $01                  
  JMP $E5F0                
--------unidentified block--------

----------------         
  CMP #$60                 
  BNE $E5FA                
  LDX $AE                  
  LDA #$00                 
  STA $AF,X                
  JMP $F091                
--------sub start--------
  STA $0C                  
  LDX $AE                  
  LDA $E0,X                
  CMP #$01                 
  BEQ $E624                
  CMP #$06                 
  BEQ $E624                
  LDX #$00                 
  LDA #$18                 
  CMP $0C                  
  BEQ $E61E                
  INX                      
  CPX #$09                 
  BEQ $E621                
  LDA $C1C4,X              
  JMP $E60F                
  LDA #$00                 
  RTS                      
----------------         
  LDA #$01                 
  RTS                      
----------------         
  LDX #$04                 
  LDA $C1C4,X              
  CMP $0C                  
  BEQ $E635                
  INX                      
  CPX #$09                 
  BEQ $E638                
  JMP $E626                
  LDA #$00                 
  RTS                      
----------------         
  LDA #$01                 
  RTS                      
----------------         
--------sub start--------
  LDX $AE                  
  LDA $AF,X                
  CMP #$13                 
  BNE $E644                
  RTS                      
----------------         
  JSR $EFEC                
  JSR $EB05                
  LDX $AE                  
  LDA $E0,X                
  CMP #$01                 
  BEQ $E655                
  JMP $E682                
  LDA $00                  
  CMP #$5C                 
  BEQ $E662                
  CMP #$C4                 
  BEQ $E672                
  JMP $E6B3                
  LDA $AE                  
  ASL A                    
  TAX                      
  LDA #$A6                 
  STA $B9,X                
  INX                      
  LDA #$C7                 
  STA $B9,X                
  JMP $E6AC                
--------unidentified block--------

----------------         
  LDA $00                  
  CMP #$2C                 
  BEQ $E68F                
  CMP #$6C                 
  BEQ $E69F                
  JMP $E6B3                
--------unidentified block--------

----------------         
  LDA $AE                  
  ASL A                    
  TAX                      
  LDA #$8A                 
  STA $B9,X                
  INX                      
  LDA #$A7                 
  STA $B9,X                
  LDA #$03                 
  LDX $AE                  
  STA $AF,X                
  RTS                      
----------------         
  LDA #$00                 
  LDX $AE                  
  STA $AF,X                
  RTS                      
----------------         
--------sub start--------
  LDA $01                  
  CLC                      
  ADC #$0B                 
  JSR $E01B                
  LDY $99                  
  LDX $AE                  
  STA $E0,X                
  LDA $53                  
  CMP #$01                 
  BNE $E6D1                
  JMP $E6DB                
--------unidentified block--------

----------------         
  LDA $00                  
  JSR $E5FD                
  BNE $E6F8                
  LDX $AE                  
  LDA $E0,X                
  AND #$01                 
  BEQ $E6F0                
  LDA $C79A,Y              
  JMP $E6F3                
  LDA $C79C,Y              
  CLC                      
  ADC $01                  
  STA $01                  
  LDX $AE                  
  LDA $E0,X                
  CMP #$01                 
  BEQ $E708                
  LDA $00                  
  CMP $C3E6,Y              
  BEQ $E710                
  RTS                      
----------------         
  LDA $00                  
  CMP $C3E8,Y              
  BEQ $E710                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDX $AE                  
  INC $E4,X                
  LDA $E4,X                
  BMI $E82A                
  CMP #$10                 
  BCS $E82A                
  JMP $E82E                
  LDA #$00                 
  STA $E4,X                
  CMP #$08                 
  BCS $E83B                
  TAX                      
  LDA $C1BC,X              
  AND $0A                  
  JMP $E844                
  SEC                      
  SBC #$08                 
  TAX                      
  LDA $C1BC,X              
  AND $0B                  
  BEQ $E848                
  LDA #$01                 
  STA $0C                  
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $39                  
  BEQ $EA7B                
  RTS                      
----------------         
  LDA #$08                 
  STA $0A                  
  LDA #$00                 
  STA $0B                  
  JSR $EAB8                
  BNE $EA89                
  RTS                      
----------------         
  LDA #$50                 
  STA $00                  
  LDA #$20                 
  STA $01                  
  LDA $02F1                
  CMP #$DB                 
  BEQ $EA9F                
  INC $B7                  
  LDA #$DB                 
  JMP $EAA1                
  LDA #$D7                 
  JSR $EAED                
  LDA #$F0                 
  JSR $F08F                
  LDA $B7                  
  CMP #$04                 
  BNE $EAB7                
  LDA #$00                 
  STA $B7                  
  LDA #$BB                 
  STA $39                  
  RTS                      
----------------         
--------sub start--------
  INC $B8                  
  LDA $B8                  
  BMI $EAC5                
  CMP #$10                 
  BCS $EAC5                
  JMP $EAC9                
  LDA #$00                 
  STA $B8                  
  CMP #$08                 
  BCS $EAD6                
  TAX                      
  LDA $C1BC,X              
  AND $0A                  
  JMP $EADF                
  SEC                      
  SBC #$08                 
  TAX                      
  LDA $C1BC,X              
  AND $0B                  
  BEQ $EAE3                
  LDA #$01                 
  STA $0C                  
  RTS                      
----------------         
--------sub start--------
  LDA #$00                 
  STA $04                  
--------sub start--------
  JMP $EAEF                
--------sub start--------
  STA $02                  
  LDA #$22                 
  STA $03                  
  RTS                      
----------------         
--------sub start--------
  JSR $EAEA                
  JMP $F091                
--------sub start--------
  LDA $0203                
  STA $00                  
  LDA $0200                
  STA $01                  
  RTS                      
----------------         
--------sub start--------
  LDA $0203,X              
  STA $00                  
  LDA $0200,X              
  STA $01                  
  RTS                      
----------------         
--------sub start--------
  LDA $50                  
  AND #$01                 
  CLC                      
  ADC $54                  
  TAX                      
  CPX #$04                 
  BCC $EB1E                
  LDX #$04                 
  RTS                      
----------------         
--------sub start--------
  LDA $0503                
  BNE $EB25                
  RTS                      
----------------         
  LDA $0505                
  AND #$0F                 
  STA $0505                
  LDA $53                  
  TAX                      
  TAY                      
  DEX                      
  LDA $C608,X              
  STA $00                  
  LDA #$20                 
  STA $01                  
  TYA                      
  CMP #$02                 
  BMI $EB6D                
  LDA $44                  
  BEQ $EB68                
  CMP #$13                 
  BNE $EB4B                
  JMP $EB9E                
  CMP #$0F                 
  BNE $EB52                
  JMP $EBA7                
  CMP #$0B                 
  BNE $EB59                
  JMP $EB9E                
  CMP #$08                 
  BNE $EB60                
  JMP $EBA7                
  CMP #$04                 
  BNE $EB67                
  JSR $EBBF                
  RTS                      
----------------         
--------unidentified block--------

----------------         
  LDA $36                  
  CMP #$18                 
  BEQ $EB8D                
  CMP #$00                 
  BEQ $EB94                
  LDA $0515                
  BEQ $EB88                
  JSR $EBBA                
  LDA #$00                 
  STA $0515                
  LDA #$1A                 
  STA $44                  
  LDA $44                  
  JMP $EB44                
  LDA #$30                 
  STA $44                  
  JMP $EBB5                
  LDA #$1A                 
  STA $44                  
  JSR $EBB0                
  JMP $EB44                
  LDA #$80                 
  STA $FE                  
  LDA #$40                 
  JMP $EBC1                
  LDA #$80                 
  STA $FE                  
  LDA #$42                 
  JMP $EBC1                
--------sub start--------
  LDA #$44                 
  JMP $EBC1                
  LDA #$3E                 
  JMP $EBC1                
--------sub start--------
  LDA #$00                 
  JMP $EBC1                
  LDA #$02                 
  JSR $C815                
  DEC $44                  
  LDA $0505                
  ORA #$10                 
  STA $0505                
  RTS                      
----------------         
--------sub start--------
  LDA $45                  
  BEQ $EBD4                
  RTS                      
----------------         
  LDA $2E                  
  BNE $EBDD                
  LDA #$FF                 
  STA $96                  
  RTS                      
----------------         
  LDA #$0B                 
  STA $45                  
  LDA #$01                 
  STA $00                  
  LDA #$0A                 
  STA $01                  
  JSR $F34D                
  LDA #$02                 
  STA $00                  
  JMP $F24B                
--------sub start--------
  LDA $050B                
  BNE $EC06                
  LDA #$01                 
  STA $050B                
  LDA #$00                 
  STA $050E                
  STA $050C                
  RTS                      
----------------         
  LDA $050C                
  BEQ $EC25                
  LDA $050D                
  CMP #$05                 
  BNE $EC19                
  LDA #$04                 
  STA $96                  
  JMP $EC21                
  STA $56                  
  AND #$03                 
  BEQ $EC21                
  STA $57                  
  DEC $050C                
  RTS                      
----------------         
  LDX $050E                
  LDA $C028,X              
  STA $050C                
  LDA $C014,X              
  STA $050D                
  INC $050E                
  RTS                      
----------------         
--------sub start--------
  JSR $EAFA                
  LDA #$4C                 
  JSR $EFF7                
  LDA $53                  
  CMP #$03                 
  BEQ $EC4A                
  CMP #$01                 
  BNE $EC4D                
  JSR $EC53                
  JSR $ED99                
  JMP $EDD4                
--------sub start--------
  LDA #$00                 
  STA $5D                  
  LDA #$3A                 
  JSR $C847                
  JSR $EFE4                
  LDA $53                  
  CMP #$01                 
  BEQ $EC6A                
  TXA                      
  CLC                      
  ADC #$30                 
  TAX                      
  JSR $EB05                
  JSR $EFFE                
  BNE $ECB6                
  LDA $96                  
  CMP #$04                 
  BNE $ECA6                
  LDA $56                  
  AND #$03                 
  BNE $EC85                
  LDA $9C                  
  BEQ $EC8F                
  JMP $ECA6                
  LDA $9C                  
  CMP #$03                 
  BCS $ECA6                
  LDA $9E                  
  BNE $ECA6                
  LDA $9D                  
  CMP #$18                 
  BCS $ECA6                
  LDA $00                  
  STA $05                  
  LDA $01                  
  STA $06                  
  LDX #$00                 
  JSR $CFC2                
--------unidentified block--------

----------------         
  INC $5D                  
  LDA $53                  
  LSR A                    
  TAX                      
  LDA $5D                  
  CMP $C1FD,X              
  BEQ $ECBE                
  JMP $EC57                
  JSR $EF60                
  LDA #$FF                 
  STA $96                  
  RTS                      
----------------         
  LDA $53                  
  CMP #$03                 
  BEQ $ECCD                
  LDA $96                  
  CMP #$0A                 
  BNE $ECCD                
  JMP $ECCE                
  RTS                      
----------------         
  LDA $A0                  
  BNE $ECD5                
  JMP $ED96                
  LDA $9F                  
  LSR A                    
  LSR A                    
  BEQ $ECE0                
  LDA #$00                 
  JMP $ECE2                
  LDA #$01                 
  BEQ $ECF7                
  LDA #$04                 
  CLC                      
  ADC $0203                
  STA $00                  
  LDA $0200                
  SEC                      
  SBC #$10                 
  STA $01                  
  JMP $ED16                
  LDA $57                  
  CMP #$01                 
  BEQ $ED06                
  LDA $0203                
  SEC                      
  SBC #$10                 
  JMP $ED0C                
  LDA $0203                
  CLC                      
  ADC #$10                 
  STA $00                  
  LDA $0200                
  CLC                      
  ADC #$06                 
  STA $01                  
  LDA #$3C                 
  JSR $EFF7                
  LDA $53                  
  CMP #$01                 
  BNE $ED43                
  LDA #$00                 
  STA $5D                  
  JSR $EFE4                
  JSR $EB05                
  LDA #$3A                 
  JSR $C847                
  JSR $EFFE                
  BNE $ED66                
  LDA $5D                  
  CLC                      
  ADC #$01                 
  STA $5D                  
  CMP #$09                 
  BEQ $ED94                
  JMP $ED25                
--------unidentified block--------

----------------         
  LDA #$02                 
  STA $FF                  
  LDA $00                  
  STA $05                  
  LDA $01                  
  STA $06                  
  LDA $53                  
  CMP #$01                 
  BNE $ED83                
  LDA #$00                 
  LDX $5D                  
  STA $68,X                
  LDA #$01                 
  JMP $ED96                
--------unidentified block--------

----------------         
  LDA #$00                 
  STA $BF                  
  RTS                      
----------------         
--------sub start--------
  LDA #$00                 
  STA $AE                  
  LDA #$3A                 
  JSR $C847                
  JSR $EFEC                
  JSR $EB05                
  JSR $EFFE                
  BNE $EDBC                
  INC $AE                  
  LDA $AE                  
  LDX $53                  
  DEX                      
  CMP $C1F6,X              
  BEQ $EDC4                
  JMP $EDA2                
--------unidentified block--------

----------------         
  LDA $96                  
  CMP #$0A                 
  BNE $EDD3                
  LDA $53                  
  CMP #$01                 
  BEQ $EDD3                
  JSR $ECCE                
  RTS                      
----------------         
  LDA $53                  
  CMP #$03                 
  BNE $EDE1                
  LDY $96                  
  CPY #$01                 
  BEQ $EDE1                
  RTS                      
----------------         
  SEC                      
  SBC #$01                 
  ASL A                    
  TAX                      
  LDA $C42B,X              
  STA $02                  
  LDA $C42C,X              
  STA $03                  
  LDA $C423,X              
  STA $00                  
  LDA $C424,X              
  STA $01                  
  JSR $EFFE                
  BNE $EE16                
  LDA $53                  
  CMP #$03                 
  BNE $EE1A                
  LDA $01                  
  CMP #$C9                 
  BEQ $EE1A                
  LDA #$70                 
  STA $00                  
  LDA #$C9                 
  STA $01                  
  JMP $EDFA                
--------unidentified block--------

----------------         
  RTS                      
----------------         
--------sub start--------
  LDA #$80                 
  STA $0A                  
  LDA #$80                 
  STA $0B                  
  JSR $DFE5                
  BNE $EE29                
  RTS                      
----------------         
  LDA $53                  
  CMP #$01                 
  BNE $EE35                
  JSR $EFE4                
  JMP $EE38                
--------unidentified block--------

----------------         
  STX $04                  
  JSR $EB05                
  LDA $BF                  
  CMP #$01                 
  BNE $EE47                
  LDY #$02                 
  STY $FF                  
  CMP #$0B                 
  BEQ $EE60                
  LDX $BF                  
  DEX                      
  LDA $C1EC,X              
  STA $02                  
  JSR $EAF4                
  LDX $04                  
  LDA #$02                 
  JSR $EE7B                
  INC $BF                  
  RTS                      
----------------         
  LDA $53                  
  CMP #$01                 
  BNE $EE6B                
  LDA #$03                 
  JSR $EE7B                
  JSR $EAEA                
  JSR $F0A3                
  LDX #$02                 
  JSR $CFC2                
  LDA #$00                 
  STA $BF                  
  RTS                      
----------------         
--------sub start--------
  STA $0202,X              
  STA $0206,X              
  STA $020A,X              
  STA $020E,X              
  RTS                      
----------------         
--------sub start--------
  LDY $53                  
  CPY #$01                 
  BNE $EE8F                
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA $96                  
  CMP #$0A                 
  BNE $EF80                
  LDA $A0                  
  BEQ $EF80                
  SEC                      
  SBC #$01                 
  TAX                      
  LDA #$00                 
  STA $0451,X              
  TXA                      
  ASL A                    
  ASL A                    
  ASL A                    
  TAX                      
  LDA #$FF                 
  STA $02D0,X              
  STA $02D4,X              
  RTS                      
----------------         
--------sub start--------
  STX $0F                  
  ASL A                    
  TAX                      
  LDA $042C,X              
  BNE $EFA3                
  STA $0436,X              
  CPX #$00                 
  BNE $EF96                
  LDA #$08                 
  JMP $EF98                
--------unidentified block--------

----------------         
  STA $0435,X              
  LDA #$F0                 
  STA $042D,X              
  JMP $EFBC                
  LDA $0435,X              
  CPX #$00                 
  BNE $EFAF                
  ADC #$10                 
  JMP $EFB1                
--------unidentified block--------

----------------         
  STA $0435,X              
  LDA $0436,X              
  ADC #$00                 
  STA $0436,X              
  LDA $042D,X              
  SEC                      
  SBC $043D,X              
  STA $042D,X              
  LDA $01                  
  SBC $043E,X              
  STA $01                  
  CLC                      
  LDA $042D,X              
  ADC $0435,X              
  STA $042D,X              
  LDA $01                  
  ADC $0436,X              
  STA $01                  
  INC $042C,X              
  LDX $0F                  
  RTS                      
----------------         
--------sub start--------
  LDA $5D                  
  CLC                      
  ADC #$03                 
  JMP $EFF1                
--------sub start--------
  LDA $AE                  
  CLC                      
  ADC #$01                 
  ASL A                    
  ASL A                    
  ASL A                    
  ASL A                    
  TAX                      
  RTS                      
----------------         
--------sub start--------
  JSR $C847                
--------sub start--------
  LDA #$00                 
  BEQ $F004                
--------sub start--------
  LDA #$01                 
  BNE $F004                
--------sub start--------
  LDA #$02                 
  STA $0C                  
  TXA                      
  PHA                      
  TYA                      
  PHA                      
  LDY #$00                 
  LDA $0C                  
  BNE $F027                
  JSR $F072                
  STA $46                  
  JSR $F078                
  STA $47                  
  JSR $F071                
  STA $48                  
  JSR $F078                
  STA $49                  
  JMP $F068                
  JSR $F072                
  STA $4A                  
  JSR $F078                
  STA $4B                  
  JSR $F071                
  STA $4C                  
  JSR $F078                
  STA $4D                  
  LDA $4A                  
  SEC                      
  SBC $46                  
  STA $9C                  
  LDA $4B                  
  SEC                      
  SBC $47                  
  STA $9D                  
  LDA $49                  
  CMP $4B                  
  BCC $F066                
  LDA $4D                  
  CMP $47                  
  BCC $F066                
  LDA $4C                  
  CMP $46                  
  BCC $F066                
  LDA $48                  
  CMP $4A                  
  BCC $F066                
  LDA #$01                 
  JMP $F068                
  LDA #$00                 
  STA $0C                  
  PLA                      
  TAY                      
  PLA                      
  TAX                      
  LDA $0C                  
  RTS                      
----------------         
--------sub start--------
  INY                      
--------sub start--------
  LDA ($02),Y              
  CLC                      
  ADC $00                  
  RTS                      
----------------         
--------sub start--------
  INY                      
  LDA ($02),Y              
  CLC                      
  ADC $01                  
  RTS                      
----------------         
--------sub start--------
  STA $02                  
  JSR $EAFA                
  JSR $EAE6                
  LDA $57                  
  AND #$03                 
  LSR A                    
  JMP $F0A5                
--------sub start--------
  STA $04                  
--------sub start--------
  LDA #$00                 
  BEQ $F0A5                
  STA $04                  
  LDA #$01                 
  BNE $F0A5                
  STA $04                  
--------sub start--------
  LDA #$04                 
  BNE $F0A5                
--------sub start--------
  STA $03                  
--------sub start--------
  LDA #$0F                 
--------sub start--------
  PHA                      
  STA $0F                  
  TXA                      
  PHA                      
  TYA                      
  PHA                      
  LDA $00                  
  PHA                      
  LDA $05                  
  PHA                      
  LDA $06                  
  PHA                      
  LDA $07                  
  PHA                      
  LDA $08                  
  PHA                      
  LDA $09                  
  PHA                      
  LDA #$02                 
  STA $05                  
  LDA $0F                  
  CMP #$04                 
  BEQ $F0FE                
  LDA #$0F                 
  AND $03                  
  STA $07                  
  LDA $03                  
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  STA $06                  
  TAX                      
  LDA #$00                 
  CLC                      
  ADC $07                  
  DEX                      
  BNE $F0DA                
  STA $08                  
  LDA $0F                  
  BNE $F0EB                
  JSR $F12D                
  JMP $F0F8                
  CMP #$01                 
  BEQ $F0F5                
  JSR $F1A4                
  JMP $F101                
  JSR $F170                
  JSR $F148                
  JMP $F101                
  JSR $F119                
  PLA                      
  STA $09                  
  PLA                      
  STA $08                  
  PLA                      
  STA $07                  
  PLA                      
  STA $06                  
  PLA                      
  STA $05                  
  PLA                      
  STA $00                  
  PLA                      
  TAY                      
  PLA                      
  TAX                      
  PLA                      
  RTS                      
----------------         
--------sub start--------
  LDX $03                  
  LDY #$00                 
  LDA #$FF                 
  STA ($04),Y              
  INY                      
  INY                      
  LDA $02                  
  STA ($04),Y              
  INY                      
  INY                      
  DEX                      
  BNE $F11D                
  RTS                      
----------------         
--------sub start--------
  LDA $02                  
  LDX $08                  
  LDY #$01                 
  STA ($04),Y              
  CLC                      
  ADC #$01                 
  INY                      
  PHA                      
  LDA ($04),Y              
  AND #$3F                 
  STA ($04),Y              
  PLA                      
  INY                      
  INY                      
  INY                      
  DEX                      
  BNE $F133                
  RTS                      
----------------         
--------sub start--------
  LDY #$00                 
  LDX $06                  
  LDA $01                  
  STA $09                  
  LDA $09                  
  STA ($04),Y              
  CLC                      
  ADC #$08                 
  STA $09                  
  INY                      
  INY                      
  INY                      
  LDA $00                  
  STA ($04),Y              
  INY                      
  DEX                      
  BNE $F150                
  LDA $00                  
  CLC                      
  ADC #$08                 
  STA $00                  
  DEC $07                  
  BNE $F14A                
  RTS                      
----------------         
--------sub start--------
  LDY #$01                 
  STY $0A                  
  LDA $08                  
  SEC                      
  SBC $06                  
  TAY                      
  STA $0B                  
  LDX $06                  
  TYA                      
  PHA                      
  CLC                      
  TYA                      
  ADC $02                  
  LDY $0A                  
  STA ($04),Y              
  INY                      
  LDA ($04),Y              
  AND #$3F                 
  EOR #$40                 
  STA ($04),Y              
  INY                      
  INY                      
  INY                      
  STY $0A                  
  PLA                      
  TAY                      
  INY                      
  DEX                      
  BNE $F17E                
  LDA $0B                  
  SEC                      
  SBC $06                  
  BPL $F179                
  RTS                      
----------------         
--------sub start--------
  LDY #$00                 
  LDX $06                  
  LDA $01                  
  STA $09                  
  LDA #$FF                 
  STA ($04),Y              
  INY                      
  INY                      
  INY                      
  INY                      
  DEX                      
  BNE $F1AE                
  LDA $00                  
  CLC                      
  ADC #$08                 
  STA $00                  
  DEC $07                  
  BNE $F1A6                
  RTS                      
----------------         
  LDA PpuStatus_2002       
  LDA $10                  
  AND #$FB                 
  STA PpuControl_2000      
  LDA #$20                 
  STA PpuAddr_2006         
  LDA #$00                 
  STA PpuAddr_2006         
  LDX #$04                 
  LDY #$00                 
  LDA #$24                 
  STA PpuData_2007         
  DEY                      
  BNE $F1DD                
  DEX                      
  BNE $F1DD                
  LDA #$23                 
  STA PpuAddr_2006         
  LDA #$C0                 
  STA PpuAddr_2006         
  LDY #$40                 
  LDA #$00                 
  STA PpuData_2007         
  DEY                      
  BNE $F1F4                
  RTS                      
----------------         
  STA PpuAddr_2006         
  INY                      
  LDA ($00),Y              
  STA PpuAddr_2006         
  INY                      
  LDA ($00),Y              
  ASL A                    
  PHA                      
  LDA $10                  
  ORA #$04                 
  BCS $F211                
  AND #$FB                 
  STA PpuControl_2000      
  STA $10                  
  PLA                      
  ASL A                    
  BCC $F21D                
  ORA #$02                 
  INY                      
  LSR A                    
  LSR A                    
  TAX                      
  BCS $F223                
  INY                      
  LDA ($00),Y              
  STA PpuData_2007         
  DEX                      
  BNE $F220                
  SEC                      
  TYA                      
  ADC $00                  
  STA $00                  
  LDA #$00                 
  ADC $01                  
  STA $01                  
--------sub start--------
  LDX PpuStatus_2002       
  LDY #$00                 
  LDA ($00),Y              
  BNE $F1FB                
  LDA $12                  
  STA PpuScroll_2005       
  LDA $13                  
  STA PpuScroll_2005       
  RTS                      
----------------         
--------sub start--------
  CLD                      
  LDA #$04                 
  LSR $00                  
  BCC $F257                
  PHA                      
  JSR $F25D                
  PLA                      
  CLC                      
  SBC #$00                 
  BPL $F24E                
  RTS                      
----------------         
--------sub start--------
  ASL A                    
  ASL A                    
  TAY                      
  STA $01                  
  LDX $0330                
  LDA $C000,Y              
  STA $0331,X              
  JSR $F33C                
  INY                      
  LDA $C000,Y              
  STA $0331,X              
  JSR $F33C                
  INY                      
  LDA $C000,Y              
  AND #$87                 
  STA $0331,X              
  AND #$07                 
  STA $02                  
  TXA                      
  SEC                      
  ADC $02                  
  JSR $F33E                
  TAX                      
  STX $0330                
  LDA #$00                 
  STA $0331,X              
  INY                      
  LDA $C000,Y              
  STA $03                  
  DEX                      
  CLC                      
  LDA $0020,Y              
  AND #$0F                 
  BEQ $F2A5                
  CLC                      
  BCC $F2A9                
  LDA #$24                 
  STA $0331,X              
  DEX                      
  DEC $02                  
  BEQ $F2D3                
  LDA $0020,Y              
  AND #$F0                 
  PHP                      
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  PLP                      
  BEQ $F2BF                
  CLC                      
  BCC $F2C3                
  LDA #$24                 
  STA $0331,X              
  LDA $03                  
  AND #$01                 
  BEQ $F2CD                
  SEC                      
  DEY                      
  DEX                      
  DEC $02                  
  BNE $F29D                
  LDA $03                  
  AND #$10                 
  BEQ $F2E5                
  INX                      
  LDY $01                  
  CLC                      
  LDA $0020,Y              
  ADC #$37                 
  STA $0331,X              
  RTS                      
----------------         
  LDY #$00                 
  LDA ($02),Y              
  AND #$0F                 
  STA $05                  
  LDA ($02),Y              
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  STA $04                  
  LDX $0330                
  LDA $01                  
  STA $0331,X              
  JSR $F33C                
  LDA $00                  
  STA $0331,X              
  JSR $F33C                
  LDA $04                  
  STA $06                  
  ORA #$80                 
  STA $0331,X              
  JSR $F33C                
  INY                      
  LDA ($02),Y              
  STA $0331,X              
  DEC $06                  
  BNE $F312                
  JSR $F33C                
  CLC                      
  LDA #$01                 
  ADC $00                  
  STA $00                  
  LDA #$00                 
  ADC $01                  
  STA $01                  
  STX $0330                
  DEC $05                  
  BNE $F2F9                
  LDA #$00                 
  STA $0331,X              
  RTS                      
----------------         
--------sub start--------
  INX                      
  TXA                      
--------sub start--------
  CMP #$3F                 
  BCC $F34C                
  LDX $0330                
  LDA #$00                 
  STA $0331,X              
  PLA                      
  PLA                      
  RTS                      
----------------         
--------sub start--------
  LDX #$FF                 
  BNE $F353                
  LDX #$00                 
  STX $04                  
  LDX #$00                 
  STX $05                  
  STX $06                  
  STX $07                  
  LDA $01                  
  AND #$08                 
  BNE $F364                
  INX                      
  LDA $00                  
  STA $06,X                
  LDA $01                  
  JMP $F36D                
  AND #$07                 
  ASL A                    
  ASL A                    
  TAX                      
  LDA $04                  
  BEQ $F39D                
  LDA $24,X                
  BEQ $F3A1                
  CLC                      
  LDA $27,X                
  STA $03                  
  LDA $07                  
  JSR $F3F2                
--------unidentified block--------

----------------         
  SEC                      
  LDA $27,X                
  STA $03                  
  LDA $07                  
  JSR $F413                
  STA $27,X                
  LDA $26,X                
  STA $03                  
  LDA $06                  
  JSR $F413                
  STA $26,X                
  LDA $25,X                
  STA $03                  
  LDA $05                  
  JSR $F413                
  STA $25,X                
  LDA $25,X                
  BNE $F3CF                
  LDA $26,X                
  BNE $F3CF                
  LDA $27,X                
  BEQ $F3D5                
  BCS $F3F1                
  LDA $24,X                
  EOR #$FF                 
  STA $24,X                
  SEC                      
  LDA #$00                 
  STA $03                  
  LDA $27,X                
  JSR $F413                
--------unidentified block--------

----------------         
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  JSR $F435                
  SBC $01                  
  STA $01                  
  BCS $F426                
  ADC #$0A                 
  STA $01                  
  LDA $02                  
  ADC #$0F                 
  STA $02                  
  LDA $03                  
  AND #$F0                 
  SEC                      
  SBC $02                  
  BCS $F432                
  ADC #$A0                 
  CLC                      
  ORA $01                  
  RTS                      
----------------         
--------sub start--------
  PHA                      
  AND #$0F                 
  STA $01                  
  PLA                      
  AND #$F0                 
  STA $02                  
  LDA $03                  
  AND #$0F                 
  RTS                      
----------------         
  LDA #$00                 
  STA $04                  
  CLC                      
  LDA $00                  
  ADC #$10                 
  AND #$F0                 
  LSR A                    
  LSR A                    
  TAY                      
  LDA $00                  
  AND #$07                 
  ASL A                    
  ASL A                    
  TAX                      
  LDA $0020,Y              
  BEQ $F4AF                
  LDA $24,X                
  BEQ $F488                
  SEC                      
  LDA $0023,Y              
  STA $03                  
  LDA $27,X                
  JSR $F413                
  LDA $0022,Y              
  STA $03                  
  LDA $26,X                
  JSR $F413                
  LDA $0021,Y              
  STA $03                  
  LDA $25,X                
  JSR $F413                
  BCS $F4B3                
  LDA $0020,Y              
  BNE $F4B8                
  LDA #$FF                 
  STA $04                  
  SEC                      
  TYA                      
  BNE $F4AE                
  BCC $F4A2                
  LDA $24,X                
  STA $20                  
  LDA $25,X                
  STA $21                  
  LDA $26,X                
  STA $22                  
  LDA $27,X                
  STA $23                  
  LDA $00                  
  AND #$08                 
  BEQ $F4AE                
  DEX                      
  DEX                      
  DEX                      
  DEX                      
  BPL $F459                
  RTS                      
----------------         
  LDA $24,X                
  BEQ $F462                
  LDA $0020,Y              
  BNE $F488                
  CLC                      
  BCC $F48D                
--------sub start--------
  LDX #$09                 
  DEC $34                  
  BPL $F4C7                
  LDA #$0A                 
  STA $34                  
  LDX #$10                 
  LDA $35,X                
  BEQ $F4CD                
  DEC $35,X                
  DEX                      
  BPL $F4C7                
  RTS                      
----------------         
--------sub start--------
  LDX $0330                
  LDA $01                  
  STA $0331,X              
  JSR $F33C                
  LDA $00                  
  STA $0331,X              
  JSR $F33C                
  LDA #$01                 
  STA $0331,X              
  JSR $F33C                
  TYA                      
  STA $0331,X              
  JSR $F33C                
  LDA #$00                 
  STA $0331,X              
  STX $0330                
  RTS                      
----------------         
--------sub start--------
  LDA $18                  
  AND #$02                 
  STA $00                  
  LDA $19                  
  AND #$02                 
  EOR $00                  
  CLC                      
  BEQ $F50C                
  SEC                      
  ROR $18                  
  ROR $19                  
  ROR $1A                  
  ROR $1B                  
  ROR $1C                  
  ROR $1D                  
  ROR $1E                  
  ROR $1F                  
  RTS                      
----------------         
--------sub start--------
  LDA #$01                 ; Controller to parallel mode
  STA Ctrl1_4016           ; Can read Human
  LDX #$00                 ; Player 1
  LDA #$00                 ; Controller to serial mode,
  STA Ctrl1_4016           ; CPU can read Controller
  JSR $F530                ; Read controller 1
  INX                      ; Player 2
  JMP $F530                ; Read controller 2
--------sub start--------
  LDY #$08                 ; range(8) ; A=0
-F532-:  
  PHA                      ; snapshot := A;
  LDA Ctrl1_4016,X         ; A := cont; A0 for std_cont, A1 for fam_exp_cont
  STA $00                  ; [$00] := A 
  LSR A                    ; A0 := A1
  ORA $00                  ; A0 := button was pressed on either
  LSR A                    ; A0 -> C
  PLA                      ; A := snapshot
  ROL A                    ; A << 1; A0 <- C
  DEY                      
  BNE $F532                  
  
  STX $00                         
  ASL $00                  
  LDX $00                  ; x= 0 for player 1 x=2 for player 2
  LDY $14,X                ; player 1: [$14] player 2: [$16]
  STY $00                  ; [$00] := prev
  STA $14,X                ; prev := snapshot; 
  STA $15,X                ; player 1: [$15] player 2: [$17]  
  AND #$FF                 ; 
  BPL $F55B                
  
  BIT $00                  
  BPL $F55B                

  AND #$7F                 
  STA $15,X       
-F55B-:           
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDA #$C0                 
  STA Ctrl2_FrameCtr_4017  
  JSR $FBF2                
  LDX #$00                 
  STX $FF                  
  STX $FE                  
  STX $FD                  
  LDA $06F0                
  CMP #$90                 
  BCS $FA64                
  LDX #$00                 
  STX $06F1                
  CMP #$D8                 
  BCC $FA6B                
  INC $06F1                
  TAY                      
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  STA $00                  
  TYA                      
  LDX $06F1                
  BNE $FA7F                
  SEC                      
  ADC $00                  
  BNE $FA82                
  CLC                      
  SBC $00                  
  STA $06F0                
  RTS                      
----------------         
--------sub start--------
  LDY #$07                 
  ASL A                    
  BCS $FA8E                
  DEY                      
  BNE $FA88                
  RTS                      
----------------         
--------sub start--------
  STA $F1                  
  STY $F2                  
--------sub start--------
  LDY #$7F                 
--------sub start--------
  STX Sq0Duty_4000         
  STY Sq0Sweep_4001        
  RTS                      
----------------         
--------unidentified block--------

----------------         
--------sub start--------
  LDX #$00                 
  TAY                      
  LDA $FB01,Y              
  BEQ $FAB2                
  STA Sq0Timer_4002,X      
  LDA $FB00,Y              
  ORA #$08                 
  STA Sq0Length_4003,X     
  RTS                      
----------------         
--------sub start--------
  STY Sq1Sweep_4005        
  LDX #$04                 
  BNE $FAA1                
--------sub start--------
  STA TrgLinear_4008       
  TXA                      
  AND #$3E                 
  LDX #$08                 
  BNE $FAA1                
--------sub start--------
  TAX                      
  ROR A                    
  TXA                      
  ROL A                    
  ROL A                    
  ROL A                    
--------sub start--------
  AND #$07                 
  CLC                      
  ADC $068D                
  TAY                      
  LDA $FB4C,Y              
  RTS                      
----------------         
--------sub start--------
  TYA                      
  LSR A                    
  LSR A                    
  LSR A                    
  STA $00                  
  TYA                      
  SEC                      
  SBC $00                  
  RTS                      
----------------         
--------sub start--------
  LDA #$90                 
  STA Sq0Duty_4000         
  RTS                      
----------------         
--------unidentified block--------

----------------         
  STY $F0                  
  LDA #$71                 
  LDY #$00                 
  LDX #$9F                 
  JSR $FA8F                
  LDX $F2                  
  LDY $FB67,X              
  DEC $F1                  
  LDA $F1                  
  BEQ $FB75                
  AND #$07                 
  BNE $FBA0                
  TYA                      
  LSR A                    
  ADC $FB67,X              
  TAY                      
  BNE $FBA7                
  AND #$03                 
  BNE $FBB2                
  INC $F2                  
  CLC                      
  STY Sq0Timer_4002        
  LDY #$28                 
  BCC $FBAF                
  INY                      
  STY Sq0Length_4003       
  LDA #$00                 
  JMP $FE00                
  STY $F0                  
  LDA #$54                 
  LDY #$6A                 
  LDX #$9C                 
  JSR $FA8F                
  LDY $F2                  
  LDA $F1                  
  AND #$03                 
  BEQ $FBD4                
  CMP #$03                 
  BNE $FBD9                
  JSR $FAD5                
  STA $F2                  
  TAY                      
  TYA                      
  LSR A                    
  ADC $F2                  
  TAY                      
  TYA                      
  ROL A                    
  ROL A                    
  ROL A                    
  STA Sq0Timer_4002        
  ROL A                    
  STA Sq0Length_4003       
  LDA $F1                  
  CMP #$18                 
  BCS $FC44                
  LSR A                    
  ORA #$90                 
  STA Sq0Duty_4000         
  BNE $FC44                
--------sub start--------
  LDY $FF                  
  LDA $F0                  
  LSR A                    
  BCS $FB89                
  LSR $FF                  
  BCS $FB7E                
  LDX $FA                  
  BNE $FC4B                
  LSR A                    
  BCS $FBC2                
  LSR $FF                  
  BCS $FBB7                
  LSR A                    
  BCS $FC28                
  LSR $FF                  
  BCS $FC19                
  LSR A                    
  BCS $FC62                
  LSR $FF                  
  BCS $FC51                
  JMP $FC90                
  STY $F0                  
  LDA #$22                 
  STA $F1                  
  LDY #$0B                 
  STY $F2                  
  LDA #$20                 
  JSR $FA9F                
  DEC $F2                  
  BNE $FC30                
  LDA #$07                 
  STA $F2                  
  LDX $F2                  
  LDY $FAF5,X              
  LDX #$5A                 
  LDA $F1                  
  CMP #$14                 
  BCS $FC41                
  LSR A                    
  ORA #$50                 
  TAX                      
  JSR $FA95                
  DEC $F1                  
  BNE $FC16                
  JSR $FAE0                
  LDA #$00                 
  STA $F0                  
  BEQ $FC16                
  STY $F0                  
  LDA #$0A                 
  STA $F1                  
  LDY $06F0                
  STY Sq0Timer_4002        
  LDA #$88                 
  STA Sq0Length_4003       
  LDA $18                  
  AND #$08                 
  CLC                      
  ADC $F1                  
  ADC #$FE                 
  TAX                      
  LDY $FAE5,X              
  LDX #$41                 
  BNE $FC41                
  LDA #$0E                 
  STA $06A5                
  LDY #$85                 
  LDA #$46                 
  JSR $FAB3                
  DEC $06A5                
  BEQ $FC9D                
  LDA $06A5                
  ORA #$90                 
  TAY                      
  DEY                      
  STY Sq1Duty_4004         
  BNE $FC9D                
  LDA $F3                  
  BNE $FC9D                
  LDA $06A5                
  BNE $FC7F                
  LDY $FE                  
  BMI $FC73                
  LDA $FC                  
  BNE $FD0B                
  LDA $F9                  
  BNE $FD0B                
  LDY $FE                  
  LDA $06A1                
  LSR $FE                  
  BCS $FCBA                
  LSR A                    
  BCS $FCBE                
  LSR A                    
  BCS $FCF0                
  LSR $FE                  
  BCS $FCDB                
  BCC $FD0B                
  LDA #$28                 
  BNE $FCDD                
  LDA $F5                  
  BNE $FCC6                
  LSR $FE                  
  BCS $FCDB                
  LDA $F6                  
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  LSR A                    
  ADC $F6                  
  BCC $FD00                
  LDA #$00                 
  STA $06A1                
  STA TrgLinear_4008       
  BEQ $FD0B                
  LDA #$FE                 
  STY $06A1                
  LDX #$0E                 
  STX $F5                  
  LDY #$FF                 
  STY TrgLinear_4008       
  LDY #$08                 
  STY TrgLength_400B       
  BNE $FD00                
  LDA #$FE                 
  LDY $F5                  
  BEQ $FCD1                
  CPY #$07                 
  BEQ $FD00                
  LDA $F6                  
  TAY                      
  JSR $FAD7                
--------unidentified block--------

----------------         
  LDX $FA                  
  BNE $FD58                
  LDA $FC                  
  BNE $FD18                
  STA $06A3                
  BEQ $FD58                
  EOR $06A3                
  BEQ $FD35                
  LDA $FC                  
  STA $06A3                
  JSR $FA86                
  LDA $FFCD,Y              
  STA $0680                
  LDA #$D4                 
  STA $F5                  
  LDA #$FF                 
  STA $F6                  
  BNE $FD3A                
  DEC $0698                
  BNE $FD58                
  LDY $0680                
  INC $0680                
  LDA ($F5),Y              
  BEQ $FD1D                
  TAX                      
  ROR A                    
  TXA                      
  ROL A                    
  ROL A                    
  ROL A                    
  AND #$07                 
  TAY                      
  LDA $FB62,Y              
  STA $0698                
  LDA #$10                 
  JSR $FABA                
  LDA $FD                  
  BNE $FD62                
  LDA $0102                
  BNE $FD9B                
  RTS                      
----------------         
  JSR $FA86                
  STY $FB                  
  LDA $FE59,Y              
  TAY                      
  LDA $FE59,Y              
  STA $068D                
  LDA $FE5A,Y              
  STA $F7                  
  LDA $FE5B,Y              
  STA $F8                  
  LDA $FE5C,Y              
  STA $F9                  
  LDA $FE5D,Y              
  STA $FA                  
  LDA #$01                 
  STA $0695                
  STA $0696                
  STA $0698                
  STA $0102                
  LDY #$00                 
  STY $F3                  
  LDA $FB                  
  BEQ $FDA4                
  LDY $FA                  
  BEQ $FDD8                
  DEC $0696                
  BNE $FDD8                
  INC $FA                  
  LDA ($F7),Y              
  BEQ $FDE9                
  BPL $FDB8                
  JSR $FACA                
  STA $0691                
  LDY $FA                  
  INC $FA                  
  LDA ($F7),Y              
  JSR $FA9F                
  BNE $FDC1                
  LDY #$10                 
  BNE $FDCF                
  LDX #$9F                 
  LDA $FB                  
  BEQ $FDCF                
  LDX #$06                 
  LDA $F9                  
  BNE $FDCF                
  LDX #$86                 
  JSR $FA93                
  LDA $0691                
  STA $0696                
  LDA $FB                  
  BEQ $FE31                
  DEC $0695                
  BNE $FE31                
  LDY $F3                  
  INC $F3                  
  LDA ($F7),Y              
  BNE $FE09                
  JSR $FAE0                
  LDA #$00                 
  STA $FA                  
  STA $F3                  
  STA $F9                  
  STA $0102                
  LDY $FB                  
  BEQ $FE00                
  LDY $06A1                
  BNE $FE03                
  STA TrgLinear_4008       
  LDA #$10                 
  STA Sq1Duty_4004         
  RTS                      
----------------         
  JSR $FAC4                
  STA $0695                
  TXA                      
  AND #$3E                 
  LDY #$7F                 
  JSR $FAB3                
  BNE $FE1D                
  LDX #$10                 
  BNE $FE2E                
  LDX #$89                 
  LDA $0695                
  CMP #$18                 
  BCS $FE2E                
  LDX #$86                 
  CMP #$10                 
  BCS $FE2E                
  LDX #$84                 
  STX Sq1Duty_4004         
  LDY $F9                  
  BEQ $FE58                
  DEC $0698                
  BNE $FE58                
  INC $F9                  
  LDA ($F7),Y              
  JSR $FAC4                
  STA $0698                
  CLC                      
  ADC #$FE                 
  ASL A                    
  ASL A                    
  CMP #$38                 
  BCC $FE4F                
  LDA #$38                 
  LDY $FB                  
  BNE $FE55                
  LDA #$FF                 
  JSR $FABA                
  RTS                      
----------------         
--------unidentified block--------

----------------        
; irq_vec $FFF0; always disabled