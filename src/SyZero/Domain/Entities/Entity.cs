using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Snowflake.Core;
namespace SyZero.Domain.Entities
{
    /// <summary>
    /// 实体继承此类直接实现到方向接口
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
    /// <summary>
    /// 实体继承此类  常用主键类型long
    /// </summary>
    public class Entity : IEntity
    {
        public long Id { get; set; } = SYID.NextId();
    }
}
