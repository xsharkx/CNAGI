using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetTicketConfig_Event : Message
    {
        public override int PID => 0x0302;
    }

    public class Msg_GetTicketConfig_Back : Message, IBackMsg
    {
        public override int PID => 0x0382;
        public int ErrCode { get; set; }

        public int CoinsPerTimes { get; set; }

        public int WinCoins { get; set; }

        public int WinTicket { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            CoinsPerTimes = stream.ReadByte();
            WinCoins = stream.ReadInt16();
            WinTicket = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteByte((byte)CoinsPerTimes);
            stream.WriteInt16(WinCoins);
            stream.WriteInt16(WinTicket);
        }
    }
}
