using Microsoft.Extensions.Logging;

namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class LogMessageHelper
{
    public static LogLevel GetLogLevel(this LogMessage message) => message.Severity switch
    {
        LogSeverity.Debug => LogLevel.Debug,
        LogSeverity.Verbose => LogLevel.Trace,
        LogSeverity.Info => LogLevel.Information,
        LogSeverity.Warning => LogLevel.Warning,
        LogSeverity.Error => LogLevel.Error,
        LogSeverity.Critical => LogLevel.Critical,
        _ => throw new NotImplementedException($"Severity '{message.Severity}' has not been implemented")
    };
}
