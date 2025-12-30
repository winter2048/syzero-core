using SqlSugar;
using SyZero.SqlSugar.DbContext;

namespace SyZero.Example2.Core.DbContext
{
    public class Example2DbContext : SyZeroDbContext
    {
        public Example2DbContext(ConnectionConfig config) : base(config)
        {

        }
    }
}
