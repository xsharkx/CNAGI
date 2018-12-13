namespace CNGPI_PayBox
{
    partial class MenuItem
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lab_name = new System.Windows.Forms.Label();
            this.txt_value = new System.Windows.Forms.TextBox();
            this.lab_fanwei = new System.Windows.Forms.Label();
            this.img_info = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.img_info)).BeginInit();
            this.SuspendLayout();
            // 
            // lab_name
            // 
            this.lab_name.Location = new System.Drawing.Point(3, 8);
            this.lab_name.Name = "lab_name";
            this.lab_name.Size = new System.Drawing.Size(281, 23);
            this.lab_name.TabIndex = 0;
            this.lab_name.Text = "我是菜单项名字";
            this.lab_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_value
            // 
            this.txt_value.Location = new System.Drawing.Point(290, 6);
            this.txt_value.Name = "txt_value";
            this.txt_value.Size = new System.Drawing.Size(128, 25);
            this.txt_value.TabIndex = 1;
            this.txt_value.Text = "我是值";
            // 
            // lab_fanwei
            // 
            this.lab_fanwei.Location = new System.Drawing.Point(424, 8);
            this.lab_fanwei.Name = "lab_fanwei";
            this.lab_fanwei.Size = new System.Drawing.Size(146, 23);
            this.lab_fanwei.TabIndex = 2;
            this.lab_fanwei.Text = "我是范围";
            this.lab_fanwei.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // img_info
            // 
            this.img_info.Image = global::CNGPI_PayBox_Niudan.Properties.Resources.ask_help_questionmark_support_24px_1400_easyicon_net;
            this.img_info.Location = new System.Drawing.Point(542, 8);
            this.img_info.Name = "img_info";
            this.img_info.Size = new System.Drawing.Size(24, 23);
            this.img_info.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_info.TabIndex = 4;
            this.img_info.TabStop = false;
            // 
            // MenuItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.img_info);
            this.Controls.Add(this.lab_fanwei);
            this.Controls.Add(this.txt_value);
            this.Controls.Add(this.lab_name);
            this.Name = "MenuItem";
            this.Size = new System.Drawing.Size(573, 37);
            this.Load += new System.EventHandler(this.MenuItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.img_info)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab_name;
        private System.Windows.Forms.TextBox txt_value;
        private System.Windows.Forms.Label lab_fanwei;
        private System.Windows.Forms.PictureBox img_info;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
