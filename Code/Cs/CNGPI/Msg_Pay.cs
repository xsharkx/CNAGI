﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_Pay_Event : Message, ITransMsg
    {
        public override int PID => 0x0103;
        public int TransID { get; set; }
        public int Coins { get; set; }
        public int CoinType { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            Coins = stream.ReadInt16();
            CoinType = stream.ReadByte();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
             stream.WriteInt16(TransID);
             stream.WriteInt16(Coins);
             stream.WriteByte((byte)CoinType);
        }

        public override string ToString()
        {
            return $"支付:币数:{Coins},币类型:{CoinType}";
        }

    }

    public class Msg_Pay_Back : Message, IBackMsg
    {
        public override int PID => 0x0183;
        public int TransID { get; set; }
        public int ErrCode { get; set; }

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
        public override string ToString()
        {
            return $"回应支付:状态码:{ErrCode}";
        }

    }
}
