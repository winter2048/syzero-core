using System;
using SyZero.Dependency;

namespace SyZero.Logger
{
    public interface ILoggerBase
    {
        void Info(string message, Exception exception = null);

        void Warn(string message, Exception exception = null);

        void Error(string message, Exception exception = null);
    }

    public interface ILogger<T> : ILoggerBase, ISingletonDependency
    {
    }

    public interface ILogger : ILoggerBase, ISingletonDependency
    {
    }
}
