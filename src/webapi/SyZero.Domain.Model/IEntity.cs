using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Domain.Entities
{
    public interface IEntity<Tkey>
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        Tkey Id { get; set; }

    }
}
