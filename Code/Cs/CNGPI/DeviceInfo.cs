using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class DeviceInfo
    {
        public DeviceInfo(uint productNum)
        {
            ProductNum = productNum;
            FillInfoByProductNum();
        }

        public void FillInfoByProductNum()
        {
            string deviceinfo = @"
谷微动漫|0003|超级抓神|gw_whwwj|00030001|礼品机|[详情](Device/0003/00030001/README.md)
油菜花|0001|芸苔盒子V1|ythzv1|00010001|支付盒子|[详情](Device/0001/00010001/README.md)
";
            string[] lines = deviceinfo.Split('\n');
            for(int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().Length < 10) continue;
                string[] items = lines[i].Split('|');
                if(items[4]== ProductNum.ToString("X2").PadLeft(8, '0'))
                {
                    Manufacturers = items[0];
                    ManufacturersNum =(uint) Int32.Parse(items[1], System.Globalization.NumberStyles.HexNumber);
                    Product= items[2];
                }
            }
            if (string.IsNullOrEmpty(Manufacturers))
            {
                string productnum = ProductNum.ToString("X2").PadLeft(8,'0');
                Manufacturers = $"未知厂商{productnum.Substring(0, 4)}";
                ManufacturersNum = (uint)Int32.Parse(productnum.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);
                Product = $"未知产品{productnum}";
            }
        }

        /// <summary>
        /// 软件版本
        /// </summary>
        public int SoftVer { get; set; }

        /// <summary>
        /// 接口版本
        /// </summary>
        public int CNGPIVer { get; set; }

        /// <summary>
        /// 厂商名
        /// </summary>
        public string Manufacturers { get; set; }

        /// <summary>
        /// 厂商编号
        /// </summary>
        public uint ManufacturersNum { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public uint ProductNum { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 协议版本
        /// </summary>
        public uint UseVer { get; set; }

        /// <summary>
        /// 游戏P位数
        /// </summary>
        public int GamePortCount { get; set; }

        /// <summary>
        /// 当前游戏P位
        /// </summary>
        public int CurrPortIndex { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public int State { get; set; }
    }
}
