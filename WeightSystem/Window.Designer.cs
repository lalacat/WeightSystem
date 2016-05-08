using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using LxControl;

namespace serial
{
    partial class MainWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

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
            this.serMCU = new System.IO.Ports.SerialPort(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.openSerial = new System.Windows.Forms.Button();
            this.OpenInternt = new System.Windows.Forms.Button();
            this.measure = new System.Windows.Forms.Button();
            this.makeZero = new System.Windows.Forms.Button();
            this.funInfo = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.emptyButton = new System.Windows.Forms.Button();
            this.measureEmpty = new System.Windows.Forms.TextBox();
            this.standardEmpty = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fullButton = new System.Windows.Forms.Button();
            this.measureFull = new System.Windows.Forms.TextBox();
            this.label = new System.Windows.Forms.Label();
            this.standardFull = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.saveData = new System.Windows.Forms.Button();
            this.revText = new LxControl.LxLedControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.weightNo8_Button = new System.Windows.Forms.RadioButton();
            this.weightNo2_Button = new System.Windows.Forms.RadioButton();
            this.weightNo16_Button = new System.Windows.Forms.RadioButton();
            this.weightNo15_Button = new System.Windows.Forms.RadioButton();
            this.weightNo14_Button = new System.Windows.Forms.RadioButton();
            this.weightNo12_Button = new System.Windows.Forms.RadioButton();
            this.weightNo13_Button = new System.Windows.Forms.RadioButton();
            this.weightNo11_Button = new System.Windows.Forms.RadioButton();
            this.weightNo9_Button = new System.Windows.Forms.RadioButton();
            this.weightNo10_Button = new System.Windows.Forms.RadioButton();
            this.weightNo7_Button = new System.Windows.Forms.RadioButton();
            this.weightNo6_Button = new System.Windows.Forms.RadioButton();
            this.weightNo5_Button = new System.Windows.Forms.RadioButton();
            this.weightNo3_Button = new System.Windows.Forms.RadioButton();
            this.weightNo4_Button = new System.Windows.Forms.RadioButton();
            this.weightNo1_Button = new System.Windows.Forms.RadioButton();
            this.groub = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbSerial = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.PortAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.revText)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groub.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // serMCU
            // 
            this.serMCU.PortName = "COM5";
            this.serMCU.WriteBufferSize = 1024;
            this.serMCU.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RevSbuf);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 0);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.openSerial);
            this.panel3.Controls.Add(this.OpenInternt);
            this.panel3.Controls.Add(this.measure);
            this.panel3.Controls.Add(this.makeZero);
            this.panel3.Location = new System.Drawing.Point(3, 450);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(321, 29);
            this.panel3.TabIndex = 9;
            // 
            // openSerial
            // 
            this.openSerial.Location = new System.Drawing.Point(4, 3);
            this.openSerial.Name = "openSerial";
            this.openSerial.Size = new System.Drawing.Size(75, 23);
            this.openSerial.TabIndex = 7;
            this.openSerial.Text = "打开通信";
            this.openSerial.Click += new System.EventHandler(this.openSerial_Click);
            // 
            // OpenInternt
            // 
            this.OpenInternt.Location = new System.Drawing.Point(243, 3);
            this.OpenInternt.Name = "OpenInternt";
            this.OpenInternt.Size = new System.Drawing.Size(75, 23);
            this.OpenInternt.TabIndex = 1;
            this.OpenInternt.Text = "网络传输";
            this.OpenInternt.Click += new System.EventHandler(this.OpenInternt_Click);
            // 
            // measure
            // 
            this.measure.Location = new System.Drawing.Point(83, 3);
            this.measure.Name = "measure";
            this.measure.Size = new System.Drawing.Size(75, 23);
            this.measure.TabIndex = 8;
            this.measure.Text = "测量";
            this.measure.UseVisualStyleBackColor = true;
            this.measure.Click += new System.EventHandler(this.measure_Click);
            // 
            // makeZero
            // 
            this.makeZero.Location = new System.Drawing.Point(163, 3);
            this.makeZero.Name = "makeZero";
            this.makeZero.Size = new System.Drawing.Size(75, 23);
            this.makeZero.TabIndex = 0;
            this.makeZero.Text = "去皮";
            this.makeZero.Click += new System.EventHandler(this.makeZero_Click);
            // 
            // funInfo
            // 
            this.funInfo.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.funInfo.Location = new System.Drawing.Point(131, 178);
            this.funInfo.Multiline = false;
            this.funInfo.Name = "funInfo";
            this.funInfo.ReadOnly = true;
            this.funInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.funInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.funInfo.Size = new System.Drawing.Size(262, 39);
            this.funInfo.TabIndex = 10;
            this.funInfo.Text = "";
            this.funInfo.WordWrap = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.saveData);
            this.groupBox1.Location = new System.Drawing.Point(131, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 220);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标定";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.emptyButton);
            this.groupBox2.Controls.Add(this.measureEmpty);
            this.groupBox2.Controls.Add(this.standardEmpty);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(13, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(114, 162);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "空秤时";
            // 
            // emptyButton
            // 
            this.emptyButton.Location = new System.Drawing.Point(21, 119);
            this.emptyButton.Name = "emptyButton";
            this.emptyButton.Size = new System.Drawing.Size(75, 23);
            this.emptyButton.TabIndex = 4;
            this.emptyButton.Text = "测量";
            this.emptyButton.UseVisualStyleBackColor = true;
            this.emptyButton.Click += new System.EventHandler(this.emptyButton_Click);
            // 
            // measureEmpty
            // 
            this.measureEmpty.Font = new System.Drawing.Font("宋体", 11F);
            this.measureEmpty.Location = new System.Drawing.Point(8, 89);
            this.measureEmpty.Name = "measureEmpty";
            this.measureEmpty.ReadOnly = true;
            this.measureEmpty.Size = new System.Drawing.Size(100, 24);
            this.measureEmpty.TabIndex = 3;
            this.measureEmpty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // standardEmpty
            // 
            this.standardEmpty.Font = new System.Drawing.Font("宋体", 11F);
            this.standardEmpty.Location = new System.Drawing.Point(8, 38);
            this.standardEmpty.Name = "standardEmpty";
            this.standardEmpty.Size = new System.Drawing.Size(100, 24);
            this.standardEmpty.TabIndex = 2;
            this.standardEmpty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.standardEmpty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.standardEmpey_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Font = new System.Drawing.Font("宋体", 11F);
            this.label2.Location = new System.Drawing.Point(31, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "测量值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Font = new System.Drawing.Font("宋体", 11F);
            this.label3.Location = new System.Drawing.Point(31, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "标定值";
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.fullButton);
            this.groupBox3.Controls.Add(this.measureFull);
            this.groupBox3.Controls.Add(this.label);
            this.groupBox3.Controls.Add(this.standardFull);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(135, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(110, 162);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "满量程时";
            // 
            // fullButton
            // 
            this.fullButton.Location = new System.Drawing.Point(17, 119);
            this.fullButton.Name = "fullButton";
            this.fullButton.Size = new System.Drawing.Size(75, 23);
            this.fullButton.TabIndex = 8;
            this.fullButton.Text = "测量";
            this.fullButton.UseVisualStyleBackColor = true;
            this.fullButton.Click += new System.EventHandler(this.fullButton_Click);
            // 
            // measureFull
            // 
            this.measureFull.Font = new System.Drawing.Font("宋体", 11F);
            this.measureFull.Location = new System.Drawing.Point(4, 89);
            this.measureFull.Name = "measureFull";
            this.measureFull.ReadOnly = true;
            this.measureFull.Size = new System.Drawing.Size(100, 24);
            this.measureFull.TabIndex = 7;
            this.measureFull.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label.Font = new System.Drawing.Font("宋体", 11F);
            this.label.Location = new System.Drawing.Point(27, 17);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(52, 15);
            this.label.TabIndex = 4;
            this.label.Text = "标定值";
            // 
            // standardFull
            // 
            this.standardFull.Font = new System.Drawing.Font("宋体", 11F);
            this.standardFull.ForeColor = System.Drawing.Color.Black;
            this.standardFull.Location = new System.Drawing.Point(4, 38);
            this.standardFull.Name = "standardFull";
            this.standardFull.Size = new System.Drawing.Size(100, 24);
            this.standardFull.TabIndex = 6;
            this.standardFull.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.standardFull.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.standardFull_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Font = new System.Drawing.Font("宋体", 11F);
            this.label4.Location = new System.Drawing.Point(27, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "测量值";
            // 
            // saveData
            // 
            this.saveData.AutoSize = true;
            this.saveData.Location = new System.Drawing.Point(90, 188);
            this.saveData.Name = "saveData";
            this.saveData.Size = new System.Drawing.Size(75, 23);
            this.saveData.TabIndex = 13;
            this.saveData.Text = "保存";
            this.saveData.UseMnemonic = false;
            this.saveData.UseVisualStyleBackColor = true;
            this.saveData.Click += new System.EventHandler(this.saveData_Click);
            // 
            // revText
            // 
            this.revText.BackColor = System.Drawing.Color.Transparent;
            this.revText.BackColor_1 = System.Drawing.Color.Black;
            this.revText.BackColor_2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.revText.BevelRate = 0.5F;
            this.revText.FadedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.revText.ForeColor = System.Drawing.Color.Lime;
            this.revText.HighlightOpaque = ((byte)(50));
            this.revText.Location = new System.Drawing.Point(0, 3);
            this.revText.Name = "revText";
            this.revText.Size = new System.Drawing.Size(393, 125);
            this.revText.TabIndex = 15;
            this.revText.Text = "00.00";
            this.revText.TextAlignment = LxControl.LxLedControl.Alignment.Right;
            this.revText.TotalCharCount = 6;
            this.revText.UseItalicStyle = true;
            this.revText.UseSmoothingMode = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.weightNo8_Button);
            this.groupBox4.Controls.Add(this.weightNo2_Button);
            this.groupBox4.Controls.Add(this.weightNo16_Button);
            this.groupBox4.Controls.Add(this.weightNo15_Button);
            this.groupBox4.Controls.Add(this.weightNo14_Button);
            this.groupBox4.Controls.Add(this.weightNo12_Button);
            this.groupBox4.Controls.Add(this.weightNo13_Button);
            this.groupBox4.Controls.Add(this.weightNo11_Button);
            this.groupBox4.Controls.Add(this.weightNo9_Button);
            this.groupBox4.Controls.Add(this.weightNo10_Button);
            this.groupBox4.Controls.Add(this.weightNo7_Button);
            this.groupBox4.Controls.Add(this.weightNo6_Button);
            this.groupBox4.Controls.Add(this.weightNo5_Button);
            this.groupBox4.Controls.Add(this.weightNo3_Button);
            this.groupBox4.Controls.Add(this.weightNo4_Button);
            this.groupBox4.Controls.Add(this.weightNo1_Button);
            this.groupBox4.Location = new System.Drawing.Point(3, 178);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(125, 265);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "称选择";
            // 
            // weightNo8_Button
            // 
            this.weightNo8_Button.AutoSize = true;
            this.weightNo8_Button.Location = new System.Drawing.Point(4, 244);
            this.weightNo8_Button.Name = "weightNo8_Button";
            this.weightNo8_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo8_Button.TabIndex = 32;
            this.weightNo8_Button.TabStop = true;
            this.weightNo8_Button.Text = "8号秤";
            this.weightNo8_Button.UseVisualStyleBackColor = true;
            this.weightNo8_Button.Click += new System.EventHandler(this.weightNo8_Click);
            // 
            // weightNo2_Button
            // 
            this.weightNo2_Button.AutoSize = true;
            this.weightNo2_Button.Location = new System.Drawing.Point(4, 52);
            this.weightNo2_Button.Name = "weightNo2_Button";
            this.weightNo2_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo2_Button.TabIndex = 31;
            this.weightNo2_Button.TabStop = true;
            this.weightNo2_Button.Text = "2号秤";
            this.weightNo2_Button.UseVisualStyleBackColor = true;
            this.weightNo2_Button.Click += new System.EventHandler(this.weightNo2_Click);
            // 
            // weightNo16_Button
            // 
            this.weightNo16_Button.AutoSize = true;
            this.weightNo16_Button.Location = new System.Drawing.Point(63, 244);
            this.weightNo16_Button.Name = "weightNo16_Button";
            this.weightNo16_Button.Size = new System.Drawing.Size(59, 16);
            this.weightNo16_Button.TabIndex = 29;
            this.weightNo16_Button.TabStop = true;
            this.weightNo16_Button.Text = "16号秤";
            this.weightNo16_Button.UseVisualStyleBackColor = true;
            this.weightNo16_Button.Click += new System.EventHandler(this.weightNo16_Click);
            // 
            // weightNo15_Button
            // 
            this.weightNo15_Button.AutoSize = true;
            this.weightNo15_Button.Location = new System.Drawing.Point(63, 212);
            this.weightNo15_Button.Name = "weightNo15_Button";
            this.weightNo15_Button.Size = new System.Drawing.Size(59, 16);
            this.weightNo15_Button.TabIndex = 28;
            this.weightNo15_Button.TabStop = true;
            this.weightNo15_Button.Text = "15号秤";
            this.weightNo15_Button.UseVisualStyleBackColor = true;
            this.weightNo15_Button.Click += new System.EventHandler(this.weightNo15_Click);
            // 
            // weightNo14_Button
            // 
            this.weightNo14_Button.AutoSize = true;
            this.weightNo14_Button.Location = new System.Drawing.Point(63, 180);
            this.weightNo14_Button.Name = "weightNo14_Button";
            this.weightNo14_Button.Size = new System.Drawing.Size(59, 16);
            this.weightNo14_Button.TabIndex = 27;
            this.weightNo14_Button.TabStop = true;
            this.weightNo14_Button.Text = "14号秤";
            this.weightNo14_Button.UseVisualStyleBackColor = true;
            this.weightNo14_Button.Click += new System.EventHandler(this.weightNo14_Click);
            // 
            // weightNo12_Button
            // 
            this.weightNo12_Button.AutoSize = true;
            this.weightNo12_Button.Location = new System.Drawing.Point(63, 116);
            this.weightNo12_Button.Name = "weightNo12_Button";
            this.weightNo12_Button.Size = new System.Drawing.Size(59, 16);
            this.weightNo12_Button.TabIndex = 26;
            this.weightNo12_Button.TabStop = true;
            this.weightNo12_Button.Text = "12号秤";
            this.weightNo12_Button.UseVisualStyleBackColor = true;
            this.weightNo12_Button.Click += new System.EventHandler(this.weightNo12_Click);
            // 
            // weightNo13_Button
            // 
            this.weightNo13_Button.AutoSize = true;
            this.weightNo13_Button.Location = new System.Drawing.Point(63, 148);
            this.weightNo13_Button.Name = "weightNo13_Button";
            this.weightNo13_Button.Size = new System.Drawing.Size(59, 16);
            this.weightNo13_Button.TabIndex = 25;
            this.weightNo13_Button.TabStop = true;
            this.weightNo13_Button.Text = "13号秤";
            this.weightNo13_Button.UseVisualStyleBackColor = true;
            this.weightNo13_Button.Click += new System.EventHandler(this.weightNo13_Click);
            // 
            // weightNo11_Button
            // 
            this.weightNo11_Button.AutoSize = true;
            this.weightNo11_Button.Location = new System.Drawing.Point(63, 84);
            this.weightNo11_Button.Name = "weightNo11_Button";
            this.weightNo11_Button.Size = new System.Drawing.Size(59, 16);
            this.weightNo11_Button.TabIndex = 24;
            this.weightNo11_Button.TabStop = true;
            this.weightNo11_Button.Text = "11号秤";
            this.weightNo11_Button.UseVisualStyleBackColor = true;
            this.weightNo11_Button.Click += new System.EventHandler(this.weightNo11_Click);
            // 
            // weightNo9_Button
            // 
            this.weightNo9_Button.AutoSize = true;
            this.weightNo9_Button.Location = new System.Drawing.Point(63, 20);
            this.weightNo9_Button.Name = "weightNo9_Button";
            this.weightNo9_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo9_Button.TabIndex = 23;
            this.weightNo9_Button.TabStop = true;
            this.weightNo9_Button.Text = "9号秤";
            this.weightNo9_Button.UseVisualStyleBackColor = true;
            this.weightNo9_Button.Click += new System.EventHandler(this.weightNo9_Click);
            // 
            // weightNo10_Button
            // 
            this.weightNo10_Button.AutoSize = true;
            this.weightNo10_Button.Location = new System.Drawing.Point(63, 52);
            this.weightNo10_Button.Name = "weightNo10_Button";
            this.weightNo10_Button.Size = new System.Drawing.Size(59, 16);
            this.weightNo10_Button.TabIndex = 22;
            this.weightNo10_Button.TabStop = true;
            this.weightNo10_Button.Text = "10号秤";
            this.weightNo10_Button.UseVisualStyleBackColor = true;
            this.weightNo10_Button.Click += new System.EventHandler(this.weightNo10_Click);
            // 
            // weightNo7_Button
            // 
            this.weightNo7_Button.AutoSize = true;
            this.weightNo7_Button.Location = new System.Drawing.Point(4, 212);
            this.weightNo7_Button.Name = "weightNo7_Button";
            this.weightNo7_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo7_Button.TabIndex = 21;
            this.weightNo7_Button.TabStop = true;
            this.weightNo7_Button.Text = "7号秤";
            this.weightNo7_Button.UseVisualStyleBackColor = true;
            this.weightNo7_Button.Click += new System.EventHandler(this.weightNo7_Click);
            // 
            // weightNo6_Button
            // 
            this.weightNo6_Button.AutoSize = true;
            this.weightNo6_Button.Location = new System.Drawing.Point(4, 180);
            this.weightNo6_Button.Name = "weightNo6_Button";
            this.weightNo6_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo6_Button.TabIndex = 20;
            this.weightNo6_Button.TabStop = true;
            this.weightNo6_Button.Text = "6号秤";
            this.weightNo6_Button.UseVisualStyleBackColor = true;
            this.weightNo6_Button.Click += new System.EventHandler(this.weightNo6_Click);
            // 
            // weightNo5_Button
            // 
            this.weightNo5_Button.AutoSize = true;
            this.weightNo5_Button.Location = new System.Drawing.Point(4, 148);
            this.weightNo5_Button.Name = "weightNo5_Button";
            this.weightNo5_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo5_Button.TabIndex = 19;
            this.weightNo5_Button.TabStop = true;
            this.weightNo5_Button.Text = "5号秤";
            this.weightNo5_Button.UseVisualStyleBackColor = true;
            this.weightNo5_Button.Click += new System.EventHandler(this.weightNo5_Click);
            // 
            // weightNo3_Button
            // 
            this.weightNo3_Button.AutoSize = true;
            this.weightNo3_Button.Location = new System.Drawing.Point(4, 84);
            this.weightNo3_Button.Name = "weightNo3_Button";
            this.weightNo3_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo3_Button.TabIndex = 18;
            this.weightNo3_Button.TabStop = true;
            this.weightNo3_Button.Text = "3号秤";
            this.weightNo3_Button.UseVisualStyleBackColor = true;
            this.weightNo3_Button.Click += new System.EventHandler(this.weightNo3_Click);
            // 
            // weightNo4_Button
            // 
            this.weightNo4_Button.AutoSize = true;
            this.weightNo4_Button.Location = new System.Drawing.Point(4, 116);
            this.weightNo4_Button.Name = "weightNo4_Button";
            this.weightNo4_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo4_Button.TabIndex = 17;
            this.weightNo4_Button.TabStop = true;
            this.weightNo4_Button.Text = "4号秤";
            this.weightNo4_Button.UseVisualStyleBackColor = true;
            this.weightNo4_Button.Click += new System.EventHandler(this.weightNo4_Click);
            // 
            // weightNo1_Button
            // 
            this.weightNo1_Button.AutoSize = true;
            this.weightNo1_Button.Location = new System.Drawing.Point(4, 20);
            this.weightNo1_Button.Name = "weightNo1_Button";
            this.weightNo1_Button.Size = new System.Drawing.Size(53, 16);
            this.weightNo1_Button.TabIndex = 16;
            this.weightNo1_Button.TabStop = true;
            this.weightNo1_Button.Text = "1号秤";
            this.weightNo1_Button.UseVisualStyleBackColor = true;
            this.weightNo1_Button.Click += new System.EventHandler(this.weightNo1_Click);
            // 
            // groub
            // 
            this.groub.Controls.Add(this.label5);
            this.groub.Controls.Add(this.cbSerial);
            this.groub.Controls.Add(this.label6);
            this.groub.Controls.Add(this.PortAddress);
            this.groub.Controls.Add(this.label1);
            this.groub.Location = new System.Drawing.Point(3, 134);
            this.groub.Name = "groub";
            this.groub.Size = new System.Drawing.Size(391, 44);
            this.groub.TabIndex = 18;
            this.groub.TabStop = false;
            this.groub.Text = "通信设定";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(231, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 21;
            // 
            // cbSerial
            // 
            this.cbSerial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerial.DropDownWidth = 62;
            this.cbSerial.FormattingEnabled = true;
            this.cbSerial.Location = new System.Drawing.Point(50, 17);
            this.cbSerial.Name = "cbSerial";
            this.cbSerial.Size = new System.Drawing.Size(66, 20);
            this.cbSerial.TabIndex = 20;
            this.cbSerial.TextChanged += new System.EventHandler(this.cbSerial_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10F);
            this.label6.Location = new System.Drawing.Point(125, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 14);
            this.label6.TabIndex = 4;
            this.label6.Text = "端口：";
            // 
            // PortAddress
            // 
            this.PortAddress.Location = new System.Drawing.Point(175, 17);
            this.PortAddress.Name = "PortAddress";
            this.PortAddress.Size = new System.Drawing.Size(33, 21);
            this.PortAddress.TabIndex = 1;
            this.PortAddress.Text = "8122";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "串口：";
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.groub);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.revText);
            this.panel2.Controls.Add(this.funInfo);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(394, 483);
            this.panel2.TabIndex = 3;
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(394, 483);
            this.Controls.Add(this.panel2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(410, 521);
            this.MinimumSize = new System.Drawing.Size(410, 521);
            this.Name = "MainWindow";
            this.Text = "称重系统";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.revText)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groub.ResumeLayout(false);
            this.groub.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SerialPort serMCU;
        private Panel panel1;
        private Panel panel3;
        private Button OpenInternt;
        private Button measure;
        private Button makeZero;
        private RichTextBox funInfo;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button emptyButton;
        private TextBox measureEmpty;
        private TextBox standardEmpty;
        private Label label2;
        private Label label3;
        private GroupBox groupBox3;
        private Button fullButton;
        private TextBox measureFull;
        private Label label;
        private TextBox standardFull;
        private Label label4;
        private Button saveData;
        private LxLedControl revText;
        private GroupBox groupBox4;
        private RadioButton weightNo8_Button;
        private RadioButton weightNo2_Button;
        private RadioButton weightNo16_Button;
        private RadioButton weightNo15_Button;
        private RadioButton weightNo14_Button;
        private RadioButton weightNo12_Button;
        private RadioButton weightNo13_Button;
        private RadioButton weightNo11_Button;
        private RadioButton weightNo9_Button;
        private RadioButton weightNo10_Button;
        private RadioButton weightNo7_Button;
        private RadioButton weightNo6_Button;
        private RadioButton weightNo5_Button;
        private RadioButton weightNo3_Button;
        private RadioButton weightNo4_Button;
        private RadioButton weightNo1_Button;
        private GroupBox groub;
        private Label label6;
        private Label label1;
        private TextBox PortAddress;
        private Panel panel2;
        private ComboBox cbSerial;
        private Button openSerial;
        private Label label5;

    }
}

