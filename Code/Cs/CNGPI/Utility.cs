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
        public static string ByteToHex(byte[] bytes)
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
        public static string ByteToHex2(byte[] bytes)
        {
            if (bytes == null) return "";
            string str = "";
            foreach (byte bt in bytes)
            {
                str += bt.ToString("X2")+" ";
            }
            return str;
        }

        public static byte[] StrToByte(string data,int length)
        {
            byte[] dataname = System.Text.Encoding.UTF8.GetBytes(data);
            if (dataname.Length > length) throw new Exception("内容太长");
            byte[] datawtname = new byte[length];
            Array.Copy(dataname, datawtname, dataname.Length);
            return datawtname;
        }

        public static string ByteToStr(byte[] data)
        {
            return System.Text.Encoding.UTF8.GetString(data).Replace("\0", "");
        }

        /// <summary>
        /// 字符转字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] HexToByte(string data)
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
