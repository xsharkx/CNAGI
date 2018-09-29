using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Msg_SetPrdInfo_Event : Message,ITransMsg
    {
        public override int PID => 0x0405;

        public int BoxNum { get; set; }

        public int TransID { get; set; }

        public int Price { get; set; }

        public int Cost { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }



        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            TransID = stream.ReadInt16();
            BoxNum = stream.ReadInt16();
            Price = stream.ReadInt16();
            Cost = stream.ReadInt16();
            Name = stream.ReadString(64);
            Url = stream.ReadString(200);
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(TransID);
            stream.WriteInt16(BoxNum);
            stream.WriteInt16(Price);
            stream.WriteInt16(Cost);
            stream.WriteString(Name,64);
            stream.WriteString(Url, 200);
        }
    }

    public class Msg_SetPrdInfo_Back : Message, IBackMsg
    {
        public override int PID => 0x0485;

        public int ErrCode { get; set; }
        public int TransID { get; set; }

        protected override void ReadData(MsgDataStream stream)
        {
            base.ReadData(stream);
            ErrCode = stream.ReadInt16();
            TransID = stream.ReadInt16();
        }
        protected override void WriteData(MsgDataStream stream)
        {
            base.WriteData(stream);
            stream.WriteInt16(ErrCode);
            stream.WriteInt16(TransID);
        }
    }    
}
