using System.Threading.Tasks;

namespace SyZero.Domain.Repository
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 释放事务
        /// </summary>
        void DisposeTransaction();

        /// <summary>
        /// 开启事务异步
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// 提交事务异步
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// 回滚事务异步
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// 释放事务异步
        /// </summary>
        Task DisposeTransactionAsync();
    }
}
