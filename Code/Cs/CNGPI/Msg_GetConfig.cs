using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetConfig_Event : Message
    {
        public override int PID => 0x0109;
    }

    public class Msg_GetConfig_Back : Message, IBackMsg
    {
        public override int PID => 0x0189;

        public int ErrCode { get; set; }
        public int CoinsPerTimes { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            CoinsPerTimes = stream.ReadByte();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteByte((byte)CoinsPerTimes);
        }
    }

    
}
