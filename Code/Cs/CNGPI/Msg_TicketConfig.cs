using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_TicketConfig_Event : Message
    {
        public override int PID => 0x0301;
        public int CoinsPerTimes { get; set; }

        public int WinCoins { get; set; }

        public int WinTicket { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            CoinsPerTimes = stream.ReadByte();
            WinCoins = stream.ReadInt16();
            WinTicket = stream.ReadInt16();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
             stream.WriteByte((byte)CoinsPerTimes);
            stream.WriteInt16(WinCoins);
            stream.WriteInt16(WinTicket);
        }

        public override string ToString()
        {
            return $"彩票机配置:每局币数:{CoinsPerTimes},概率:{WinTicket}票/{WinCoins}币";
        }

    }

    public class Msg_TicketConfig_Back : Message, IBackMsg
    {
        public override int PID => 0x0381;
        public int ErrCode { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
        }
        public override string ToString()
        {
            return $"回应彩票机配置";
        }
    }
}
