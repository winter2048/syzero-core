using SqlSugar;
using System.Collections.Generic;
using System.Linq;
namespace SyZero.SqlSugar.DbContext
{
    public class SyZeroDbContext : SqlSugarClient, ISyZeroDbContext
    {
        public SyZeroDbContext(ConnectionConfig config):base(config)
        {
            
        }
        //public SyZeroDbContext()
        //{
        //    Database = new SqlSugarClient(new ConnectionConfig()
        //    {

        //        ConnectionString = AppConfig.ConnectionOptions.Master,
        //        DbType = (DbType)AppConfig.ConnectionOptions.Type,
        //        IsAutoCloseConnection = true,
        //        InitKeyType = InitKeyType.Attribute,
        //        SlaveConnectionConfigs = AppConfig.ConnectionOptions.Slave.Select(p => new SlaveConnectionConfig()
        //        {
        //            HitRate = p.Value,
        //            ConnectionString = p.Key
        //        }).ToList()
        //    }); ;
        //}
        //public SqlSugarClient Database;//用来处理事务多表查询和复杂的操作
    }
}
