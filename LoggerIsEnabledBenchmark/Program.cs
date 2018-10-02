using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace LoggerIsEnabledBenchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var job = Job.Core.WithLaunchCount(10);

            var summery = BenchmarkRunner.Run<LoggingPattern>();
        }
    }

    [Config(typeof(Config))]
    public class LoggingPattern
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.Core);
            }
        }

        private ILogger<LoggingPattern> _logger;

        [Params(0, 1, 2)]
        public int NLogConfigNumber { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var loggerFactory = new LoggerFactory().AddNLog();
            NLog.LogManager.LoadConfiguration($"nlog{NLogConfigNumber}.config");

            _logger = loggerFactory.CreateLogger<LoggingPattern>();
        }

        [Benchmark(Baseline = true)]
        public void Logging_Default()
        {
            _logger.LogInformation("test");
        }

        [Benchmark]
        public void Logging_With_IsEnabled_Check()
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("test");
            }
        }

        [Benchmark]
        public void Logging_With_Formatting()
        {
            _logger.LogInformation($"test {nameof(Program)}");
        }

        [Benchmark]
        public void Logging_With_Formatting_And_IsEnabled_Check()
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"test {nameof(Program)}");
            }
        }

        [Benchmark]
        public void Logging_With_BuildIn2()
        {
            _logger.LogInformation("test {name}", nameof(Program));
        }

        [Benchmark]
        public void Logging_With_BuildIn_And_IsEnabled_Check2()
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("test {name}", nameof(Program));
            }
        }

        [Benchmark]
        public void Logging_With_BuildIn3()
        {
            _logger.LogInformation("test {0}", nameof(Program));
        }

        [Benchmark]
        public void Logging_With_BuildIn_And_IsEnabled_Check3()
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("test {0}", nameof(Program));
            }
        }
    }
}