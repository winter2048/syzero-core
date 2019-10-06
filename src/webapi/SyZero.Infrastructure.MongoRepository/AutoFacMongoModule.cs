
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using SyZero.Domain.Repository;

namespace SyZero.MongoDB
{
    public class AutoFacMongoModule : Module
    {
        private IConfigurationSection _configurationSection;
        public AutoFacMongoModule(IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var optionsBuilder = new ConfigurationBuilder();
                optionsBuilder.AddConfiguration(_configurationSection);
                return optionsBuilder;
            });
            builder.RegisterType<MongoContext>().As<IMongoContext>();
            builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();//注册仓储泛型
        }
    }
}
