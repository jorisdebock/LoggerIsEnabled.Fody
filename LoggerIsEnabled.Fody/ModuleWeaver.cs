using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;
using System.Collections.Generic;
using System.Linq;

public class ModuleWeaver : BaseModuleWeaver
{
    private const string ILoggerField = "Microsoft.Extensions.Logging.ILogger";
    private const string ILoggerIsEnabled = "Microsoft.Extensions.Logging.ILogger::IsEnabled";

    private const string LogIdentifier = "Microsoft.Extensions.Logging.LoggerExtensions::Log";
    private List<EnumValue> _logLevelValues;
    private HashSet<string> _logInstructionsHash;
    private bool _isDebugBuild;

    public override void Execute()
    {
        LogInfo("Weaving ILogger with IsEnabled - Start");
        Init();
        ProcessAssembly();
        LogInfo("Weaving ILogger with IsEnabled - End");
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "mscorlib";
        yield return "netstandard";
        yield return "Microsoft.Extensions.Logging.Abstractions";
    }

    public override bool ShouldCleanReference => true;

    private void Init()
    {
        _isDebugBuild = ModuleDefinition.Assembly.IsDebugBuild();
        _logLevelValues = GetEnumValuesOfType("Microsoft.Extensions.Logging.LogLevel");
        _logInstructionsHash = new HashSet<string>(_logLevelValues.Select(x => LogIdentifier + x.Name));
    }

    private void ProcessAssembly()
    {
        foreach (var type in ModuleDefinition.GetTypes())
        {
            if (!IsApplicableTypeDefintion(type))
            {
                LogDebug($"Skipping type {type.Name}, not applicable");
                continue;
            }

            var iLoggerField = type.GetFieldDefintion(ILoggerField);
            if (iLoggerField == null)
            {
                LogDebug($"Skipping type {type.Name}, ILogger field not found");
                continue;
            }

            LogDebug($"Processing type {type.Name}");

            foreach (var method in type.Methods)
            {
                if (!IsApplicableMethodDefintion(method))
                {
                    LogDebug($"Skipping method {method.Name}, not applicable");
                    continue;
                }

                LogDebug($"Processing method {method.Name}");

                ProcessMethod(iLoggerField, method.Body);
            }
        }
    }

    private bool IsApplicableTypeDefintion(TypeDefinition typeDefinition)
    {
        return !typeDefinition.IsEnum
            && !typeDefinition.IsInterface;
    }

    private bool IsApplicableMethodDefintion(MethodDefinition methodDefinition)
    {
        return methodDefinition.HasBody;
    }

    private void ProcessMethod(FieldDefinition iLoggerField, MethodBody methodBody)
    {
        var loggingInstructionBlocks = GetLoggingInstructionBlocksWithoutIsEnabled(methodBody.Instructions);

        methodBody.SimplifyMacros();
        for (int i = loggingInstructionBlocks.Count - 1; i >= 0; i--)
        {
            LogDebug($"Weaving logging block: [{i}]");
            RewriteLoggingInBody(iLoggerField, methodBody, loggingInstructionBlocks[i]);
        }
        methodBody.OptimizeMacros();
    }

    private List<InstructionBlock> GetLoggingInstructionBlocksWithoutIsEnabled(Collection<Instruction> instructions)
    {
        // IL_0001: ldarg.0      // this
        // IL_0002: ldfld        class [Microsoft.Extensions.Logging.Abstractions]Microsoft.Extensions.Logging.ILogger LoggerIsEnabledSenarios::_logger
        // IL_0007: ldstr        "message"
        // IL_000c: call         !!0/*object*/[] [mscorlib]System.Array::Empty<object>()
        // IL_0011: call         void [Microsoft.Extensions.Logging.Abstractions]Microsoft.Extensions.Logging.LoggerExtensions::LogTrace(class [Microsoft.Extensions.Logging.Abstractions]Microsoft.Extensions.Logging.ILogger, string, object[])
        // IL_0016: nop

        var instructionBlocks = new List<InstructionBlock>();

        for (var i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];
            var instructionString = instruction.ToString();

            if (!_logInstructionsHash.Any(instructionString.Contains))
            {
                continue;
            }

            var startIndex = i;
            var instructionBlock = new InstructionBlock
            {
                LogInstructionIndex = 4
            };

            instructionBlock.Instructions.Add(instructions[i - 4]);
            instructionBlock.Instructions.Add(instructions[i - 3]);
            instructionBlock.Instructions.Add(instructions[i - 2]);
            instructionBlock.Instructions.Add(instructions[i - 1]);
            instructionBlock.Instructions.Add(instructions[i]);

            if (_isDebugBuild)
            {
                instructionBlock.Instructions.Add(instructions[i + 1]);
                i += 1;
            }

            if (!IsSurroundedWithIsEnabled(instructions, startIndex, instructionBlock))
            {
                instructionBlocks.Add(instructionBlock);
            }
        }

