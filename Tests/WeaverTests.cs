using Fody;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq.Expressions;
using Xunit;
using ApprovalTests;
using ApprovalTests.Namers;

#if(DEBUG)
[UseApprovalSubdirectory("approvals-debug")]
#else
[UseApprovalSubdirectory("approvals-release")]
#endif

#pragma warning disable 618

public class WeaverTests:IDisposable
{
    private static readonly TestResult _testResult;
    private static readonly Type _type;

    private readonly Expression<Action<ILogger>> _logAction = x => x.Log(It.IsAny<LogLevel>(), 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>());
    private IDisposable uniqueForRuntime;

    static WeaverTests()
    {
        var weavingTask = new ModuleWeaver();
        _testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll", runPeVerify: false);
        _type = _testResult.Assembly.GetType("LoggerIsEnabledScenarios");
    }

    public WeaverTests()
    {
        uniqueForRuntime = ApprovalResults.UniqueForRuntime();
    }

    [Fact]
    public void LogTrace_IsEnabledTrue()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_IsEnabledFalse()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(false);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Never);
    }

    [Fact]
    public void LogDebug_IsEnabledTrue()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogDebug();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Debug), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogDebug_IsEnabledFalse()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogDebug();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Debug), Times.Once);
        mockLogger.Verify(_logAction, Times.Never);
    }

    [Fact]
    public void LogInformation_IsEnabledTrue()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogInformation();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Information), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogInformation_IsEnabledFalse()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(false);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogInformation();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Information), Times.Once);
        mockLogger.Verify(_logAction, Times.Never);
    }

    [Fact]
    public void LogWarning_IsEnabledTrue()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogWarning();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Warning), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogWarning_IsEnabledFalse()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(false);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogWarning();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Warning), Times.Once);
        mockLogger.Verify(_logAction, Times.Never);
    }

    [Fact]
    public void LogError_IsEnabledTrue()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Error)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogError();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Error), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogError_IsEnabledFalse()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Error)).Returns(false);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogError();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Error), Times.Once);
        mockLogger.Verify(_logAction, Times.Never);
    }

    [Fact]
    public void LogCritical_IsEnabledTrue()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Critical)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogCritical();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Critical), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogCritical_IsEnabledFalse()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Critical)).Returns(false);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogCritical();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Critical), Times.Once);
        mockLogger.Verify(_logAction, Times.Never);
    }

    [Fact]
    public void LogTraceWithEnabled()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTraceWithEnabled();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTraceWithEnabled_With_Code_Before()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTraceWithEnabled_With_Code_Before();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTraceWithEnabled_With_Code_After()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTraceWithEnabled_With_Code_After();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTraceWithEnabled_With_Code_Before_And_After()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTraceWithEnabled_With_Code_Before_And_After();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_With_Code_Before()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_With_Code_Before();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_With_Code_After()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_With_Code_After();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_With_Code_Before_And_After()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_With_Code_Before_And_After();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_Multiple()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_Multiple();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Exactly(2));
        mockLogger.Verify(_logAction, Times.Exactly(2));
    }

    [Fact]
    public void LogTrace_Multiple_With_First_IsEnabled()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_Multiple_With_First_IsEnabled();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Exactly(2));
        mockLogger.Verify(_logAction, Times.Exactly(2));
    }

    [Fact]
    public void LogTrace_Multiple_With_Second_IsEnabled()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_Multiple_With_Second_IsEnabled();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Exactly(2));
        mockLogger.Verify(_logAction, Times.Exactly(2));
    }

    [Fact]
    public void LogTrace_Multiple_With_Code()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_Multiple_With_Code();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Exactly(2));
        mockLogger.Verify(_logAction, Times.Exactly(2));
    }

    [Fact]
    public void LogTrace_In_Switch()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Switch();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Switch_With_Before_Code()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Switch_With_Before_Code();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Switch_With_After_Code()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Switch_With_After_Code();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Switch_With_Before_And_After_Code()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Switch_With_Before_And_After_Code();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_Code()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception_With_Before_Code();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception_With_After_Code()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception_With_After_Code();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_And_After_Code()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception_With_Before_And_After_Code();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_Code_In_Try()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception_With_Before_Code_In_Try();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception_With_After_Code_In_Try()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception_With_After_Code_In_Try();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_And_After_Code_In_Try()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception_With_Before_And_After_Code_In_Try();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void LogTrace_In_Exception_Catch()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);
        mockLogger.Setup(_logAction);

        var instance = (dynamic) Activator.CreateInstance(_type, mockLogger.Object);

        instance.LogTrace_In_Exception_Catch();

        mockLogger.Verify(x => x.IsEnabled(LogLevel.Trace), Times.Once);
        mockLogger.Verify(_logAction, Times.Once);
    }

    [Fact]
    public void Interface_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "ILoggerIsEnabledScenarios"));
    }

    [Fact]
    public void Abstract_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenariosAbstract"));
    }

    [Fact]
    public void AbstractImplementation_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenariosAbstractImplementation"));
    }

    [Fact]
    public void Enum_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledEnum"));
    }

    [Fact]
    public void LogTrace_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace"));
    }

    [Fact]
    public void LogDebug_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogDebug"));
    }

    [Fact]
    public void LogInformation_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogInformation"));
    }

    [Fact]
    public void LogWarning_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogWarning"));
    }

    [Fact]
    public void LogError_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogError"));
    }

    [Fact]
    public void LogCritical_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogCritical"));
    }

    [Fact]
    public void LogTraceWithEnabled_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTraceWithEnabled"));
    }

    [Fact]
    public void LogTraceWithEnabled_With_Code_Before_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTraceWithEnabled_With_Code_Before"));
    }

    [Fact]
    public void LogTraceWithEnabled_With_Code_After_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTraceWithEnabled_With_Code_After"));
    }

    [Fact]
    public void LogTraceWithEnabled_With_Code_Before_And_After_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTraceWithEnabled_With_Code_Before_And_After"));
    }

    [Fact]
    public void LogTrace_With_Code_Before_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_With_Code_Before"));
    }

    [Fact]
    public void LogTrace_With_Code_After_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_With_Code_After"));
    }

    [Fact]
    public void LogTrace_With_Code_Before_And_After_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_With_Code_Before_And_After"));
    }

    [Fact]
    public void LogTrace_Multiple_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_Multiple"));
    }

    [Fact]
    public void LogTrace_Multiple_With_First_IsEnabled_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_Multiple_With_First_IsEnabled"));
    }

    [Fact]
    public void LogTrace_Multiple_With_Second_IsEnabled_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_Multiple_With_Second_IsEnabled"));
    }

    [Fact]
    public void LogTrace_Multiple_With_Code_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_Multiple_With_Code"));
    }

    [Fact]
    public void LogTrace_In_Switch_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Switch"));
    }

    [Fact]
    public void LogTrace_In_Switch_With_Before_Code_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Switch_With_Before_Code"));
    }

    [Fact]
    public void LogTrace_In_Switch_With_After_Code_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Switch_With_After_Code"));
    }

    [Fact]
    public void LogTrace_In_Switch_With_Before_And_After_Code_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Switch_With_Before_And_After_Code"));
    }

    [Fact]
    public void LogTrace_In_Exception_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception"));
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_Code_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception_With_Before_Code"));
    }

    [Fact]
    public void LogTrace_In_Exception_With_After_Code_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception_With_After_Code"));
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_And_After_Code_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception_With_Before_And_After_Code"));
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_Code_In_Try_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception_With_Before_Code_In_Try"));
    }

    [Fact]
    public void LogTrace_In_Exception_With_After_Code_In_Try_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception_With_After_Code_In_Try"));
    }

    [Fact]
    public void LogTrace_In_Exception_With_Before_And_After_Code_In_Try_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception_With_Before_And_After_Code_In_Try"));
    }

    [Fact]
    public void LogTrace_In_Exception_Catch_Decompiled()
    {
        Approvals.Verify(Ildasm.Decompile(_testResult.AssemblyPath, "LoggerIsEnabledScenarios::LogTrace_In_Exception_Catch"));
    }

    public void Dispose()
    {
        uniqueForRuntime.Dispose();
    }
}