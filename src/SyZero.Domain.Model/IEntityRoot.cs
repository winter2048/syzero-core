using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Domain.Model
{
    public interface IEntityRoot
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        int Id { get; set; }

    }
}
