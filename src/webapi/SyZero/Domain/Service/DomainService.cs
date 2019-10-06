using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Dependency;
namespace SyZero.Domain.Service
{
    /// <summary>
    /// 这个类作为域服务的基类
    /// </summary>
    public abstract class DomainService : SyZeroServiceBase, IDomainService, ITransientDependency
    {
    }
}
