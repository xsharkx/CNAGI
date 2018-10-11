using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_SetTime_Event : Message
    {
        public override int PID => 0x0407;

        public long Time { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            Time = stream.ReadInt64();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt64(Time);
        }

        public override string ToString()
        {
            return $"设置系统时间";
        }

    }

    /// <summary>
    /// 回应
    /// </summary>
    public class Msg_SetTime_Back : Message, IBackMsg
    {
        public override int PID => 0x0487;

        public override string ToString()
        {
            return $"回应设置系统时间";
        }
    }
}
