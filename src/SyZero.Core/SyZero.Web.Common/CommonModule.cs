using Autofac;
using SyZero.Client;
using SyZero.Runtime.Security;
using SyZero.Serialization;
using SyZero.Web.Common.Jwt;
using SyZero.Web.Common.Util;

namespace SyZero.Web.Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonSerialize>().As<IJsonSerialize>().SingleInstance();
            builder.RegisterType<XmlSerialize>().As<IXmlSerialize>().SingleInstance();
            builder.RegisterType<SyEncode>().As<ISyEncode>().SingleInstance();
            builder.RegisterType<JwtToken>().As<IToken>().SingleInstance();
            builder.RegisterType<PrizeUtil>().As<IPrizeUtil>().SingleInstance();
            builder.RegisterType<AliasMethod>().As<IAliasMethod>().InstancePerLifetimeScope();
            builder.RegisterType<HttpRestClient>().As<IClient>().InstancePerLifetimeScope();
        }
    }
}

