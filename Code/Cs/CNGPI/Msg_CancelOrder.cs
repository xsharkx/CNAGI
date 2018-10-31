using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_CancelOrder_Event : Message,ITransMsg
    {
        public override int PID => 0x0402;

        public int TransID { get; set; }

        public string OrderNum { get; set; }        

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            OrderNum = stream.ReadHex(16);
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
           stream.WriteInt16(TransID);
            stream.WriteHex(OrderNum);
        }

        public override string ToString()
        {
            return $"请求取消订单:单号:{OrderNum}";
        }
    }

    public class Msg_CancelOrder_Back : Message, IBackMsg
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
        public override string ToString()
        {
            return $"响应取消订单:错误码:{ErrCode}";
        }
    }
}
