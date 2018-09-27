using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_ConfigEx_Event :Message
    {
        public override int PID => 0x0107;
        public byte[] ExData { get; set; }
        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ExData = stream.ReadToEnd();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteByteArray(ExData);
        }
    }

    public class Msg_ConfigEx_Back : Message, IBackMsg
    {
        public override int PID => 0x0187;
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
