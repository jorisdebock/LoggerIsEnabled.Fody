using Mono.Cecil;
using System.Diagnostics;
using System.Linq;

public static class CecilExtensions
{
    public static bool IsDebugBuild(this AssemblyDefinition assemblyDefinition)
    {
        var debuggableAttributeDefinition = assemblyDefinition.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == typeof(DebuggableAttribute).FullName);
        if (debuggableAttributeDefinition == null)
        {
            return false;
        }

        var debuggableAttribute = new DebuggableAttribute((DebuggableAttribute.DebuggingModes)debuggableAttributeDefinition.ConstructorArguments[0].Value);

        return debuggableAttribute.IsJITOptimizerDisabled;
    }

    public static FieldDefinition GetFieldDefintion(this TypeDefinition typeDefinition, string fullName)
    {
        var field = typeDefinition.Fields.FirstOrDefault(x => x.FieldType.FullName.Contains(fullName));
        if (field == null)
        {
            field = typeDefinition.BaseType?.Resolve().Fields.FirstOrDefault(x => x.FieldType.FullName.Contains(fullName));
        }

        return field;
    }
}