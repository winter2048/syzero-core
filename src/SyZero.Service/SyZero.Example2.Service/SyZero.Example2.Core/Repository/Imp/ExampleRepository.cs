using SyZero.Example2.Core.Entities;
using SyZero.SqlSugar.Repositories;

namespace SyZero.Example2.Core.Repository.Imp
{
    public class ExampleRepository : SqlSugarRepository<Example>, IExampleRepository
    {
    }
}
