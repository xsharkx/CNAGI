namespace CNGPI_Vending
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chk_showsync = new System.Windows.Forms.CheckBox();
            this.base_bt_conn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.base_comname = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.event_txt = new System.Windows.Forms.TextBox();
            this.grp_gift = new System.Windows.Forms.GroupBox();
            this.gift_alert = new System.Windows.Forms.Button();
            this.gift_finish2 = new System.Windows.Forms.Button();
            this.gift_finish = new System.Windows.Forms.Button();
            this.gift_start = new System.Windows.Forms.Button();
            this.grp_sell = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_boxnum = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grp_gift.SuspendLayout();
            this.grp_sell.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 79);
            this.panel1.TabIndex = 3;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(86, 43);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(417, 15);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Github 开源项目地址 https://github.com/xsharkx/CNGPI";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(677, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "CNGPI是开源的游戏机行业通用协议，用于将游戏机与其他外围设备互通，欢迎各位行业大神参与项目";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chk_showsync);
            this.groupBox1.Controls.Add(this.base_bt_conn);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.base_comname);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(17, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 136);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基础参数";
            // 
            // chk_showsync
            // 
            this.chk_showsync.AutoSize = true;
            this.chk_showsync.Location = new System.Drawing.Point(191, 55);
            this.chk_showsync.Name = "chk_showsync";
            this.chk_showsync.Size = new System.Drawing.Size(89, 19);
            this.chk_showsync.TabIndex = 10;
            this.chk_showsync.Text = "显示同步";
            this.chk_showsync.UseVisualStyleBackColor = true;
            // 
            // base_bt_conn
            // 
            this.base_bt_conn.Location = new System.Drawing.Point(11, 83);
            this.base_bt_conn.Name = "base_bt_conn";
            this.base_bt_conn.Size = new System.Drawing.Size(273, 42);
            this.base_bt_conn.TabIndex = 4;
            this.base_bt_conn.Text = "开始模拟售货机";
            this.base_bt_conn.UseVisualStyleBackColor = true;
            this.base_bt_conn.Click += new System.EventHandler(this.base_bt_conn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "波特率： 38400";
            // 
            // base_comname
            // 
            this.base_comname.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.base_comname.FormattingEnabled = true;
            this.base_comname.Location = new System.Drawing.Point(91, 23);
            this.base_comname.Name = "base_comname";
            this.base_comname.Size = new System.Drawing.Size(189, 23);
            this.base_comname.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "通讯端口:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.event_txt);
            this.groupBox3.Location = new System.Drawing.Point(335, 85);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(428, 572);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "事件";
            // 
            // event_txt
            // 
            this.event_txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.event_txt.Location = new System.Drawing.Point(3, 21);
            this.event_txt.Multiline = true;
            this.event_txt.Name = "event_txt";
            this.event_txt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.event_txt.Size = new System.Drawing.Size(422, 548);
            this.event_txt.TabIndex = 0;
            // 
            // grp_gift
            // 
            this.grp_gift.Controls.Add(this.gift_alert);
            this.grp_gift.Controls.Add(this.gift_finish2);
            this.grp_gift.Controls.Add(this.gift_finish);
            this.grp_gift.Controls.Add(this.gift_start);
            this.grp_gift.Enabled = false;
            this.grp_gift.Location = new System.Drawing.Point(17, 227);
            this.grp_gift.Name = "grp_gift";
            this.grp_gift.Size = new System.Drawing.Size(294, 176);
            this.grp_gift.TabIndex = 8;
            this.grp_gift.TabStop = false;
            this.grp_gift.Text = "礼品机";
            // 
            // gift_alert
            // 
            this.gift_alert.Location = new System.Drawing.Point(11, 120);
            this.gift_alert.Name = "gift_alert";
            this.gift_alert.Size = new System.Drawing.Size(273, 42);
            this.gift_alert.TabIndex = 9;
            this.gift_alert.Text = "报警";
            this.gift_alert.UseVisualStyleBackColor = true;
            this.gift_alert.Click += new System.EventHandler(this.gift_alert_Click);
            // 
            // gift_finish2
            // 
            this.gift_finish2.Location = new System.Drawing.Point(156, 72);
            this.gift_finish2.Name = "gift_finish2";
            this.gift_finish2.Size = new System.Drawing.Size(128, 42);
            this.gift_finish2.TabIndex = 7;
            this.gift_finish2.Text = "结束游戏(不中)";
            this.gift_finish2.UseVisualStyleBackColor = true;
            this.gift_finish2.Click += new System.EventHandler(this.gift_finish2_Click);
            // 
            // gift_finish
            // 
            this.gift_finish.Location = new System.Drawing.Point(11, 72);
            this.gift_finish.Name = "gift_finish";
            this.gift_finish.Size = new System.Drawing.Size(128, 42);
            this.gift_finish.TabIndex = 6;
            this.gift_finish.Text = "结束游戏（中）";
            this.gift_finish.UseVisualStyleBackColor = true;
            this.gift_finish.Click += new System.EventHandler(this.gift_finish_Click);
            // 
            // gift_start
            // 
            this.gift_start.Location = new System.Drawing.Point(11, 24);
            this.gift_start.Name = "gift_start";
            this.gift_start.Size = new System.Drawing.Size(273, 42);
            this.gift_start.TabIndex = 5;
            this.gift_start.Text = "开始游戏";
            this.gift_start.UseVisualStyleBackColor = true;
            this.gift_start.Click += new System.EventHandler(this.gift_start_Click);
            // 
            // grp_sell
            // 
            this.grp_sell.Controls.Add(this.button1);
            this.grp_sell.Controls.Add(this.label3);
            this.grp_sell.Controls.Add(this.txt_boxnum);
            this.grp_sell.Controls.Add(this.button2);
            this.grp_sell.Controls.Add(this.button4);
            this.grp_sell.Enabled = false;
            this.grp_sell.Location = new System.Drawing.Point(17, 409);
            this.grp_sell.Name = "grp_sell";
            this.grp_sell.Size = new System.Drawing.Size(294, 135);
            this.grp_sell.TabIndex = 9;
            this.grp_sell.TabStop = false;
            this.grp_sell.Text = "售货机";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 42);
            this.button1.TabIndex = 12;
            this.button1.Text = "获取正扫二维码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "柜号";
            // 
            // txt_boxnum
            // 
            this.txt_boxnum.Location = new System.Drawing.Point(81, 35);
            this.txt_boxnum.Name = "txt_boxnum";
            this.txt_boxnum.Size = new System.Drawing.Size(58, 25);
            this.txt_boxnum.TabIndex = 10;
            this.txt_boxnum.Text = "1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(156, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 42);
            this.button2.TabIndex = 7;
            this.button2.Text = "取消购买";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(156, 24);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(128, 42);
            this.button4.TabIndex = 5;
            this.button4.Text = "发起购买";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(298, -1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 22);
            this.button3.TabIndex = 12;
            this.button3.Text = "清空";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 661);
            this.Controls.Add(this.grp_sell);
            this.Controls.Add(this.grp_gift);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CNGPI v1 调试助手_游戏售货机模拟器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grp_gift.ResumeLayout(false);
            this.grp_sell.ResumeLayout(false);
            this.grp_sell.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox base_comname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button base_bt_conn;
        private System.Windows.Forms.TextBox event_txt;
        private System.Windows.Forms.GroupBox grp_gift;
        private System.Windows.Forms.Button gift_start;
        private System.Windows.Forms.Button gift_finish2;
        private System.Windows.Forms.Button gift_finish;
        private System.Windows.Forms.Button gift_alert;
        private System.Windows.Forms.GroupBox grp_sell;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txt_boxnum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chk_showsync;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
    }
}

