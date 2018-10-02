## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

Adds ILogger.IsEnabled(LogLevel) check around the logging statement. To reduce the boilerplate code, but still have the performance benefit when a certain LogLevel is turned off.


## Usage

See also [Fody usage](https://github.com/Fody/Fody#usage).


### NuGet installation

Install the [LoggerIsEnabled.Fody NuGet package](https://nuget.org/packages/LoggerIsEnabled.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```
PM> Install-Package LoggerIsEnabled.Fody
PM> Update-Package Fody
```

The `Update-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<LoggerIsEnabled/>` to [FodyWeavers.xml](https://github.com/Fody/Fody#add-fodyweaversxml)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
  <LoggerIsEnabled/>
</Weavers>
```


### Your Code

```c#
public class Example
{
    private readonly ILogger _logger;

    public Example(ILogger logger)
    {
        _logger = logger;
    }

    public void MethodWithLogging()
    {
        _logger.LogTrace("message");
    }
}
```

### What gets compiled

```c#

public class Example
{
    private readonly ILogger _logger;

    public Example(ILogger logger)
    {
        _logger = logger;
    }

    public void MethodWithLogging()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("message");
        }
    }
}
```