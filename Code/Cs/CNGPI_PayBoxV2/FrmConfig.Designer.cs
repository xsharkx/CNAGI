namespace CNGPI_PayBox
{
    partial class FrmConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfig));
            this.pl_menulist = new System.Windows.Forms.FlowLayoutPanel();
            this.menuItem1 = new CNGPI_PayBox.MenuItem();
            this.menuItem2 = new CNGPI_PayBox.MenuItem();
            this.menuItem3 = new CNGPI_PayBox.MenuItem();
            this.bt_save = new System.Windows.Forms.Button();
            this.pl_menulist.SuspendLayout();
            this.SuspendLayout();
            // 
            // pl_menulist
            // 
            this.pl_menulist.AutoScroll = true;
            this.pl_menulist.Controls.Add(this.menuItem1);
            this.pl_menulist.Controls.Add(this.menuItem2);
            this.pl_menulist.Controls.Add(this.menuItem3);
            this.pl_menulist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pl_menulist.Location = new System.Drawing.Point(0, 0);
            this.pl_menulist.Name = "pl_menulist";
            this.pl_menulist.Size = new System.Drawing.Size(581, 792);
            this.pl_menulist.TabIndex = 0;
            // 
            // menuItem1
            // 
            this.menuItem1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.menuItem1.Location = new System.Drawing.Point(3, 3);
            this.menuItem1.Menu = null;
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(573, 39);
            this.menuItem1.TabIndex = 0;
            // 
            // menuItem2
            // 
            this.menuItem2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.menuItem2.Location = new System.Drawing.Point(3, 48);
            this.menuItem2.Menu = null;
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(573, 39);
            this.menuItem2.TabIndex = 1;
            // 
            // menuItem3
            // 
            this.menuItem3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.menuItem3.Location = new System.Drawing.Point(3, 93);
            this.menuItem3.Menu = null;
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Size = new System.Drawing.Size(573, 39);
            this.menuItem3.TabIndex = 2;
            // 
            // bt_save
            // 
            this.bt_save.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bt_save.Location = new System.Drawing.Point(0, 792);
            this.bt_save.Name = "bt_save";
            this.bt_save.Size = new System.Drawing.Size(581, 35);
            this.bt_save.TabIndex = 1;
            this.bt_save.Text = "保存";
            this.bt_save.UseVisualStyleBackColor = true;
            this.bt_save.Click += new System.EventHandler(this.bt_save_Click);
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 827);
            this.Controls.Add(this.pl_menulist);
            this.Controls.Add(this.bt_save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmConfig";
            this.Text = "菜单配置";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.pl_menulist.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pl_menulist;
        private System.Windows.Forms.Button bt_save;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
    }
}