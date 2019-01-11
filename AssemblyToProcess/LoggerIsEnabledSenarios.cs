using Microsoft.Extensions.Logging;
using System;

public class LoggerIsEnabledScenarios
{
    private readonly ILogger _logger;

    public LoggerIsEnabledScenarios(ILogger logger)
    {
        _logger = logger;
    }

    public void LogTrace()
    {
        _logger.LogTrace("message");
    }

    public void LogDebug()
    {
        _logger.LogDebug("message");
    }

    public void LogInformation()
    {
        _logger.LogInformation("message");
    }

    public void LogWarning()
    {
        _logger.LogWarning("message");
    }

    public void LogError()
    {
        _logger.LogError("message");
    }

    public void LogCritical()
    {
        _logger.LogCritical("message");
    }

    public void LogTraceWithEnabled()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("message");
        }
    }

    public void LogTraceWithEnabled_With_Code_Before()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            CodeBefore();
            _logger.LogTrace("message");
        }
    }

    public void LogTraceWithEnabled_With_Code_After()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("message");
            CodeAfter();
        }
    }

    public void LogTraceWithEnabled_With_Code_Before_And_After()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            CodeBefore();
            _logger.LogTrace("message");
            CodeAfter();
        }
    }

    public void LogTrace_With_Code_Before()
    {
        CodeBefore();
        _logger.LogTrace("message");
    }

    public void LogTrace_With_Code_After()
    {
        _logger.LogTrace("message");
        CodeAfter();
    }

    public void LogTrace_With_Code_Before_And_After()
    {
        CodeBefore();
        _logger.LogTrace("message");
        CodeAfter();
    }

    public void LogTrace_Multiple()
    {
        _logger.LogTrace("message");
        _logger.LogTrace("message");
    }

    public void LogTrace_Multiple_With_First_IsEnabled()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("message");
        }
        _logger.LogTrace("message");
    }

    public void LogTrace_Multiple_With_Second_IsEnabled()
    {
        _logger.LogTrace("message");
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("message");
        }
    }

    public void LogTrace_Multiple_With_Code()
    {
        CodeBefore();
        _logger.LogTrace("message");
        CodeBefore();
        _logger.LogTrace("message");
    }

    public void LogTrace_In_Switch()
    {
        var number = 2 / 2; // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                _logger.LogTrace("message");
                break;

            default:
                CodeAfter();
                break;
        }
    }

    public void LogTrace_In_Switch2()
    {
        var number = 2 / 2; // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace("message");
                }
                break;

            default:
                CodeAfter();
                break;
        }
    }

    public void LogTrace_In_Switch_With_Before_Code()
    {
        var number = 2 / 2;  // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                CodeBefore();
                _logger.LogTrace("message");
                break;

            default:
                CodeAfter();
                break;
        }
    }

    public void LogTrace_In_Switch_With_Before_Code2()
    {
        var number = 2 / 2;  // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                CodeBefore();
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace("message");
                }
                break;

            default:
                CodeAfter();
                break;
        }
    }

    public void LogTrace_In_Switch_With_After_Code()
    {
        var number = 2 / 2;  // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                _logger.LogTrace("message");
                CodeAfter();
                break;

            default:
                CodeAfter();
                break;
        }
    }

    public void LogTrace_In_Switch_With_After_Code2()
    {
        var number = 2 / 2;  // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace("message");
                }
                CodeAfter();
                break;

            default:
                CodeAfter();
                break;
        }
    }

    public void LogTrace_In_Switch_With_Before_And_After_Code()
    {
        var number = 2 / 2;  // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                CodeBefore();
                _logger.LogTrace("message");
                CodeAfter();
                break;

            default:
                CodeAfter();
                break;
        }
    }

    public void LogTrace_In_Switch_With_Before_And_After_Code2()
    {
        var number = 2 / 2;  // trick compiler to not remove unreachable code
        switch (number)
        {
            case 0:
                CodeBefore();
                break;

            case 1:
                CodeBefore();
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace("message");
                }
                CodeAfter();
                break;

            default:
                CodeAfter();
                break;
        }
    }

#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.

    public void LogTrace_In_Exception()
    {
        try
        {
            _logger.LogTrace("message");
        }
        catch (Exception)
        {
        }
    }

    public void LogTrace_In_Exception_With_Before_Code()
    {
        CodeBefore();
        try
        {
            _logger.LogTrace("message");
        }
        catch (Exception)
        {
        }
    }

    public void LogTrace_In_Exception_With_After_Code()
    {
        try
        {
            _logger.LogTrace("message");
        }
        catch (Exception)
        {
        }

        CodeAfter();
    }

    public void LogTrace_In_Exception_With_Before_And_After_Code()
    {
        CodeBefore();
        try
        {
            _logger.LogTrace("message");
        }
        catch (Exception)
        {
        }

        CodeAfter();
    }

    public void LogTrace_In_Exception_With_Before_Code_In_Try()
    {
        try
        {
            CodeBefore();
            _logger.LogTrace("message");
        }
        catch (Exception)
        {
        }
    }

    public void LogTrace_In_Exception_With_After_Code_In_Try()
    {
        try
        {
            _logger.LogTrace("message");
            CodeAfter();
        }
        catch (Exception)
        {
        }
    }

    public void LogTrace_In_Exception_With_Before_And_After_Code_In_Try()
    {
        try
        {
            CodeBefore();
            _logger.LogTrace("message");
            CodeAfter();
        }
        catch (Exception)
        {
        }
    }

#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.

    public void LogTrace_In_Exception_Catch()
    {
        try
        {
            CodeThrowException();
        }
        catch (Exception)
        {
            _logger.LogTrace("message");
        }
    }

    private void CodeBefore()
    {
        // some random code to fill in
        string z = null;
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                z += i.ToString();
            }
        }
    }

    private void CodeAfter()
    {
        // some random code to fill in
        string z = null;
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                z += i.ToString();
            }
        }
    }

    private void CodeThrowException()
    {
        CodeBefore();
        throw new Exception();
    }
}