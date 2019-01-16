using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SyZero.Infrastructure.EntityFramework;
using SyZero.Domain.Interface;
using SyZero.Domain.Model;

namespace SyZero.Infrastructure.Repository
{
    /// <summary>
    /// 泛型仓储类，实现泛型仓储接口。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepositoryBase<T> : IRepository<T> where T :EntityRoot  //约束在最后面。
    {
        private readonly SyDbContext _context;
        private DbSet<T> _entities;

        /// <summary>
        /// 
        /// </summary>
        public EfRepositoryBase(SyDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return Table;
        }
        public List<T> GetAllList()
        {
            return GetAll().ToList();
        }
        public List<T> GetAllList(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }
        public async Task<List<T>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }
        public DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities;
            }
        }

        /// <summary>
        /// 实现泛型接口中的IQueryable<T>类型的 Table属性
        /// 标记为virtual是为了可以重写它
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }

        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public void Insert(T model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }
                else
                {
                    this.Entities.Add(model);
                    this._context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(T model)
        {
            try
            {
                //model为空，抛空异常
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }
                else
                {
                    //直接保存了
                    this._context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(T model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Remove(model);
                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
