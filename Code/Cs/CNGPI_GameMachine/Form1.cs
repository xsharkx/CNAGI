using CNGPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CNGPI_GameMachine
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
            Devinfo = new CNGPI.DeviceInfo(0x00030001);
            Devinfo.DeviceType = 0x0003;
            Devinfo.ID = CNGPI.Utility.ByteToHex(Guid.NewGuid().ToByteArray());
            Devinfo.SoftVer = 100;
            Devinfo.CNGPIVer = 116;
        }

        CNGPI.CNGPIGameMachine GameMac = null;
        CNGPI.DeviceInfo Devinfo = null;

        private void DebugInfo(string msg)
        {
            this.BeginInvoke((EventHandler)delegate {
                event_txt.AppendText(System.DateTime.Now.ToString("HH:mm:ss(fff)")+"\r\n"+msg + "\r\n\r\n\r\n");
            });
            
        }

        private void Box_OnIODebug(string msg, byte[] data)
        {
            if (msg.IndexOf("状态同步") != -1 && !chk_showsync.Checked)
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
                    CNGPI.CNGPIGameMachine box = new CNGPI.CNGPIGameMachine(Devinfo, base_comname.Text.ToString());
                    box.OnIODebug += Box_OnIODebug;
                    box.OnReviceMsg += Box_OnReviceMsg;
                    if (box.Connect())
                    {
                        GameMac = box;
                    }
                }
                catch(Exception ex)
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
            grp_gift.Enabled = GameMac != null;
        }

        int CoinCount = 0;
        int State = 1;
        int CoinPertime = 2;
        byte[] exdate = Guid.NewGuid().ToByteArray();

        Dictionary<int, uint> MenuValue = new Dictionary<int, uint>();

        private uint GetMenuV(int itemid,uint defv)
        {
            if (MenuValue.ContainsKey(itemid))
            {
                return MenuValue[itemid];
            }
            return defv;
        }

        private void SetMenuV(int itemid,uint value)
        {
            if (MenuValue.ContainsKey(itemid))
            {
                MenuValue[itemid] = value;
            }
            else
            {
                MenuValue.Add(itemid,value);
            }
        }

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
                        GPIVersion=(uint)Devinfo.CNGPIVer,
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
                case 0x0107:
                    exdate = (msg as Msg_ConfigEx_Event).ExData;
                    return new CNGPI.Msg_ConfigEx_Back()
                    {
                        ADR = 1,
                        ErrCode = 0
                    };
                case 0x0108:
                    CoinCount = 0;
                    State = 1;
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
                case 0x010A:
                    return new CNGPI.Msg_GetConfigEx_Back()
                    {
                        ADR = 1,
                        ErrCode = 0,
                        ExData=exdate
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
                case 0x010D:
                    if((msg as Msg_Pause_Event).Action == 0)
                    {
                        State = 0x07;
                    }
                    else
                    {
                        State = 1;
                    }
                    return new CNGPI.Msg_Pause_Back()
                    {
                        ADR = 1,
                        TransID = (msg as CNGPI.ITransMsg).TransID
                    };
                case 0x0201:
                    CoinCount += (msg as Msg_PayWithWinList_Event).WinList.Count*CoinPertime;
                    return new CNGPI.Msg_PayWithWinList_Back()
                    {
                        ADR = 1,
                        ErrCode = 0,
                        TransID = (msg as CNGPI.ITransMsg).TransID
                    };

                case 0x0202:
                    CoinPertime = (msg as Msg_GiftConfig_Event).CoinsPerTimes;
                    return new CNGPI.Msg_GiftConfig_Back()
                    {
                        ADR = 1,
                        ErrCode = 0
                    };
                case 0x0203:
                    return new CNGPI.Msg_GetGiftConfig_Back()
                    {
                        ADR = 1,
                        ErrCode=0,
                        CoinsPerTimes= CoinPertime,
                        GameSec=10,
                        HightPower=90,
                        LowPower=10,
                        WinCoins=100,
                        WinGifts=9
                    };
                case 0x0111:
                    return new CNGPI.Msg_GetMenuIndex_Back()
                    {
                        ADR = 1,
                        ErrCode = 0,
                        MenuItems = new int[] { 10,11,12,14,15,17,18,19},
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
                                ItemDiscribe = "越低越容易中奖",
                                ItemID= it.ItemID,
                                ItemName="中奖难度",
                                Value=GetMenuV(it.ItemID,10),
                                MaxValue=20,
                                MinValue=1,
                            };
                        case 11:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "不中奖时按钮按下时的爪力",
                                ItemID = it.ItemID,
                                ItemName = "弱爪第一爪力",
                                Value = GetMenuV(it.ItemID, 5),
                                MaxValue = 10,
                                MinValue = 1,
                            };
                        case 12:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "不中奖时提起奖品爪力",
                                ItemID = it.ItemID,
                                ItemName = "弱爪第二爪力",
                                Value = GetMenuV(it.ItemID, 5),
                                MaxValue = 10,
                                MinValue = 1,
                            };
                        case 14:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "不中奖时拉回奖品爪力",
                                ItemID = it.ItemID,
                                ItemName = "弱爪第三爪力",
                                Value = GetMenuV(it.ItemID, 5),
                                MaxValue = 10,
                                MinValue = 1,
                            };
                        case 15:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "1=欢快,2=萌萌,3=酷",
                                ItemID = it.ItemID,
                                ItemName = "游戏音乐",
                                Value = GetMenuV(it.ItemID, 2),
                                MaxValue = 3,
                                MinValue = 1,
                            };
                        case 17:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "1=开,0=关",
                                ItemID = it.ItemID,
                                ItemName = "中奖爆闪灯",
                                Value = GetMenuV(it.ItemID, 1),
                                MaxValue = 1,
                                MinValue = 0,
                            };
                        case 18:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "一轮游戏次数",
                                ItemID = it.ItemID,
                                ItemName = "每轮次数",
                                Value = GetMenuV(it.ItemID, 40),
                                MaxValue = 100,
                                MinValue = 0,
                            };
                        case 19:
                            return new CNGPI.Msg_GetMenuDetail_Back()
                            {
                                ADR = msg.ADR,
                                ErrCode = 0,
                                ItemDiscribe = "一轮中奖次数,与一轮次数结合使用",
                                ItemID = it.ItemID,
                                ItemName = "每轮中奖数",
                                Value = GetMenuV(it.ItemID, 2),
                                MaxValue = 100,
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

        private void gift_alert_Click(object sender, EventArgs e)
        {
            try
            {
                GameMac.SendNoRepeatAndBackMsg<Msg_GameAlert_Back>(new Msg_GameAlert_Event()
                {
                    ADR = 1,
                    AlertType = 1
                }, 100, 5);
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            event_txt.Clear();
        }
    }
}
