namespace Demo
{
    partial class Dashboard
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.bnInit = new System.Windows.Forms.Button();
            this.bnOpen = new System.Windows.Forms.Button();
            this.bnEnroll = new System.Windows.Forms.Button();
            this.bnFree = new System.Windows.Forms.Button();
            this.bnClose = new System.Windows.Forms.Button();
            this.bnTimeIn = new System.Windows.Forms.Button();
            this.textRes = new System.Windows.Forms.TextBox();
            this.picFPImg = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbIdx = new System.Windows.Forms.ComboBox();
            this.textBox_emp_id = new System.Windows.Forms.TextBox();
            this.empIdLabel = new System.Windows.Forms.Label();
            this.bnVerify = new System.Windows.Forms.Button();
            this.bnTimeOut = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.logo = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.date = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bnInit
            // 
            this.bnInit.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnInit.FlatAppearance.BorderSize = 0;
            this.bnInit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnInit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bnInit.ForeColor = System.Drawing.SystemColors.Menu;
            this.bnInit.Location = new System.Drawing.Point(0, 93);
            this.bnInit.Margin = new System.Windows.Forms.Padding(4);
            this.bnInit.Name = "bnInit";
            this.bnInit.Size = new System.Drawing.Size(194, 51);
            this.bnInit.TabIndex = 0;
            this.bnInit.Text = "Initialize";
            this.bnInit.UseVisualStyleBackColor = true;
            this.bnInit.Visible = false;
            this.bnInit.Click += new System.EventHandler(this.bnInit_Click);
            // 
            // bnOpen
            // 
            this.bnOpen.AutoSize = true;
            this.bnOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnOpen.Enabled = false;
            this.bnOpen.FlatAppearance.BorderSize = 0;
            this.bnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bnOpen.ForeColor = System.Drawing.SystemColors.Menu;
            this.bnOpen.Location = new System.Drawing.Point(0, 144);
            this.bnOpen.Margin = new System.Windows.Forms.Padding(4);
            this.bnOpen.Name = "bnOpen";
            this.bnOpen.Size = new System.Drawing.Size(194, 54);
            this.bnOpen.TabIndex = 1;
            this.bnOpen.Text = "Open";
            this.bnOpen.UseVisualStyleBackColor = true;
            this.bnOpen.Click += new System.EventHandler(this.bnOpen_Click);
            // 
            // bnEnroll
            // 
            this.bnEnroll.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnEnroll.Enabled = false;
            this.bnEnroll.FlatAppearance.BorderSize = 0;
            this.bnEnroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnEnroll.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bnEnroll.ForeColor = System.Drawing.SystemColors.Control;
            this.bnEnroll.Location = new System.Drawing.Point(0, 301);
            this.bnEnroll.Margin = new System.Windows.Forms.Padding(4);
            this.bnEnroll.Name = "bnEnroll";
            this.bnEnroll.Size = new System.Drawing.Size(194, 52);
            this.bnEnroll.TabIndex = 2;
            this.bnEnroll.Text = "Enroll";
            this.bnEnroll.UseVisualStyleBackColor = true;
            this.bnEnroll.Click += new System.EventHandler(this.bnEnroll_Click);
            // 
            // bnFree
            // 
            this.bnFree.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnFree.Enabled = false;
            this.bnFree.FlatAppearance.BorderSize = 0;
            this.bnFree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnFree.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bnFree.ForeColor = System.Drawing.SystemColors.Menu;
            this.bnFree.Location = new System.Drawing.Point(0, 406);
            this.bnFree.Margin = new System.Windows.Forms.Padding(4);
            this.bnFree.Name = "bnFree";
            this.bnFree.Size = new System.Drawing.Size(194, 46);
            this.bnFree.TabIndex = 4;
            this.bnFree.Text = "Finalize";
            this.bnFree.UseVisualStyleBackColor = true;
            this.bnFree.Visible = false;
            this.bnFree.Click += new System.EventHandler(this.bnFree_Click);
            // 
            // bnClose
            // 
            this.bnClose.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnClose.Enabled = false;
            this.bnClose.FlatAppearance.BorderSize = 0;
            this.bnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bnClose.ForeColor = System.Drawing.SystemColors.Menu;
            this.bnClose.Location = new System.Drawing.Point(0, 353);
            this.bnClose.Margin = new System.Windows.Forms.Padding(4);
            this.bnClose.Name = "bnClose";
            this.bnClose.Size = new System.Drawing.Size(194, 53);
            this.bnClose.TabIndex = 5;
            this.bnClose.Text = "Close";
            this.bnClose.UseVisualStyleBackColor = true;
            this.bnClose.Click += new System.EventHandler(this.bnClose_Click);
            // 
            // bnTimeIn
            // 
            this.bnTimeIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnTimeIn.Enabled = false;
            this.bnTimeIn.FlatAppearance.BorderSize = 0;
            this.bnTimeIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnTimeIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bnTimeIn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.bnTimeIn.Location = new System.Drawing.Point(0, 198);
            this.bnTimeIn.Margin = new System.Windows.Forms.Padding(4);
            this.bnTimeIn.Name = "bnTimeIn";
            this.bnTimeIn.Size = new System.Drawing.Size(194, 49);
            this.bnTimeIn.TabIndex = 6;
            this.bnTimeIn.Text = "Time-in";
            this.bnTimeIn.UseVisualStyleBackColor = true;
            this.bnTimeIn.Click += new System.EventHandler(this.bnIdentify_Click);
            // 
            // textRes
            // 
            this.textRes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(29)))), ((int)(((byte)(46)))));
            this.textRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textRes.ForeColor = System.Drawing.SystemColors.Menu;
            this.textRes.Location = new System.Drawing.Point(210, 399);
            this.textRes.Margin = new System.Windows.Forms.Padding(4);
            this.textRes.Multiline = true;
            this.textRes.Name = "textRes";
            this.textRes.ReadOnly = true;
            this.textRes.Size = new System.Drawing.Size(752, 92);
            this.textRes.TabIndex = 7;
            this.textRes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textRes.TextChanged += new System.EventHandler(this.textRes_TextChanged);
            // 
            // picFPImg
            // 
            this.picFPImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(56)))), ((int)(((byte)(89)))));
            this.picFPImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picFPImg.Location = new System.Drawing.Point(693, 23);
            this.picFPImg.Margin = new System.Windows.Forms.Padding(4);
            this.picFPImg.Name = "picFPImg";
            this.picFPImg.Size = new System.Drawing.Size(250, 336);
            this.picFPImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFPImg.TabIndex = 8;
            this.picFPImg.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(831, 371);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Index:";
            this.label1.Visible = false;
            // 
            // cmbIdx
            // 
            this.cmbIdx.FormattingEnabled = true;
            this.cmbIdx.Location = new System.Drawing.Point(878, 368);
            this.cmbIdx.Margin = new System.Windows.Forms.Padding(4);
            this.cmbIdx.Name = "cmbIdx";
            this.cmbIdx.Size = new System.Drawing.Size(46, 23);
            this.cmbIdx.TabIndex = 10;
            this.cmbIdx.Visible = false;
            // 
            // textBox_emp_id
            // 
            this.textBox_emp_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_emp_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_emp_id.BackColor = System.Drawing.Color.White;
            this.textBox_emp_id.ForeColor = System.Drawing.SystemColors.InfoText;
            this.textBox_emp_id.Location = new System.Drawing.Point(368, 225);
            this.textBox_emp_id.Name = "textBox_emp_id";
            this.textBox_emp_id.Size = new System.Drawing.Size(133, 23);
            this.textBox_emp_id.TabIndex = 11;
            this.textBox_emp_id.Visible = false;
            this.textBox_emp_id.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // empIdLabel
            // 
            this.empIdLabel.AutoSize = true;
            this.empIdLabel.BackColor = System.Drawing.Color.Transparent;
            this.empIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.empIdLabel.ForeColor = System.Drawing.Color.White;
            this.empIdLabel.Location = new System.Drawing.Point(368, 186);
            this.empIdLabel.Name = "empIdLabel";
            this.empIdLabel.Size = new System.Drawing.Size(133, 25);
            this.empIdLabel.TabIndex = 12;
            this.empIdLabel.Text = "Employee ID";
            this.empIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.empIdLabel.Visible = false;
            // 
            // bnVerify
            // 
            this.bnVerify.Enabled = false;
            this.bnVerify.Location = new System.Drawing.Point(824, 34);
            this.bnVerify.Margin = new System.Windows.Forms.Padding(4);
            this.bnVerify.Name = "bnVerify";
            this.bnVerify.Size = new System.Drawing.Size(88, 29);
            this.bnVerify.TabIndex = 3;
            this.bnVerify.Text = "Verify";
            this.bnVerify.UseVisualStyleBackColor = true;
            this.bnVerify.Visible = false;
            this.bnVerify.Click += new System.EventHandler(this.bnVerify_Click);
            // 
            // bnTimeOut
            // 
            this.bnTimeOut.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnTimeOut.Enabled = false;
            this.bnTimeOut.FlatAppearance.BorderSize = 0;
            this.bnTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnTimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bnTimeOut.ForeColor = System.Drawing.SystemColors.Menu;
            this.bnTimeOut.Location = new System.Drawing.Point(0, 247);
            this.bnTimeOut.Margin = new System.Windows.Forms.Padding(4);
            this.bnTimeOut.Name = "bnTimeOut";
            this.bnTimeOut.Size = new System.Drawing.Size(194, 54);
            this.bnTimeOut.TabIndex = 13;
            this.bnTimeOut.Text = "Time-out";
            this.bnTimeOut.UseVisualStyleBackColor = true;
            this.bnTimeOut.Click += new System.EventHandler(this.bnTimeOut_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(29)))), ((int)(((byte)(46)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.bnFree);
            this.panel1.Controls.Add(this.bnClose);
            this.panel1.Controls.Add(this.bnEnroll);
            this.panel1.Controls.Add(this.bnTimeOut);
            this.panel1.Controls.Add(this.bnTimeIn);
            this.panel1.Controls.Add(this.bnOpen);
            this.panel1.Controls.Add(this.bnInit);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(196, 498);
            this.panel1.TabIndex = 14;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel2.Controls.Add(this.logo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 93);
            this.panel2.TabIndex = 14;
            // 
            // logo
            // 
            this.logo.BackColor = System.Drawing.Color.Transparent;
            this.logo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.logo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.logo.Image = ((System.Drawing.Image)(resources.GetObject("logo.Image")));
            this.logo.Location = new System.Drawing.Point(12, -7);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(168, 111);
            this.logo.TabIndex = 0;
            this.logo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.BackColor = System.Drawing.Color.Transparent;
            this.time.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.time.ForeColor = System.Drawing.SystemColors.Control;
            this.time.Location = new System.Drawing.Point(273, 69);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(59, 25);
            this.time.TabIndex = 15;
            this.time.Text = "Time";
            this.time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.time.Click += new System.EventHandler(this.Time_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // date
            // 
            this.date.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.date.AutoSize = true;
            this.date.BackColor = System.Drawing.Color.Transparent;
            this.date.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.date.ForeColor = System.Drawing.SystemColors.Control;
            this.date.Location = new System.Drawing.Point(273, 107);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(57, 25);
            this.date.TabIndex = 16;
            this.date.Text = "Date";
            this.date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(29)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(975, 498);
            this.Controls.Add(this.date);
            this.Controls.Add(this.time);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.empIdLabel);
            this.Controls.Add(this.textBox_emp_id);
            this.Controls.Add(this.cmbIdx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picFPImg);
            this.Controls.Add(this.textRes);
            this.Controls.Add(this.bnVerify);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnInit;
        private System.Windows.Forms.Button bnOpen;
        private System.Windows.Forms.Button bnEnroll;
        private System.Windows.Forms.Button bnFree;
        private System.Windows.Forms.Button bnClose;
        private System.Windows.Forms.Button bnTimeIn;
        private System.Windows.Forms.TextBox textRes;
        private System.Windows.Forms.PictureBox picFPImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbIdx;
        private System.Windows.Forms.TextBox textBox_emp_id;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bnVerify;
        private System.Windows.Forms.Button bnTimeOut;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label logo;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Label empIdLabel;
    }
}

