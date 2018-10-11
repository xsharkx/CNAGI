using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetGiftConfig_Event : Message
    {
        public override int PID => 0x0203;

        public override string ToString()
        {
            return $"获取娃娃机配置";
        }
    }

    public class Msg_GetGiftConfig_Back : Message, IBackMsg
    {
        public override int PID => 0x0283;
        public int ErrCode { get; set; }

        public int CoinsPerTimes { get; set; }

        public int GameSec { get; set; }

        public int LowPower { get; set; }

        public int HightPower { get; set; }

        public int WinCoins { get; set; }

        public int WinGifts { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            CoinsPerTimes = stream.ReadByte();
            GameSec = stream.ReadByte();
            LowPower = stream.ReadByte();
            HightPower = stream.ReadByte();
            WinCoins = stream.ReadInt16();
            WinGifts = stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteByte((byte)CoinsPerTimes);
            stream.WriteByte((byte)GameSec);
            stream.WriteByte((byte)LowPower);
            stream.WriteByte((byte)HightPower);
            stream.WriteInt16(WinCoins);
            stream.WriteInt16(WinGifts);
        }

        public override string ToString()
        {
            return $"回应获取娃娃机配置:每局币数:{CoinsPerTimes},游戏时间:{GameSec},弱爪力:{LowPower},强爪力:{HightPower},概率:{WinGifts}奖/{WinCoins}币";
        }
    }
}
