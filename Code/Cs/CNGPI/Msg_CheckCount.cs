using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_CheckCount_Event : Message,ITransMsg
    {
        public override int PID => 0x0406;
        public int TransID { get; set; }

        public int BoxNum { get; set; }

        public int Amount { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            BoxNum = stream.ReadInt16();
            Amount = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteInt16(BoxNum);
            stream.WriteInt16(Amount);
        }
    }

    public class Msg_CheckCount_Back : Message, IBackMsg
    {
        public override int PID => 0x0486;
        public int ErrCode { get; set; }
        public int TransID { get; set; }
        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            ErrCode = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteInt16(ErrCode);
        }
    }
}
