using System;
using System.Collections.Generic;
using System.Text;
using Autofac;


namespace SyZero.Web.Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonSerialize>().As<IJsonSerialize>().SingleInstance();
            builder.RegisterType<XmlSerialize>().As<IXmlSerialize>().SingleInstance();
        }
    }
}

