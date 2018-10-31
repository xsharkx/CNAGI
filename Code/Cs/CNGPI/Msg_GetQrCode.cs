using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetQrCode_Event:Message
    {
        public override int PID => 0x0409;

        public override string ToString()
        {
            return "获取正扫二维码";
        }
    }

    public class Msg_GetQrCode_Back : Message, IBackMsg
    {
        public override int PID => 0x0489;
        public int ErrCode { get; set; }
        public string QrCode { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            QrCode = stream.ReadString(150);
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteString(QrCode,150);
        }

        public override string ToString()
        {
            return $"响应正扫二维码:错误码:{ErrCode},二维码:{QrCode}";
        }
    }
}
