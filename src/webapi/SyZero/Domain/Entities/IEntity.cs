using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Domain.Entities
{
    /// <summary>
    /// 定义基本实体类型的接口。系统中的所有实体都必须实现此接口。
    /// </summary>
    /// <typeparam name="Tkey">主键类型</typeparam>
    public interface IEntity<Tkey>
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        Tkey Id { get; set; }

    }
    /// <summary>
    /// 定义基本实体类型的接口。系统中的所有实体都必须实现此接口。  常用long类型主键
    /// </summary>
    public interface IEntity :IEntity<long>
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        long Id { get; set; }

    }
}
