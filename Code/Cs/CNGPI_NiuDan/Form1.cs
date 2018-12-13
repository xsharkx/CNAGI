using CNGPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CNGPI_NiuDan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }        

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports= System.IO.Ports.SerialPort.GetPortNames();
            foreach(string pn in ports)
            {
                base_comname.Items.Add(pn);
            }
            if (base_comname.Items.Count > 0)
            {
                base_comname.SelectedIndex = 0;
            }            
        }

        CNGPI.CNGPIGameMachine GameMac = null;
        CNGPI.DeviceInfo Devinfo = null;

        private void DebugInfo(string msg)
        {
            this.BeginInvoke((EventHandler)delegate {
                event_txt.AppendText(System.DateTime.Now.ToString("HH:mm:ss(fff)") + "\r\n" + msg + "\r\n\r\n\r\n");
            });
            
        }

        private void Box_OnIODebug(string msg, byte[] data)
        {
            if(msg.IndexOf("状态同步")!=-1 && !chk_showsync.Checked)
            {
                return;
            }
            DebugInfo($"{msg}:\r\n{CNGPI.Utility.ByteToHex2(data)}");
        }

        private void base_bt_conn_Click(object sender, EventArgs e)
        {
           

            if (GameMac == null)
            {
                try
                {
                    Devinfo = new CNGPI.DeviceInfo((uint)Int32.Parse(textBox2.Text,System.Globalization.NumberStyles.HexNumber));
                    Devinfo.DeviceType = 0x0004;
                    Devinfo.ID = CNGPI.Utility.ByteToHex(Guid.NewGuid().ToByteArray());
                    Devinfo.SoftVer = 120;
                    Devinfo.CNGPIVer = 119;

                    CNGPI.CNGPIGameMachine box = new CNGPI.CNGPIGameMachine(Devinfo, base_comname.Text.ToString());
                    box.OnIODebug += Box_OnIODebug;
                    box.OnReviceMsg += Box_OnReviceMsg;
                    if (box.Connect())
                    {
                        GameMac = box;
                    }
                }
                catch (Exception ex)
                {
                    event_txt.AppendText(ex.ToString());
                }
                if (GameMac != null)
                {
                    base_bt_conn.Text = "断开连接";
                }
            }
            else
            {
                GameMac.Dispose();
                GameMac = null;
                base_bt_conn.Text = "连接并握手";
            }
        }

        int CoinCount = 0;
        byte State = 0;
        int CoinPertime = 2;

        string lastordernum = "";


        byte[] exdate = Guid.NewGuid().ToByteArray();

        private CNGPI.Message Box_OnReviceMsg(CNGPI.Message msg)
        {
            switch (msg.PID)
            {
                case 0x0101:
                    return new CNGPI.Msg_Connect_Back()
                    {
                        ADR = 0,
                        CurrGamePortIndex = 1,
                        DeviceID = CNGPI.Utility.HexToByte(Devinfo.ID),
                        DeviceType=Devinfo.DeviceType,
                        GamePortCount=1,
                        GPIVersion= Devinfo.CNGPIVer,
                        ProductNum=Devinfo.ProductNum,
                        SoftVer=Devinfo.SoftVer
                    };
                case 0x0102:
                    return new CNGPI.Msg_Sync_Back()
                    {
                        ADR = 1,
                        ErrCode=0,
                        RemainCoin= CoinCount,
                        RemainSec=0,
                        State= State
                    };
                case 0x0103:
                    CoinCount += (msg as Msg_Pay_Event).Coins;
                    return new CNGPI.Msg_Pay_Back()
                    {
                        ADR = 1,
                        ErrCode=0,
                        TransID= (msg as CNGPI.ITransMsg).TransID
                    };
                case 0x0104:
                    CoinPertime = (msg as Msg_Config_Event).CoinsPerTimes;
                    return new CNGPI.Msg_Config_Back()
                    {
                        ADR = 1,
                        ErrCode = 0
                    };
                case 0x0108:
                    CoinCount = 0;
                    State = 0;
                    CoinPertime = 2;
                    exdate = Guid.NewGuid().ToByteArray();
                    return new CNGPI.Msg_Reset_Back()
                    {
                        ADR = 1,
                        ErrCode = 0
                    };
                case 0x0109:
                    return new CNGPI.Msg_GetConfig_Back()
                    {
                        ADR = 1,
                        ErrCode=0,
                        CoinsPerTimes= CoinPertime
                    };
                case 0x010B:
                    return new CNGPI.Msg_GetCounter_Back()
                    {
                        ADR = 1,
                        ErrCode = 0,
                        PCoins=123456,
                        ECoins=456789,
                        PGifts=9876,
                        PTickets=976655344
                    };
                case 0x0403:                    
                    int errcode = 0;
                    string ordernum = (msg as Msg_PayOrder_Event).OrderNum;
                    if (lastordernum == ordernum)
                    {
                        errcode = 0;
                    }
                    else
                    {
                        lastordernum = ordernum;
                        int currkucun = Int32.Parse(textBox1.Text);
                        if (currkucun > 0)
                        {
                            this.BeginInvoke((EventHandler)delegate
                            {
                                textBox1.Text = (currkucun - 1).ToString();
                            });
                        }
                        else
                        {
                            errcode = 0x020C;
                        }

                        if (errcode == 0 && checkBox1.Checked)
                        {
                            errcode = 0x020A;
                        }
                        else
                        {
                            if (checkBox2.Checked)
                            {
                                errcode = 0x020B;
                            }
                        }
                    }
                    return new CNGPI.Msg_PayOrder_Back()
                    {
                        ADR = 1,
                        ErrCode = errcode,
                        TransID = (msg as CNGPI.ITransMsg).TransID
                    };
                case 0x0111:
                    return new CNGPI.Msg_GetMenuIndex_Back()
                    {
                        ADR = 1,
                        ErrCode = 0,
                        MenuItems = new int[] { 10, 11},
                    };
                case 0x0110:
                    var it = msg as CNGPI.Msg_GetMenuDetail_Event;
                    switch (it.ItemID)
                    {
                        case 10:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "实际扭蛋直径",
                                ItemID = it.ItemID,
                                ItemName = "扭蛋直径(mm)",
                                Value = GetMenuV(it.ItemID, 45),
                                MaxValue = 50,
                                MinValue = 40,
                            };
                        case 11:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "货将售罄检测",
                                ItemID = it.ItemID,
                                ItemName = "货将售罄检测",
                                Value = GetMenuV(it.ItemID, 1),
                                MaxValue = 1,
                                MinValue = 0,
                            };                        
                        default:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 1,
                            };
                    }
                case 0x010F:
                    var menuset = msg as CNGPI.Msg_MenuSet_Event;
                    SetMenuV(menuset.ItemID, menuset.ItemValue);
                    return new CNGPI.Msg_MenuSet_Back()
                    {
                        ADR = 1,
                        ErrCode = 0,
                    };
                default:
                    return null;
            }
        }

        Dictionary<int, uint> MenuValue = new Dictionary<int, uint>();

        private uint GetMenuV(int itemid, uint defv)
        {
            if (MenuValue.ContainsKey(itemid))
            {
                return MenuValue[itemid];
            }
            return defv;
        }

        private void SetMenuV(int itemid, uint value)
        {
            if (MenuValue.ContainsKey(itemid))
            {
                MenuValue[itemid] = value;
            }
            else
            {
                MenuValue.Add(itemid, value);
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/xsharkx/CNGPI");
            }
            catch
            {

            }
        }

        private void gift_start_Click(object sender, EventArgs e)
        {
            try
            {
                GameMac.SendNoRepeatAndBackMsg<Msg_GameStart_Back>(new Msg_GameStart_Event()
            {
                ADR=1
            },500,10);
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void gift_finish_Click(object sender, EventArgs e)
        {
            try
            {
                GameMac.SendNoRepeatAndBackMsg<Msg_GameFinish_Back>(new Msg_GameFinish_Event()
                {
                    ADR = 1,
                    GiftPort = 0,
                    GiftType = 2,
                    OutGift = 100
                }, 500, 10);
            }
            catch(Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void gift_finish2_Click(object sender, EventArgs e)
        {
            try
            {
                GameMac.SendNoRepeatAndBackMsg<Msg_GameFinish_Back>(new Msg_GameFinish_Event()
                {
                    ADR = 1,
                    GiftPort = 0,
                    GiftType = 2,
                    OutGift = 0
                }, 500, 10);
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            event_txt.Clear();
        }
    }
}
