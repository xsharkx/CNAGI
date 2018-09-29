using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_QueryOrder_Event : Message
    {
        public override int PID => 0x0404;

        public string OrderNum { get; set; }

        

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            OrderNum = stream.ReadHex(16);
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteHex(OrderNum);
        }
    }

    public class Msg_QueryOrder_Back : Message, IBackMsg
    {
        public override int PID => 0x0484;

        public int ErrCode { get; set; }
        public int State { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            State = stream.ReadByte();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteByte((byte)State);
        }
    }

    
}
