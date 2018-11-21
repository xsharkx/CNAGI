using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CNGPI_PayBox
{
    public partial class FrmConfig : Form
    {
        public FrmConfig()
        {
            InitializeComponent();
        }

        public CNGPI.CNGPIPayBox PayBox
        {
            get;set;
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            FreshItems();
        }

        private void FreshItems()
        {
            bt_save.Enabled = false;
            bt_save.Text = "正在读取菜单索引";
            pl_menulist.Controls.Clear();
            Application.DoEvents();
            try
            {
                var back = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_GetMenuIndex_Back>(new CNGPI.Msg_GetMenuIndex_Event()
                {
                    ADR = PayBox.RemoteDev.CurrPortIndex,
                }, 500, 2);
                if (back.ErrCode == 0)
                {
                    for (int i = 0; i < back.MenuItems.Length; i++)
                    {
                        bt_save.Text = $"正在读取菜单项:{back.MenuItems[i]}";
                        Application.DoEvents();
                        var item = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_GetMenuDetail_Back>(new CNGPI.Msg_GetMenuDetail_Event()
                        {
                            ADR = PayBox.RemoteDev.CurrPortIndex,
                            ItemID = back.MenuItems[i]
                        }, 500, 2);
                        MenuItem it = new MenuItem();
                        it.Menu = item;
                        pl_menulist.Controls.Add(it);
                    }
                    bt_save.Text = "保存";
                    bt_save.Enabled = true;
                }
                else
                {
                    MessageBox.Show($"错误{back.ErrCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < pl_menulist.Controls.Count; i++)
            {
                var item = pl_menulist.Controls[i] as MenuItem;
                try
                {
                    var newv = item.Value;
                    if (newv == null) continue;
                    var itemback = PayBox.SendNoRepeatAndBackMsg<CNGPI.Msg_MenuSet_Back>(new CNGPI.Msg_MenuSet_Event()
                    {
                        ADR = PayBox.RemoteDev.CurrPortIndex,
                        ItemID = item.Menu.ItemID,
                        ItemValue = newv.Value
                    }, 500, 2);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            FreshItems();
        }
    }
}
