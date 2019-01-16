using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SyZero.Domain.Model;

namespace SyZero.Domain.Interface
{
    /// <summary>
    /// 泛型仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : EntityRoot
    {
        /// <summary>
        /// IQueryable to be used to select entities from database
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();
        /// <summary>
        /// 用来获取所有实体
        /// </summary>
        /// <returns>所有实体的List集合</returns>
        List<T> GetAllList();
        /// <summary>
        ///  用来获取所有实体
        /// </summary>
        /// <returns>所有实体的List集合</returns>
        Task<List<T>> GetAllListAsync();
        /// <summary>
        /// 基于提供的表达式 获取所有实体
        /// </summary>
        /// <param name="predicate">所有实体的List集合</param>
        /// <returns></returns>
        List<T> GetAllList(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 泛型方法，通过id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(object id);

        /// <summary>
        /// 泛型方法--添加实体
        /// </summary>
        /// <param name="model"></param>
        void Insert(T model);

        /// <summary>
        /// 泛型方法，更新实体
        /// </summary>
        /// <param name="model"></param>
        void Update(T model);

        /// <summary>
        /// 泛型方法--删除实体
        /// </summary>
        /// <param name="model"></param>
        void Delete(T model);

        /// <summary>
        /// 只读Table
        /// </summary>
        IQueryable<T> Table { get; }
    }

}
