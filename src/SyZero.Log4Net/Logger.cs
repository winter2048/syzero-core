using log4net;
using log4net.Config;
using System;
using System.IO;
using SyZero.Logger;

namespace SyZero.Log4Net
{
    public class Logger : ILogger
    {
        private ILog logger;
        public Logger()
        {
            if (logger == null)
            {
                var repository = LogManager.CreateRepository("NETCoreRepository");
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                logger = LogManager.GetLogger(repository.Name, "InfoLogger");
            }
        }
        public void Error(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Error(message);
            else
                logger.Error(message, exception);
        }

        public void Info(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Info(message);
            else
                logger.Info(message, exception);
        }

        public void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Warn(message);
            else
                logger.Warn(message, exception);
        }
    }
}
