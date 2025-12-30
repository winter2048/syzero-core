using SyZero.Example1.Core.Entities;
using SyZero.SqlSugar.Repositories;

namespace SyZero.Example1.Core.Repository.Imp
{
    public class ExampleRepository : SqlSugarRepository<Example>, IExampleRepository
    {
    }
}
