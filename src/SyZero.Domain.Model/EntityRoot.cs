using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Domain.Model
{
    public abstract class EntityRoot : IEntityRoot
    {
        public virtual int Id { get; set; }
    }
}
