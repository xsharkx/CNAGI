using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    /// <summary>
    /// 握手
    /// </summary>
    public class Msg_Connect_Event :Message
    {
        public override int PID => 0x0101;

        public uint ProductNum { get; set; }

        public byte[] DeviceID { get; set; }

        public int DeviceType { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ProductNum = stream.ReadInt32();
            DeviceID = stream.ReadByteArray(16);
            DeviceType = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
             stream.WriteInt32(ProductNum);
             stream.WriteByteArray(DeviceID);
             stream.WriteInt16(DeviceType);
        }

        public override string ToString()
        {
            return $"握手:设备编号:{ProductNum.ToString("X2").PadLeft(8,'0')},设备ID:{Utility.ByteToHex(DeviceID)},设备类型:{DeviceType}";
        }
    }

    /// <summary>
    /// 握手_回应
    /// </summary>
    public class Msg_Connect_Back : Message, IBackMsg
    {
        public override int PID => 0x0181;

        public uint GPIVersion { get; set; }

        public uint ProductNum { get; set; }

        public byte[] DeviceID { get; set; }

        public int DeviceType { get; set; }

        public int GamePortCount { get; set; }

        public int CurrGamePortIndex { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            GPIVersion = stream.ReadInt32();
            ProductNum = stream.ReadInt32();
            DeviceID = stream.ReadByteArray(16);
            GamePortCount = stream.ReadByte();
            CurrGamePortIndex = stream.ReadByte();
            DeviceType = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
             stream.WriteInt32(GPIVersion);
             stream.WriteInt32(ProductNum);
             stream.WriteByteArray(DeviceID);
             stream.WriteByte((byte)GamePortCount);
             stream.WriteByte((byte)CurrGamePortIndex);
             stream.WriteInt16(DeviceType);
        }

        public override string ToString()
        {
            return $"回应握手:设备编号:{ProductNum.ToString("X2").PadLeft(8, '0')},设备ID:{Utility.ByteToHex(DeviceID)},设备类型:{DeviceType},P位数:{GamePortCount},当前P位:{CurrGamePortIndex}";
        }
    }
}
