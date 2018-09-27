using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{

    public delegate Message ReviceMsg(Message msg);
    public delegate void IODebug(string msg, byte[] data);

    public  class CNGPIDevice : IDisposable
    {
        public event ReviceMsg OnReviceMsg;
        public event IODebug OnIODebug;

        /// <summary>
        /// 最后接收的事务ID
        /// </summary>
        int LastReciveTransID = 0;

        /// <summary>
        /// 最后发送的事务ID
        /// </summary>
        int LastSendTransID = 0;

        /// <summary>
        /// 线程控制
        /// </summary>
        private System.Threading.AutoResetEvent resetEvent;

        /// <summary>
        /// 端口
        /// </summary>
        private string portName;

        /// <summary>
        /// 本地模拟的设备
        /// </summary>
        public DeviceInfo LocalDev { get; private set; }

        /// <summary>
        /// 远程设备
        /// </summary>
        public DeviceInfo RemoteDev { get; private set; }

        /// <summary>
        /// 已连接
        /// </summary>
        private bool connected;

        /// <summary>
        /// 回应的数据
        /// </summary>
        private Message backMsg = null;

        private System.IO.Ports.SerialPort serialPort;

        public CNGPIDevice(DeviceInfo dev, string portname)
        {
            portName = portname;
            LocalDev = dev;
            resetEvent = new System.Threading.AutoResetEvent(false);
        }

        /// <summary>
        /// 正在发送数据
        /// </summary>
        bool isbuzy = false;

        /// <summary>
        /// 是否正在发送数据
        /// </summary>
        public bool IsBuzy
        {
            get
            {
                return isbuzy;
            }
        }

        DateTime LastSend = System.DateTime.Now;

        /// <summary>
        /// 发送并等待回应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public T SendAndBackMsg<T>(Message msg,int timeout) where T:Message
        {
            lock (this)
            {
                try
                {
                    isbuzy = true;
                    if ((System.DateTime.Now - LastSend).TotalMilliseconds < 6)
                    {
                        System.Threading.Thread.Sleep(6);//如果连续发送需要间隔6毫秒
                    }
                    if(msg is ITransMsg && !msg.MsgDataPacked)
                    {
                        (msg as ITransMsg).TransID = GetNextTransID();
                    }
                    byte[] data = msg.GetMsgData();
                    OnIODebug?.Invoke("发送", data);
                    backMsg = null;
                    LastSend = System.DateTime.Now;                    
                    serialPort.Write(data, 0, data.Length);
                    resetEvent.Reset();
                    if (!resetEvent.WaitOne(timeout))
                    {
                        throw new Exception("接收数据超时");
                    }
                    if(!(backMsg is T))
                    {
                        throw new Exception("回应的数据与预期不一致");
                    }
                    return backMsg as T;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    isbuzy = false;
                }
            }           
        }

        /// <summary>
        /// 回应数据
        /// </summary>
        /// <param name="msg"></param>
        public void BackMsg(Message msg)
        {
            if (msg == null) return;
            lock (this)
            {
                try
                {
                    isbuzy = true;
                    if ((System.DateTime.Now - LastSend).TotalMilliseconds < 6)
                    {
                        System.Threading.Thread.Sleep(6);//如果连续发送需要间隔6毫秒
                    }
                    byte[] data = msg.GetMsgData();
                    OnIODebug?.Invoke("发送", data);
                    backMsg = null;
                    LastSend = System.DateTime.Now;
                    serialPort.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    isbuzy = false;
                }
            }
        }

        /// <summary>
        /// 多次尝试发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="timeout"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public T SendNoRepeatAndBackMsg<T>(Message msg,int timeout,int times) where T : Message
        {
            Exception lastex = null;
            for(int i=0;i< times; i++)
            {
                try
                {
                    return SendAndBackMsg<T>(msg, timeout);
                }
                catch(Exception ex)
                {
                    lastex = ex;
                }
            }
            throw lastex;
        }

        /// <summary>
        /// 握手
        /// </summary>
        /// <returns></returns>
        public bool ConnectAndShakeHands()
        {
            if (connected) return true;
            serialPort = new System.IO.Ports.SerialPort(portName);
            try
            {
                serialPort.BaudRate = 38400;
                serialPort.Open();
                serialPort.DataReceived += SerialPort_DataReceived;
                Msg_Connect_Event msg_Connect_Event = new Msg_Connect_Event();
                msg_Connect_Event.ADR = 0;
                msg_Connect_Event.DeviceID = Utility.StrToByte(LocalDev.ID);
                msg_Connect_Event.DeviceType = LocalDev.DeviceType;
                msg_Connect_Event.ProductNum = LocalDev.ProductNum;
                var back = SendAndBackMsg<Msg_Connect_Back>(msg_Connect_Event,2000);
                RemoteDev = new DeviceInfo(back.ProductNum);
                RemoteDev.GamePortCount = back.GamePortCount;
                RemoteDev.ID = Utility.ByteToStr(back.DeviceID);
                RemoteDev.CurrPortIndex = back.CurrGamePortIndex;
                RemoteDev.UseVer = back.GPIVersion;
                RemoteDev.DeviceType= back.DeviceType;
                connected = true;
                LastReciveTransID = 0;
                LastSendTransID = 0;
            }
            catch(Exception ex)
            {
                serialPort.DataReceived -= SerialPort_DataReceived;
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                connected = false;
                throw ex;
            }
            return connected;
        }

        /// <summary>
        /// 尝试连接
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            if (connected) return true;
            serialPort = new System.IO.Ports.SerialPort(portName);
            try
            {
                serialPort.BaudRate = 38400;
                serialPort.Open();
                serialPort.DataReceived += SerialPort_DataReceived;
                connected = true;
            }
            catch (Exception ex)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                connected = false;
                throw ex;
            }
            return connected;
        }

        /// <summary>
        /// 得到下次
        /// </summary>
        /// <returns></returns>
        private int GetNextTransID()
        {
            LastSendTransID++;
            if(LastReciveTransID > 0xffff)
            {
                LastSendTransID = 0;
            }
            return LastSendTransID;
        }

        /// <summary>
        /// 判断是否接收过
        /// </summary>
        /// <param name="transID"></param>
        /// <returns></returns>
        private bool IsNewTransID(int transID)
        {
            return LastReciveTransID != transID;
        }

        Message LastTransBackMsg=null;

        private void ReciveEvent(Message recivemsg)
        {
            if(recivemsg is ITransMsg)
            {
                ITransMsg tran = recivemsg as ITransMsg;
                if (IsNewTransID(tran.TransID))
                {
                    LastReciveTransID = tran.TransID;
                    if (OnReviceMsg != null)
                    {
                        LastTransBackMsg = OnReviceMsg(recivemsg);
                        BackMsg(LastTransBackMsg);
                    }
                }
                else
                {
                    BackMsg(LastTransBackMsg);
                }
            }
            else
            {
                if (OnReviceMsg != null)
                {
                    BackMsg(OnReviceMsg(recivemsg));
                }
            }           
        }

        private Message ReadMsgFromPort()
        {
            if (serialPort.BytesToRead < 8) return null;
            byte[] buffer = new byte[40960];
            int bufferlength = 0;
            bufferlength = serialPort.Read(buffer, 0, 8);//读取包头
            if (buffer[0] != 0xEF || buffer[1] != 0x37)
            {
                byte[] bts = new byte[serialPort.BytesToRead + bufferlength];
                Array.Copy(buffer, bts, bufferlength);
                serialPort.Read(bts, bufferlength, serialPort.BytesToRead);
                OnIODebug?.Invoke("包头错误", bts);
                return null;
            }
            int DataLen = (((int)(buffer[6])) << 8) | buffer[7];
            int bttoread = serialPort.BytesToRead;
            int waittimes = 0;
            while (bttoread < DataLen + 4) //没接收完成
            {
                if (serialPort.BytesToRead > bttoread)
                {
                    waittimes = 0;//一直有数据收到就继续等
                }
                System.Threading.Thread.Sleep(1);
                waittimes++;
                if (waittimes > 5)//6毫秒都没收到数据代表没有了
                {
                    break;
                }
            }
            if (bttoread < DataLen + 4)
            {
                byte[] bts = new byte[serialPort.BytesToRead + bufferlength];
                Array.Copy(buffer, bts, bufferlength);
                serialPort.Read(bts, bufferlength, serialPort.BytesToRead);
                OnIODebug?.Invoke("接收到不完整的帧", bts);
                return null;
            }
            bufferlength += serialPort.Read(buffer, bufferlength, DataLen + 4);
            if (buffer[DataLen + 10] != 0xFE || buffer[DataLen + 11] != 0x73)
            {
                byte[] bts = new byte[bufferlength];
                Array.Copy(buffer, bts, bufferlength);
                OnIODebug?.Invoke("包尾错误", bts);
                return null;
            }
            byte sum = buffer[0];
            byte xor = buffer[0];
            for (int i = 1; i < bufferlength - 4; i++)
            {
                sum = (byte)(sum + buffer[i]);
                xor = (byte)(xor ^ buffer[i]);
            }
            if (sum != buffer[DataLen + 8] || xor != buffer[DataLen + 9])
            {
                byte[] bts = new byte[bufferlength];
                Array.Copy(buffer, bts, bufferlength);
                OnIODebug?.Invoke("数据校验错误", bts);
                return null;
            }
            if (OnIODebug != null)
            {
                byte[] bts = new byte[bufferlength];
                Array.Copy(buffer, bts, bufferlength);
                OnIODebug("收到", bts);
            }
            Message recivemsg = Message.ParseFromData(buffer, bufferlength);
            if (recivemsg == null)
            {
                byte[] bts = new byte[bufferlength];
                Array.Copy(buffer, bts, bufferlength);
                OnIODebug("不支持的指令", bts);
                return null;
            }
            return recivemsg;
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            var recivemsg = ReadMsgFromPort();
            while (recivemsg != null)
            {
                if (recivemsg is IBackMsg)
                {
                    //接收到其他信息就当作时回复
                    backMsg = recivemsg;
                    resetEvent.Set();
                }
                else
                {
                    //接收到事件消息冒泡出去
                    ReciveEvent(recivemsg);
                }
                recivemsg = ReadMsgFromPort();
            }
            serialPort.ReadExisting();
        }

        public void Dispose()
        {
            if (serialPort == null) return;
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            serialPort = null;
            connected = false;
        }
    }
}
