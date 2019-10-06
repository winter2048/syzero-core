using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SyZero.Common;

namespace SyZero.Domain.Entities
{
    public class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
