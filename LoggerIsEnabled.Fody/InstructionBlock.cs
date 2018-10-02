using Mono.Cecil.Cil;
using System.Collections.Generic;

public class InstructionBlock
{
    public List<Instruction> Instructions { get; } = new List<Instruction>();
    public int LogInstructionIndex { get; set; }
}
