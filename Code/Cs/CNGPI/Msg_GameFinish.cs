using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GameFinish_Event: Message,ITransMsg
    {
        public override int PID => 0x0106;
        public int TransID { get; set; }
        public uint OutGift { get; set; }

        public int GiftType { get; set; }

        public int GiftPort { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            OutGift = stream.ReadInt32();
            GiftType = stream.ReadByte();
            GiftPort = stream.ReadByte();
        }

        public override string ToString()
        {
            return $"事务:{TransID}\r\n出奖数:{OutGift}\r\n出奖类型:{GiftType}\r\n端口:{GiftPort}";
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
             stream.WriteInt16(TransID);
             stream.WriteInt32(OutGift);
             stream.WriteByte((byte)GiftType);
             stream.WriteByte((byte)GiftPort);
        }
    }

    public class Msg_GameFinish_Back : Message, IBackMsg
    {
        public override int PID => 0x0186;
        public int TransID { get; set; }

        public int Result { get; set; }
        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            Result = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteInt16(Result);
        }
    }
}
