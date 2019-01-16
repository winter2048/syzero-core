using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SyZero.Infrastructure.EntityFramework;
using SyZero.Domain.Interface;
using SyZero.Domain.Model;

namespace SyZero.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntityRoot
    {
        private readonly SyDbContext _context;
        private DbSet<T> _entities;

        /// <summary>
        /// 
        /// </summary>
        public BaseRepository(SyDbContext context)
        {
            _context = context;
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


        public virtual void Add(T entity)
        {
            Entities.Add(entity);
        }

        //新增方法
        public virtual void AddAll(IEnumerable<T> entities)
        {
            Entities.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            Entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        //新增方法
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T obj in entities)
            {
                Entities.Attach(obj);
                _context.Entry(obj).State = EntityState.Modified;
            }
        }

        public virtual void Delete(T entity)
        {
            Entities.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = Entities.Where<T>(where).AsEnumerable();
            Entities.RemoveRange(objects);
        }

        //新增方法
        public virtual void DeleteAll(IEnumerable<T> entities)
        {
            Entities.RemoveRange(entities);
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual T GetById(long id)
        {
            return Entities.Find(id);
        }

        public virtual T GetById(string id)
        {
            return Entities.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return Entities.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return Entities.Where(where).FirstOrDefault<T>();
        }

        public virtual IEnumerable<T> GetAllLazy()
        {
            return Entities;
        }

    }
}
