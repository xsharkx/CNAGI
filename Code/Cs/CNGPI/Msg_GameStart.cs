using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GameStart_Event : Message, ITransMsg
    {
        public override int PID => 0x0105;
        public int TransID { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
        }
    }

    public class Msg_GameStart_Back : Message, IBackMsg
    {
        public override int PID => 0x0185;
        public int TransID { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
        }
    }
}
