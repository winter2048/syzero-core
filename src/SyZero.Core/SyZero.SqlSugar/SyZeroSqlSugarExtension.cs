using Autofac;
using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SyZero.SqlSugar.DbContext;
using SyZero.Util;
using SyZero.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using SyZero.SqlSugar.Repositories;
using SyZero.Domain.Repository;
using SyZero.SqlSugar;
using System;
using System.Reflection;

namespace SyZero
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
                                var databaseGeneratedAttribute = (DatabaseGeneratedAttribute)attributes.FirstOrDefault(it => it is DatabaseGeneratedAttribute);
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

                            // int?  decimal?这种 isnullable=true
                            if (property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                column.IsNullable = true;
                            }
                            else if (property.PropertyType == typeof(string) &&
                                     property.GetCustomAttribute<RequiredAttribute>() == null)
                            {
                                //string类型如果没有Required isnullable=true
                                column.IsNullable = true;
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
            }).As<ConnectionConfig>().SingleInstance().PropertiesAutowired();

            // 注册 DbContext
            builder.RegisterType<TContext>()
                .As<ISyZeroDbContext>()
                .InstancePerLifetimeScope().PropertiesAutowired();

            //注册仓储泛型
            builder.RegisterGeneric(typeof(SqlSugarRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope().PropertiesAutowired();
            ////注册持久化
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope().PropertiesAutowired();

            return builder;
        }

        /// <summary>
        /// 注册SqlSugar
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZeroSqlSuga(this ContainerBuilder builder)
        {
            builder.AddSyZeroSqlSugar<SyZeroDbContext>();
            return builder;
        }

        /// <summary>
        /// 初始化表
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder InitTables(this IApplicationBuilder app)
        {
            System.Console.WriteLine("检查数据库,初始化表...");
            AutofacUtil.GetService<ISyZeroDbContext>()
            .CodeFirst.SetStringDefaultLength(200)
            .InitTables(ReflectionHelper.GetTypes()
            .Where(m => typeof(IEntity).IsAssignableFrom(m) && m != typeof(IEntity) && m != typeof(Entity))
            .ToArray());
            return app;
        }
    }
}
