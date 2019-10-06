using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyZero.BlogAPI.Controllers
{
    public class BaseController: Controller
    {
        protected ResultJson ResultJson(string Msg, object Data = null, int Code = 200, int Total = 0)
        {
            return new ResultJson(Msg,Data,Code,Total);
        }
    }
}
