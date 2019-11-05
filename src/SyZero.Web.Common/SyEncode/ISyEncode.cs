using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Web.Common
{
    interface ISyEncode
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">内容</param>
        /// <param name="sKey">密匙</param>
        /// <returns></returns>
        string Encrypt(string Text, string sKey,EncryptType type);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text">内容</param>
        /// <param name="sKey">密匙</param>
        /// <returns></returns>
        string Decrypt(string Text, string sKey, EncryptType type);

        /// <summary>
        /// MD5加密 32位
        /// </summary>
        /// <param name="Text">内容</param>
        /// <returns></returns>
        string Get32MD5One(string Text);

        /// <summary>
        /// MD5加密 16位
        /// </summary>
        /// <param name="Text">内容</param>
        /// <returns></returns>
        string Get16MD5One(string Text);
    }
}
