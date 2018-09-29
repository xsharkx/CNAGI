using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_PayOrder_Event : Message,ITransMsg
    {
        public override int PID => 0x0402;

        public int TransID { get; set; }

        public string OrderNum { get; set; }

        public int State { get; set; }

        

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            OrderNum = stream.ReadHex(16);
            State= stream.ReadByte();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
           stream.WriteInt16(TransID);
            stream.WriteHex(OrderNum);
            stream.WriteByte((byte)State);
        }
    }

    public class Msg_PayOrder_Back : Message, IBackMsg
    {
        public override int PID => 0x0482;

        public int ErrCode { get; set; }
        public int TransID { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            TransID = stream.ReadInt16();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteInt16(TransID);
        }
    }

    
}
