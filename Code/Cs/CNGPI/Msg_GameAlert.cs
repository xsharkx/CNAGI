using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GameAlert_Event : Message, ITransMsg
    {
        public override int PID => 0x010C;
        public int TransID { get; set; }
        public int AlertType { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            AlertType=stream.ReadByte();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteByte((byte)AlertType);
        }
    }

    public class Msg_GameAlert_Back : Message, IBackMsg
    {
        public override int PID => 0x018C;
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
