using System.Threading.Tasks;
using SyZero.Application.Routing;
using SyZero.Application.Service;
using SyZero.Application.Service.Dto;
using SyZero.Example1.IApplication.Examples.Dto;

namespace SyZero.Example1.IApplication.Examples
{
    public interface IExample1AppService : IAsyncCrudAppService<Example1Dto, PageAndSortQueryDto, CreateExample1Dto>, IApplicationServiceBase
    {
        /// <summary>
        /// 根据ID获取示例
        /// </summary>
        /// <param name="id">示例ID</param>
        /// <returns></returns>
        [Get("GetExample")]
        Task<Example1Dto> GetExample(long id);

        /// <summary>
        /// 根据名称获取示例
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [Get("GetByName")]
        Task<Example1Dto> GetByName(string name);

        /// <summary>
        /// 获取当前服务信息
        /// </summary>
        /// <returns></returns>
        [Get("GetServiceInfo")]
        Task<ServiceInfoDto> GetServiceInfo();

        /// <summary>
        /// 调用 Example2 服务获取信息
        /// </summary>
        /// <returns></returns>
        [Get("CallExample2Service")]
        Task<RemoteServiceResultDto> CallExample2Service();

        /// <summary>
        /// 从 Example2 服务获取示例数据
        /// </summary>
        /// <param name="id">示例 ID</param>
        /// <returns></returns>
        [Get("GetExample2Data")]
        Task<RemoteExampleResultDto> GetExample2Data(long id);
    }

    /// <summary>
    /// 服务信息 DTO
    /// </summary>
    public class ServiceInfoDto
    {
        public string ServiceName { get; set; }
        public string Version { get; set; }
        public int Port { get; set; }
        public string Timestamp { get; set; }
    }

    /// <summary>
    /// 远程服务调用结果
    /// </summary>
    public class RemoteServiceResultDto
    {
        public string LocalService { get; set; }
        public string RemoteService { get; set; }
        public string RemoteVersion { get; set; }
        public int RemotePort { get; set; }
        public string CallTime { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 远程示例数据结果
    /// </summary>
    public class RemoteExampleResultDto
    {
        public string LocalService { get; set; }
        public string RemoteService { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
