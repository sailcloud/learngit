namespace C0710_CharRoom_Client
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.txtShow = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHeartbeat = new System.Windows.Forms.Button();
            this.txtSNNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKickstand = new System.Windows.Forms.TextBox();
            this.txtTotolKM = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnCharger = new System.Windows.Forms.Button();
            this.txtCoordinates = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LatMinute = new System.Windows.Forms.Label();
            this.LonMinute = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCoordinates = new System.Windows.Forms.Button();
            this.txtLatMinute = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtlonMinute = new System.Windows.Forms.TextBox();
            this.txtCellId = new System.Windows.Forms.TextBox();
            this.txtLonDeg = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLatDeg = new System.Windows.Forms.TextBox();
            this.MqttAlarmTextBox = new System.Windows.Forms.TextBox();
            this.txtBattery = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAlarmType = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGPS = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.btnMqttAlarm = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.snend = new System.Windows.Forms.TextBox();
            this.mqttTextBox2 = new System.Windows.Forms.TextBox();
            this.mqttPort2 = new System.Windows.Forms.TextBox();
            this.snbegin = new System.Windows.Forms.TextBox();
            this.mqttPort1 = new System.Windows.Forms.TextBox();
            this.mqttTextBox1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.mqttbtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnAlarmType = new System.Windows.Forms.Button();
            this.btnFive = new System.Windows.Forms.Button();
            this.txtBTID = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnTw = new System.Windows.Forms.Button();
            this.txtAuthkeys = new System.Windows.Forms.TextBox();
            this.cmbIPAddress = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.positionSetBtn = new System.Windows.Forms.Button();
            this.taskStatLabel = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtShow2 = new System.Windows.Forms.TextBox();
            this.txtShow3 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(16, 45);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(86, 23);
            this.btnSend.TabIndex = 42;
            this.btnSend.Text = "发送连接请求";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtShow
            // 
            this.txtShow.Location = new System.Drawing.Point(1, 66);
            this.txtShow.Multiline = true;
            this.txtShow.Name = "txtShow";
            this.txtShow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtShow.Size = new System.Drawing.Size(565, 359);
            this.txtShow.TabIndex = 40;
            // 
            // btnConnect
            // 
            this.btnConnect.Enabled = false;
            this.btnConnect.Location = new System.Drawing.Point(720, 74);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 39;
            this.btnConnect.Text = "连接服务端";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(497, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 37;
            this.label2.Text = "Port：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 35;
            this.label1.Text = "IP：";
            // 
            // btnHeartbeat
            // 
            this.btnHeartbeat.Enabled = false;
            this.btnHeartbeat.Location = new System.Drawing.Point(108, 45);
            this.btnHeartbeat.Name = "btnHeartbeat";
            this.btnHeartbeat.Size = new System.Drawing.Size(75, 23);
            this.btnHeartbeat.TabIndex = 46;
            this.btnHeartbeat.Text = "发送心跳";
            this.btnHeartbeat.UseVisualStyleBackColor = true;
            this.btnHeartbeat.Click += new System.EventHandler(this.btnHeartbeat_Click);
            // 
            // txtSNNo
            // 
            this.txtSNNo.Location = new System.Drawing.Point(42, 19);
            this.txtSNNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtSNNo.Name = "txtSNNo";
            this.txtSNNo.Size = new System.Drawing.Size(156, 21);
            this.txtSNNo.TabIndex = 51;
            this.txtSNNo.Text = "mimacx0000000002";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 52;
            this.label3.Text = "SN:";
            // 
            // txtKickstand
            // 
            this.txtKickstand.Location = new System.Drawing.Point(442, 18);
            this.txtKickstand.Name = "txtKickstand";
            this.txtKickstand.Size = new System.Drawing.Size(37, 21);
            this.txtKickstand.TabIndex = 53;
            this.txtKickstand.Text = "1";
            // 
            // txtTotolKM
            // 
            this.txtTotolKM.Location = new System.Drawing.Point(482, 18);
            this.txtTotolKM.Name = "txtTotolKM";
            this.txtTotolKM.Size = new System.Drawing.Size(66, 21);
            this.txtTotolKM.TabIndex = 54;
            this.txtTotolKM.Text = "21";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(588, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 56;
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(812, 74);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(62, 23);
            this.btnClean.TabIndex = 58;
            this.btnClean.Text = "清空";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnCharger
            // 
            this.btnCharger.Enabled = false;
            this.btnCharger.Location = new System.Drawing.Point(189, 45);
            this.btnCharger.Name = "btnCharger";
            this.btnCharger.Size = new System.Drawing.Size(75, 23);
            this.btnCharger.TabIndex = 59;
            this.btnCharger.Text = "插入充电";
            this.btnCharger.UseVisualStyleBackColor = true;
            this.btnCharger.Click += new System.EventHandler(this.btnCharger_Click);
            // 
            // txtCoordinates
            // 
            this.txtCoordinates.Location = new System.Drawing.Point(87, 18);
            this.txtCoordinates.Margin = new System.Windows.Forms.Padding(2);
            this.txtCoordinates.Name = "txtCoordinates";
            this.txtCoordinates.Size = new System.Drawing.Size(303, 21);
            this.txtCoordinates.TabIndex = 60;
            this.txtCoordinates.Text = "31.2375793299037,120.412701006625";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 61;
            this.label5.Text = "坐标:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 48);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 62;
            this.label6.Text = "latDeg:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 48);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 63;
            this.label7.Text = "lonDeg:";
            // 
            // LatMinute
            // 
            this.LatMinute.AutoSize = true;
            this.LatMinute.Location = new System.Drawing.Point(14, 73);
            this.LatMinute.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LatMinute.Name = "LatMinute";
            this.LatMinute.Size = new System.Drawing.Size(65, 12);
            this.LatMinute.TabIndex = 64;
            this.LatMinute.Text = "latMinute:";
            // 
            // LonMinute
            // 
            this.LonMinute.AutoSize = true;
            this.LonMinute.Location = new System.Drawing.Point(214, 73);
            this.LonMinute.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LonMinute.Name = "LonMinute";
            this.LonMinute.Size = new System.Drawing.Size(65, 12);
            this.LonMinute.TabIndex = 65;
            this.LonMinute.Text = "lonMinute:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCoordinates);
            this.groupBox1.Controls.Add(this.txtLatMinute);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtlonMinute);
            this.groupBox1.Controls.Add(this.txtCellId);
            this.groupBox1.Controls.Add(this.txtLonDeg);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtLatDeg);
            this.groupBox1.Controls.Add(this.MqttAlarmTextBox);
            this.groupBox1.Controls.Add(this.txtBattery);
            this.groupBox1.Controls.Add(this.txtCoordinates);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.LonMinute);
            this.groupBox1.Controls.Add(this.txtAlarmType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.LatMinute);
            this.groupBox1.Controls.Add(this.txtGPS);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.btnMqttAlarm);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(1, 508);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(1114, 106);
            this.groupBox1.TabIndex = 66;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "坐标计算";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnCoordinates
            // 
            this.btnCoordinates.Location = new System.Drawing.Point(414, 73);
            this.btnCoordinates.Name = "btnCoordinates";
            this.btnCoordinates.Size = new System.Drawing.Size(116, 23);
            this.btnCoordinates.TabIndex = 67;
            this.btnCoordinates.Text = "计算坐标";
            this.btnCoordinates.UseVisualStyleBackColor = true;
            this.btnCoordinates.Click += new System.EventHandler(this.btnCoordinates_Click);
            // 
            // txtLatMinute
            // 
            this.txtLatMinute.Location = new System.Drawing.Point(87, 69);
            this.txtLatMinute.Name = "txtLatMinute";
            this.txtLatMinute.ReadOnly = true;
            this.txtLatMinute.Size = new System.Drawing.Size(112, 21);
            this.txtLatMinute.TabIndex = 69;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(415, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 79;
            this.label11.Text = "基站ID：";
            // 
            // txtlonMinute
            // 
            this.txtlonMinute.Location = new System.Drawing.Point(281, 69);
            this.txtlonMinute.Name = "txtlonMinute";
            this.txtlonMinute.ReadOnly = true;
            this.txtlonMinute.Size = new System.Drawing.Size(109, 21);
            this.txtlonMinute.TabIndex = 68;
            // 
            // txtCellId
            // 
            this.txtCellId.Location = new System.Drawing.Point(472, 19);
            this.txtCellId.Margin = new System.Windows.Forms.Padding(2);
            this.txtCellId.Name = "txtCellId";
            this.txtCellId.Size = new System.Drawing.Size(117, 21);
            this.txtCellId.TabIndex = 80;
            this.txtCellId.Text = "460.00.20831.12002";
            // 
            // txtLonDeg
            // 
            this.txtLonDeg.Location = new System.Drawing.Point(281, 44);
            this.txtLonDeg.Name = "txtLonDeg";
            this.txtLonDeg.ReadOnly = true;
            this.txtLonDeg.Size = new System.Drawing.Size(109, 21);
            this.txtLonDeg.TabIndex = 67;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(412, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 77;
            this.label10.Text = "电量";
            // 
            // txtLatDeg
            // 
            this.txtLatDeg.Location = new System.Drawing.Point(87, 44);
            this.txtLatDeg.Name = "txtLatDeg";
            this.txtLatDeg.ReadOnly = true;
            this.txtLatDeg.Size = new System.Drawing.Size(112, 21);
            this.txtLatDeg.TabIndex = 66;
            // 
            // MqttAlarmTextBox
            // 
            this.MqttAlarmTextBox.Location = new System.Drawing.Point(900, 15);
            this.MqttAlarmTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.MqttAlarmTextBox.Name = "MqttAlarmTextBox";
            this.MqttAlarmTextBox.Size = new System.Drawing.Size(156, 21);
            this.MqttAlarmTextBox.TabIndex = 51;
            this.MqttAlarmTextBox.Text = "mimacx0000030002";
            // 
            // txtBattery
            // 
            this.txtBattery.Location = new System.Drawing.Point(448, 50);
            this.txtBattery.Margin = new System.Windows.Forms.Padding(2);
            this.txtBattery.Name = "txtBattery";
            this.txtBattery.Size = new System.Drawing.Size(32, 21);
            this.txtBattery.TabIndex = 78;
            this.txtBattery.Text = "90";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(869, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 75;
            this.label9.Text = "报警类型";
            // 
            // txtAlarmType
            // 
            this.txtAlarmType.Location = new System.Drawing.Point(934, 40);
            this.txtAlarmType.Margin = new System.Windows.Forms.Padding(2);
            this.txtAlarmType.Name = "txtAlarmType";
            this.txtAlarmType.Size = new System.Drawing.Size(32, 21);
            this.txtAlarmType.TabIndex = 76;
            this.txtAlarmType.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(598, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 73;
            this.label8.Text = "GPS星数";
            // 
            // txtGPS
            // 
            this.txtGPS.Location = new System.Drawing.Point(650, 48);
            this.txtGPS.Margin = new System.Windows.Forms.Padding(2);
            this.txtGPS.Name = "txtGPS";
            this.txtGPS.Size = new System.Drawing.Size(32, 21);
            this.txtGPS.TabIndex = 74;
            this.txtGPS.Text = "7";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(872, 19);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(23, 12);
            this.label21.TabIndex = 52;
            this.label21.Text = "SN:";
            // 
            // btnMqttAlarm
            // 
            this.btnMqttAlarm.Location = new System.Drawing.Point(871, 73);
            this.btnMqttAlarm.Name = "btnMqttAlarm";
            this.btnMqttAlarm.Size = new System.Drawing.Size(75, 23);
            this.btnMqttAlarm.TabIndex = 67;
            this.btnMqttAlarm.Text = "提交报警";
            this.btnMqttAlarm.UseVisualStyleBackColor = true;
            this.btnMqttAlarm.Click += new System.EventHandler(this.btnAlarmType_Click2);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(531, 42);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 86;
            this.label19.Text = "状态：";
            // 
            // snend
            // 
            this.snend.Location = new System.Drawing.Point(134, 39);
            this.snend.MaxLength = 5;
            this.snend.Name = "snend";
            this.snend.Size = new System.Drawing.Size(56, 21);
            this.snend.TabIndex = 85;
            this.snend.TextChanged += new System.EventHandler(this.Snend_TextChanged);
            this.snend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.snend_KeyPress);
            // 
            // mqttTextBox2
            // 
            this.mqttTextBox2.Location = new System.Drawing.Point(307, 39);
            this.mqttTextBox2.Name = "mqttTextBox2";
            this.mqttTextBox2.Size = new System.Drawing.Size(161, 21);
            this.mqttTextBox2.TabIndex = 73;
            this.mqttTextBox2.TextChanged += new System.EventHandler(this.mqttTextBox2_TextChanged);
            // 
            // mqttPort2
            // 
            this.mqttPort2.Location = new System.Drawing.Point(473, 39);
            this.mqttPort2.Name = "mqttPort2";
            this.mqttPort2.Size = new System.Drawing.Size(52, 21);
            this.mqttPort2.TabIndex = 73;
            this.mqttPort2.TextChanged += new System.EventHandler(this.mqttPort2_TextChanged);
            this.mqttPort2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mqttPort2_KeyPress);
            // 
            // snbegin
            // 
            this.snbegin.Location = new System.Drawing.Point(134, 15);
            this.snbegin.MaxLength = 5;
            this.snbegin.Name = "snbegin";
            this.snbegin.Size = new System.Drawing.Size(56, 21);
            this.snbegin.TabIndex = 84;
            this.snbegin.TextChanged += new System.EventHandler(this.Snbegin_TextChanged);
            this.snbegin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.snbegin_KeyPress);
            // 
            // mqttPort1
            // 
            this.mqttPort1.Location = new System.Drawing.Point(473, 15);
            this.mqttPort1.Name = "mqttPort1";
            this.mqttPort1.Size = new System.Drawing.Size(52, 21);
            this.mqttPort1.TabIndex = 73;
            this.mqttPort1.TextChanged += new System.EventHandler(this.mqttPort1_TextChanged);
            this.mqttPort1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mqttPort1_KeyPress);
            // 
            // mqttTextBox1
            // 
            this.mqttTextBox1.Location = new System.Drawing.Point(307, 15);
            this.mqttTextBox1.Name = "mqttTextBox1";
            this.mqttTextBox1.Size = new System.Drawing.Size(161, 21);
            this.mqttTextBox1.TabIndex = 73;
            this.mqttTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(225, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 12);
            this.label14.TabIndex = 72;
            this.label14.Text = "Mqtt_Broker2:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(466, 42);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(11, 12);
            this.label16.TabIndex = 72;
            this.label16.Text = ":";
            this.label16.Click += new System.EventHandler(this.label15_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(466, 19);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(11, 12);
            this.label15.TabIndex = 72;
            this.label15.Text = ":";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(63, 45);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 12);
            this.label18.TabIndex = 72;
            this.label18.Text = "mimacx00000";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(63, 21);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(71, 12);
            this.label17.TabIndex = 72;
            this.label17.Text = "mimacx00000";
            this.label17.Click += new System.EventHandler(this.label17_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(225, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 12);
            this.label13.TabIndex = 72;
            this.label13.Text = "Mqtt_Broker1:";
            // 
            // mqttbtn
            // 
            this.mqttbtn.Location = new System.Drawing.Point(642, 13);
            this.mqttbtn.Name = "mqttbtn";
            this.mqttbtn.Size = new System.Drawing.Size(99, 23);
            this.mqttbtn.TabIndex = 71;
            this.mqttbtn.Text = "2.启动所有任务";
            this.mqttbtn.UseVisualStyleBackColor = true;
            this.mqttbtn.Click += new System.EventHandler(this.mqttbtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(747, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 23);
            this.button2.TabIndex = 70;
            this.button2.Text = "3.停止所有任务";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btnAlarmType
            // 
            this.btnAlarmType.Enabled = false;
            this.btnAlarmType.Location = new System.Drawing.Point(270, 45);
            this.btnAlarmType.Name = "btnAlarmType";
            this.btnAlarmType.Size = new System.Drawing.Size(75, 23);
            this.btnAlarmType.TabIndex = 67;
            this.btnAlarmType.Text = "提交报警";
            this.btnAlarmType.UseVisualStyleBackColor = true;
            this.btnAlarmType.Click += new System.EventHandler(this.btnAlarmType_Click);
            // 
            // btnFive
            // 
            this.btnFive.Enabled = false;
            this.btnFive.Location = new System.Drawing.Point(351, 46);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(86, 23);
            this.btnFive.TabIndex = 68;
            this.btnFive.Text = "上报5分钟";
            this.btnFive.UseVisualStyleBackColor = true;
            this.btnFive.Click += new System.EventHandler(this.btnFive_Click);
            // 
            // txtBTID
            // 
            this.txtBTID.Location = new System.Drawing.Point(254, 18);
            this.txtBTID.Margin = new System.Windows.Forms.Padding(2);
            this.txtBTID.Name = "txtBTID";
            this.txtBTID.Size = new System.Drawing.Size(156, 21);
            this.txtBTID.TabIndex = 69;
            this.txtBTID.Text = "mimacx0000000002";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbPort);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnTest);
            this.groupBox2.Controls.Add(this.btnHistory);
            this.groupBox2.Controls.Add(this.btnTw);
            this.groupBox2.Controls.Add(this.txtAuthkeys);
            this.groupBox2.Controls.Add(this.cmbIPAddress);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.txtSNNo);
            this.groupBox2.Controls.Add(this.txtBTID);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnFive);
            this.groupBox2.Controls.Add(this.btnClean);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnHeartbeat);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnAlarmType);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtKickstand);
            this.groupBox2.Controls.Add(this.btnCharger);
            this.groupBox2.Controls.Add(this.txtTotolKM);
            this.groupBox2.Location = new System.Drawing.Point(1, 618);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(1115, 150);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // cmbPort
            // 
            this.cmbPort.Enabled = false;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(535, 76);
            this.cmbPort.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(48, 20);
            this.cmbPort.TabIndex = 72;
            this.cmbPort.SelectedIndexChanged += new System.EventHandler(this.cmbPort_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(227, 23);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 12);
            this.label12.TabIndex = 83;
            this.label12.Text = "BT:";
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(108, 74);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(51, 23);
            this.btnTest.TabIndex = 81;
            this.btnTest.Text = "test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Enabled = false;
            this.btnHistory.Location = new System.Drawing.Point(16, 74);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(86, 23);
            this.btnHistory.TabIndex = 73;
            this.btnHistory.Text = "发送历史数据";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnTw
            // 
            this.btnTw.Enabled = false;
            this.btnTw.Location = new System.Drawing.Point(442, 45);
            this.btnTw.Name = "btnTw";
            this.btnTw.Size = new System.Drawing.Size(86, 23);
            this.btnTw.TabIndex = 72;
            this.btnTw.Text = "上报30秒";
            this.btnTw.UseVisualStyleBackColor = true;
            this.btnTw.Click += new System.EventHandler(this.btnTw_Click);
            // 
            // txtAuthkeys
            // 
            this.txtAuthkeys.Location = new System.Drawing.Point(16, 102);
            this.txtAuthkeys.Margin = new System.Windows.Forms.Padding(2);
            this.txtAuthkeys.Name = "txtAuthkeys";
            this.txtAuthkeys.Size = new System.Drawing.Size(104, 21);
            this.txtAuthkeys.TabIndex = 71;
            this.txtAuthkeys.Text = "20170308112003";
            // 
            // cmbIPAddress
            // 
            this.cmbIPAddress.Enabled = false;
            this.cmbIPAddress.FormattingEnabled = true;
            this.cmbIPAddress.Location = new System.Drawing.Point(225, 76);
            this.cmbIPAddress.Margin = new System.Windows.Forms.Padding(2);
            this.cmbIPAddress.Name = "cmbIPAddress";
            this.cmbIPAddress.Size = new System.Drawing.Size(271, 20);
            this.cmbIPAddress.TabIndex = 71;
            this.cmbIPAddress.SelectedIndexChanged += new System.EventHandler(this.cmbIPAddress_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(534, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 70;
            this.button1.Text = "TEST";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.positionSetBtn);
            this.groupBox3.Controls.Add(this.mqttbtn);
            this.groupBox3.Controls.Add(this.mqttTextBox2);
            this.groupBox3.Controls.Add(this.taskStatLabel);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.mqttPort2);
            this.groupBox3.Controls.Add(this.snend);
            this.groupBox3.Controls.Add(this.mqttPort1);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.mqttTextBox1);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Controls.Add(this.snbegin);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1115, 63);
            this.groupBox3.TabIndex = 73;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mqtt配置区";
            // 
            // positionSetBtn
            // 
            this.positionSetBtn.Location = new System.Drawing.Point(529, 13);
            this.positionSetBtn.Name = "positionSetBtn";
            this.positionSetBtn.Size = new System.Drawing.Size(107, 23);
            this.positionSetBtn.TabIndex = 67;
            this.positionSetBtn.Text = "1.设置车辆坐标";
            this.positionSetBtn.UseVisualStyleBackColor = true;
            this.positionSetBtn.Click += new System.EventHandler(this.btnCoordinates_Click);
            // 
            // taskStatLabel
            // 
            this.taskStatLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskStatLabel.BackColor = System.Drawing.Color.RoyalBlue;
            this.taskStatLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.taskStatLabel.Location = new System.Drawing.Point(564, 42);
            this.taskStatLabel.Name = "taskStatLabel";
            this.taskStatLabel.Size = new System.Drawing.Size(291, 15);
            this.taskStatLabel.TabIndex = 86;
            this.taskStatLabel.Text = "空闲";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 31);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 86;
            this.label20.Text = "SN范围：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(564, 50);
            this.progressBar1.MarqueeAnimationSpeed = 2;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(291, 10);
            this.progressBar1.Step = 500;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 70;
            this.progressBar1.Visible = false;
            // 
            // txtShow2
            // 
            this.txtShow2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShow2.Location = new System.Drawing.Point(568, 66);
            this.txtShow2.Multiline = true;
            this.txtShow2.Name = "txtShow2";
            this.txtShow2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtShow2.Size = new System.Drawing.Size(547, 376);
            this.txtShow2.TabIndex = 40;
            // 
            // txtShow3
            // 
            this.txtShow3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShow3.Location = new System.Drawing.Point(1, 426);
            this.txtShow3.Multiline = true;
            this.txtShow3.Name = "txtShow3";
            this.txtShow3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtShow3.Size = new System.Drawing.Size(1114, 81);
            this.txtShow3.TabIndex = 40;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 779);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtShow3);
            this.Controls.Add(this.txtShow2);
            this.Controls.Add(this.txtShow);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "MQTT车辆客户端 v1.14";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtShow;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHeartbeat;
        private System.Windows.Forms.TextBox txtSNNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKickstand;
        private System.Windows.Forms.TextBox txtTotolKM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnCharger;
        private System.Windows.Forms.TextBox txtCoordinates;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LatMinute;
        private System.Windows.Forms.Label LonMinute;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLatMinute;
        private System.Windows.Forms.TextBox txtlonMinute;
        private System.Windows.Forms.TextBox txtLonDeg;
        private System.Windows.Forms.TextBox txtLatDeg;
        private System.Windows.Forms.Button btnCoordinates;
        private System.Windows.Forms.Button btnAlarmType;
        private System.Windows.Forms.Button btnFive;
        private System.Windows.Forms.TextBox txtBTID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbIPAddress;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtAuthkeys;
        private System.Windows.Forms.Button btnTw;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGPS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAlarmType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBattery;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCellId;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox snend;
        private System.Windows.Forms.TextBox snbegin;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button mqttbtn;
        private System.Windows.Forms.TextBox mqttTextBox2;
        private System.Windows.Forms.TextBox mqttPort2;
        private System.Windows.Forms.TextBox mqttPort1;
        private System.Windows.Forms.TextBox mqttTextBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button positionSetBtn;
        private System.Windows.Forms.Label taskStatLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtShow2;
        private System.Windows.Forms.TextBox txtShow3;
        private System.Windows.Forms.Button btnMqttAlarm;
        private System.Windows.Forms.TextBox MqttAlarmTextBox;
        private System.Windows.Forms.Label label21;
    }
}

