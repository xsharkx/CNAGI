using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CNGPI_PayBox
{
    public partial class MenuItem : UserControl
    {
        CNGPI.Msg_GetMenuDetail_Back configdet = null;
        public MenuItem()
        {
            InitializeComponent();
        }

        public CNGPI.Msg_GetMenuDetail_Back Menu
        {
            get
            {
                return configdet;
            }
            set
            {
                configdet = value;
            }
        }

        public uint? Value
        {
            get
            {
                uint valueset = 0;
                if (!UInt32.TryParse(txt_value.Text,out valueset))
                {
                    throw new Exception($"{configdet.ItemName}填写值不正确");
                }
                if (valueset == configdet.Value) return null;
                if(valueset< configdet.MinValue)
                {
                    throw new Exception($"{configdet.ItemName}填写值太小");
                }
                if (valueset > configdet.MaxValue)
                {
                    throw new Exception($"{configdet.ItemName}填写值太大");
                }
                return valueset;
            }
        }

        private void MenuItem_Load(object sender, EventArgs e)
        {
            if (configdet == null) return;
            lab_name.Text = configdet.ItemName;
            txt_value.Text = configdet.Value.ToString();
            lab_fanwei.Text = $"{configdet.MinValue}-{configdet.MaxValue}";
            toolTip1.SetToolTip(img_info, configdet.ItemDiscribe);
        }
    }
}
