using Microsoft.Extensions.Logging;

public abstract class LoggerIsEnabledSenariosAbstract
{
    protected readonly ILogger _logger;

    public LoggerIsEnabledSenariosAbstract(ILogger logger)
    {
        _logger = logger;
    }

    public void LogTrace()
    {
        _logger.LogTrace("message");
    }

    public abstract void LogDebug();
}