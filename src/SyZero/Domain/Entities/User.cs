
using System;
namespace SyZero.Domain.Entities
{
    public class User : Entity
    {
        #region 用户属性
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public long PictureId { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 性别  0男  1女  2保密
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public System.DateTime AddTime { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public System.DateTime LastTime { get; set; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastIP { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } 
        #endregion
     

    }
}
