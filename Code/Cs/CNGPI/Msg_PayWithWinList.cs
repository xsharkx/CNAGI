using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_PayWithWinList_Event : Message, ITransMsg
    {
        public override int PID => 0x0201;

        public int TransID { get; set; }
        public List<int> WinList { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            int winlistlen = stream.ReadByte();
            byte[] list = stream.ReadByteArray(winlistlen);
            WinList = new List<int>();
            for (int i= 0; i < winlistlen; i++)
            {
                WinList.Add(list[i]);
            }
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteByte((byte)WinList.Count);
            byte[] list = new byte[WinList.Count];
            for (int i = 0; i < WinList.Count; i++)
            {
                list[i]=(byte)WinList[i];
            }
            stream.WriteByteArray(list);
        }
    }

    public class Msg_PayWithWinList_Back : Message, IBackMsg
    {
        public override int PID => 0x0281;
        public int TransID { get; set; }

        public int ErrCode { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            ErrCode= stream.ReadInt16();
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteInt16(ErrCode);
        }
    }
}
