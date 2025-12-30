using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyZero.Domain.Repository;

namespace SyZero.MongoDB
{
    public static class MongoDBModule
    {
        public static IServiceCollection AddMongoDBModule(this IServiceCollection builder)
        {
            //builder.Register(c =>
            //{
            //    var optionsBuilder = new ConfigurationBuilder();
            //    optionsBuilder.AddConfiguration(_configurationSection);
            //    return optionsBuilder;
            //});
            //builder.RegisterType<MongoContext>().As<IMongoContext>();
            //builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();//注册仓储泛型

            return builder;
        }
    }
}
