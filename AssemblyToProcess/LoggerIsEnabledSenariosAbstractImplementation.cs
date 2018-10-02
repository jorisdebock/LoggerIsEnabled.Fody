using Microsoft.Extensions.Logging;

public sealed class LoggerIsEnabledSenariosAbstractImplementation : LoggerIsEnabledSenariosAbstract
{
    public LoggerIsEnabledSenariosAbstractImplementation(ILogger logger) : base(logger)
    {
    }

    public override void LogDebug()
    {
        _logger.LogDebug("message");
    }
}