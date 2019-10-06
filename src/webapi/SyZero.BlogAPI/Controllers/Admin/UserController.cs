using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SyZero.Application;
using SyZero.Common;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyZero.BlogAPI.Controllers.Admin
{
    [AllowAnonymous]
    [Route("api/a/[controller]")]
    public class UserController : BaseController
    {
        public ILogger pp;
        public IUserService _userService;
      
        public UserController(IUserService userService)
        {
            _userService = userService;
        
        }

        [HttpPost("Login")]
        public ResultJson Login([FromQuery]LoginDto request)
        {
            bool IsExits = _userService.Login(request.UserName, request.Password);
            if (IsExits)
            {
                var member = _userService.GetUser(request.UserName);
                var claims = new[] {
                            new Claim("name",request.UserName),
                            new Claim("id",member.Id)
                        };

                string usertoken = JwtHelper.GetToken(claims);
                return ResultJson("登陆成功", usertoken);
            }
            else
            {
             
                return ResultJson("账号或密码错误", null, 401);
            }
        }

       

        // POST api/<controller>
        [HttpPost]
        public ResultJson Post([FromBody]RegisterDto register)
        {
            int i = _userService.Register(register);
            return ResultJson("注册成功", i);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
          
        }

       
    }
}
