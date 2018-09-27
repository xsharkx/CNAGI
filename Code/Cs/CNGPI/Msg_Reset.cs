using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_Reset_Event : Message
    {
        public override int PID => 0x0108;
    }

    public class Msg_Reset_Back : Message, IBackMsg
    {
        public override int PID => 0x0188;
        public int Result { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            Result = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(Result);
        }
    }
}
