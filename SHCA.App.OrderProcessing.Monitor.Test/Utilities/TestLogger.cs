using Microsoft.Extensions.Logging;

public class TestLogger<T> : ILogger<T>
{
    public List<LogEntry> LogEntries { get; } = new List<LogEntry>();

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        LogEntries.Add(new LogEntry
        {
            LogLevel = logLevel,
            Message = formatter(state, exception)
        });
    }
}

public class LogEntry
{
    public LogLevel LogLevel { get; set; }
    public string Message { get; set; }
}