using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class Message
    {
        public bool MsgDataPacked
        {
            get
            {
                return _MsgData != null;
            }
        }

        byte[] _MsgData = null;
        public Byte[] GetMsgData()
        {
            if (_MsgData != null) return _MsgData;
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            MsgDataStream msgstm = new MsgDataStream(mem);
            WriteData(msgstm);
            msgstm.WriteByteArray(new byte[] { 0, 0, 0xFE, 0x73 });
            _MsgData = mem.ToArray();
            mem.Close();
            int datalen = _MsgData.Length - 12;
            _MsgData[6] = (byte)((datalen >> 8) & 0xFF);
            _MsgData[7] = (byte)((datalen) & 0xFF);
            byte sum = _MsgData[0];
            byte xor = _MsgData[0];
            for(int i=1;i< _MsgData.Length - 4; i++)
            {
                sum = (byte)(sum + _MsgData[i]);
                xor = (byte)(xor ^ _MsgData[i]);
            }
            _MsgData[datalen + 8] = sum;
            _MsgData[datalen + 9] = xor;
            return _MsgData;
        }

        public virtual int PID
        {
            get;
        }

        public virtual int ADR { get; set; }

        protected virtual void ReadData(MsgDataStream stream)
        {
            stream.ReadByteArray(2);
            ADR = stream.ReadInt16();
            stream.ReadInt16();//PID
            stream.ReadInt16();//len
        }

        protected virtual void WriteData(MsgDataStream stream)
        {
            stream.WriteByte(0xEF);
            stream.WriteByte(0x37);
            stream.WriteInt16(ADR);
            stream.WriteInt16(PID);
            stream.WriteInt16(0);//len
        }

        private static Dictionary<int, Type> MsgDic;

        private static void RegMsg(Type ty)
        {
            int tpid = (Activator.CreateInstance(ty) as Message).PID;
            MsgDic.Add(tpid, ty);
        }

        static Message()
        {
            MsgDic = new Dictionary<int, Type>();
            RegMsg(typeof(Msg_ConfigEx_Back));
            RegMsg(typeof(Msg_ConfigEx_Event));
            RegMsg(typeof(Msg_Config_Back));
            RegMsg(typeof(Msg_Config_Event));
            RegMsg(typeof(Msg_Connect_Back));
            RegMsg(typeof(Msg_Connect_Event));
            RegMsg(typeof(Msg_GameAlert_Back));
            RegMsg(typeof(Msg_GameAlert_Event));
            RegMsg(typeof(Msg_GameFinish_Back));
            RegMsg(typeof(Msg_GameFinish_Event));
            RegMsg(typeof(Msg_GameStart_Back));
            RegMsg(typeof(Msg_GameStart_Event));
            RegMsg(typeof(Msg_GetConfigEx_Back));
            RegMsg(typeof(Msg_GetConfigEx_Event));
            RegMsg(typeof(Msg_GetConfig_Back));
            RegMsg(typeof(Msg_GetConfig_Event));
            RegMsg(typeof(Msg_GetCounter_Back));
            RegMsg(typeof(Msg_GetCounter_Event));
            RegMsg(typeof(Msg_GetGiftConfig_Back));
            RegMsg(typeof(Msg_GetGiftConfig_Event));
            RegMsg(typeof(Msg_GetTicketConfig_Back));
            RegMsg(typeof(Msg_GetTicketConfig_Event));
            RegMsg(typeof(Msg_GiftConfig_Back));
            RegMsg(typeof(Msg_GiftConfig_Event));
            RegMsg(typeof(Msg_Pause_Back));
            RegMsg(typeof(Msg_Pause_Event));
            RegMsg(typeof(Msg_PayWithWinList_Back));
            RegMsg(typeof(Msg_PayWithWinList_Event));
            RegMsg(typeof(Msg_Pay_Back));
            RegMsg(typeof(Msg_Pay_Event));
            RegMsg(typeof(Msg_Reset_Back));
            RegMsg(typeof(Msg_Reset_Event));
            RegMsg(typeof(Msg_Sync_Back));
            RegMsg(typeof(Msg_Sync_Event));
            RegMsg(typeof(Msg_TicketConfig_Back));
            RegMsg(typeof(Msg_TicketConfig_Event));
            RegMsg(typeof(Msg_CreateOrder_Event));
            RegMsg(typeof(Msg_CreateOrder_Back));
            RegMsg(typeof(Msg_PayOrder_Event));
            RegMsg(typeof(Msg_PayOrder_Back));
            RegMsg(typeof(Msg_CancelOrder_Event));
            RegMsg(typeof(Msg_CancelOrder_Back));
            RegMsg(typeof(Msg_QueryOrder_Event));
            RegMsg(typeof(Msg_QueryOrder_Back));
            RegMsg(typeof(Msg_SetPrdInfo_Event));
            RegMsg(typeof(Msg_SetPrdInfo_Back));
            RegMsg(typeof(Msg_CheckCount_Event));
            RegMsg(typeof(Msg_CheckCount_Back));
            RegMsg(typeof(Msg_SetTime_Event));
            RegMsg(typeof(Msg_SetTime_Back));
            
        }

        public static Message ParseFromData(byte[] data,int datalength)
        {
            int tpid = (((int)(data[4])) << 8) | data[5];
            Type msgty = null;
            if(!MsgDic.TryGetValue(tpid,out msgty))
            {
                return null;
            }
            Message msg = Activator.CreateInstance(msgty) as Message;
            MsgDataStream ds = new MsgDataStream(data, datalength);
            msg.ReadData(ds);
            ds.Dispose();
            return msg;
        }
    }
}
