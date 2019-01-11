using Microsoft.Extensions.Logging;

public sealed class LoggerIsEnabledScenariosAbstractImplementation : LoggerIsEnabledScenariosAbstract
{
    public LoggerIsEnabledScenariosAbstractImplementation(ILogger logger) : base(logger)
    {
    }

    public override void LogDebug()
    {
        _logger.LogDebug("message");
    }
}