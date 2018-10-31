using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_MenuGet_Event : Message
    {
        public override int PID => 0x010E;

        public int ItemID { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ItemID = stream.ReadInt16();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ItemID);
        }
        public override string ToString()
        {
            return $"读取菜单项:{ItemID}";
        }
    }

    public class Msg_MenuGet_Back : Message, IBackMsg
    {
        public override int PID => 0x018E;

        public int ErrCode { get; set; }
        public int ItemID { get; set; }
        public uint ItemValue { get; set; }
        public Byte Display { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            ItemID = stream.ReadInt16();
            ItemValue = stream.ReadInt32();
            Display = stream.ReadByte();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteInt16(ItemID);
            stream.WriteInt32(ItemValue);
            stream.WriteByte(Display);
        }
        public override string ToString()
        {
            return $"回应读取菜单项:项:{ItemID},值:{ItemValue},显示:{Display},错误码:{ErrCode}";
        }

    }

    public class Msg_MenuSet_Event : Message
    {
        public override int PID => 0x010F;
        public int ItemID { get; set; }
        public uint ItemValue { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ItemID = stream.ReadInt16();
            ItemValue = stream.ReadInt32();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ItemID);
            stream.WriteInt32(ItemValue);
        }
        public override string ToString()
        {
            return $"设置菜单项:项:{ItemID},值:{ItemValue}";
        }
    }

    public class Msg_MenuSet_Back : Message, IBackMsg
    {
        public override int PID => 0x018F;
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
            return $"回应设置菜单项:错误码:{ErrCode}";
        }
    }
}
