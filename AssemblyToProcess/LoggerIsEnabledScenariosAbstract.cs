using Microsoft.Extensions.Logging;

public abstract class LoggerIsEnabledScenariosAbstract
{
    protected readonly ILogger _logger;

    public LoggerIsEnabledScenariosAbstract(ILogger logger)
    {
        _logger = logger;
    }

    public void LogTrace()
    {
        _logger.LogTrace("message");
    }

    public abstract void LogDebug();
}