using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using ServiceStack.Redis;
using System.Configuration;
using Comm;
using Newtonsoft.Json;



using System.Linq;

using System.Threading.Tasks;

using System.Security.Cryptography.X509Certificates;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net.Security;


namespace C0710_CharRoom_Client
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            this.mqttTextBox1.Text = "192.168.1.81";
            this.mqttTextBox2.Text = "192.168.1.80";
            this.mqttPort1.Text = "1883";
            this.mqttPort2.Text = "1883";
            this.snend.Text = "30005";
            this.snbegin.Text = "30000";
        }

        //begin added by pengshirui for test on 20190628 

        ///// <summary>
        ///// 
        ///// </summary>
        //List<Socket> sokClinets;
        //List<Thread> threadClients;
        //List<System.Timers.Timer> 

        //end add


        List<DeviceSocket> des = null;

        byte[] buffer = null;
        Hashtable dataHt = null;

        string timeStamp    = "";
        string privateKey   = "";
        string authKey      = "";
        string latdeg       = "";
        string latMinute    = "";
        string longed       = "";
        string lonminutte   = "";
        string totolkm      = "";
        string gps          = "";
        string Battery      = "";

        bool stopall = false;
        Socket sokClient = null;//负责与服务端通信的套接字
        Socket sokClient2 = null;//负责与服务端通信的套接字

        Thread threadClient = null;//负责 监听 服务端发送来的消息的线程
        Thread threadClient2 = null;//负责 监听 服务端发送来的消息的线程

        int snbeginno = 30000;
        int snendno = 30002;

        bool isRec = true;//是否循环接收服务端数据

        System.Timers.Timer fiveMinutesTimer = null;
        
        System.Timers.Timer twentySecondsTimer = null;

        System.Timers.Timer heartbeatTimer = null;

        Hashtable mqttht = null;
        Hashtable mqttht2 = null;
        Hashtable sn2mqttclient = null;



        int HeartbeatTime = Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatTime"]);

        #region 018车坐标
        string latitudeDegree = "31";
        string latitudeMinute = "27.60414382";
        string longitudeDegree = "120";
        string longitudeMinute = "53.70686586";
        #endregion

        #region 上报参数
        int satellite = 7; //GPS卫星数
        int kickstand = 0;  //脚撑状态
        bool reason = true;//断开充电原因
        decimal totalMileage = 721;  //总里程(单位：KM)
        decimal battery = 50;  //电池剩余电量
        bool charging = false; //电池充电状态
        string errorCode = "000000000";//故障码
        bool batteryState = true; //电池丢失状态
        string standbyBattery = string.Empty; //备用电池使用状态
        int alarmType = 1;//报警类型
        string location = string.Empty;//
        int chargeCount = 9;//电池组有多少次循环
        int ctrlState = 2;
        string cellId = "460.00.20831.12002";
        #endregion

        public static string mqttBroker1 = "192.168.1.81";
        public static string mqttBroker2 = "192.168.1.80";
        public static int imqttPort1 = 1883;
        public static int imqttPort2 = 1883;

        public static double started_tasks = 0;
        public static double started_tasks2 = 0;
        public static int total_tasks = 0;

        public static Task startAllTask = null;
        public static CancellationTokenSource startTokenSource = new CancellationTokenSource();
        public static CancellationToken startToken = startTokenSource.Token;
        public static ManualResetEvent startResetEvent = new ManualResetEvent(true);

        public static Task stopAllTask = null;
        public static CancellationTokenSource stopTokenSource = new CancellationTokenSource();
        public static CancellationToken stopToken = stopTokenSource.Token;
        public static ManualResetEvent stopResetEvent = new ManualResetEvent(true);

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.stopall = false;

            string ipAddress = this.cmbIPAddress.SelectedValue.ToString().Trim();
            string port = this.cmbPort.SelectedValue.ToString().Trim();


            IPHostEntry host = Dns.GetHostEntry(ipAddress);
            //创建 ip对象
            IPAddress address = host.AddressList[0];
            //创建网络节点对象 包含 ip和port
            IPEndPoint endpoint = new IPEndPoint(address, int.Parse(port));

            //begin added by pengshirui for one or more socket by DeviceSocket
            string mm = "mimacx00000";
            string topic_in = "/+/+/+/req/";
            string[] topics = new string[1];
            byte[] qos_levels = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };

            for (int i = snbeginno; i <= snendno; i++)
            {
                topic_in += mm + i.ToString();
                topics[0] = topic_in;
                DeviceSocket de = new DeviceSocket(topics, qos_levels);

                de.sokClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                de.sokClient.Connect(endpoint);
                de.sn = mm + i.ToString();// "mimacx0000010002";
                de.bt = "0000" + i.ToString();


                //创建负责接收 服务端发送来数据的 线程
                //de.firstSendTask = new Task(new ParameterizedThreadStart(ReceiveMsg));
                //de.firstSendThread.IsBackground = true;

                //如果在win7下要通过 某个线程 来调用 文件选择框的代码，就需要设置如下
                //de.firstSendThread.SetApartmentState(ApartmentState.STA);
                //de.firstSendThread.Start(de);


                de.firstSendTask = new Task(() =>
                {
                    ReceiveMsgMqtt(de);
                });
                de.firstSendTask.Start();

                

                bool dd = de.sokClient.Connected;
                this.label4.Text = "连接服务器成功" + i.ToString();
                des.Add(de);
            }


 //end add


            
        }


        void ReportDataWithFiveMinutes(object obj)
        {

            //延时
            int iTime = 0;

            DeviceSocket de = (DeviceSocket)obj;
            
            while (de.mqttClient != null && !stopall)
            {
                
                if (de.fiveMinutetoken.IsCancellationRequested)
                {
                    return;
                }

                de.fiveMinuteResetEvent.WaitOne();

                Hashtable ht = getHT2(de.sn, de.bt);
                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);

                //de.sokClient.Send(buffer);
                de.mqttClient.Publish("/area/report/lock/log", buffer);

                ProtocolBean basesP = ProtocolUtils.decode(buffer);
                if (basesP == null)
                {
                    this.txtShow.AppendText("basesP is null");
                }
                this.txtShow.AppendText(string.Format("\r\n<--Topic:{0}\r\nCmId:{1}  sn:{2}\r\nPayload:{3}\r\n---- {4} ----\r\n", "/area/report/lock/log", basesP.CmId, de.sn, basesP.Payload, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff")));

                iTime = 30;
                while (iTime != 0)
                {
                    if (de.fiveMinutetoken.IsCancellationRequested)
                    {
                        return;
                    }
                    Thread.Sleep(10000);
                    iTime--;
                }
            }
        }

        void ReportDataWithTwentySeconds(object obj)
        {
            //延时
            int iTime = 0;

            DeviceSocket de = (DeviceSocket)obj;
            while (de.mqttClient != null && !stopall)
            {
                

                de.twentySecondsResetEvent.WaitOne();

                Hashtable ht = getHT(de.sn, de.bt);
                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);

                //de.sokClient.Send(buffer);

                de.mqttClient.Publish("/area/report/unlock/log", buffer);

                ProtocolBean basesP = ProtocolUtils.decode(buffer);
                if (basesP == null)
                {
                    this.txtShow.AppendText("basesP is null");
                }
                this.txtShow.AppendText(string.Format("\r\n<--Topic:{0}\r\nCmId:{1}  sn:{2}\r\nPayload:{3}\r\n---- {4} ----\r\n", "/area/report/unlock/log", basesP.CmId, de.sn, basesP.Payload, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff")));

                iTime = 2;
                while (iTime != 0)
                {
                    if (de.twentySecondstoken.IsCancellationRequested)
                    {
                        return;
                    }
                    Thread.Sleep(10000);
                    iTime--;
                }
            }
        }

        private void btnAlarmType_Click2(object sender, EventArgs e)
        {
            string sn = this.MqttAlarmTextBox.Text.Trim();//SN
            string snEn = this.Encryption(sn);

            latitudeDegree = this.txtLatDeg.Text.Trim();
            latitudeMinute = this.txtLatMinute.Text.Trim();
            longitudeDegree = this.txtLonDeg.Text.Trim();
            longitudeMinute = this.txtlonMinute.Text.Trim();
            alarmType = Convert.ToInt32(this.txtAlarmType.Text.Trim());
            totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
            cellId = this.txtCellId.Text.Trim();

            Hashtable ht = new Hashtable();
            ht.Add("sn", snEn);
            ht.Add("messageType", 5);

            Hashtable dataHt = new Hashtable();
            dataHt.Add("latitudeDegree", latitudeDegree);
            dataHt.Add("latitudeMinute", latitudeMinute);
            dataHt.Add("longitudeDegree", longitudeDegree);
            dataHt.Add("longitudeMinute", longitudeMinute);
            dataHt.Add("battery", battery);
            dataHt.Add("alarmType", alarmType);
            dataHt.Add("totalMileage", totalMileage);
            dataHt.Add("cellId", cellId);

            ht.Add("messageBody", dataHt);

            byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
            //this.sokClient.Send(buffer);
            DeviceSocket de = (DeviceSocket)sn2mqttclient[sn];
            de.mqttClient.Publish("/area/report/alarm/log", buffer);
            ProtocolBean basesP = ProtocolUtils.decode(buffer);
            if (basesP == null)
            {
                this.txtShow.AppendText("basesP is null");
            }
            this.txtShow.AppendText(string.Format("\r\n<--Topic:{0}\r\nCmId:{1}  sn:{2}\r\nPayload:{3}\r\n---- {4} ----\r\n", "/area/report/alarm/log", basesP.CmId, de.sn, basesP.Payload, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff")));
        }

        void reSend(object obj)
        {

            //延时
            int iTime = 0;
            DeviceSocket de = (DeviceSocket)obj;
            while (de.mqttClient != null && !stopall)
            {

                if (de.heartbeettoken.IsCancellationRequested)
                {
                    this.txtShow3.AppendText(string.Format("!!!{0}  mqtttask:{1} stop sucess! \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), de.sn));
                    return;
                }

                de.heartbeetResetEvent.WaitOne();

                try
                {
                    //this.sokClient
                    Hashtable ht = new Hashtable();
                    string snEn = this.Encryption(de.sn);
                    ht.Add("sn", snEn);
                    byte[] buffer = ProtocolUtils.encode(7, ht, 2345);
                    //de.sokClient.Send(buffer);

                    de.mqttClient.Publish("/area/report/heartbeat/log", buffer);
                    this.txtShow.AppendText(string.Format("\r\n<--Topic:{0}\r\nsn:{1}\r\n---- {2} ----\r\n", "/area/report/heartbeat/log", de.sn, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff")));

                    de.mqttClient2.Publish("/area/report/heartbeat/log", buffer);
                    //Thread.Sleep(100);
                }
                catch (Exception ex)
                { }
                iTime = 12;
                while (iTime != 0)
                {
                    if (de.heartbeettoken.IsCancellationRequested)
                    {
                        return;
                    }
                    Thread.Sleep(10000);
                    iTime--;
                }
            }

            //this.txtShow.AppendText(string.Format("!!!{0}  mqtttask:{1} stop sucess! \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), de.sn));

        }



        /// <summary>
        /// 接收服务端发送来的消息数据
        /// </summary>
        void ReceiveMsg(object obj)
        {
            DeviceSocket de = (DeviceSocket)obj;
            try
            {
                Thread fiveMinutesTimer = null;
                Thread twentySecondsTimer = null;
                Thread heartbeatTimer = null;

                while (this.isRec && !stopall)
                {
                    byte[] msgArr = new byte[1024];//接收到的消息的缓冲区
                    int length = 0;
                    //接收服务端发送来的消息数据
                    length = de.sokClient.Receive(msgArr);//Receive会阻断线程
                    if (length == 0)
                    {
                        continue;
                    }
                    string message = string.Empty;
                    ProtocolBean protocol = ProtocolUtils.decode(msgArr);
                    if (protocol == null)
                    {
                        this.txtShow.AppendText("ProtocolBean is null");
                        continue;
                    }
                    this.txtShow.AppendText(string.Format("CmId:{0},Payload:{1}\r\n", protocol.CmId, protocol.Payload));
                    switch (protocol.CmId)
                    {
                        case 1://车辆向服务器发出鉴权请求
                            break;
                        case 2://车辆接收到服务器关于鉴权的应答
                            bool isSucessed = false;
                            AnsisleProtocol.ConnectionResponse(protocol, ref message, ref isSucessed);
                            if (isSucessed)
                            {
                                if(fiveMinutesTimer == null)
                                {
                                    fiveMinutesTimer = new Thread(new ParameterizedThreadStart(ReportDataWithFiveMinutes));
                                    fiveMinutesTimer.Start(obj);
                                }

                                if (heartbeatTimer == null)
                                {
                                    heartbeatTimer = new Thread(new ParameterizedThreadStart(reSend));
                                    heartbeatTimer.Start(obj);

                                }
                                //fiveMinutesTimer.Stop();
                                //fiveMinutesTimer.Start();
                                //heartbeatTimer.Start();
                                //ReportDataWithFiveMinutes(null, null);
                            }
                            break;
                        case 3://车辆向服务器发送数据 
                            break;
                        case 4://车辆接收到服务器关于上报数据的应答
                            AnsisleProtocol.ReportResponse(protocol, ref message);
                            break;
                        case 5://车辆收到服务器的命令请求
                            byte[] buffer = new byte[1024];
                            int cmdid = 0;
                            AnsisleProtocol.CmdRequest(protocol, Convert.ToDouble(this.txtLatDeg.Text.Trim()),
                                Convert.ToDouble(this.txtLatMinute.Text.Trim()), Convert.ToDouble(this.txtLonDeg.Text.Trim()),
                                Convert.ToDouble(this.txtlonMinute.Text.Trim()), Convert.ToDouble(this.txtTotolKM.Text.Trim()),
                                Convert.ToInt32(this.txtGPS.Text.Trim()), Convert.ToInt32(this.txtBattery.Text.Trim()), ref message, ref buffer, ref cmdid);
                            int ss = de.sokClient.Send(buffer);
                            if (ss <= 0)
                            {
                                this.txtShow.AppendText("ss<=0 \r\n");
                            }
                            ProtocolBean basesP = ProtocolUtils.decode(buffer);
                            if (basesP == null)
                            {
                                this.txtShow.AppendText("basesP is null");
                            }
                            this.txtShow.AppendText(string.Format("CmId:{0},Payload:{1}\r\n---------", basesP.CmId, basesP.Payload));
                            if (cmdid == 1)
                            {
                                if (twentySecondsTimer == null)
                                {
                                    twentySecondsTimer = new Thread(new ParameterizedThreadStart(ReportDataWithTwentySeconds));
                                    twentySecondsTimer.Start(obj);
                                }

                                //fiveMinutesTimer.Stop();
                                //twentySecondsTimer.Stop();
                                //twentySecondsTimer.Start();
                                //ReportDataWithTwentySeconds(null, null);
                            }
                            else if (cmdid == 2)
                            {
                                if (fiveMinutesTimer == null)
                                {
                                    fiveMinutesTimer = new Thread(new ParameterizedThreadStart(ReportDataWithFiveMinutes));
                                    fiveMinutesTimer.Start(obj);
                                }
                                //twentySecondsTimer.Stop();
                                //fiveMinutesTimer.Stop();
                                //fiveMinutesTimer.Start();
                                //ReportDataWithFiveMinutes(null, null);
                            }

                            break;
                        case 6://
                            break;
                        case 7:
                            break;
                        case 8:
                            break;
                        case 9:
                            break;
                        case 10:
                            break;
                        case 11:
                            break;
                        case 12:
                            break;
                        case 13:
                            break;
                        case 14:
                            break;
                        case 15:
                            break;
                    }
                    txtShow.AppendText(string.Format("{0}  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), message + "\r\n"));
                }
            }
            catch (Exception ex)
            {
                this.label4.Text = "连接已断开";
                txtShow.AppendText(string.Format("发生未知异常：Error：{0}", ex.Message));
                de.sokClient = null;
            }
        }
        /// <summary>
        /// 接收服务端发送来的消息数据
        /// </summary>
        void ReceiveMsgMqtt(object obj)
        {
            DeviceSocket de = (DeviceSocket)obj;
            try
            {
                if (de.fiveMinutesTask == null)
                {
                    de.fiveMinutesTask = new Task(() => {
                        ReportDataWithFiveMinutes(de);
                    }, de.fiveMinutetoken);
                }

                de.fiveMinutesTask.Start();

                if (de.heartbeatTask == null)
                {
                    de.heartbeatTask = new Task(() =>
                    {
                        reSend(de);
                    });
                    de.heartbeatTask.Start();
                }
                txtShow.AppendText(string.Format("{0}  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"),  "start mqtttask\r\n"));
            }
            catch (Exception ex)
            {
                this.label4.Text = "连接已断开";
                txtShow.AppendText(string.Format("发生未知异常：Error：{0}", ex.Message));
                de.sokClient = null;
            }
        }
        /// <summary>
        /// 接收服务端发送来的消息数据
        /// </summary>

        void ReceiveMsg()
        {
            try
            {
                while (isRec)
                {
                    byte[] msgArr = new byte[1024];//接收到的消息的缓冲区
                    int length = 0;
                    //接收服务端发送来的消息数据
                    length = sokClient.Receive(msgArr);//Receive会阻断线程
                    if (length == 0)
                    {
                        this.label4.Text = "连接已断开";
                        continue;
                    }
                    string message = string.Empty;
                    ProtocolBean protocol = ProtocolUtils.decode(msgArr);
                    if (protocol == null)
                    {
                        txtShow.AppendText("ProtocolBean is null");
                        continue;
                    }
                    txtShow.AppendText(string.Format("CmId:{0},Payload:{1}\r\n", protocol.CmId, protocol.Payload));
                    switch (protocol.CmId)
                    {
                        case 1://车辆向服务器发出鉴权请求
                            break;
                        case 2://车辆接收到服务器关于鉴权的应答
                            bool isSucessed = false;
                            AnsisleProtocol.ConnectionResponse(protocol, ref message, ref isSucessed);
                            if (isSucessed)
                            {
                                this.fiveMinutesTimer.Stop();
                                this.fiveMinutesTimer.Start();
                                this.heartbeatTimer.Start();
                                this.ReportDataWithFiveMinutes(null, null);
                            }
                            break;
                        case 3://车辆向服务器发送数据 
                            break;
                        case 4://车辆接收到服务器关于上报数据的应答
                            AnsisleProtocol.ReportResponse(protocol, ref message);
                            break;
                        case 5://车辆收到服务器的命令请求
                            byte[] buffer = new byte[1024];
                            int cmdid = 0;
                            AnsisleProtocol.CmdRequest(protocol, Convert.ToDouble(this.txtLatDeg.Text.Trim()),
                                Convert.ToDouble(this.txtLatMinute.Text.Trim()), Convert.ToDouble(this.txtLonDeg.Text.Trim()),
                                Convert.ToDouble(this.txtlonMinute.Text.Trim()), Convert.ToDouble(this.txtTotolKM.Text.Trim()),
                                Convert.ToInt32(this.txtGPS.Text.Trim()), Convert.ToInt32(this.txtBattery.Text.Trim()), ref message, ref buffer, ref cmdid);
                            int ss = sokClient.Send(buffer);
                            if (ss <= 0)
                            {
                                txtShow.AppendText("ss<=0 \r\n");
                            }
                            ProtocolBean basesP = ProtocolUtils.decode(buffer);
                            if (basesP == null)
                            {
                                txtShow.AppendText("basesP is null");
                            }
                            txtShow.AppendText(string.Format("CmId:{0},Payload:{1}\r\n---------", basesP.CmId, basesP.Payload));
                            if (cmdid == 1)
                            {
                                this.fiveMinutesTimer.Stop();
                                this.twentySecondsTimer.Stop();
                                this.twentySecondsTimer.Start();
                                this.ReportDataWithTwentySeconds(null, null);
                            }
                            else if (cmdid == 2)
                            {
                                this.twentySecondsTimer.Stop();
                                this.fiveMinutesTimer.Stop();
                                this.fiveMinutesTimer.Start();
                                this.ReportDataWithFiveMinutes(null, null);
                            }

                            break;
                        case 6://
                            break;
                        case 7:
                            break;
                        case 8:
                            break;
                        case 9:
                            break;
                        case 10:
                            break;
                        case 11:
                            break;
                        case 12:
                            break;
                        case 13:
                            break;
                        case 14:
                            break;
                        case 15:
                            break;
                    }
                    txtShow.AppendText(string.Format("{0}  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), message + "\r\n"));
                }
            }
            catch (Exception ex)
            {
                this.label4.Text = "连接已断开";
                txtShow.AppendText(string.Format("发生未知异常：Error：{0}", ex.Message));
            }
        }

        private void LoadAllData()
        {
            this.timeStamp    = this.txtAuthkeys.Text.Trim();
            this.privateKey   = ConfigurationManager.AppSettings["privateKey"].ToString();

            this.latdeg       = this.txtLatDeg.Text.Trim();
            this.latMinute    = this.txtLatMinute.Text.Trim();
            this.longed       = this.txtLonDeg.Text.Trim();
            this.lonminutte   = this.txtlonMinute.Text.Trim();
            this.totolkm      = this.txtTotolKM.Text.Trim();
            this.gps          = this.txtGPS.Text.Trim();
            this.Battery      = this.txtBattery.Text.Trim();

            getBuffer(ref buffer);
            this.dataHt = getdataHT();
        }
        private Hashtable getht(string sn, string BTID)
        {
            //string sn = this.txtSNNo.Text.Trim();
            //string BTID = this.txtBTID.Text.Trim();// "Tv221u-164866E3";

            string enSn = this.Encryption(sn);
            //string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string timeStamp = this.timeStamp;
            string BMSID = sn;
            string ctrlID = sn;
            string authKey = CreateAuthKey(sn, timeStamp); ;

            Hashtable ht = new Hashtable();
            ht.Add("sn", enSn);
            ht.Add("timeStamp", timeStamp);
            ht.Add("BTID", BTID);
            ht.Add("BMSID", BMSID);
            ht.Add("ctrlID", ctrlID);
            ht.Add("authKey", authKey);
            return ht;
        }
        /// <summary>
        /// 发送连接请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                //string sn = this.txtSNNo.Text.Trim();
                //string enSn = this.Encryption(sn);
                ////string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //string timeStamp = this.txtAuthkeys.Text.Trim();
                //string BTID = this.txtBTID.Text.Trim();// "Tv221u-164866E3";
                //string BMSID = sn;
                //string ctrlID = sn;
                //string authKey = CreateAuthKey(sn, timeStamp);

                //Hashtable ht = new Hashtable();
                //ht.Add("sn", enSn);
                //ht.Add("timeStamp", timeStamp);
                //ht.Add("BTID", BTID);
                //ht.Add("BMSID", BMSID);
                //ht.Add("ctrlID", ctrlID);
                //ht.Add("authKey", authKey);

                
                LoadAllData();

                //Hashtable ht1 = getht("mimacx0000010002", "000000002");
                //    Hashtable ht2 = getht("mimacx0000010003", "000000003");

                //byte[] arrMsg1 = ProtocolUtils.encode(1, ht1, 1234);
                //    byte[] arrMsg2 = ProtocolUtils.encode(1, ht2, 1234);


                foreach (DeviceSocket de in des)
                {
                    if(de.sokClient != null)
                    {
                        Hashtable ht1 = getht(de.sn, de.bt);

                        byte[] arrMsg1 = ProtocolUtils.encode(1, ht1, 1234);
                        de.sokClient.Send(arrMsg1);
                    }
                        
                }
             //   sokClient.Send(arrMsg1);
            //    sokClient2.Send(arrMsg2);
            }
            catch (Exception ex)
            {
                this.label4.Text = "连接已断开";
            }
        }

        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHeartbeat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtSNNo.Text))
            {
                MessageBox.Show("SN不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //Hashtable ht = new Hashtable();
            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = this.HeartbeatTime * 1000;
            //timer.Elapsed += reSend;
            //timer.AutoReset = true;
            //timer.Enabled = true;
            reSend(null, null);
        }
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reSend(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //this.sokClient
                Hashtable ht = new Hashtable();
                string snEn = this.Encryption(this.txtSNNo.Text.Trim());
                ht.Add("sn", snEn);
                byte[] buffer = ProtocolUtils.encode(7, ht, 2345);
                sokClient.Send(buffer);
                //Thread.Sleep(100);
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 生成私有KEY
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="timeStamp"></param>
        private string CreateAuthKey(string sn, string timeStamp)
        {
            string privateKey = this.privateKey;
            string result = string.Format("sn={0}&privateKey={1}&timeStamp={2}", sn, privateKey, timeStamp);
            return GetMD5(result).ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String GetMD5(String str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string builder = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            builder = builder.Replace("-", "");
            return builder;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">需加密的字符串</param>
        private string Encryption(string str)
        {
            char[] arr = str.ToCharArray();// 字符串转为字符数组 
            Array.Reverse(arr);  // 将数组反转 
            string _strPwd = new String(arr);//反转后的字符串
            byte[] array = System.Text.Encoding.ASCII.GetBytes(_strPwd);//将反转后的字符串转成ascii码
            return Convert.ToBase64String(array);//通过Base64对数组加密，形成加密后的字符串
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str">需解密的字符串</param>
        private string Decryption(string str)
        {
            byte[] array = Convert.FromBase64String(str);//通过Base64对加密后的字符串解码成数组
            Array.Reverse(array);  // 将数组反转            
            return System.Text.Encoding.ASCII.GetString(array);//通过ascii恢复出加密前的字符串
        }

        /// <summary>
        /// 历史数据上报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHistoryReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtSNNo.Text))
            {
                MessageBox.Show("SN不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Hashtable ht = new Hashtable();
            string snEn = this.Encryption(this.txtSNNo.Text.Trim());
            ht.Add("sn", snEn);
            ht.Add("messageType", 8);

            totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());

            latitudeDegree = this.txtLatDeg.Text.Trim();
            latitudeMinute = this.txtLatMinute.Text.Trim();
            longitudeDegree = this.txtLonDeg.Text.Trim();
            longitudeMinute = this.txtlonMinute.Text.Trim();

            List<Hashtable> hsTlist = new List<Hashtable>();

            for (int i = 0; i < 3; i++)
            {
                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("at", Convert.ToDateTime("2017-03-30 11:23:09"));

                hsTlist.Add(dataHt);
            }
            Hashtable dd = new Hashtable();
            dd.Add("location", hsTlist);

            ht.Add("messageBody", dd);

            byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
            this.sokClient.Send(buffer);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            des = new List<DeviceSocket>();
            //5分钟上报一次
            fiveMinutesTimer = new System.Timers.Timer();
            fiveMinutesTimer.Interval = 2 * 60 * 1000;
            fiveMinutesTimer.Elapsed += ReportDataWithFiveMinutes;
            fiveMinutesTimer.AutoReset = true;
            fiveMinutesTimer.Enabled = false;

            //20秒上报一次
            twentySecondsTimer = new System.Timers.Timer();
            twentySecondsTimer.Interval = 20 * 1000;
            twentySecondsTimer.Elapsed += ReportDataWithTwentySeconds;
            twentySecondsTimer.AutoReset = true;
            twentySecondsTimer.Enabled = false;

            heartbeatTimer = new System.Timers.Timer();
            heartbeatTimer.Interval = 30 * 1000;
            heartbeatTimer.Elapsed += reSend;
            heartbeatTimer.AutoReset = true;
            heartbeatTimer.Enabled = false;

            List<ReportDataType> IPType = new List<ReportDataType>();
            //IPType.Add(new ReportDataType("mqtttest.findbug.net", "mqtttest.findbug.net"));
            IPType.Add(new ReportDataType("10.0.0.163", "10.0.0.163"));
            IPType.Add(new ReportDataType("127.0.0.1", "127.0.0.1"));
            IPType.Add(new ReportDataType("192.168.6.33", "192.168.6.33"));
            IPType.Add(new ReportDataType("192.168.6.27", "192.168.6.27"));
            IPType.Add(new ReportDataType("192.168.6.21", "192.168.6.21"));
            IPType.Add(new ReportDataType("ip24.pengshirui.com", "ip24.pengshirui.com"));
            IPType.Add(new ReportDataType("192.168.6.24", "192.168.6.24"));
            IPType.Add(new ReportDataType("116.62.246.156", "116.62.246.156"));
            IPType.Add(new ReportDataType("192.168.1.34 ", "192.168.1.34 "));
            IPType.Add(new ReportDataType("120.27.221.91", "120.27.221.91"));
            IPType.Add(new ReportDataType("116.62.138.101", "116.62.138.101"));
            IPType.Add(new ReportDataType("192.168.1.9", "192.168.1.9"));
            IPType.Add(new ReportDataType("192.168.1.200", "192.168.1.200"));
            IPType.Add(new ReportDataType("47.74.132.53", "47.74.132.53"));

            this.cmbIPAddress.DataSource = IPType;
            this.cmbIPAddress.DisplayMember = "name";
            this.cmbIPAddress.ValueMember = "value";

            List<ReportDataType> PortType = new List<ReportDataType>();
            PortType.Add(new ReportDataType("4535", "4535"));
            PortType.Add(new ReportDataType("4530", "4530"));

            this.cmbPort.DataSource = PortType;
            this.cmbPort.DisplayMember = "name";
            this.cmbPort.ValueMember = "value";
        }

       

        /// <summary>
        /// 清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClean_Click(object sender, EventArgs e)
        {
            this.txtShow.Text = string.Empty;
        }

        /// <summary>
        /// 未锁状态下，每5分钟上报一次车辆信息
        /// </summary>
        private void ReportDataWithFiveMinutes(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
                || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);
                satellite = Convert.ToInt32(this.txtGPS.Text.Trim());

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 2);

                kickstand = Convert.ToInt32(this.txtKickstand.Text.Trim());
                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();
                battery = Convert.ToInt32(this.txtBattery.Text.Trim());
                cellId = this.txtCellId.Text.Trim();

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("satellite", satellite);
                dataHt.Add("kickstand", kickstand);
                dataHt.Add("battery", battery);
                dataHt.Add("charging", charging);
                dataHt.Add("batteryState", batteryState);
                dataHt.Add("errorCode", errorCode);
                dataHt.Add("chargeCount", chargeCount);
                dataHt.Add("ctrlState", ctrlState);
                dataHt.Add("gpstype", 1);
                dataHt.Add("cellId", cellId);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 开锁状态下，20秒上报一次车辆信息
        /// </summary>
        private void ReportDataWithTwentySeconds(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);
                satellite = Convert.ToInt32(this.txtGPS.Text.Trim());

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 1);

                Hashtable dataHt = new Hashtable();
                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();
                battery = Convert.ToDecimal(this.txtBattery.Text.Trim());
                cellId = this.txtCellId.Text.Trim();

                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("battery", battery);
                dataHt.Add("satellite", satellite);
                dataHt.Add("cellId", cellId);
                dataHt.Add("gpstype", 1);
                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            { }
        }

        private Hashtable getHT(string sn, string bt)
        {
            Hashtable ht = new Hashtable();
            try
            {
                //string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);
                ht.Add("sn", snEn);
                ht.Add("messageType", 1);
                ht.Add("messageBody", this.dataHt);

                //buffer = ProtocolUtils.encode(3, ht, 1234);
            }
            catch (Exception ex)
            { }

            return ht;
        }
        private Hashtable getHT2(string sn, string bt)
        {
            Hashtable ht = new Hashtable();
            try
            {
                //string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);
                ht.Add("sn", snEn);
                ht.Add("messageType", 1);
                ht.Add("messageBody", this.dataHt);

                //buffer = ProtocolUtils.encode(3, ht, 1234);
            }
            catch (Exception ex)
            { }

            return ht;
        }

        private Hashtable getdataHT()
        {
            Hashtable dataHt = new Hashtable();

            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return dataHt;
            }
            try
            {
                //string sn = this.txtSNNo.Text.Trim();//SN
                //string snEn = this.Encryption(sn);
                satellite = Convert.ToInt32(this.txtGPS.Text.Trim());

                //Hashtable ht = new Hashtable();
                //ht.Add("sn", snEn);
                //ht.Add("messageType", 1);


                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();
                battery = Convert.ToDecimal(this.txtBattery.Text.Trim());
                cellId = this.txtCellId.Text.Trim();

                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("battery", battery);
                dataHt.Add("satellite", satellite);
                dataHt.Add("cellId", cellId);
                dataHt.Add("gpstype", 1);
                //ht.Add("messageBody", dataHt);

                //buffer = ProtocolUtils.encode(3, ht, 1234);
            }
            catch (Exception ex)
            { }

            return dataHt;
        }
        private void getBuffer(ref byte[] buffer)
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);
                satellite = Convert.ToInt32(this.txtGPS.Text.Trim());

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 1);

                Hashtable dataHt = new Hashtable();
                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();
                battery = Convert.ToDecimal(this.txtBattery.Text.Trim());
                cellId = this.txtCellId.Text.Trim();

                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("battery", battery);
                dataHt.Add("satellite", satellite);
                dataHt.Add("cellId", cellId);
                dataHt.Add("gpstype", 1);
                ht.Add("messageBody", dataHt);

                buffer = ProtocolUtils.encode(3, ht, 1234);
            }
            catch (Exception ex)
            { }
        }

        void ReportData(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                //MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);
                satellite = Convert.ToInt32(this.txtGPS.Text.Trim());

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 1);

                Hashtable dataHt = new Hashtable();
                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();
                battery = Convert.ToDecimal(this.txtBattery.Text.Trim());
                cellId = this.txtCellId.Text.Trim();

                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("battery", battery);
                dataHt.Add("satellite", satellite);
                dataHt.Add("cellId", cellId);
                dataHt.Add("gpstype", 1);
                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 开始充电时，上报一次数据
        /// </summary>
        private void ReportDataWithStartBattery()
        {
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 3);

                Hashtable dataHt = new Hashtable();
                dataHt.Add("battery", battery);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 断开充电时，上报信息
        /// </summary>
        private void ReportDataWithEndBatter()
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 4);
                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("battery", battery);
                dataHt.Add("reason", reason);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 报警时上报信息
        /// </summary>
        private void ReprotDataWithAlarm()
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 5);

                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("battery", battery);
                dataHt.Add("alarmType", alarmType);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 脚撑撑起，自动上锁成功，上报数据
        /// </summary>
        private void ReprotDataWithAutoLock()
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);
                satellite = Convert.ToInt32(this.txtGPS.Text.Trim());
                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 6);
                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);
                dataHt.Add("satellite", satellite);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 蓝牙开锁成功，上报信息
        /// </summary>
        private void ReprotDataWithBluetooth()
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 7);
                totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("totalMileage", totalMileage);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 低能耗时，上报数据
        /// </summary>
        private void ReprotDataWithLow()
        {
            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sn = this.txtSNNo.Text.Trim();//SN
                string snEn = this.Encryption(sn);

                latitudeDegree = this.txtLatDeg.Text.Trim();
                latitudeMinute = this.txtLatMinute.Text.Trim();
                longitudeDegree = this.txtLonDeg.Text.Trim();
                longitudeMinute = this.txtlonMinute.Text.Trim();

                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 9);

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1234);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 车辆开始充电 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCharger_Click(object sender, EventArgs e)
        {
            this.ReportDataWithStartBattery();
        }

        /// <summary>
        /// 坐标计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCoordinates_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtCoordinates.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string coordinates = this.txtCoordinates.Text.Trim();

            string[] arr = coordinates.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string lat = arr[0];
            string lon = arr[1];

            string[] latArr = lat.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            this.txtLatDeg.Text = latArr[0];

            this.txtLatMinute.Text = ((Convert.ToDouble(lat) - Convert.ToDouble(latArr[0])) * 60).ToString("#0.00000000");

            string[] lonArr = lon.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            this.txtLonDeg.Text = lonArr[0];

            this.txtlonMinute.Text = ((Convert.ToDouble(lon) - Convert.ToDouble(lonArr[0])) * 60).ToString("#0.000000000");

        }

        private void btnAlarmType_Click(object sender, EventArgs e)
        {

            string sn = this.txtSNNo.Text.Trim();//SN
            string snEn = this.Encryption(sn);

            latitudeDegree = this.txtLatDeg.Text.Trim();
            latitudeMinute = this.txtLatMinute.Text.Trim();
            longitudeDegree = this.txtLonDeg.Text.Trim();
            longitudeMinute = this.txtlonMinute.Text.Trim();
            alarmType = Convert.ToInt32(this.txtAlarmType.Text.Trim());
            totalMileage = Convert.ToDecimal(this.txtTotolKM.Text.Trim());
            cellId = this.txtCellId.Text.Trim();

            Hashtable ht = new Hashtable();
            ht.Add("sn", snEn);
            ht.Add("messageType", 5);

            Hashtable dataHt = new Hashtable();
            dataHt.Add("latitudeDegree", latitudeDegree);
            dataHt.Add("latitudeMinute", latitudeMinute);
            dataHt.Add("longitudeDegree", longitudeDegree);
            dataHt.Add("longitudeMinute", longitudeMinute);
            dataHt.Add("battery", battery);
            dataHt.Add("alarmType", alarmType);
            dataHt.Add("totalMileage", totalMileage);
            dataHt.Add("cellId", cellId);

            ht.Add("messageBody", dataHt);

            byte[] buffer = ProtocolUtils.encode(3, ht, 1234);

            this.sokClient.Send(buffer);
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            this.ReportDataWithFiveMinutes(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool resutl = false;
            try
            {
                satellite = Convert.ToInt32(this.txtGPS.Text.Trim());
                string sn = this.txtSNNo.Text.Trim();
                string snEn = Algorithm.Encryption(sn);
                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 6);

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", latitudeDegree);
                dataHt.Add("latitudeMinute", latitudeMinute);
                dataHt.Add("longitudeDegree", longitudeDegree);
                dataHt.Add("longitudeMinute", longitudeMinute);
                dataHt.Add("battery", battery);
                dataHt.Add("satellite", satellite);
                dataHt.Add("totalMileage", totalMileage);

                ht.Add("messageBody", dataHt);

                byte[] buffer = ProtocolUtils.encode(18, ht, 1222);
                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            {

            }

        }

        private void btnTw_Click(object sender, EventArgs e)
        {
            ReportDataWithTwentySeconds(null, null);
            //btnHistory_Click(null, null);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            bool resutl = false;
            try
            {
                List<Hashtable> htList = new List<Hashtable>();

                string sn = this.txtSNNo.Text.Trim();
                string snEn = Algorithm.Encryption(sn);
                Hashtable ht = new Hashtable();
                ht.Add("sn", snEn);
                ht.Add("messageType", 8);

                Hashtable dataHt = new Hashtable();
                dataHt.Add("latitudeDegree", Convert.ToDouble(latitudeDegree));
                dataHt.Add("latitudeMinute", Convert.ToDouble(latitudeMinute));
                dataHt.Add("longitudeDegree", Convert.ToDouble(longitudeDegree));
                dataHt.Add("longitudeMinute", Convert.ToDouble(longitudeMinute));
                dataHt.Add("at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dataHt.Add("totalMileage", totalMileage);
                htList.Add(dataHt);

                Hashtable dataHt1 = new Hashtable();
                dataHt1.Add("latitudeDegree", latitudeDegree);
                dataHt1.Add("latitudeMinute", latitudeMinute);
                dataHt1.Add("longitudeDegree", longitudeDegree);
                dataHt1.Add("longitudeMinute", longitudeMinute);
                dataHt1.Add("at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dataHt1.Add("totalMileage", totalMileage);
                htList.Add(dataHt1);

                Hashtable subHt = new Hashtable();
                subHt.Add("location", htList);

                ht.Add("messageBody", subHt);

                byte[] buffer = ProtocolUtils.encode(3, ht, 1222);

                ProtocolBean ddd = ProtocolUtils.decode(buffer);

                this.sokClient.Send(buffer);
            }
            catch (Exception ex)
            {

            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }




        private void ShowMessage()
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Snbegin_TextChanged(object sender, EventArgs e)
        {
            if (snbegin.Text != "")
            {
                this.snbeginno = int.Parse(this.snbegin.Text.Trim());
            }
            
        }

        private void Snend_TextChanged(object sender, EventArgs e)
        {
            this.snendno = int.Parse(this.snend.Text.Trim());
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.stopall = true;
            this.button2.Enabled = false;

            if (stopAllTask == null)
            {
                stopAllTask = new Task(() =>{


                    int num = des.Count();
                    try
                    {
                        while (num != 0)
                        {

                            for (int i = des.Count - 1; i >= 0; i--)
                            {
                                if (des[i].firstSendThread != null)
                                {
                                    des[i].firstSendThread.Abort();
                                }
                                if (des[i].fiveMinutesThread != null)
                                {
                                    des[i].fiveMinutesThread.Abort();
                                }
                                if (des[i].heartbeatThread != null)
                                {
                                    des[i].heartbeatThread.Abort();
                                }
                                if (des[i].twentySecondsThread != null)
                                {
                                    des[i].twentySecondsThread.Abort();
                                }
                                if (des[i].sokClient != null)
                                {
                                    des[i].sokClient.Close();
                                    des[i].sokClient = null;
                                }

                                if (des[i].fiveMinutesTask != null)
                                {
                                    if (!des[i].fiveMinutetoken.IsCancellationRequested)
                                    {
                                        des[i].fiveMinutetokenSource.Cancel();
                                        //de.fiveMinutesTask = null;
                                    }
                                }

                                if (des[i].twentySecondsTask != null)
                                {
                                    if (!des[i].twentySecondstoken.IsCancellationRequested)
                                    {
                                        des[i].twentySecondstokenSource.Cancel();
                                        //de.twentySecondsTask = null;
                                    }

                                }

                                if (des[i].heartbeatTask != null)
                                {
                                    if (!des[i].heartbeettoken.IsCancellationRequested)
                                    {
                                        des[i].heartbeettokenSource.Cancel();
                                        //de.heartbeatTask = null;
                                    }
                                }

                                if (des[i].firstSendTask != null)
                                {
                                    if (!des[i].firstSendTask.IsCanceled)
                                    {
                                        des[i].tokenSource.Cancel();
                                        //de.firstSendTask = null;
                                    }

                                }

                                try
                                {
                                    if (des[i].mqttClient != null)
                                    {
                                        if (des[i].mqttClient.IsConnected)
                                        {
                                            des[i].mqttClient.Disconnect();
                                        }

                                    }

                                    if (des[i].mqttClient2 != null)
                                    {
                                        if (des[i].mqttClient2.IsConnected)
                                        {
                                            des[i].mqttClient2.Disconnect();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                   

                                    if (des[i].mqttClient != null)
                                    {
                                        if (des[i].mqttClient.IsConnected)
                                        {
                                            try
                                            {
                                                des[i].mqttClient.Disconnect();
                                            }
                                            catch (Exception)
                                            {
                                                //txtShow.AppendText("mqtt断联失败!" + des[i].sn + "\r\n");
                                            }
                                            
                                        }

                                    }

                                    if (des[i].mqttClient2 != null)
                                    {
                                        if (des[i].mqttClient2.IsConnected)
                                        {
                                            try
                                            {
                                                des[i].mqttClient2.Disconnect();
                                            }
                                            catch (Exception)
                                            {

                                                //txtShow.AppendText("mqtt断联失败!" + des[i].sn + "\r\n");
                                            }
                                            
                                        }
                                    }
                                    //throw;
                                }

                                

                                if (des[i].fiveMinutesTask != null)
                                {
                                    if (des[i].fiveMinutesTask.IsCompleted && des[i].heartbeatTask.IsCompleted)
                                    {
                                        des[i].topics = null;
                                        this.txtShow3.AppendText(string.Format("!!!{0}  mqtttask:{1} stop sucess! \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), des[i].sn));
                                        des.Remove(des[i]);
                                        num--;
                                    }
                                    else if (des[i].twentySecondsTask != null)
                                    {
                                        if (des[i].twentySecondsTask.IsCompleted && des[i].heartbeatTask.IsCompleted)
                                        {
                                            des[i].topics = null;
                                            this.txtShow3.AppendText(string.Format("!!!{0}  mqtttask:{1} stop sucess! \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), des[i].sn));
                                            des.Remove(des[i]);
                                            num--;
                                        }
                                    }
                                }
                                else if (des[i].twentySecondsTask == null && des[i].heartbeatTask == null)
                                {
                                    des[i].topics = null;
                                    des.Remove(des[i]);
                                    num--;
                                }



                                num = des.Count();

                                this.taskStatLabel.Text = "剩余" + num + "任务正在关闭";
                                this.button2.Text = "剩余" + num;
                                if (num == 0)
                                {
                                    this.taskStatLabel.Text = "所有任务已经关闭";
                                    this.button2.Text = "3.停止所有任务";
                                }


                            }
                            Thread.Sleep(200);

                            /*
                            foreach (DeviceSocket de in des)
                            {
                                if (de.fiveMinutesTask.IsCompleted && de.heartbeatTask.IsCompleted)
                                {
                                    this.txtShow.AppendText(string.Format("!!!{0}  mqtttask:{1} stop sucess! \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), de.sn));
                                    //des.Remove(de);
                                    num--;
                                }
                                else if (de.twentySecondsTask != null)
                                {
                                    if (de.twentySecondsTask.IsCompleted && de.heartbeatTask.IsCompleted)
                                    {
                                        this.txtShow.AppendText(string.Format("!!!{0}  mqtttask:{1} stop sucess! \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), de.sn));
                                        //des.Remove(de);
                                        num--;
                                    }
                                }

                                this.taskStatLabel.Text = "剩余" + num + "任务正在关闭";
                                this.button2.Text = "剩余" + num;
                            }
                            */

                        }

                        if (startAllTask != null)
                        {
                            while (!startAllTask.IsCompleted)
                            {

                            }
                        }

                        if (des!=null)
                        {
                            des.Clear();
                        }
                       
                        if (mqttht !=null)
                        {
                            mqttht.Clear();
                        }

                        if (mqttht2 != null)
                        {
                            mqttht2.Clear();
                        }

                        if (sn2mqttclient != null)
                        {
                            sn2mqttclient.Clear();
                        }

                        this.button2.Enabled = true;
                        this.button2.Text = "3.停止所有任务";
                        this.mqttbtn.Enabled = true;

                        startAllTask = null;

                        stopAllTask = null;

                        this.progressBar1.Visible = false;

                    }
                    catch (Exception ex)
                    {
                        this.txtShow.AppendText("error:" + ex.Message);
                    }
                    

                },stopToken);

                stopAllTask.Start();
            }


        }

        

        private void mqtt1publish(object sender, MqttMsgPublishedEventArgs e)
        {
            MqttClient client = (MqttClient)sender;
            DeviceSocket de = (DeviceSocket)mqttht[client];
            //byte[] buffer = new byte[1024];

            //buffer = e.
           // ProtocolBean basesP = ProtocolUtils.decode(buffer);
            //this.txtShow.AppendText(string.Format("\r\n<--Topic To Mqtt1:{0}\r\nsn:{1}\r\nCmId:{2}Payload:{3}\r\n---------", topic_p, de.sn, basesP.CmId, basesP.Payload));
        }

        private void mqtt2publish(object sender, MqttMsgPublishedEventArgs e)
        {

        }

        private void mqtt1connent(object sender, MqttMsgConnectEventArgs e)
        {
            //started_tasks += 0.5;
        }

        private void mqtt2connent(object sender, MqttMsgConnectEventArgs e)
        {
            //started_tasks += 0.5;
        }

        private void mqtt1close(object sender, EventArgs e)
        {
            int _TryCount = 0;
            MqttClient client = (MqttClient)sender;
            DeviceSocket de = (DeviceSocket)mqttht[client];

            if (de == null) return;

            if (de.mqttClient.IsConnected) return;

            while (de.mqttClient == null || !de.mqttClient.IsConnected)
            {
                if (stopall) return;

                if (de.mqttClient == null)
                {
                    de.mqttClient = new MqttClient(de.brokerHostName, de.brokerPort, false, null, null, MqttSslProtocols.None);
                    //消息接受，并处理
                    de.mqttClient.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(messageReceive);
                    de.mqttClient.ConnectionClosed += new MqttClient.ConnectionClosedEventHandler(mqtt1close);
                    de.mqttClient.MqttMsgPublished += new MqttClient.MqttMsgPublishedEventHandler(mqtt1publish);
                    Thread.Sleep(3000);
                    continue;
                }

                try
                {
                    _TryCount++;
                    de.mqttClient.Connect(de.clientId, de.username, de.password, true, 0);
                }
                catch (Exception ce)
                {
                    txtShow3.AppendText("mqtt1重连接"+_TryCount+"次失败!" + de.sn + " "+ ce.Message + "\r\n");
                    //started_tasks -= 1;
                    taskStatLabel.Text = "已连接Mqtt1:" + started_tasks.ToString() + "/" + total_tasks.ToString() + ";  Mqtt2:" + started_tasks2.ToString() + "/" + total_tasks.ToString();
                }

                // 如果还没连接不符合结束条件则睡2秒
                if (!de.mqttClient.IsConnected)
                {
                    Thread.Sleep(2000);
                }
            }


           


            if (de.mqttClient.IsConnected)
            {
                txtShow3.AppendText("mqtt1连接重连成功!" + de.sn + "\r\n");
                de.mqttClient.Subscribe(de.topics, de.qosLevels);
            }
 
        }
        private void mqtt2close(object sender, EventArgs e)
        {
            
            MqttClient client = (MqttClient)sender;
            DeviceSocket de = (DeviceSocket)mqttht2[client];

            if (de == null) return;

            if (de.mqttClient2.IsConnected) return;


            int _TryCount = 0;
            while (de.mqttClient2 == null || !de.mqttClient2.IsConnected)
            {
                if (stopall) return;

                if (de.mqttClient2 == null)
                {
                    de.mqttClient2 = new MqttClient(de.brokerHostName2, de.brokerPort2, false, null, null, MqttSslProtocols.None);
                    //消息接受，并处理
                    de.mqttClient2.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(messageReceive);
                    de.mqttClient2.ConnectionClosed += new MqttClient.ConnectionClosedEventHandler(mqtt1close);
                    de.mqttClient2.MqttMsgPublished += new MqttClient.MqttMsgPublishedEventHandler(mqtt1publish);
                    Thread.Sleep(3000);
                    continue;
                }

                try
                {
                    _TryCount++;
                    de.mqttClient2.Connect(de.clientId2, de.username2, de.password2, true, 0);
                }
                catch (Exception ce)
                {
                    txtShow3.AppendText("mqtt2重连接" + _TryCount + "次失败!" + de.sn + " " + ce.Message + "\r\n");
                    //started_tasks -= 1;
                    taskStatLabel.Text = "已连接Mqtt1:" + started_tasks.ToString() + "/" + total_tasks.ToString() + ";  Mqtt2:" + started_tasks2.ToString() + "/" + total_tasks.ToString();
                }

                // 如果还没连接不符合结束条件则睡2秒
                if (!de.mqttClient2.IsConnected)
                {
                    Thread.Sleep(2000);
                }
            }

            if (de.mqttClient2.IsConnected)
            {
                txtShow3.AppendText("mqtt2连接重连成功!" + de.sn + "\r\n");
                //de.mqttClient2.Subscribe(de.topics, de.qosLevels);
            }
        }

        private void messageReceive(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = "\r\n\r\n-->Topic:" + e.Topic + "\r\nMessage:" + System.Text.Encoding.Default.GetString(e.Message);
            txtShow2.AppendText(msg + "\r\n");
            MqttClient client = (MqttClient)sender;
            DeviceSocket de = (DeviceSocket)mqttht[client];
            try
            {
                //Thread fiveMinutesTimer = null;
                //Thread twentySecondsTimer = null;
                //Thread heartbeatTimer = null;

                if (this.isRec && !stopall)
                {
                    //byte[] msgArr = new byte[1024];//接收到的消息的缓冲区
                    //int length = 0;
                    //接收服务端发送来的消息数据
                    //length = de.sokClient.Receive(msgArr);//Receive会阻断线程
                    //if (length == 0)
                    //{
                    //    continue;
                    //}
                    string message = string.Empty;
                    ProtocolBean protocol = ProtocolUtils.decode(e.Message);
                    if (protocol == null)
                    {
                        this.txtShow.AppendText("ProtocolBean is null");
                        return;
                    }
                    this.txtShow2.AppendText(string.Format("\r\nCmId:{0},Payload:{1}\r\n", protocol.CmId, protocol.Payload));
                    switch (protocol.CmId)
                    {
                        case 1://车辆向服务器发出鉴权请求
                            break;
                        case 2://车辆接收到服务器关于鉴权的应答
                            bool isSucessed = true;
                            //AnsisleProtocol.ConnectionResponse(protocol, ref message, ref isSucessed);//mqtt服务器完成鉴权工作
                            if (isSucessed)
                            {
                                if (de.fiveMinutesTask == null)
                                {
                                    de.fiveMinutesTask = new Task(() =>
                                    {
                                        ReportDataWithFiveMinutes(de);
                                    }, de.fiveMinutetoken);
                                    de.fiveMinutesTask.Start();
                                }

                                if (de.heartbeatTask == null)
                                {
                                    de.heartbeatTask = new Task(() =>
                                    {
                                        reSend(de);
                                    }, de.heartbeettoken);

                                    de.heartbeatTask.Start();

                                }
                                //fiveMinutesTimer.Stop();
                                //fiveMinutesTimer.Start();
                                //heartbeatTimer.Start();
                                //ReportDataWithFiveMinutes(null, null);
                            }
                            break;
                        case 3://车辆向服务器发送数据 
                            break;
                        case 4://车辆接收到服务器关于上报数据的应答
                            //AnsisleProtocol.ReportResponse(protocol, ref message);
                            break;
                        case 5://车辆收到服务器的命令请求
                            byte[] buffer = new byte[1024];
                            int cmdid = 0;
                            AnsisleProtocol.CmdRequest(protocol, Convert.ToDouble(this.txtLatDeg.Text.Trim()),
                                Convert.ToDouble(this.txtLatMinute.Text.Trim()), Convert.ToDouble(this.txtLonDeg.Text.Trim()),
                                Convert.ToDouble(this.txtlonMinute.Text.Trim()), Convert.ToDouble(this.txtTotolKM.Text.Trim()),
                                Convert.ToInt32(this.txtGPS.Text.Trim()), Convert.ToInt32(this.txtBattery.Text.Trim()), ref message, ref buffer, ref cmdid);
                            //int ss = de.sokClient.Send(buffer);
                            string oldstr = "req/" + de.sn;
                            string topic_p = e.Topic.Replace(oldstr, "resp");
                            int ss = de.mqttClient.Publish(topic_p, buffer);
                            //de.mqttClient2.Publish(topic_p, buffer);
                            if (ss <= 0)
                            {
                                this.txtShow.AppendText("ss<=0 \r\n");
                            }
                            ProtocolBean basesP = ProtocolUtils.decode(buffer);
                            if (basesP == null)
                            {
                                this.txtShow.AppendText("basesP is null\r\n");
                            }
                            this.txtShow2.AppendText(string.Format("\r\n<--Topic To Mqtt1:{0}\r\nCmId:{1}  sn:{2}\r\nPayload:{3}\r\n---------", topic_p, basesP.CmId, de.sn, basesP.Payload));

                            ss = de.mqttClient2.Publish(topic_p, buffer);
                            if (ss <= 0)
                            {
                                this.txtShow.AppendText("ss<=0 \r\n");
                            }
                            ProtocolBean basesP2 = ProtocolUtils.decode(buffer);
                            if (basesP == null)
                            {
                                this.txtShow.AppendText("basesP2 is null\r\n");
                            }
                            this.txtShow2.AppendText(string.Format("\r\n<--Topic To Mqtt2:{0}\r\nCmId:{1}  sn:{2}\r\nPayload:{3}\r\n---------", topic_p, basesP2.CmId, de.sn, basesP2.Payload));

                            if (cmdid == 1)
                            {
                                if (de.twentySecondsTask == null)
                                {
                                    de.twentySecondstoken = de.twentySecondstokenSource.Token;
                                    de.twentySecondsTask = new Task(() =>
                                    {
                                        ReportDataWithTwentySeconds(de);
                                    }, de.twentySecondstoken);
                                    de.twentySecondsTask.Start();
                                }
                                else if (!de.twentySecondstoken.IsCancellationRequested)
                                {
                                    de.twentySecondsResetEvent.Set();
                                }

                                if (de.fiveMinutesTask != null)
                                {
                                    if (!de.fiveMinutetoken.IsCancellationRequested)
                                    {
                                        de.fiveMinuteResetEvent.Reset();
                                    }
                                    
                                }
                         
                                //fiveMinutesTimer.Stop();
                                //twentySecondsTimer.Stop();
                                //twentySecondsTimer.Start();
                                //ReportDataWithTwentySeconds(null, null);
                            }
                            else if (cmdid == 2)
                            {
                                if (de.fiveMinutesTask == null)
                                {
                                    de.fiveMinutetoken = de.fiveMinutetokenSource.Token;
                                    de.fiveMinutesTask = new Task(() =>
                                    {
                                        ReportDataWithFiveMinutes(de);
                                    }, de.fiveMinutetoken);
                                    de.fiveMinutesTask.Start();
                                }
                                else if (!de.fiveMinutetoken.IsCancellationRequested)
                                {
                                    de.fiveMinuteResetEvent.Set();
                                }

                                if (de.twentySecondsTask != null)
                                {
                                    if (!de.twentySecondstoken.IsCancellationRequested)
                                    {
                                        de.twentySecondsResetEvent.Reset();
                                    }
                                }

                                //twentySecondsTimer.Stop();
                                //fiveMinutesTimer.Stop();
                                //fiveMinutesTimer.Start();
                                //ReportDataWithFiveMinutes(null, null);
                            }

                            break;
                        case 6://
                            break;
                        case 7:
                            break;
                        case 8:
                            break;
                        case 9:
                            break;
                        case 10:
                            break;
                        case 11:
                            break;
                        case 12:
                            break;
                        case 13:
                            break;
                        case 14:
                            break;
                        case 15:
                            break;
                    }
                    txtShow2.AppendText(string.Format("\r\n---- {0}  {1} ----\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), message));
                }
            }
            catch (Exception ex)
            {
                this.taskStatLabel.Text = "有异常发生";
                txtShow3.AppendText(string.Format("发生未知异常：Error：{0}\r\n", ex.Message));
                de.sokClient = null;
            }


        }
        private bool cafileValidCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            string msg = "X509 链状态:";
            foreach (X509ChainStatus status in chain.ChainStatus)
            {
                msg += status.StatusInformation + "\n";
            }
            msg += "SSL策略问题：" + (int)sslPolicyErrors;

            Console.WriteLine(msg);

            if (sslPolicyErrors != SslPolicyErrors.None)
                return false;
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mqttbtn_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(this.txtLatDeg.Text) || string.IsNullOrWhiteSpace(this.txtLatMinute.Text)
              || string.IsNullOrWhiteSpace(this.txtLonDeg.Text) || string.IsNullOrWhiteSpace(this.txtlonMinute.Text))
            {
                MessageBox.Show("请计算坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LoadAllData();

            this.progressBar1.Visible = true;

            if(mqttht == null)
            {
                mqttht = new Hashtable();
            }

            if (mqttht2 == null)
            {
                mqttht2 = new Hashtable();
            }

            if (sn2mqttclient == null)
            {
                sn2mqttclient = new Hashtable();
            }

            if (mqttht != null && mqttht2 != null && sn2mqttclient != null)
            {


                this.stopall = false;

                //string ipAddress = this.cmbIPAddress.SelectedValue.ToString().Trim();
                //string port = this.cmbPort.SelectedValue.ToString().Trim();


                //IPHostEntry host = Dns.GetHostEntry(ipAddress);
                //创建 ip对象
                //IPAddress address = host.AddressList[0];
                //创建网络节点对象 包含 ip和port
                //IPEndPoint endpoint = new IPEndPoint(address, int.Parse(port));

                this.mqttbtn.Enabled = false;
                this.button2.Enabled = true;
                stopAllTask = null;
                string username_key = "Bm89=%xz9#";
                string password_key = "!19xyF20w$";

                if (startAllTask == null)
                {
                    startAllTask = new Task(() =>
                    {
                        //begin added by pengshirui for one or more socket by DeviceSocket
                        string mm = "mimacx00000";
                        string topic_in = "/+/+/+/req/";
                        string[] topics = null; ;
                        byte[] qos_levels = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };

                        total_tasks = snendno - snbeginno + 1;
                        started_tasks = 0;
                        started_tasks2 = 0;

                        for (int i = snbeginno; i <= snendno; i++)
                        {
                            topics = new string[1];
                            topic_in = "/+/+/+/req/";
                            topic_in += mm + i.ToString();
                            topics[0] = topic_in;
                            DeviceSocket de = new DeviceSocket(topics, qos_levels);
                            de.sn = mm + i.ToString();// "mimacx0000010002";
                            de.bt = "0000" + i.ToString();
                            de.clientId = de.sn;
                            

                            de.brokerHostName = mqttBroker1;
                            de.brokerHostName2 = mqttBroker2;
                            de.brokerPort = imqttPort1;
                            de.brokerPort2 = imqttPort2;

                            de.firstSendTask = new Task(() =>
                            {

                                de.SetUserName(username_key);
                                de.SetPassord(password_key);

                                int _TryCount = 0;
                                while (de.mqttClient == null || !de.mqttClient.IsConnected)
                                {
                                    if (de.mqttClient == null)
                                    {
                                        de.mqttClient = new MqttClient(de.brokerHostName, de.brokerPort, false, null, null, MqttSslProtocols.None);
                                        //消息接受，并处理
                                        de.mqttClient.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(messageReceive);
                                        de.mqttClient.ConnectionClosed += new MqttClient.ConnectionClosedEventHandler(mqtt1close);
                                        de.mqttClient.MqttMsgPublished += new MqttClient.MqttMsgPublishedEventHandler(mqtt1publish);
                                        
                                        mqttht.Add(de.mqttClient, de);
                                        sn2mqttclient.Add(de.sn, de);

                                        //Thread.Sleep(1000);
                                        continue;
                                    }

                                    if (stopall) break;

                                    try
                                    {
                                        _TryCount++;
                                        de.mqttClient.Connect(de.clientId, de.username, de.password, true, 0);
                                    }
                                    catch (Exception ce)
                                    {
                                        txtShow3.AppendText("mqtt1重连接" + _TryCount + "次失败!" + de.sn + " " + ce.Message + "\r\n");
                                        //started_tasks -= 1;
                                        taskStatLabel.Text = "任务:" + des.Count().ToString() + "/" + total_tasks.ToString() + ";  Mqtt1:" + started_tasks.ToString() + "/" + total_tasks.ToString() + ";  Mqtt2:" + started_tasks2.ToString() + "/" + total_tasks.ToString();
                                    }

                                    // 如果还没连接不符合结束条件则睡2秒
                                    if (!de.mqttClient.IsConnected)
                                    {
                                        Thread.Sleep(2000);
                                    }
                                }

                                
                                if (de.mqttClient.IsConnected)
                                {
                                    de.mqttClient.Subscribe(de.topics, de.qosLevels);
                                }

                                started_tasks += 1;


                                int _TryCount2 = 0;
                                while (de.mqttClient2 == null || !de.mqttClient2.IsConnected)
                                {
                                    if (de.mqttClient2 == null)
                                    {
                                        de.mqttClient2 = new MqttClient(de.brokerHostName2, de.brokerPort2, false, null, null, MqttSslProtocols.None);
                                        //消息接受，并处理
                                        de.mqttClient2.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(messageReceive);
                                        de.mqttClient2.ConnectionClosed += new MqttClient.ConnectionClosedEventHandler(mqtt1close);
                                        de.mqttClient2.MqttMsgPublished += new MqttClient.MqttMsgPublishedEventHandler(mqtt1publish);

                                        de.clientId2 = de.sn;
                                        de.username2 = de.username;
                                        de.password2 = de.password;

                                        mqttht2.Add(de.mqttClient2, de);

                                        //Thread.Sleep(1000);
                                        continue;
                                    }

                                    if (stopall) break;

                                    try
                                    {
                                        _TryCount2++;
                                        de.mqttClient2.Connect(de.clientId2, de.username2, de.password2, true, 0);
                                    }
                                    catch (Exception ce)
                                    {
                                        txtShow3.AppendText("mqtt2重连接" + _TryCount2 + "次失败!" + de.sn + " " + ce.Message + "\r\n");
                                        //started_tasks -= 1;
                                        taskStatLabel.Text = "任务:" + des.Count().ToString() + "/" + total_tasks.ToString() + ";  Mqtt1:" + started_tasks.ToString() + "/" + total_tasks.ToString() + ";  Mqtt2:" + started_tasks2.ToString() + "/" + total_tasks.ToString();
                                    }

                                    // 如果还没连接不符合结束条件则睡2秒
                                    if (!de.mqttClient2.IsConnected)
                                    {
                                        Thread.Sleep(2000);
                                    }
                                }

                                
                                started_tasks2 += 1;
                                taskStatLabel.Text = "任务:" + des.Count().ToString() + "/" + total_tasks.ToString() + ";  Mqtt1:" + started_tasks.ToString() + "/" + total_tasks.ToString() + ";  Mqtt2:" + started_tasks2.ToString() + "/" + total_tasks.ToString();


                                this.txtShow3.AppendText(string.Format("###{0}  mqtttask:{1} start sucess! \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ffff"), de.sn));

                                

                                #region

                                //开始启动上报的线程

                                bool isSucessed = true;
                                //AnsisleProtocol.ConnectionResponse(protocol, ref message, ref isSucessed);//mqtt服务器完成鉴权工作
                                if (isSucessed)
                                {
                                    if (de.fiveMinutesTask == null)
                                    {
                                        de.fiveMinutesTask = new Task(() =>
                                        {
                                            ReportDataWithFiveMinutes(de);
                                        }, de.fiveMinutetoken);
                                        de.fiveMinutesTask.Start();
                                    }

                                    if (de.heartbeatTask == null)
                                    {
                                        de.heartbeatTask = new Task(() =>
                                        {
                                            reSend(de);
                                        }, de.heartbeettoken);

                                        de.heartbeatTask.Start();

                                    }
                                    //fiveMinutesTimer.Stop();
                                    //fiveMinutesTimer.Start();
                                    //heartbeatTimer.Start();
                                    //ReportDataWithFiveMinutes(null, null);
                                }

                                    #endregion
                                
                            }, de.token);

                            de.firstSendTask.Start();

                            taskStatLabel.Text = "任务:"+ des.Count().ToString() +"/" + total_tasks.ToString()+ ";  Mqtt1:" + started_tasks.ToString() + "/" + total_tasks.ToString() + ";  Mqtt2:" + started_tasks2.ToString() + "/" + total_tasks.ToString();
                            des.Add(de);
                        }
                    }, startToken);

                    startAllTask.Start();

                    txtShow.AppendText("------------------------启动-------------------------------\r\n");
                }
            }
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            mqttBroker1 = this.mqttTextBox1.Text.Trim();
        }


        private void mqttTextBox2_TextChanged(object sender, EventArgs e)
        {
            mqttBroker2 = this.mqttTextBox2.Text.Trim();
        }

        private void mqttPort1_TextChanged(object sender, EventArgs e)
        {
            imqttPort1 = int.Parse(this.mqttPort1.Text.Trim());
        }

        private void mqttPort2_TextChanged(object sender, EventArgs e)
        {
            imqttPort2 = int.Parse(this.mqttPort2.Text.Trim());
        }

        private void snbegin_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限制只能输入数字，Backspace键

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')

            {

                e.Handled = true;  //非以上键则禁止输入

            }

            //if (e.KeyChar == '.' && txtDownPrice.Text.Trim() == "") e.Handled = true; //禁止第一个字符就输入小数点
            //
            //if (e.KeyChar == '.' && txtDownPrice.Text.Contains(".")) e.Handled = true; //禁止输入多个小数点

         }

        private void snend_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限制只能输入数字，Backspace键

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')

            {

                e.Handled = true;  //非以上键则禁止输入

            }
        }

        private void mqttPort1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限制只能输入数字，Backspace键

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')

            {

                e.Handled = true;  //非以上键则禁止输入

            }
        }

        private void mqttPort2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限制只能输入数字，Backspace键

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')

            {

                e.Handled = true;  //非以上键则禁止输入

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Button2_Click(sender,e);
        }

        private void cmbIPAddress_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class Md5Helper
    {
        public static string Md5(string value)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(value)) return result;
            using (var md5 = MD5.Create())
            {
                result = GetMd5Hash(md5, value);
            }
            return result;
        }


        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            var hashOfInput = GetMd5Hash(md5Hash, input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }

    public class DeviceSocket : Object
    {
        public DeviceSocket(string[] topics_in, byte[] qosLevels_in)
        {
            des = null;
            sokClient = null;
            mqttClient = null;
            mqttClient2 = null;
            dataHt = null;
            sn = "";
            bt = "";

            topics = topics_in;
            qosLevels = qosLevels_in;

            firstSendThread = null;
            fiveMinutesThread = null;
            twentySecondsThread = null;
            heartbeatThread = null;

            firstSendTask = null;
            fiveMinutesTask = null;
            twentySecondsTask = null;
            heartbeatTask = null;

            brokerHostName = "ipd360.com";
             brokerPort = 1883;
             clientId = "200008888";
             username = "admin";
             password = "pass";

            brokerHostName2 = "ipd360.com";
            brokerPort2 = 1885;
            clientId2 = "200008888";
            username2 = "admin";
            password2 = "pass";

            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            resetEvent = new ManualResetEvent(true);

            fiveMinutetokenSource = new CancellationTokenSource();
            fiveMinutetoken = fiveMinutetokenSource.Token;
            fiveMinuteResetEvent = new ManualResetEvent(true);

            twentySecondstokenSource = new CancellationTokenSource();
            twentySecondstoken = twentySecondstokenSource.Token;
            twentySecondsResetEvent = new ManualResetEvent(true);

            heartbeettokenSource = new CancellationTokenSource();
            heartbeettoken = heartbeettokenSource.Token;
            heartbeetResetEvent = new ManualResetEvent(true);
        }

        public string SetUserName(string key)//Bm89=%xz9#
        {
            string src_S = sn.Replace("mimacx","s") + key;
            string src_U = Md5Helper.Md5(src_S);
            username = src_U.Substring(3, 7) + sn.Substring(sn.Length - 1, 1) + src_U.Substring(20,8); 
            return username;
        }

        public string SetPassord(string key)//!19xyF20w$
        {
            string src_S = sn.Replace("mimacx", "s") + key;
            string src_P = Md5Helper.Md5(src_S);
            password = src_P.Substring(3, 7) + sn.Substring(sn.Length - 1, 1) + src_P.Substring(20, 8);
            return password;
        }

        public string brokerHostName;
        public int brokerPort;
        public string clientId;
        public string username;
        public string password;

        public string[] topics;
        public byte[] qosLevels;
        public MqttClient mqttClient;

        public MqttClient mqttClient2;
        public string brokerHostName2;
        public int brokerPort2;
        public string clientId2;
        public string username2;
        public string password2;


        public object des;
        public Socket sokClient;
        
        public Hashtable dataHt;
        public string sn;
        public string bt;

        public Thread firstSendThread;
        public Thread fiveMinutesThread;
        public Thread twentySecondsThread;
        public Thread heartbeatThread;


        public Task firstSendTask;
        public Task fiveMinutesTask;
        public Task twentySecondsTask;
        public Task heartbeatTask;

        

        public CancellationTokenSource tokenSource;
        public CancellationToken token;
        public ManualResetEvent resetEvent;

        public CancellationTokenSource fiveMinutetokenSource;
        public CancellationToken fiveMinutetoken;
        public ManualResetEvent fiveMinuteResetEvent;

        public CancellationTokenSource twentySecondstokenSource;
        public CancellationToken twentySecondstoken;
        public ManualResetEvent twentySecondsResetEvent;

        public CancellationTokenSource heartbeettokenSource;
        public CancellationToken heartbeettoken;
        public ManualResetEvent heartbeetResetEvent;
    }
}
