using CNGPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CNGPI_PayBox
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
            Devinfo = new CNGPI.DeviceInfo(0x00010001);
            Devinfo.DeviceType = 0x0103;
            Devinfo.ID = CNGPI.Utility.ByteToHex(Guid.NewGuid().ToByteArray());
            sync_cb_state.SelectedIndex = 0;
        }

        CNGPI.CNGPIPayBox PayBox = null;
        CNGPI.DeviceInfo Devinfo = null;

        private void base_bt_fresh_Click(object sender, EventArgs e)
        {
            if (PayBox != null)
            {
                PayBox.Dispose();
                PayBox = null;
            }
            for (int i=0;i< base_comname.Items.Count; i++)
            {
                try
                {
                    CNGPI.CNGPIPayBox box = new CNGPI.CNGPIPayBox(Devinfo, base_comname.Items[i].ToString());
                    box.OnIODebug += Box_OnIODebug;
                    box.OnReviceMsg += Box_OnReviceMsg;
                    if (box.ConnectAndShakeHands())
                    {
                        PayBox = box;
                        break;
                    }
                }
                catch
                {

                }
            }
            PayBoxConnect();
        }

        private CNGPI.Message Box_OnReviceMsg(CNGPI.Message msg)
        {
            switch (msg.PID)
            {
                case 0x010C:
                    DebugInfo("收到报警");
                    return new Msg_GameAlert_Back()
                    {
                        TransID = (msg as CNGPI.ITransMsg).TransID,
                        ADR = PayBox.RemoteDev.CurrPortIndex
                    };
                case 0x0105:
                    DebugInfo("开始游戏");
                    return new Msg_GameStart_Back()
                    {
                        TransID = (msg as CNGPI.ITransMsg).TransID,
                        ADR = PayBox.RemoteDev.CurrPortIndex
                    };
                case 0x0106:
                    DebugInfo($"结束游戏:{msg}");
                    return new Msg_GameFinish_Back()
                    {
                        TransID = (msg as CNGPI.ITransMsg).TransID,
                        ADR = PayBox.RemoteDev.CurrPortIndex,
                        ErrCode=0
                    };
                case 0x0401:
                    DebugInfo($"下单支付");
                    return new Msg_CreateOrder_Back()
                    {
                        ADR = PayBox.RemoteDev.CurrPortIndex,
                        ErrCode = 0,
                        OrderNum = (msg as CNGPI.Msg_CreateOrder_Event).OrderNum,
                        State = 0,
                        QrCode = "https://github.com/xsharkx/CNGPI/raw/master/logo/CNGPIV1.png"
                    };
                case 0x0403:
                    DebugInfo($"取消订单");
                    return new Msg_CancelOrder_Back()
                    {
                        ADR = PayBox.RemoteDev.CurrPortIndex,
                        ErrCode = 0,
                    };
                case 0x0404:
                    DebugInfo($"查询订单");
                    return new Msg_QueryOrder_Back()
                    {
                        ADR = PayBox.RemoteDev.CurrPortIndex,
                        ErrCode = 0,
                        State=1,
                    };
            }
            return null;
        }

        private void DebugInfo(string msg)
        {
            this.BeginInvoke((EventHandler)delegate {
                event_txt.AppendText(msg + "\r\n\r\n");
            });
        }

        private void Box_OnIODebug(string msg, byte[] data)
        {
            DebugInfo($"{msg}:\r\n{CNGPI.Utility.ByteToHex2(data)}");
        }

        private void PayBoxConnect()
        {
            grp_nomal.Enabled=grp_sync.Enabled = (PayBox != null);
            if (PayBox != null)
            {
                grp_gift.Enabled = PayBox.RemoteDev.DeviceType == 0x0003 || PayBox.RemoteDev.DeviceType == 0x0005;
                grp_tigket.Enabled = PayBox.RemoteDev.DeviceType == 0x0002;
                grp_sell.Enabled = PayBox.RemoteDev.DeviceType == 0x0004 || PayBox.RemoteDev.DeviceType == 0x0005;
            }
            else
            {
                grp_gift.Enabled = false;
                grp_tigket.Enabled = false;
                grp_sell.Enabled = false;
            }
        }

        private void base_bt_conn_Click(object sender, EventArgs e)
        {
            if (PayBox == null)
            {
                try
                {
                    CNGPI.CNGPIPayBox box = new CNGPI.CNGPIPayBox(Devinfo, base_comname.Text.ToString());
                    box.OnIODebug += Box_OnIODebug;
                    box.OnReviceMsg += Box_OnReviceMsg;
                    if (box.ConnectAndShakeHands())
                    {
                        PayBox = box;
                    }
                }
                catch
                {

                }
                if (PayBox != null)
                {
                    base_bt_conn.Text = "断开连接";
                }
            }
            else
            {
                PayBox.Dispose();
                PayBox = null;
                base_bt_conn.Text = "连接并握手";
            }
            PayBoxConnect();
        }

        private void all_bt_readconfig_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_GetConfig_Back>(new CNGPI.Msg_GetConfig_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex
                }, 2000);
                if (back.ErrCode == 0)
                {
                    all_num_coin.Value = back.CoinsPerTimes;
                    DebugInfo("成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }

            }
            catch(Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private string GetErrorCode(int code)
        {
            return code.ToString("X2").PadLeft(4, '0');
        }

        private void all_bt_writeconfig_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_Config_Back>(new CNGPI.Msg_Config_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    CoinsPerTimes = (int)all_num_coin.Value
                }, 2000);
                if (back.ErrCode == 0)
                {
                    DebugInfo("成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_readexconfig_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_GetConfigEx_Back>(new CNGPI.Msg_GetConfigEx_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                }, 2000);
                if (back.ErrCode == 0)
                {
                    DebugInfo("成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_writeexconfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            if (of.ShowDialog() != DialogResult.OK) return;
            byte[] exd = System.IO.File.ReadAllBytes(of.FileName);
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_ConfigEx_Back>(new CNGPI.Msg_ConfigEx_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    ExData= exd
                }, 2000);
                if (back.ErrCode == 0)
                {
                    DebugInfo("成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_reset_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_Reset_Back>(new CNGPI.Msg_Reset_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex
                }, 2000);
                if (back.ErrCode == 0)
                {
                    DebugInfo("成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_getcount_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_GetCounter_Back>(new CNGPI.Msg_GetCounter_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex
                }, 2000);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功\r\n{back}");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_stop_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_Pause_Back>(new CNGPI.Msg_Pause_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    Action=0
                }, 2000);
                if (back != null)
                {
                    DebugInfo("成功");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_restart_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_Pause_Back>(new CNGPI.Msg_Pause_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    Action = 1
                }, 2000);
                if (back != null)
                {
                    DebugInfo("成功");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_pay_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_Pay_Back>(new CNGPI.Msg_Pay_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    Coins= (int)all_num_coin.Value,
                    CoinType=0
                },500, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void gift_bt_readconfig_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_GetGiftConfig_Back>(new CNGPI.Msg_GetGiftConfig_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                }, 500);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                    gift_num_coin.Value = back.CoinsPerTimes;
                    gift_num_gametime.Value = back.GameSec;
                    gift_num_highpower.Value = back.HightPower;
                    gift_num_lowpower.Value = back.LowPower;
                    gift_tb_paycoin.Text = back.WinCoins.ToString();
                    gift_tb_paycoin_out.Text = back.WinGifts.ToString();
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void gift_bt_writeconfig_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_GiftConfig_Back>(new CNGPI.Msg_GiftConfig_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    CoinsPerTimes = (int)gift_num_coin.Value,
                    GameSec = (int)gift_num_gametime.Value,
                    HightPower = (int)gift_num_highpower.Value,
                    LowPower = (int)gift_num_lowpower.Value,
                    WinCoins = Int32.Parse(gift_tb_paycoin.Text),
                    WinGifts= Int32.Parse(gift_tb_paycoin_out.Text),
                }, 500);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void gift_bt_paywithwinlist_Click(object sender, EventArgs e)
        {
            try
            {
                string[] items = gift_tb_winlist.Text.Split(',');
                List<int> list = new List<int>();
                foreach(string item in items)
                {
                    if (item.Trim().Length == 0) continue;
                    list.Add(Int32.Parse(item));
                }
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_PayWithWinList_Back>(new CNGPI.Msg_PayWithWinList_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    WinList = list
                }, 500, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void all_bt_real_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_Pay_Back>(new CNGPI.Msg_Pay_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    Coins = (int)all_num_coin.Value,
                    CoinType =1
                }, 500, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
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

        private void sync_bt_sync_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_Sync_Back>(new CNGPI.Msg_Sync_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    State= Int32.Parse(sync_cb_state.Text.Split('=')[0].Replace("0x",""))
                }, 500);
                if (back.ErrCode ==0)
                {
                    sync_lab_state.Text = back.State.ToString("X2");
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void sell_bt_payok_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_PayOrder_Back>(new CNGPI.Msg_PayOrder_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    OrderNum = Utility.ByteToHex(Guid.NewGuid().ToByteArray()),
                    State=1
                }, 500, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }           
        }

        private void sell_bt_payerr_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_PayOrder_Back>(new CNGPI.Msg_PayOrder_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    OrderNum = Utility.ByteToHex(Guid.NewGuid().ToByteArray()),
                    State = 2
                }, 500, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void sell_bt_setpro_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_SetPrdInfo_Back>(new CNGPI.Msg_SetPrdInfo_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    BoxNum = 0,
                    Name = "口红2",
                    Price = 100,
                    Cost=80,
                    Url = "https://github.com/xsharkx/CNGPI/raw/master/logo/CNGPIV1.png",
                }, 1000, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void sell_bt_checkpro_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_CheckCount_Back>(new CNGPI.Msg_CheckCount_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    BoxNum = 0,
                    Amount=1,
                }, 1000, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void sell_paying_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_PayOrder_Back>(new CNGPI.Msg_PayOrder_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    OrderNum = Utility.ByteToHex(Guid.NewGuid().ToByteArray()),
                    State = 0
                }, 500, 10);
                if (back.ErrCode == 0)
                {
                    DebugInfo($"成功");
                }
                else
                {
                    DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        public long GetTimeLikeJS()
        {
            long lLeft = 621355968000000000;
            DateTime dt = DateTime.Now;
            long Sticks = (dt.Ticks - lLeft) / 10000;
            return Sticks;
        }

        private void sell_settime_Click(object sender, EventArgs e)
        {
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_SetTime_Back>(new CNGPI.Msg_SetTime_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    Time= GetTimeLikeJS()/1000
                }, 2000);
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }
    }
}
