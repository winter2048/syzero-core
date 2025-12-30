using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Log4Net
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly ILog _log;

        public Log4NetLoggerProvider(string loggerName = "DefaultLogger")
        {
            _log = LogManager.GetLogger(loggerName);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Log4NetLogger(_log, categoryName);
        }

        public void Dispose()
        {
            // 如果需要，可以添加清理逻辑
        }
    }
}
