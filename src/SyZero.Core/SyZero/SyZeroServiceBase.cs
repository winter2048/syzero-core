using SyZero.Domain.Repository;
using SyZero.Logger;
using SyZero.ObjectMapper;

namespace SyZero
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public abstract class SyZeroServiceBase
    {
        /// <summary>
        /// 持久化
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 实体映射
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }
    }
}
