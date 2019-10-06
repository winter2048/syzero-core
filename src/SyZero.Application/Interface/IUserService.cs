using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Application
{
  public  interface IUserService :IBaseService<UserDto>
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        bool Login(string UserName, string Password);
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>1注册成功 0失败 -1用户名重复 -2信息错误</returns>
        int Register(RegisterDto registerDto);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>1修改成功 0失败 -1用户名重复 -2信息错误</returns>
        int UpdataInfo(UserDto userDto);
        /// <summary>
        /// 用户名是否重复
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        bool IsRepeatByName(string UserName);
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        UserDto GetUser(string name);


    }
}
