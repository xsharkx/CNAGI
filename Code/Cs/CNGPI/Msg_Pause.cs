using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_Pause_Event:Message,ITransMsg
    {
        public override int PID => 0x010D;
        public int TransID { get; set; }

        public int Action { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            Action = stream.ReadByte();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteByte((byte)Action);
        }
        public override string ToString()
        {
            return $"启停游戏机:启停:{Action}";
        }
    }

    public class Msg_Pause_Back : Message, IBackMsg
    {
        public override int PID => 0x018D;
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

        public override string ToString()
        {
            return $"回应启停游戏机";
        }
    }
}
