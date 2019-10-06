using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyZero.Domain.Repository
{
    /// <summary>
    /// 事务提交实现此接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     保存更改
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        int SaveChange(bool acceptAllChangesOnSuccess = true);

        /// <summary>
        ///     异步保存更改
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken">异步取消凭据</param>
        Task<int> SaveAsyncChange(bool acceptAllChangesOnSuccess = true,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
