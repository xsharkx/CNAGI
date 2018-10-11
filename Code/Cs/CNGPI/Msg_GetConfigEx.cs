using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetConfigEx_Event:Message
    {
        public override int PID => 0x010A;
        public override string ToString()
        {
            return $"获取扩展配置";
        }
    }

    public class Msg_GetConfigEx_Back : Message, IBackMsg
    {
        public override int PID => 0x018A;

        public int ErrCode { get; set; }
        public byte[] ExData { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            ExData = stream.ReadToEnd();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteByteArray(ExData);
        }

        public override string ToString()
        {
            return $"回应获取扩展配置";
        }
    }
}
