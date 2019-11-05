using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Web.Common
{
    public class SyEncode : ISyEncode
    {
        public string Decrypt(string Text, string sKey, EncryptType typecs)
        {
            IEncrypt encrypt = null;
            switch (typecs)
            {
                case EncryptType.DES:
                    encrypt = new DESEncrypt();
                    break;
                case EncryptType.AES:
                    encrypt = new AESEncrypt();
                    break;
            }
            return encrypt.Decrypt(Text, sKey);
        }

        public string Encrypt(string Text, string sKey, EncryptType typecs)
        {
            IEncrypt encrypt = null;
            switch (typecs)
            {
                case EncryptType.DES:
                    encrypt = new DESEncrypt();
                    break;
                case EncryptType.AES:
                    encrypt = new AESEncrypt();
                    break;
            }
            return encrypt.Encrypt(Text, sKey);
        }

        public string Get16MD5One(string Text)
        {
            MD5Encrypt encrypt = new MD5Encrypt();
            return encrypt.Get16MD5One(Text);
        }

        public string Get32MD5One(string Text)
        {
            MD5Encrypt encrypt = new MD5Encrypt();
            return encrypt.Get32MD5One(Text);
        }
    }
}
