using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using SyZero.Domain.Repository;

namespace SyZero.EntityFrameworkCore
{
    /// <summary>
    /// EF Core 事务作用域
    /// </summary>
    public class EfCoreTransactionScope : ITransactionScope
    {
        private readonly IDbContextTransaction _transaction;
        private bool _committed;
        private bool _disposed;

        public EfCoreTransactionScope(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(EfCoreTransactionScope));
            if (_committed) return;
            _transaction.Commit();
            _committed = true;
        }

        public async Task CommitAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(EfCoreTransactionScope));
            if (_committed) return;
            await _transaction.CommitAsync();
            _committed = true;
        }

        public void Rollback()
        {
            if (_disposed) return;
            if (_committed) return;
            _transaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            if (_disposed) return;
            if (_committed) return;
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _transaction.Dispose();
            _disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;
            await _transaction.DisposeAsync();
            _disposed = true;
        }
    }

    public class UnitOfWork<TDbContext> : IUnitOfWork
         where TDbContext : DbContext
    {
        private TDbContext dataContext;

        public UnitOfWork(TDbContext _dataContext)
        {
            this.dataContext = _dataContext;
        }

        public void BeginTransaction()
        {
            if (dataContext.Database.CurrentTransaction == null)
            {
                dataContext.Database.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                dataContext.Database.CurrentTransaction.Commit();
            }
        }

        public void RollbackTransaction()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                dataContext.Database.CurrentTransaction.Rollback();
            }
        }

        public void DisposeTransaction()
        {
            if (dataContext.Database.CurrentTransaction != null)
            {
                dataContext.Database.CurrentTransaction.Dispose();
            }
        }

        public async Task<ITransactionScope> BeginTransactionAsync()
        {
            var transaction = await dataContext.Database.BeginTransactionAsync();
            return new EfCoreTransactionScope(transaction);
        }

        public void ExecuteInTransaction(Action action)
        {
            using var transaction = dataContext.Database.BeginTransaction();
            try
            {
                action();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public T ExecuteInTransaction<T>(Func<T> func)
        {
            using var transaction = dataContext.Database.BeginTransaction();
            try
            {
                var result = func();
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> func)
        {
            await using var scope = await BeginTransactionAsync();
            await func();
            await scope.CommitAsync();
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> func)
        {
            await using var scope = await BeginTransactionAsync();
            var result = await func();
            await scope.CommitAsync();
            return result;
        }
    }
}
