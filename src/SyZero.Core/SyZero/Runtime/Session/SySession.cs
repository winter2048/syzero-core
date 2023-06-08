using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using SyZero.Dependency;
using SyZero.Runtime.Security;
using SyZero.Serialization;
using SyZero.Util;

namespace SyZero.Runtime.Session
{
    public class SySession : ISySession, ISingletonDependency
    {
        private readonly IJsonSerialize _jsonSerialize;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private ClaimsPrincipal _principal;

        public SySession(IJsonSerialize jsonSerialize, IHttpContextAccessor httpContextAccessor)
        {
            _jsonSerialize = jsonSerialize;
            _httpContextAccessor = httpContextAccessor;
            _principal = _httpContextAccessor.HttpContext.User;
        }

        public ClaimsPrincipal Principal
        {
            get
            {
                return _principal;
            }
        }

        public long? UserId
        {
            get
            {
                var tenantIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == SyClaimTypes.UserId);
                if (!string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return tenantIdClaim.Value.ToLong();
                }
                return null;
            }
        }

        public string UserRole
        {
            get
            {
                var tenantIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == SyClaimTypes.UserRole);
                if (!string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return tenantIdClaim.Value;
                }
                return null;
            }
        }

        public string UserName
        {
            get
            {
                var tenantIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == SyClaimTypes.UserName);
                if (!string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return tenantIdClaim.Value;
                }
                return null;
            }
        }

        public List<string> Permission
        {
            get
            {
                var tenantIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == SyClaimTypes.Permission);
                if (!string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return _jsonSerialize.JSONToObject<List<string>>(tenantIdClaim.Value);
                }
                return null;
            }
        }

        public string Token
        {
            get
            {
                var tenantIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == SyClaimTypes.Token);
                if (!string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return tenantIdClaim.Value;
                }
                return null;
            }
        }

        public ISySession Parse(string token)
        {
            this._principal = AutofacUtil.GetService<IToken>().GetPrincipal(token);
            return this;
        }

        ISySession ISySession.Parse(ClaimsPrincipal claimsPrincipal)
        {
            this._principal = claimsPrincipal;
            return this;
        }
    }
}
