using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetCounter_Event : Message
    {
        public override int PID => 0x010B;
    }

    public class Msg_GetCounter_Back : Message, IBackMsg
    {
        public override int PID => 0x018B;
        public int ErrCode { get; set; }

        public uint PCoins { get; set; }

        public uint ECoins { get; set; }

        public uint PGifts { get; set; }

        public uint PTickets { get; set; }

        public override string ToString()
        {
            return $"实物币数:{PCoins}\r\n电子币数:{ECoins}\r\n礼品数:{PGifts}\r\n票数:{PTickets}";
        }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            PCoins = stream.ReadInt32();
            ECoins = stream.ReadInt32();
            PGifts = stream.ReadInt32();
            PTickets = stream.ReadInt32();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
             stream.WriteInt16(ErrCode);
             stream.WriteInt32(PCoins);
             stream.WriteInt32(ECoins);
             stream.WriteInt32(PGifts);
             stream.WriteInt32(PTickets);
        }
    }
}
