using SqlSugar;
using SyZero.SqlSugar.DbContext;

namespace SyZero.Example1.Core.DbContext
{
    public class Example1DbContext : SyZeroDbContext
    {
        public Example1DbContext(ConnectionConfig config) : base(config)
        {

        }
    }
}
