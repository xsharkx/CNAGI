using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_GetMenuIndex_Event:Message
    {
        public override int PID => 0x0111;
        public override string ToString()
        {
            return $"获取菜单索引";
        }
    }

    public class Msg_GetMenuIndex_Back : Message, IBackMsg
    {
        public override int PID => 0x0191;

        public int ErrCode { get; set; }
        public int[] MenuItems { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            int MenuItemscount = stream.ReadByte();
            MenuItems = new int[MenuItemscount];
            for (int i = 0; i < MenuItemscount; i++)
            {
                MenuItems[i] = stream.ReadInt16();
            }
        }

        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            if (MenuItems == null)
            {
                stream.WriteByte(0);
            }
            else
            {
                stream.WriteByte((byte)MenuItems.Length);
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    stream.WriteInt16(MenuItems[i]);
                }
            }
        }

        public override string ToString()
        {
            if (MenuItems == null || MenuItems.Length==0)
            {
                return $"回应获取菜单索引:无";
            }
            string itemsstr = "";
            foreach(int id in MenuItems)
            {
                itemsstr += id.ToString() + ",";
            }
            return $"回应获取菜单索引:{MenuItems.Length}项,{itemsstr}";
        }
    }
}
