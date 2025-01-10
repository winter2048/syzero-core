using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SyZero.Logger;

namespace SyZero.Log4Net
{
    public class Logger<T> : ILogger<T>, ILogger
    {
        private ILog logger;
        public Logger()
        {
            logger = LogManager.GetLogger(typeof(T));
        }
        public void Error(string message, Exception exception = null)
        {
            Console.WriteLine("Error:" + message);
            if (exception == null)
                logger.Error(message);
            else
                logger.Error(message, exception);
        }

        public void Info(string message, Exception exception = null)
        {
            Console.WriteLine("Info:" + message);
            if (exception == null)
                logger.Info(message);
            else
                logger.Info(message, exception);
        }

        public void Warn(string message, Exception exception = null)
        {
            Console.WriteLine("Warn:" + message);
            if (exception == null)
                logger.Warn(message);
            else
                logger.Warn(message, exception);
        }
    }
}
