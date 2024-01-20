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
using Microsoft.Extensions.DependencyInjection;
using SyZero.Extension;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SyZero
{
    public static class SyZeroSqlSugarExtension
    {
        /// <summary>
        /// 注册SqlSugar
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZeroSqlSugar<TContext>(this IServiceCollection services)
            where TContext : SyZeroDbContext
        {
            services.AddSingleton<ConnectionConfig>(p =>
            {
                ConnectionConfig connection = new ConnectionConfig()
                {
                    ConnectionString = AppConfig.ConnectionOptions.Master,
                    DbType = (DbType)AppConfig.ConnectionOptions.Type,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    SlaveConnectionConfigs = AppConfig.ConnectionOptions.Slave.Select(p => new SlaveConnectionConfig()
                    {
                        HitRate = p.HitRate,
                        ConnectionString = p.ConnectionString
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
            });
            //注册上下文
            services.AddScoped<ISyZeroDbContext, TContext>();
            //注册仓储泛型
            //services.AddScoped(typeof(IRepository<>), typeof(SqlSugarRepository<>));
            services.AddClassesAsImplementedInterface(typeof(IRepository<>));
            ////注册持久化
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// 注册SqlSugar
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZeroSqlSuga(this IServiceCollection services)
        {
            services.AddSyZeroSqlSugar<SyZeroDbContext>();
            return services;
        }

        /// <summary>
        /// 初始化表
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder InitTables(this IApplicationBuilder app)
        {
            System.Console.WriteLine("检查数据库,初始化表...");
            SyZeroUtil.GetService<ISyZeroDbContext>()
            .CodeFirst.SetStringDefaultLength(200)
            .InitTables(ReflectionHelper.GetTypes()
            .Where(m => typeof(IEntity).IsAssignableFrom(m) && m != typeof(IEntity) && m != typeof(Entity))
            .ToArray());
            return app;
        }
    }
}
