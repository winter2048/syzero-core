using System.Threading.Tasks;
using SyZero.Application.Routing;
using SyZero.Application.Service;
using SyZero.Application.Service.Dto;
using SyZero.Example1.IApplication.Examples.Dto;

namespace SyZero.Example1.IApplication.Examples
{
    public interface IExampleAppService : IAsyncCrudAppService<ExampleDto, PageAndSortQueryDto, CreateExampleDto>, IApplicationServiceBase
    {
        /// <summary>
        /// 根据ID获取示例
        /// </summary>
        /// <param name="id">示例ID</param>
        /// <returns></returns>
        [Get("GetExample")]
        Task<ExampleDto> GetExample(long id);

        /// <summary>
        /// 根据名称获取示例
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [Get("GetByName")]
        Task<ExampleDto> GetByName(string name);
    }
}