        return instructionBlocks;
    }

    private bool IsSurroundedWithIsEnabled(Collection<Instruction> instructions, int startIndex, InstructionBlock instructionBlock)
    {
        // IL_0001: ldarg.0      // this
        // IL_0002: ldfld        class [Microsoft.Extensions.Logging.Abstractions]Microsoft.Extensions.Logging.ILogger LoggerIsEnabledSenarios::_logger
        // IL_0007: ldc.i4.0
        // IL_0008: callvirt     instance bool [Microsoft.Extensions.Logging.Abstractions]Microsoft.Extensions.Logging.ILogger::IsEnabled(valuetype [Microsoft.Extensions.Logging.Abstractions]Microsoft.Extensions.Logging.LogLevel)
        // IL_000d: stloc.0      // V_0
        // IL_000e: ldloc.0      // V_0
        // IL_000f: brfalse.s    IL_0030

        for (int index = startIndex; index >= 0; index--)
        {
            var brfalseSInstruction = instructions[index];
            if (brfalseSInstruction.OpCode != OpCodes.Brfalse_S)
            {
                continue;
            }

            var isEnabledCallInstructionIndex = index - (_isDebugBuild ? 3 : 1);
            if (isEnabledCallInstructionIndex <= 0)
            {
                continue;
            }

            var isEnabledCallInstruction = instructions[isEnabledCallInstructionIndex];
            if (!isEnabledCallInstruction.ToString().Contains(ILoggerIsEnabled))
            {
                continue;
            }

            if (!(brfalseSInstruction.Operand is Instruction instructionOperand))
            {
                continue;
            }

            if (IsInstructionAfterInstructionBlock(instructionBlock, instructionOperand))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsInstructionAfterInstructionBlock(InstructionBlock instructionBlock, Instruction instruction)
    {
        return instruction.Offset > instructionBlock.Instructions.Last().Offset;
    }

    private void RewriteLoggingInBody(FieldDefinition iLoggerField, MethodBody methodBody, InstructionBlock loggingInstructionBlock)
    {
        var logInstruction = loggingInstructionBlock.Instructions[loggingInstructionBlock.LogInstructionIndex];
        var logLevel = _logLevelValues.First(x => logInstruction.ToString().Contains(x.Name));

        LogDebug($"Weaving logging block for LogLevel: [{logLevel.Name}]");

        if (_isDebugBuild)
        {
            methodBody.Variables.Add(new VariableDefinition(ModuleDefinition.TypeSystem.Boolean));
        }

        var il = methodBody.GetILProcessor();

        var firstInstruction = il.Create(OpCodes.Ldarg_0);
        var beginInstruction = loggingInstructionBlock.Instructions[0];

        il.InsertBefore(beginInstruction, firstInstruction);

        ReplaceExceptionHandlerStartWithFirstInstruction(methodBody, beginInstruction, firstInstruction);
        ReplacePreviousBeginInstructionOffsetsWithFirstInstruction(il, methodBody, beginInstruction, firstInstruction);

        il.InsertBefore(beginInstruction, il.Create(OpCodes.Ldfld, iLoggerField));
        il.InsertBefore(beginInstruction, il.Create(ConvertLogLevelToOpCode(logLevel.Value)));
        il.InsertBefore(beginInstruction, il.Create(OpCodes.Callvirt, CreateMethodReference("Microsoft.Extensions.Logging.ILogger", "IsEnabled")));

        var lastInstruction = loggingInstructionBlock.Instructions.Last();
        if (_isDebugBuild)
        {
            il.InsertBefore(beginInstruction, il.Create(OpCodes.Stloc_0));
            il.InsertBefore(beginInstruction, il.Create(OpCodes.Ldloc_0));
            var nop = il.Create(OpCodes.Nop);
            il.InsertAfter(lastInstruction, nop);
            lastInstruction = nop;
        }

        var gotoInstruction = FindGotoInstruction(methodBody, lastInstruction);

        il.InsertBefore(beginInstruction, il.Create(OpCodes.Brfalse_S, gotoInstruction));

        if (_isDebugBuild)
        {
            il.InsertBefore(beginInstruction, il.Create(OpCodes.Nop));
            methodBody.InitLocals = true;
        }
    }

    private static Instruction FindGotoInstruction(MethodBody methodBody, Instruction lastInstruction)
    {
        var gotoInstruction = lastInstruction.Next;
        if (gotoInstruction.OpCode != OpCodes.Ret)
        {
            return gotoInstruction;
        }

        var startIndex = methodBody.Instructions.IndexOf(gotoInstruction);
        for (int index = startIndex + 1; index < methodBody.Instructions.Count; index++)
        {
            var instruction = methodBody.Instructions[index];
            if (instruction.OpCode == OpCodes.Ret)
            {
                gotoInstruction = instruction;
                break;
            }
        }

        return gotoInstruction;
    }

    private void ReplacePreviousBeginInstructionOffsetsWithFirstInstruction(ILProcessor il, MethodBody methodBody, Instruction beginInstruction, Instruction firstInstruction)
    {
        var beginInstructionIndex = methodBody.Instructions.IndexOf(beginInstruction);

        for (int index = beginInstructionIndex; index >= 0; index--)
        {
            var instruction = methodBody.Instructions[index];
            if (!(instruction.Operand is Instruction instructionOperand))
            {
                continue;
            }

            if (instructionOperand.Offset == beginInstruction.Offset)
            {
                il.Replace(instruction, il.Create(instruction.OpCode, firstInstruction));
            }
        }
    }

    private void ReplaceExceptionHandlerStartWithFirstInstruction(MethodBody methodBody, Instruction beginInstruction, Instruction firstInstruction)
    {
        if (!methodBody.HasExceptionHandlers)
        {
            return;
        }

        foreach (var exceptionHandler in methodBody.ExceptionHandlers)
        {
            if (beginInstruction.Offset != exceptionHandler.TryStart.Offset)
            {
                continue;
            }

            if (exceptionHandler.TryStart is Instruction)
            {
                exceptionHandler.TryStart = firstInstruction;
                return;
            }
        }
    }

    private MethodReference CreateMethodReference(string typeName, string methodName)
    {
        return ModuleDefinition.ImportReference(FindType(typeName).Methods.Single(x => x.Name == methodName));
    }

    private OpCode ConvertLogLevelToOpCode(int value)
    {
        switch (value)
        {
            case 0:
                return OpCodes.Ldc_I4_0;

            case 1:
                return OpCodes.Ldc_I4_1;

            case 2:
                return OpCodes.Ldc_I4_2;

            case 3:
                return OpCodes.Ldc_I4_3;

            case 4:
                return OpCodes.Ldc_I4_4;

            case 5:
                return OpCodes.Ldc_I4_5;

            default:
                return OpCodes.Ldc_I4_0;
        }
    }

    private List<EnumValue> GetEnumValuesOfType(string typeName)
    {
        var type = FindType(typeName);
        if (!type.IsEnum)
        {
            return Enumerable.Empty<EnumValue>().ToList();
        }

        var enumValues = new List<EnumValue>();
        foreach (var field in type.Fields)
        {
            if (!field.HasConstant)
            {
                continue;
            }

            enumValues.Add(new EnumValue { Value = (int)field.Constant, Name = field.Name });
        }

        return enumValues;
    }
}