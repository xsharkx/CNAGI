using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_CreateOrder_Event : Message
    {
        public override int PID => 0x0401;

        public  int BoxNum { get; set; }

        public int Price { get; set; }

        public string OrderNum { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            BoxNum = stream.ReadInt16();
            Price = stream.ReadInt16();
            OrderNum = stream.ReadHex(16);
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(BoxNum);
            stream.WriteInt16(Price);
            stream.WriteHex(OrderNum);
        }

        public override string ToString()
        {
            return $"创建订单:格子号:{BoxNum},价格:{Price},单号:{OrderNum}";
        }
    }

    public class Msg_CreateOrder_Back : Message, IBackMsg
    {
        public override int PID => 0x0481;

        public int ErrCode { get; set; }
        public string OrderNum { get; set; }

        public int State { get; set; }

        public string QrCode { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            OrderNum = stream.ReadHex(16);
            State = stream.ReadByte();
            QrCode = stream.ReadString(150);
        }
        protected override void WriteData(MsgDataStream stream)
        {
            if (OrderNum.Length != 32) throw new Exception("单号错误");
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteHex(OrderNum);
            stream.WriteByte((byte)State);
            stream.WriteString(QrCode,150);
        }

        public override string ToString()
        {
            return $"回应创建订单:二维码:{QrCode},状态:{State},单号:{OrderNum}";
        }
    }

    
}
