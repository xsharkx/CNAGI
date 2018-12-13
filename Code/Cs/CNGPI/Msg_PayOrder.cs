using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_PayOrder_Event : Message,ITransMsg
    {
        public override int PID => 0x0403;

        public int TransID { get; set; }

        public string OrderNum { get; set; }

        public int BoxNum { get; set; }

        public uint Price { get; set; }



        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            OrderNum = stream.ReadHex(16);
            BoxNum = stream.ReadInt16();
            Price = stream.ReadInt32();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
           stream.WriteInt16(TransID);
            stream.WriteHex(OrderNum);
            stream.WriteInt16(BoxNum);
            stream.WriteInt32(Price);
        }
        public override string ToString()
        {
            return $"支付订单:单号:{OrderNum},事务:{TransID},格子:{BoxNum},价格:{Price}";
        }

    }

    public class Msg_PayOrder_Back : Message, IBackMsg
    {
        public override int PID => 0x0483;

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
            return $"回应支付订单:状态码:0X{ErrCode:X2}";
        }
    }

    
}
