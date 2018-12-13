using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CNGPI_PayBox_Wawa
{
    public partial class Wawa : Form
    {
        public Wawa()
        {
            InitializeComponent();
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

        System.Windows.Forms.Timer tm = null;

        CNGPI.CNGPIPayBox PayBox = null;
        CNGPI.DeviceInfo Devinfo = null;

        int laststate = 0;

        private void base_bt_conn_Click(object sender, EventArgs e)
        {
            if (PayBox == null)
            {
                try
                {
                    CNGPI.CNGPIPayBox box = new CNGPI.CNGPIPayBox(Devinfo, base_comname.Text.ToString());
                    box.OnIODebug += Box_OnIODebug; ;
                    box.OnReviceMsg += Box_OnReviceMsg; ;
                    if (box.ConnectAndShakeHands())
                    {
                        PayBox = box;
                    }
                }
                catch (Exception ex)
                {
                    event_txt.AppendText(ex.ToString());
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
                base_bt_conn.Text = "连接";
            }
            if (PayBox != null)
            {
                if (PayBox.RemoteDev.DeviceType != 0x0004)
                {
                    MessageBox.Show("连接设备非扭蛋机");
                }
                else
                {
                    lab_devid.Text = PayBox.RemoteDev.ID;
                    lab_proid.Text = PayBox.RemoteDev.ProductNum.ToString("X2").PadLeft(8, '0');
                    lab_softver.Text= PayBox.RemoteDev.SoftVer.ToString();
                    lab_ver.Text= $"V{PayBox.RemoteDev.CNGPIVer / 100}.{PayBox.RemoteDev.CNGPIVer % 100}";
                    try
                    {
                        var back = PayBox.SendAndBackMsg<CNGPI.Msg_GetConfig_Back>(new CNGPI.Msg_GetConfig_Event()
                        {
                            ADR = PayBox.RemoteDev.CurrPortIndex,
                        }, 500);
                        if (back.ErrCode == 0)
                        {
                            if (back.CoinsPerTimes != Int32.Parse(txt_gameprice.Text))
                            {
                                var backset = PayBox.SendAndBackMsg<CNGPI.Msg_Config_Back>(new CNGPI.Msg_Config_Event()
                                {
                                    ADR = PayBox.RemoteDev.CurrPortIndex,
                                    CoinsPerTimes = Int32.Parse(txt_gameprice.Text)
                                }, 500);
                                if (backset.ErrCode != 0)
                                {
                                    DebugInfo($"错误{GetErrorCode(backset.ErrCode)}");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            DebugInfo($"错误{GetErrorCode(back.ErrCode)}");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugInfo(ex.Message);
                        return;
                    }

                    try
                    {
                        var backcount = PayBox.SendAndBackMsg<CNGPI.Msg_GetCounter_Back>(new CNGPI.Msg_GetCounter_Event()
                        {
                            ADR = PayBox.RemoteDev.CurrPortIndex
                        }, 2000);
                        if (backcount.ErrCode == 0)
                        {
                            lab_out.Text = backcount.PGifts.ToString();
                        }
                        else
                        {
                            DebugInfo($"错误{GetErrorCode(backcount.ErrCode)}");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugInfo(ex.Message);
                        return;
                    }
                    groupBox2.Enabled = true;
                }
            }
        }

        private CNGPI.Message Box_OnReviceMsg(CNGPI.Message msg)
        {
            return null;
        }

        private void NiuDan_Load(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string pn in ports)
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

            //订单号生成方法
            byte[] ordernum = Guid.NewGuid().ToByteArray();

            tm = new System.Windows.Forms.Timer();
            tm.Interval = 10000;
            tm.Tick += Tm_Tick; ;
            tm.Enabled = true;
        }

        private void Tm_Tick(object sender, EventArgs e)
        {
            if (PayBox == null) return;
            if (PayBox.IsBuzy) return;
            try
            {
                var back = PayBox.SendAndBackMsg<CNGPI.Msg_Sync_Back>(new CNGPI.Msg_Sync_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    State = 0
                }, 500);
                if (back.ErrCode == 0)
                {
                    laststate = back.State;
                    lab_state.Text = back.State.ToString("X2");
                }
                else
                {
                    lab_state.Text = $"错误{GetErrorCode(back.ErrCode)}";
                }
            }
            catch (Exception ex)
            {
                DebugInfo(ex.Message);
            }
        }

        private void NiuDan_FormClosed(object sender, FormClosedEventArgs e)
        {
            tm.Enabled = false;
        }

        private void bt_pay_Click(object sender, EventArgs e)
        {
            if(laststate== 0x020C)
            {
                MessageBox.Show("Sell out");
                return;
            }
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_PayOrder_Back>(new CNGPI.Msg_PayOrder_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                    OrderNum = CNGPI.Utility.ByteToHex(Guid.NewGuid().ToByteArray()),
                    BoxNum = 0,
                    Price = (uint)Int32.Parse(txt_sellprice.Text),
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

        private void button1_Click(object sender, EventArgs e)
        {
            CNGPI_PayBox.FrmConfig cfg = new CNGPI_PayBox.FrmConfig();
            cfg.PayBox = PayBox;
            cfg.Show();
        }

        private void DebugInfo(string msg)
        {
            this.BeginInvoke((EventHandler)delegate {
                event_txt.AppendText(System.DateTime.Now.ToString("HH:mm:ss(fff)") + "\r\n" + msg + "\r\n\r\n\r\n");
            });

        }

        private void Box_OnIODebug(string msg, byte[] data)
        {
            DebugInfo($"{msg}:\r\n{CNGPI.Utility.ByteToHex2(data)}");
        }

        private string GetErrorCode(int code)
        {
            return code.ToString("X2").PadLeft(4, '0');
        }

        private void button4_Click(object sender, EventArgs e)
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
    }
}
