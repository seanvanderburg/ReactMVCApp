using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;

public class LoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new Logger();
    }

    public void Dispose() { }

    private class Logger : ILogger
    {
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            File.AppendAllText(@"C:\temp\log.txt", formatter(state, exception));
            Console.WriteLine(formatter(state, exception));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    } 
}