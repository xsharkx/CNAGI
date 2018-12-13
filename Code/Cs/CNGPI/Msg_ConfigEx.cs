using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_ConfigEx_Event :Message
    {
        public override int PID => 0x0107;
        public byte[] ExData { get; set; }
        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ExData = stream.ReadToEnd();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteByteArray(ExData);
        }

        public override string ToString()
        {
            if (ExData != null)
            {
                return $"扩展设置:长度:{ExData.Length}";
            }
            else
            {
                return $"扩展设置:长度:0";
            }
        }
    }

    public class Msg_ConfigEx_Back : Message, IBackMsg
    {
        public override int PID => 0x0187;
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
            return $"回应扩展设置:状态码:{ErrCode}";
        }
    }
}
