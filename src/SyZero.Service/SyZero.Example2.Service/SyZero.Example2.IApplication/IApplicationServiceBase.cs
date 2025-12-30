using SyZero.Application.Attributes;
using SyZero.Application.Service;

namespace SyZero.Example2.IApplication
{
    [DynamicApi]
    public interface IApplicationServiceBase : IApplicationService, IDynamicApi
    {
    }
}
