using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Cuong_WebsiteTimPhongTro.Models
{
    public class Encode
    {
        public static string EncodeMD5(string key)
        {
            MD5 md5 = MD5.Create();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }
    }
}