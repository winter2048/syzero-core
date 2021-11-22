using Microsoft.AspNetCore.Mvc;
using SyZero.Cache;
using SyZero.Logger;
using SyZero.ObjectMapper;
using SyZero.Runtime.Session;

namespace SyZero.AspNetCore.Controllers
{
    public class SyZeroController : Controller
    {

        /// <summary>
        /// 缓存
        /// </summary>
        public ICache Cache { get; set; }
        /// <summary>
        /// 对象映射
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Sy会话
        /// </summary>
        public ISySession SySession { get; set; }
    }
}
