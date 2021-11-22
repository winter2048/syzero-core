using Autofac;
using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SyZero.SqlSugar.DbContext;

namespace SyZero.SqlSugar
{
    public static class SyZeroSqlSugarExtension
    {
        /// <summary>
        /// 注册SqlSugar
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZeroSqlSugar<TContext>(this ContainerBuilder builder)
            where TContext : SyZeroDbContext
        {
            builder.Register(p =>
            {
                ConnectionConfig connection = new ConnectionConfig()
                {
                    ConnectionString = AppConfig.ConnectionOptions.Master,
                    DbType = (DbType)AppConfig.ConnectionOptions.Type,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    SlaveConnectionConfigs = AppConfig.ConnectionOptions.Slave.Select(p => new SlaveConnectionConfig()
                    {
                        HitRate = p.Value,
                        ConnectionString = p.Key
                    }).ToList(),
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityService = (property, column) =>
                        {
                            var attributes = property.GetCustomAttributes(true);//get all attributes 
                            if (attributes.Any(it => it is KeyAttribute))// by attribute set primarykey
                            {
                                column.IsPrimarykey = true;
                            }
                            if (attributes.Any(it => it is DatabaseGeneratedAttribute))
                            {
                                var databaseGeneratedAttribute  = (DatabaseGeneratedAttribute)attributes.FirstOrDefault(it => it is DatabaseGeneratedAttribute);
                                column.IsIdentity = databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity;
                            }
                            if (attributes.Any(it => it is ColumnAttribute))
                            {
                                var columnAttribute = (ColumnAttribute)attributes.FirstOrDefault(it => it is ColumnAttribute);
                                column.DbColumnName = columnAttribute.Name;
                            }
                            if (attributes.Any(it => it is NotMappedAttribute))
                            {
                                column.IsIgnore = true;
                            }
                        },
                        EntityNameService = (type, entity) =>
                        {
                            var attributes = type.GetCustomAttributes(true);
                            if (attributes.Any(it => it is TableAttribute))
                            {
                                entity.DbTableName = (attributes.First(it => it is TableAttribute) as TableAttribute).Name;
                            }
                        }
                    }
                };
                return connection;
            }).As<ConnectionConfig>().InstancePerLifetimeScope().PropertiesAutowired();

            // 注册 DbContext
            builder.RegisterType<TContext>()
                .AsSelf()
                .InstancePerLifetimeScope().PropertiesAutowired();
            return builder;
        }
    }
}
