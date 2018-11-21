using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetMenuDetail_Event:Message
    {
        public override int PID => 0x0110;

        public int ItemID { get; set; }
        public override string ToString()
        {
            return $"获取菜单详细信息:ID:{ItemID}";
        }

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
    }

    public class Msg_GetMenuDetail_Back : Message, IBackMsg
    {
        public override int PID => 0x0190;

        public int ErrCode { get; set; }

        public int ItemID { get; set; }

        public uint Value { get; set; }

        public string ItemName { get; set; }
        public string ItemDiscribe { get; set; }
        public uint MaxValue { get; set; }
        public uint MinValue { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            ItemID = stream.ReadInt16();
            Value = stream.ReadInt32();
            MaxValue = stream.ReadInt32();
            MinValue= stream.ReadInt32();
            byte[] bts = stream.ReadByteArray(32);
            ItemName = System.Text.Encoding.Unicode.GetString(bts).Replace("\0","");
            int discribelen = stream.ReadByte();
            if (discribelen > 0)
            {
                byte[] bts2 = stream.ReadByteArray(discribelen);
                ItemDiscribe= System.Text.Encoding.Unicode.GetString(bts2).Replace("\0", "");
            }
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteInt16(ItemID);
            stream.WriteInt32(Value);
            stream.WriteInt32(MaxValue);
            stream.WriteInt32(MinValue);
            byte[] bts = Utility.StrToByteUniCode(ItemName, 32);
            stream.WriteByteArray(bts);
            if (ItemDiscribe == null)
            {
                stream.WriteByte(0);
            }
            else
            {
                byte[] bts2 = System.Text.Encoding.Unicode.GetBytes(ItemDiscribe);
                stream.WriteByte((byte)bts2.Length);
                stream.WriteByteArray(bts2);
            }
        }

        public override string ToString()
        {
            return $"回应获取菜单详细信息:ID:{ItemID},值:{Value},最大值:{MaxValue},最小值:{MinValue},名称:{ItemName},描述:{ItemDiscribe}";
        }
    }
}
