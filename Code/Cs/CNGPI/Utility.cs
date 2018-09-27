using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Utility
    {
        /// <summary>
        /// 字节转字符 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToStr(byte[] bytes)
        {
            string str = "";
            foreach (byte bt in bytes)
            {
                str += bt.ToString("X2");
            }
            return str;
        }

        /// <summary>
        /// 字节转字符 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToStr2(byte[] bytes)
        {
            string str = "";
            foreach (byte bt in bytes)
            {
                str += bt.ToString("X2")+" ";
            }
            return str;
        }

        /// <summary>
        /// 字符转字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] StrToByte(string data)
        {
            List<byte> bts = new List<byte>();
            for (int i = 0; i < data.Length; i += 2)
            {
                bts.Add(Convert.ToByte(data.Substring(i, 2), 16));
            }
            return bts.ToArray();
        }
    }
}
