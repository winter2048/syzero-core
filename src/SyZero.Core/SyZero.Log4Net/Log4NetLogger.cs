using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Log4Net
{
    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        public void Dispose() { }
    }
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;
        private readonly string _categoryName;

        public Log4NetLogger(ILog log, string categoryName)
        {
            _log = log;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // 可实现：追踪作用域，维持上下文
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // 根据log4net配置映射日志级别
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                default:
                    return false;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message) && exception == null)
                return;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    if (_log.IsDebugEnabled)
                        _log.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    if (_log.IsInfoEnabled)
                        _log.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    if (_log.IsWarnEnabled)
                        _log.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    if (_log.IsErrorEnabled)
                        _log.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    if (_log.IsFatalEnabled)
                        _log.Fatal(message, exception);
                    break;
            }
        }
    }
}
