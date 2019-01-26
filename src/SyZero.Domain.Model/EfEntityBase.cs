using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Domain.Model
{
    public class EfEntityBase : IEfEntity
    {
        public long Id { get; set; }
    }
}
