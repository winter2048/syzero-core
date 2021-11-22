using SyZero.Dependency;
using SyZero.Runtime.Security;
using SyZero.Runtime.Session;

namespace SyZero.Application.Service
{
    public class ApplicationService : SyZeroServiceBase, IApplicationService, ITransientDependency
    {
        public ISySession SySession { get; set; }

        protected virtual void CheckPermission(string permissionName)
        {
            if (!SySession.UserId.HasValue)
            {
                throw new SyMessageException(SyMessageBoxStatus.UnAuthorized);
            }
            if (!string.IsNullOrEmpty(permissionName))
            {
                PermissionChecker.Authorize(permissionName);
            }
        }
    }

}
