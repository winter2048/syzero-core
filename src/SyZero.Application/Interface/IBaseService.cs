using System;
using System.Collections.Generic;
using System.Text;


namespace SyZero.Application
{
    public interface IBaseService<Dto>:IDependency
    {
        /// <summary>
        /// 获取Dto
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Dto GetDto(long Id);
        /// <summary>
        /// 获取Dto列表
        /// </summary>
        /// <returns></returns>
        List<Dto> GetList(QueryDto queryDto);
        /// <summary>
        /// 修改Dto信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>1修改成功 0失败 -2信息错误</returns>
        int Updata(Dto dto);
        /// <summary>
        /// 删除Dto
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        bool Delect(string IDs);
        /// <summary>
        /// 添加Dto
        /// </summary>
        /// <param name="articleDto"></param>
        /// <returns></returns>
        bool Add(Dto dto);
    }
}
