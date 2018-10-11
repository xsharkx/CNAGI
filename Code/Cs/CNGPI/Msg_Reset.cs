using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_Reset_Event : Message
    {
        public override int PID => 0x0108;

        public override string ToString()
        {
            return $"恢复出厂设置";
        }
    }

    public class Msg_Reset_Back : Message, IBackMsg
    {
        public override int PID => 0x0188;
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
            return $"回应恢复出厂设置";
        }

    }
}
