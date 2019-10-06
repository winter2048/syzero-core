using System;

namespace SyZero.Domain.Entities
{
    public class Config : EntityBase
    {
        #region 属性
        /// <summary>
        /// 键
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
     
        #endregion

     
    }
}
