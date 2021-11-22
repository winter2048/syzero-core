
using Autofac;
using Microsoft.Extensions.Configuration;
using SyZero.Domain.Repository;

namespace SyZero.MongoDB
{
    public class MongoDBModule : Module
    {
        private IConfigurationSection _configurationSection { get; set; }
        public MongoDBModule()
        {

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
