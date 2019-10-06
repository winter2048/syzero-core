using System;
using System.Collections.Generic;
using Autofac;
using System.Text;
using SyZero.Logger;

namespace SyZero.Log4Net
{
    public class Log4NetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        }
    }
}
