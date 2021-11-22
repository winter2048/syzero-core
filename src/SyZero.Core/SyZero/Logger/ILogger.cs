using System;
using SyZero.Dependency;

namespace SyZero.Logger
{
    public interface ILogger : ISingletonDependency
    {
        void Info(string message, Exception exception = null);

        void Warn(string message, Exception exception = null);

        void Error(string message, Exception exception = null);
    }
}
