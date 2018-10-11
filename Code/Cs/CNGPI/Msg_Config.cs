using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_Config_Event:Message
    {
        public override int PID => 0x0104;
        public int CoinsPerTimes { get; set; }
        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            CoinsPerTimes = stream.ReadByte();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteByte((byte)CoinsPerTimes);
        }
        public override string ToString()
        {
            return $"通用设置:每局币数:{CoinsPerTimes}";
        }
    }

    public class Msg_Config_Back : Message, IBackMsg
    {
        public override int PID => 0x0184;
        public int ErrCode { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
        }
        public override string ToString()
        {
            return $"回应通用设置:错误码:{ErrCode}";
        }
    }
}
