using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace serial
{
    public partial class MainWindow : Form
    {
        //数据处理显示变量
        private float _factMeasureValue;
        private int _factWeightMax;
        private int _factWeightZero;
        private float _zeroPoint;

        //重量中间变量
        private float _weightMcu;
        private float[] _filterData = new float[16];

        //发送模块变量
        private byte _addMcu;
        private const byte STX = 0x5f;
        private const byte ETX = 0xaf;

        //发送标志和发送次数（用来测量串口是否在线）
        private int _sendSerialTimes;
        private float _sendOnlineWeightValue;
        //获得串口
        private string[] _comStr;
        private int _comCount;
        //标志区
        //数据正常接收标志
        private bool _revFinishFlag;
        private bool _revCorrectFlag;
        //标定所需的标志
        private bool _standardFinishFlag;
        private bool _emptyMeasureFlag;
        private bool _fullMeasureFlag;
        private bool _updataFinishFlag;
        //       private bool _dataBaseCheckFlag;
        //测量标志位
        private bool _measureFlag;
        //秤选定标志和在线标志
        private bool[] _weightCheckFlag;
        private bool[] _weightOnlineFlag;
        private bool[] _firstRevFlag;
        private bool _onlineModelChoose;
        //开网络传输标志
        private bool _openSendOnlineFlag;
        private bool _beConnectedFlag;
        private bool _openNetWorkFlag;

        //标定模块
        private float _weightMax;
        private float _weightZero;
        private float _weightRate;
        private float _weightCrosspoint;


        //网络通信模块
        private IPEndPoint _ipp;
        private Socket _socket;
        private Thread _newListenThread;
        private int _port;

        //定时器
        private readonly Timer _dataDealTimer = new Timer();
        private readonly Timer _reSendTimer = new Timer();
        private readonly Timer _checkOnlineTimer = new Timer();
        private readonly Timer _sendOnlineTimer = new Timer();
        private readonly Timer _startSysremTimer = new Timer();
        private readonly Timer _checkProtExistTimer = new Timer();


        //定时器间隔设定
        private const int DelayTimeSet = 500;
        private const int DataDealTimeSet = 10;
        private const int CheckOnlineTimerSet = 100;
        private const int SendOnlineTimerSet = 100;
        private const int StartSystemTimerSet = 10000;
        private const int CheckProtExistTimerSet = 10;



        public MainWindow()
        {
            InitializeComponent();
            InitFlag();
            InitWeightValue();
            InitNetConfig();

            _startSysremTimer.Interval = StartSystemTimerSet;
            _startSysremTimer.Elapsed += StartSysremTimerTick;

            _checkProtExistTimer.Interval = CheckProtExistTimerSet;
            _checkProtExistTimer.Elapsed += CheckProtExistTimerTick;

        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*-----------------------------------初始化区-------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        private void StartSysremTimerTick(object sender, EventArgs e)
        {
            _startSysremTimer.Enabled = false;

            // serMCU.PortName = _comStr[0];
            this.cbSerial.TextChanged -= new System.EventHandler(this.cbSerial_TextChanged);
            cbSerial.Text = _comStr[0];
            this.cbSerial.TextChanged += new System.EventHandler(this.cbSerial_TextChanged);
            weightNo1_Button.PerformClick();
            openSerial.PerformClick();
            OpenInternt.PerformClick();
        }

        private void CheckProtExistTimerTick(object sender, EventArgs e)
        {
            _checkProtExistTimer.Enabled = false;
            if (SerialPort.GetPortNames().Length == _comStr.Length)
            {
        //        _checkProtExistTimer.Enabled = true;
            }
            else
            {
                WriteLogFile("COM口丢失，软件重启");
                Application.Restart();
            }
        }

        private void InitFlag()
        {
            _revCorrectFlag = false;
            _standardFinishFlag = false;
            _revFinishFlag = false;
            _measureFlag = false;
            _updataFinishFlag = false;
            _onlineModelChoose = false;
            _weightOnlineFlag = new bool[16];
            _weightCheckFlag = new bool[16];
            _firstRevFlag = new bool[16];
            _sendOnlineTimer.Enabled = false;
            _openSendOnlineFlag = false;
            _openNetWorkFlag = false;
        }

        private void InitWeightValue()
        {
            _factWeightMax = 0;
            _factWeightZero = 0;
            _weightMax = 0;
            _weightZero = 0;
            _sendSerialTimes = 0; //检测秤是否上线
        }

        private void InitNetConfig()
        {
            _addMcu = 0;
            _sendOnlinetimes = 0;
            _comCount = 0;
            if (serMCU.IsOpen)
            {
                serMCU.Close();
                serMCU.PortName = _comStr[_comCount];
                serMCU.Open();
                cbSerial.Text = _comStr[_comCount];
            }

            _comSendTimes = 1;
            _sendOnlineMessage = null;

        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*-----------------------------------窗体加载-------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        private void MainWindow_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            _comStr = SerialPort.GetPortNames();
            serMCU.DtrEnable = true;
            serMCU.RtsEnable = true;
            measure.Enabled = false;
            makeZero.Enabled = false;
            emptyButton.Enabled = false;
            fullButton.Enabled = false;
            saveData.Enabled = false;
            OpenInternt.Enabled = false;



            //添加串口项目
            foreach (string s in _comStr)
            {
//获取有多少个COM口
                //System.Diagnostics.Debug.WriteLine(s);
                cbSerial.Items.Add(s);
                //   CreatNewSheet(s);
                UpdateStrctureTable(s);
            }

            //数据处理定时器
            _dataDealTimer.Interval = DataDealTimeSet;
            _dataDealTimer.Elapsed += DataDealTimerTick;

            //重发定时器
            _reSendTimer.Interval = DelayTimeSet;
            _reSendTimer.Elapsed += ReSendTimerTick;

            //上线定时器
            _checkOnlineTimer.Interval = CheckOnlineTimerSet;
            _checkOnlineTimer.Elapsed += CheckOnlineTimerTick;

            //通信定时器
            _sendOnlineTimer.Interval = SendOnlineTimerSet;
            _sendOnlineTimer.Elapsed += SendOnlineTimerTick;

            _startSysremTimer.Enabled = true;
        }

        private void OpenAllButoon()
        {
            measure.Enabled = true;
            makeZero.Enabled = true;
            emptyButton.Enabled = true;
            fullButton.Enabled = true;
            saveData.Enabled = true;

            weightNo1_Button.Enabled = true;
            weightNo2_Button.Enabled = true;
            weightNo3_Button.Enabled = true;
            weightNo4_Button.Enabled = true;
            weightNo5_Button.Enabled = true;
            weightNo6_Button.Enabled = true;
            weightNo7_Button.Enabled = true;
            weightNo8_Button.Enabled = true;
            weightNo9_Button.Enabled = true;
            weightNo10_Button.Enabled = true;
            weightNo11_Button.Enabled = true;
            weightNo12_Button.Enabled = true;
            weightNo13_Button.Enabled = true;
            weightNo14_Button.Enabled = true;
            weightNo15_Button.Enabled = true;
            weightNo16_Button.Enabled = true;
        }

        private void CloseAllButton()
        {
            measure.Enabled = false;
            makeZero.Enabled = false;
            emptyButton.Enabled = false;
            fullButton.Enabled = false;
            saveData.Enabled = false;

            weightNo1_Button.Enabled = false;
            weightNo2_Button.Enabled = false;
            weightNo3_Button.Enabled = false;
            weightNo4_Button.Enabled = false;
            weightNo5_Button.Enabled = false;
            weightNo6_Button.Enabled = false;
            weightNo7_Button.Enabled = false;
            weightNo8_Button.Enabled = false;
            weightNo9_Button.Enabled = false;
            weightNo10_Button.Enabled = false;
            weightNo11_Button.Enabled = false;
            weightNo12_Button.Enabled = false;
            weightNo13_Button.Enabled = false;
            weightNo14_Button.Enabled = false;
            weightNo15_Button.Enabled = false;
            weightNo16_Button.Enabled = false;
        }

        private void cbSerial_TextChanged(object sender, EventArgs e)
        {
            if (serMCU.IsOpen)
            {
                serMCU.Close();
                serMCU.PortName = cbSerial.Text;
                serMCU.Open();
            }
            else
            {
                serMCU.PortName = cbSerial.Text;
            }

        }



/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*-----------------------------定时器处理方法-------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
        //数据更新定时器
        private byte _sendOnlinetimes;
        private string _sendOnlineMessage;
        private int _comSendTimes;
        private bool _sendOnlineFinishFlag;
        private static readonly ManualResetEvent DataUpdataFinish = new ManualResetEvent(false);
        private static readonly ManualResetEvent ReadytoSendResetEvent = new ManualResetEvent(false);
        private static readonly ManualResetEvent CloseSendOnlineEvent = new ManualResetEvent(false);

        private void SendOnlineTimerTick(object sender, EventArgs e)
        {
            WriteLogFile("--------------------------进入网络传输定时器--------------------------");
            DataUpdataFinish.Reset();
            _sendOnlineTimer.Enabled = false;
            _addMcu = _sendOnlinetimes;

            WriteLogFile(serMCU.PortName);

            cbSerial.Text = _comStr[_comCount];

            DataBaseView(serMCU.PortName, _addMcu);
            SendCommand();

            WriteLogFile("数据等待中......");
            DataUpdataFinish.WaitOne();


            if (_weightOnlineFlag[_addMcu])
            {
                DisplayData(_sendOnlineWeightValue);
                
                MessageShow((_addMcu + 1) + "号秤测量");

                WriteLogFile((_addMcu + 1) + "号秤测量");
                
                StandardValueView(true);
               
                _sendOnlineMessage += "{" + "\"sid\"" + ":" + "\"" + _comSendTimes + Convert.ToString(_addMcu, 16) +
                                      "\"" + "," + "\"weight\"" + ":" + "\"" +
                                      _sendOnlineWeightValue + "\"" + "},";

            }
            else
            {
                MessageShow((_addMcu + 1) + "号秤未上线");

                WriteLogFile((_addMcu + 1) + "号秤未上线");

                StandardValueView(false);
            }
            _sendOnlinetimes++;
            if (_sendOnlinetimes == 16)
            {

                WriteLogFile("一个分机组轮询完毕.......");
                WriteLogFile(_sendOnlineMessage);


                _sendOnlinetimes = 0;
                _comCount++;

                _comSendTimes ++;
                try
                {
                  if (_comCount == _comStr.Length)
                    {
                        WriteLogFile("所有分机组轮询完毕准备发往服务器.......");
                        if (_sendOnlineMessage != null)
                        {
                            _sendOnlineMessage = _sendOnlineMessage.TrimEnd(',');
                            _sendOnlineMessage = "[" + _sendOnlineMessage + "]";

                            WriteLogFile(_sendOnlineMessage);

                            if (_beConnectedFlag)
                            {
                                ReadytoSendResetEvent.Set();
                            }
                            else
                            {
                                _sendOnlineMessage = null;
                            }
                        }
                        _comCount = 0;
                        _comSendTimes = 1;
                    }
                }
                catch (Exception ex)
                {

                    WriteLogFile(ex.Message); 
                }

                try
                {
                  //  if (SerialPort.GetPortNames().Length == _comStr.Length)
                 //   {
                        serMCU.Close();
                        serMCU.PortName = _comStr[_comCount];
                        serMCU.Open();
                  //  }
                   /* else
                    {
                       WriteLogFile("串口丢失，软件重启");
                     Application.Restart();
                   }*/


                }
                catch (Exception ex)
                {
                    
                    WriteLogFile(ex.Message);
                }


            }

            WriteLogFile("--------------------------离开网络传输定时器--------------------------");
            if (!_sendOnlineFinishFlag)
            {
                _sendOnlineTimer.Enabled = true;
            }
            else
            {
                CloseSendOnlineEvent.Set();
            }

        }

        private void CheckOnlineTimerTick(object sender, EventArgs e)
        {
           
            _checkOnlineTimer.Enabled = false;
            _weightOnlineFlag[_addMcu] = false;
            if (_onlineModelChoose)
                DataUpdataFinish.Set();
            else
            {
                _dataDealTimer.Enabled = true;
            }
        }

        //重发方法
        private void ReSendTimerTick(object sender, EventArgs e)
        {
            WriteLogFile("-------------------------重发定时器开始-----------------------------------------");

            _reSendTimer.Enabled = false;
            var tempByte = new byte[serMCU.BytesToRead];
            serMCU.Read(tempByte, 0, serMCU.BytesToRead);
            string msg = null;
            foreach (byte t in tempByte)
            {
                msg += Convert.ToString(t, 16) + "\t";

            }
            WriteLogFile(msg);

            _revCorrectFlag = false;
            serMCU.DiscardInBuffer();
            _sendSerialTimes++;
            if (_sendSerialTimes < 4)
            {
                SendCommand();
            }
            else
            {
                _weightOnlineFlag[_addMcu] = false;
                if (_sendSerialTimes == 4)
                {
                    try
                    {
                        serMCU.Close();
                        serMCU.PortName = _comStr[_comCount];
                        serMCU.Open();
                        cbSerial.Text = _comStr[_comCount];
                        SendCommand();
                    }
                    catch (Exception ex)
                    {
                        
                        WriteLogFile(ex.Message);
                    }

                }
                else
                {
                    _sendSerialTimes = 0;
                    if (_onlineModelChoose)
                        DataUpdataFinish.Set();
                    else
                    {
                        _dataDealTimer.Enabled = true;
                    }
                }
            }
            WriteLogFile("_sendTimes=" + _sendSerialTimes);
            if (_sendSerialTimes == 0)
                WriteLogFile("-------------------------重发定时器结束--------------------------");

        }

        //数据刷新
        private void DataDealTimerTick(object sender, EventArgs e)
        {
            WriteLogFile("--------------------------进入数据处理定时器--------------------------");
            _dataDealTimer.Enabled = false;
            if (_weightOnlineFlag[_addMcu])
            {
                if (_standardFinishFlag)
                {
                    DataBaseView(serMCU.PortName, _addMcu);
                }
                if (!_revFinishFlag) return;
                if (!_measureFlag)
                {
                    if (_emptyMeasureFlag && !_standardFinishFlag)
                    {
                        _weightZero = _weightMcu;
                        measureEmpty.Text = _weightZero.ToString();
                    }
                    else if (_fullMeasureFlag && !_standardFinishFlag)
                    {
                        _weightMax = _weightMcu;
                        measureFull.Text = _weightMax.ToString();
                    }
                }
                else
                {
                    MessageShow("测量");
                   DisplayData(_sendOnlineWeightValue);
                }
            }
            else
            {
                revText.Text = "00.00";
                measureEmpty.Clear();
                measureFull.Clear();
                standardEmpty.Clear();
                standardFull.Clear();
                MessageShow((_addMcu + 1) + "号秤未上线");
            }
         WriteLogFile("--------------------------离开数据处理定时器--------------------------");

        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*---------------------------串口接收缓冲区---------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
        private byte _byteTemp;
        private float _sendOnlineWeightTemp;

        private void RevSbuf(object sender, SerialDataReceivedEventArgs e)
        {
            lock (e)
            {
                try
                {
                    _checkOnlineTimer.Enabled = false;
                    var byteRead = new byte[23]; //接受的字符数
                    var byteReadTemp = new byte[22];
                    if (!_revCorrectFlag)
                    {
                        WriteLogFile("--------------------------第一次数据接收缓冲--------------------------");
                        _byteTemp = (byte) serMCU.ReadByte();
                        _reSendTimer.Enabled = true;
                    }

                    if (_byteTemp != STX) return;
                    _revCorrectFlag = true;
                    if (serMCU.BytesToRead != 22) return;

                    WriteLogFile("-------------------------接收缓冲区开始-----------------------------------------");


                    serMCU.Read(byteReadTemp, 0, 22);
                    byteRead[0] = STX;
                    for (int i = 0; i < 22; i++)
                    {
                        byteRead[i + 1] = byteReadTemp[i];
                    }

                    if (_addMcu != byteRead[1] || byteRead[22] != ETX)
                    {
                        _revCorrectFlag = false;
                        return;
                    }

                    string msg = null;
                    foreach (byte t in byteRead)
                    {
                        msg += Convert.ToString(t, 16) + "\t";
                    }
                    WriteLogFile(msg);
                    WriteLogFile("\n-------------------------接收缓冲区结束-----------------------------------------\n");
                    _reSendTimer.Enabled = false;
                    _weightMcu = WeightDate(byteRead);
                    serMCU.DiscardInBuffer();
                }
                catch (System.InvalidOperationException ex)
                {
                    _reSendTimer.Enabled = false;
                    openSerial.Enabled = true;
                    _sendOnlineTimer.Enabled = false;
                }


                _sendSerialTimes = 0;
                _weightOnlineFlag[_addMcu] = true;
                _revCorrectFlag = false;

                if (_weightMcu == float.MaxValue)
                {
                    SendCommand();
                }
                else
                { 
                    WriteLogFile("--------------------------离开数据接收缓冲区--------------------------");
                    _sendOnlineWeightTemp = FinalDateDeal(_weightMcu);
                    if (Math.Abs(_sendOnlineWeightTemp - _sendOnlineWeightValue) > 0.1)
                        _sendOnlineWeightValue = _sendOnlineWeightTemp;
                    if (_onlineModelChoose)
                    {
                     WriteLogFile("--------------------------数据接收完毕--------------------------");
                        DataUpdataFinish.Set();
                    }
                    else
                    {
                        _revFinishFlag = true;

                        WriteLogFile((_addMcu + 1) + "号秤的原码" + _weightMcu);
                        _dataDealTimer.Enabled = true;
                    }
                  
                }
            }

        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*----------------------------接收数据处理函数------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
        private float _weightTemp;

        private float WeightDate(params byte[] revsbuf)
        {
            WriteLogFile("--------------------------进入数据处理函数--------------------------");
            _reSendTimer.Enabled = false;
            var dataTemp = new int[10];
            int fliterRate;
            var newDataTemp = new float[11];

            for (var i = 1; i < 11; i++)
            {
                int weightMcuTemp;
                if ((revsbuf[2*i] & 0x80) == 0x80)
                {
                    weightMcuTemp = -((~(((revsbuf[2*i] << 8) + revsbuf[2*i + 1]) - 1)) & 0xffff);
                }
                else
                {
                    weightMcuTemp = (((revsbuf[2*i]) << 8) + revsbuf[2*i + 1]);

                }
                dataTemp[i - 1] = weightMcuTemp;
            }
            if (!_firstRevFlag[_addMcu])
            {
                WriteLogFile("--------------------------初次滤波--------------------------");
                _firstRevFlag[_addMcu] = true;
                _filterData[_addMcu] = dataTemp.Sum()/10;
                return float.MaxValue;
            }
            if (dataTemp.Max() - dataTemp.Min() < 100)
            {
                fliterRate = 10;
            }
            else if(100<dataTemp.Max() - dataTemp.Min() &&dataTemp.Max() - dataTemp.Min()< 200)
            {
                fliterRate = 50;
            }
            else
            {
                fliterRate = 200;
            }
            WriteLogFile("滤波系数： " + fliterRate);
            newDataTemp[0] = _filterData[_addMcu];
            for (int i = 0; i < 10; i++)
            {
                if (dataTemp[i] < newDataTemp[i])
                    newDataTemp[i + 1] = newDataTemp[i] - (newDataTemp[i] - dataTemp[i])*fliterRate/256;
                else
                {
                    newDataTemp[i + 1] = newDataTemp[i] + (dataTemp[i] - newDataTemp[i])*fliterRate/256;
                }
            }
            _filterData[_addMcu] = newDataTemp[10];
            
            WriteLogFile("--------------------------离开数据处理函数--------------------------");
            return (float) Math.Round(newDataTemp.Sum()/11, 2);
        }

        private float FinalDateDeal(float weight)
        {
            float _weightTemp = 0;
            _factMeasureValue = (weight - _weightCrosspoint)/_weightRate;
            if (Math.Abs(_factMeasureValue) < 0.1)
                _factMeasureValue = _factWeightZero;
            else if (0 < (_factMeasureValue - _factWeightMax) && (_factMeasureValue - _factWeightMax) < 0.5)
                _factMeasureValue = _factWeightMax;
            _weightTemp = (float) Math.Round((_factMeasureValue - _zeroPoint), 2);
            if (_weightTemp < 0.1)
            {
                _weightTemp = _factWeightZero;
            }
            return _weightTemp;
        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------发送指令----------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        //发送格式;开始标志+地址+结束标志
        private void SendCommand()
        {
            WriteLogFile("发送命令");
            var sendMeg = new byte[3];
            sendMeg[0] = STX;
            sendMeg[1] = _addMcu;
            sendMeg[2] = ETX;
            try
            {
                serMCU.Write(sendMeg, 0, 3);
            }
            catch (Exception)
            {
                MessageShow("重新操作");
            }

            _checkOnlineTimer.Enabled = true;

        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*---------------------------------按键区-----------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        //测量键
        private void measure_Click(object sender, EventArgs e)
        {
            WriteLogFile("点击测量键");


            _dataDealTimer.Enabled = false;
            if (_weightOnlineFlag[_addMcu])
            {
                if (_standardFinishFlag)
                {
                    _revFinishFlag = false;
                    _measureFlag = true;
                    SendCommand();
                }
                else
                {
                    MessageShow((_addMcu + 1) + "号秤未标定");
                    WriteLogFile((_addMcu + 1) + "号秤未标定");
                }
            }
            else
            {
                MessageShow((_addMcu + 1) + "号秤未上线");
                WriteLogFile((_addMcu + 1) + "号秤未上线");
            }

        }

        //打开配置文件，串口，网络通信
        private void openSerial_Click(object sender, EventArgs e)
        {
            WriteLogFile("点击打开通信键");
            _startSysremTimer.Enabled = false;
            //检查配置文件是否存在
            if (File.Exists(".\\access.mdb"))
            {
                if (_weightCheckFlag[_addMcu])
                {
                    if (serMCU.IsOpen)
                    {
                        serMCU.Close();
                    }
                    else
                    {
                        try
                        {
                            serMCU.PortName = cbSerial.Text;
                            serMCU.Open();
                            NetWork();
                            MessageShow("通信接口已打开");

     //                       _checkProtExistTimer.Enabled = true;
                            openSerial.Enabled = false;
                            measure.Enabled = true;
                            makeZero.Enabled = true;
                            emptyButton.Enabled = true;
                            fullButton.Enabled = true;
                            saveData.Enabled = true;
                            OpenInternt.Enabled = true;
                            DataBaseView(serMCU.PortName, _addMcu);
                        }
                        catch (Exception ex)
                        {
                            MessageShow(ex.Message + "访问拒绝");
                            WriteLogFile(ex.Message + "访问拒绝");
                        }
                    }
                }
                else
                {
                    MessageShow("请选择秤后，再打开通信");
                    WriteLogFile("请选择秤后，再打开通信");
                }
            }

            else
            {
                MessageShow("数据库文件不存在");
                WriteLogFile("数据库文件不存在");
            }

        }

        //打开网络通信
        private void OpenInternt_Click(object sender, EventArgs e)
        {
            WriteLogFile("点击网络传输键");
            CloseSendOnlineEvent.Reset();
            if (!_openSendOnlineFlag)
            {
                _sendOnlineFinishFlag = false;
                _openSendOnlineFlag = true;
                InitNetConfig();
                OpenInternt.Text = "关闭传输";

                WriteLogFile("网络传输");

                _onlineModelChoose = true;
                CloseAllButton();
                _sendOnlineTimer.Enabled = true;
            }
            else
            {
                _sendOnlineFinishFlag = true;
                CloseSendOnlineEvent.WaitOne();
                _sendOnlineTimer.Enabled = false;
                _onlineModelChoose = false;
                OpenInternt.Text = "网络传输";

                WriteLogFile("关闭传输");

                OpenAllButoon();
                _openSendOnlineFlag = false;
            }

        }

        //去皮键
        private void makeZero_Click(object sender, EventArgs e)
        {
            WriteLogFile("点击去皮键");
            if (serMCU.IsOpen)
            {
                if (!_weightOnlineFlag[_addMcu]) return;
                _dataDealTimer.Enabled = false;

                _zeroPoint = _factMeasureValue;

                WriteLogFile((_addMcu + 1) + "号秤去皮：" + _zeroPoint);

                UpdateDataAccess(serMCU.PortName, _addMcu);
                SendCommand();
            }
            else
            {
                MessageShow("COM口未打开");
                WriteLogFile("点击去皮键时：COM口未打开");
            }

        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*-------------------------------标定模块-----------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
        //保存键
        private void saveData_Click(object sender, EventArgs e)
        {
            WriteLogFile("点击保存键");
            if (!_weightOnlineFlag[_addMcu])
            {
                MessageShow((_addMcu + 1) + "号秤未上线");
                WriteLogFile((_addMcu + 1) + "号秤未上线");
            }
            else
            {
                _dataDealTimer.Enabled = false;
                _dataDealTimer.Interval = DataDealTimeSet;

                var temp1 = standardEmpty.Text;
                var temp2 = standardFull.Text;
                if (temp1 != "" && temp2 != "")
                {
                    _factWeightZero = Convert.ToInt32(temp1);
                    _factWeightMax = Convert.ToInt32(temp2);
                }
                else
                {
                    MessageShow("标定值未输入");
                    WriteLogFile("标定值未输入");
                }
                if (_factWeightMax != 0 && _weightMax != 0 && _weightZero != 0)
                {
                    _emptyMeasureFlag = false;
                    _fullMeasureFlag = false;
                    _weightRate = (_weightMax - _weightZero)/_factWeightMax;
                    if ((int) _weightRate == 0)
                    {
                        MessageShow(string.Format("{0}号秤定错误", (_addMcu + 1)));
                        WriteLogFile(string.Format("{0}号秤定错误", (_addMcu + 1)));
                    }
                    else
                    {
                        _weightCrosspoint = _weightZero;
                        _zeroPoint = 0; //去皮归零                  
                        UpdateDataAccess(serMCU.PortName, _addMcu);
                        if (_updataFinishFlag)
                        {
                            MessageShow((_addMcu + 1) + "号秤定完成");

                            WriteLogFile((_addMcu + 1) + "号秤定完成");
                            WriteLogFile("_factWeightZero=" + _factWeightZero);
                            WriteLogFile("_factWeightMax=" + _factWeightMax);
                            WriteLogFile("_weightZero=" + _weightZero);
                            WriteLogFile("_weightMax=" + _weightMax);
                            WriteLogFile("_weightCrosspoint=" + _weightCrosspoint);
                            WriteLogFile("_weightRate=" + _weightRate);



                            _standardFinishFlag = true;
                        }
                        else
                        {
                            MessageShow((_addMcu + 1) + "号秤标定失败");
                            WriteLogFile((_addMcu + 1) + "号秤标定失败");
                        }
                    }
                }
                else
                {
                    MessageShow((_addMcu + 1) + "号秤标定未完成");
                    WriteLogFile((_addMcu + 1) + "号秤标定未完成");
                }
                    
                _dataDealTimer.Enabled = true;
            }
        }

        //空秤时测量按键
        private void emptyButton_Click(object sender, EventArgs e)
        {
            WriteLogFile("点击空秤测量键");
            if (serMCU.IsOpen)
            {
                if (_weightOnlineFlag[_addMcu])
                {
                    _weightMcu = 0;
                    _standardFinishFlag = false;

                    _dataDealTimer.Enabled = false;
                    _dataDealTimer.Interval = DataDealTimeSet;

                    MessageShow("零点测量完成");
                    WriteLogFile("零点测量完成");
                    SendCommand();
                    _emptyMeasureFlag = true;
                    _fullMeasureFlag = false;
                    _measureFlag = false;
                }
                else
                {
                    MessageShow((_addMcu + 1) + "号秤未上线");
                    WriteLogFile((_addMcu + 1) + "号秤未上线");
                }
            }
            else
            {
                MessageShow("COM口未打开");
                WriteLogFile("COM口未打开");
            }

        }

        //满量程测量按键
        private void fullButton_Click(object sender, EventArgs e)
        {
            WriteLogFile("点击满量程测量键");
            if (serMCU.IsOpen)
            {
                if (_weightOnlineFlag[_addMcu])
                {
                    _standardFinishFlag = false;
                    _dataDealTimer.Enabled = false;
                    _dataDealTimer.Interval = DataDealTimeSet;


                    MessageShow("满载测量完成");
                    WriteLogFile("满载测量完成");

                    SendCommand();
                    _fullMeasureFlag = true;
                    _measureFlag = false;
                    _emptyMeasureFlag = false;
                }

                else
                {
                    MessageShow((_addMcu + 1) + "号秤未上线");
                    WriteLogFile((_addMcu + 1) + "号秤未上线");
                }
            }
            else
            {
                MessageShow("COM口未打开");
                WriteLogFile("COM口未打开");

            }


        }

        //只输入数字处理模块
        private void standardEmpey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
                MessageShow("输入数字");

            }
            else
            {
                e.Handled = false;
            }
        }

        private void standardFull_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
                MessageShow("输入数字");

            }
            else
            {
                e.Handled = false;
            }
        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*---------------------------------网络通信---------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        private class StateObject
        {
            // Client  socket.
            public Socket WorkSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public readonly byte[] Buffer = new byte[BufferSize];
            // Received data string.
            public readonly StringBuilder Sb = new StringBuilder();
        }

        private static readonly ManualResetEvent AllDone = new ManualResetEvent(false);

        private void NetWork()
        {
            try
            {
                _openNetWorkFlag = true;
                /*          if (GetNetConfig() != 0)
                    PortAddress.Text = GetNetConfig().ToString();
                else
                {
                    MessageShow("配置端口");
                    return;
                }*/
                _port = int.Parse(PortAddress.Text);
                _ipp = new IPEndPoint(System.Net.IPAddress.Any, _port);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(_ipp);
                _socket.Listen(0);
                _newListenThread = new Thread(new ThreadStart(Listening)) {IsBackground = true};
                _newListenThread.Start();
            }
            catch (Exception)
            {

                MessageShow("无法建立网络通信");
                WriteLogFile("无法建立网络通信");
            }
        }

        private void Listening()
        {
            while (true)
            {
                // Set the event to nonsignaled state.
                AllDone.Reset();
                // Start an asynchronous socket to listen for connections.
                _socket.BeginAccept(new AsyncCallback(AcceptCallback), _socket);
                // Wait until a connection is made before continuing.
                WriteLogFile("等待网络连接................");

                AllDone.WaitOne();
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            AllDone.Set();
            WriteLogFile("网络连接上................");

            _beConnectedFlag = true;

            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
           
            WriteLogFile("网络连接上.........IP:  "+(handler.AddressFamily == AddressFamily.InterNetwork));
            // Create the state object.
            StateObject state = new StateObject();
            state.WorkSocket = handler;
            //          InitNetConfig();
            while (_beConnectedFlag)
            {
                ReadytoSendResetEvent.WaitOne();
                
                if (_sendOnlineMessage != null)
                    Send(handler, _sendOnlineMessage);
                _sendOnlineMessage = null;
                ReadytoSendResetEvent.Reset();
            }
            /*         handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);*/
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject) ar.AsyncState;
            Socket handler = state.WorkSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.Sb.Clear();
                // There  might be more data, so store the data received so far.
                state.Sb.Append(Encoding.ASCII.GetString(
                    state.Buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.Sb.ToString();
                WriteLogFile(content);
                Thread.Sleep(10000);
                if (content.Equals("query"))
                {
                    Send(handler, "finish");
                }
                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
        }

        private void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);

            }
            catch (Exception ex)
            {
                WriteLogFile(ex.Message);     
                //handler.Dispose();
                //_beConnectedFlag = false;
            }
        }

        private  void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket) ar.AsyncState;
                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                WriteLogFile("Sent {0} bytes to client."+bytesSent);
                
           //     handler.Shutdown(SocketShutdown.Both);
            //    handler.Close();

            }
            catch (Exception ex)
            {
                WriteLogFile(ex.Message);
            }
        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*----------------------------------数据库区-------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
        //连接数据库
        private static OleDbConnection GetDataAccess()
        {
            const string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= .\\access.mdb";
            var tempconn = new OleDbConnection(connstr);
            return tempconn;
        }

        //端口配置
        /*    private void UpdataNetConfig(int port)
        {
            var strTemp = GetDataAccess();
            strTemp.Open();
            var sqlstr = "update netconfig set Port=" + port + " where ID=0";

            OleDbCommand myCommand = new OleDbCommand(sqlstr, strTemp);

            try
            {
                myCommand.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                MessageShow(ex.Message);
            }
            finally
            {
                if (strTemp.State == ConnectionState.Open)
                {
                    strTemp.Close();
                    strTemp.Dispose();
                }
            }

        }
        //端口初始化
        private int GetNetConfig()
        {
            int a = 0;
            OleDbConnection strTemp = GetDataAccess();

            string str = "select Port from netconfig where ID=0";
            try
            {
                strTemp.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(str, strTemp);
                DataSet dsTemp = new DataSet();
                adapter.Fill(dsTemp, "dateTemp");
                DataRow drTemp = dsTemp.Tables["dateTemp"].Rows[0];
                a = Convert.ToInt32(drTemp[0]);
            }
            catch (SystemException ex)
            {
                MessageShow(ex.Message);
            }
            return a;

        }*/

        //创建表单
        private void UpdateStrctureTable(string com)
        {
            var strTemp = GetDataAccess();
            strTemp.Open();

            string sqlstr = "select * into " + com + " from init ";


            var myCommand = new OleDbCommand(sqlstr, strTemp);
            try
            {
                myCommand.ExecuteNonQuery();

            }
            catch (SystemException ex)
            {
                WriteLogFile(ex.Message);
            }
            finally
            {
                if (strTemp.State == ConnectionState.Open)
                {
                    strTemp.Close();
                    strTemp.Dispose();
                }
            }
        }

        //数据更新
        private void UpdateDataAccess(string com, int addTemp)
        {
            OleDbConnection strTemp = GetDataAccess();
            strTemp.Open();
            _updataFinishFlag = false;
            string sqlstr = "update " + com + " set ";
            sqlstr += "weightZero='" + _weightZero + "',";
            sqlstr += "weightMax='" + _weightMax + "',";
            sqlstr += "factWeightZero=" + _factWeightZero + ",";
            sqlstr += "factWeightMax=" + _factWeightMax + ",";
            sqlstr += "zeroPoint='" + _zeroPoint;
            sqlstr += "' where weightNo=" + addTemp;

            OleDbCommand myCommand = new OleDbCommand(sqlstr, strTemp);

            try
            {
                myCommand.ExecuteNonQuery();
                _updataFinishFlag = true;
            }
            catch (SystemException ex)
            {
                MessageShow(ex.Message);
                WriteLogFile(ex.Message);
            }
            finally
            {
                if (strTemp.State == ConnectionState.Open)
                {
                    strTemp.Close();
                    strTemp.Dispose();
                }
            }
        }

        //数据库数据显示
        private void DataBaseView(string com, int addStr)
        {
            if (!_onlineModelChoose)
            {
                if (!_weightOnlineFlag[addStr]) return;
            }
            OleDbConnection strTemp = GetDataAccess();
            string str = "select weightZero,weightMax,factWeightZero,factWeightMax,zeroPoint from " + com +
                         " where weightNo=" + addStr;
            try
            {
                strTemp.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(str, strTemp);
                DataSet dsTemp = new DataSet();
                adapter.Fill(dsTemp, "dateTemp");
                DataRow drTemp = dsTemp.Tables["dateTemp"].Rows[0];

                _weightZero = Convert.ToSingle(drTemp[0]);
                _weightMax = Convert.ToSingle(drTemp[1]);
                _factWeightZero = Convert.ToInt32(drTemp[2]);
                _factWeightMax = Convert.ToInt32(drTemp[3]);
                _zeroPoint = Convert.ToSingle(drTemp[4]);

            }
            catch (SystemException ex)
            {
                MessageShow(ex.Message);
                WriteLogFile(ex.Message);

            }
            finally
            {
                if (strTemp.State == ConnectionState.Open)
                {
                    strTemp.Close();
                    strTemp.Dispose();
                }
                //判断是否标定过
                if (_weightMax != 0 && _factWeightMax != 0)
                {
                    if (_weightOnlineFlag[addStr])
                    {
                        if (!_measureFlag)
                        {
                            MessageShow((_addMcu + 1) + "号秤已标定");
                        }
                    }

                    StandardValueView(true);

                    _weightRate = (_weightMax - _weightZero)/_factWeightMax;
                    _weightCrosspoint = _weightZero;
                    _standardFinishFlag = true;
                }
                else
                {
                    if (_weightOnlineFlag[addStr])
                    {
                        StandardValueView(false);
                        MessageShow((_addMcu + 1) + "号秤未标定");
                        _standardFinishFlag = false;
                    }

                }
            }
        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*---------------------------------秤选择区---------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/


        private void WeightChooseFlagInit(int addr)
        {
            WriteLogFile("选取" + (addr + 1) + "号秤");
            if (!_measureFlag)
                DataBaseView(serMCU.PortName, _addMcu);
            _weightCheckFlag[addr] = true;
            _weightOnlineFlag[_addMcu] = false;
            _emptyMeasureFlag = false;
            _fullMeasureFlag = false;
        }

        private void weightNo1_Click(object sender, EventArgs e)
        {
            if (weightNo1_Button.Checked != true) return;

            _addMcu = 0;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo2_Click(object sender, EventArgs e)
        {
            if (weightNo2_Button.Checked != true) return;

            _addMcu = 1;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();

        }

        private void weightNo3_Click(object sender, EventArgs e)
        {
            if (weightNo3_Button.Checked != true) return;

            _addMcu = 2;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();


        }

        private void weightNo4_Click(object sender, EventArgs e)
        {
            if (weightNo4_Button.Checked != true) return;

            _addMcu = 3;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();

        }

        private void weightNo5_Click(object sender, EventArgs e)
        {
            if (weightNo5_Button.Checked != true) return;

            _addMcu = 4;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();


        }

        private void weightNo6_Click(object sender, EventArgs e)
        {
            if (weightNo6_Button.Checked != true) return;
            //          if (!serMCU.IsOpen) return;
            _addMcu = 5;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();

        }

        private void weightNo7_Click(object sender, EventArgs e)
        {
            if (weightNo7_Button.Checked != true) return;
            //          if (!serMCU.IsOpen) return;       
            _addMcu = 6;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo8_Click(object sender, EventArgs e)
        {
            if (weightNo8_Button.Checked != true) return;
            //          if (!serMCU.IsOpen) return;
            _addMcu = 7;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();

        }

        private void weightNo9_Click(object sender, EventArgs e)
        {
            if (weightNo9_Button.Checked != true) return;
            //           if (!serMCU.IsOpen) return;
            _addMcu = 8;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo10_Click(object sender, EventArgs e)
        {
            if (weightNo10_Button.Checked != true) return;
            //      if (!serMCU.IsOpen) return;
            _addMcu = 9;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo11_Click(object sender, EventArgs e)
        {
            if (weightNo11_Button.Checked != true) return;
            //        if (!serMCU.IsOpen) return;
            _addMcu = 10;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo12_Click(object sender, EventArgs e)
        {
            if (weightNo12_Button.Checked != true) return;
            //         if (!serMCU.IsOpen) return;
            _addMcu = 11;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo13_Click(object sender, EventArgs e)
        {
            if (weightNo13_Button.Checked != true) return;
            //          if (!serMCU.IsOpen) return;
            _addMcu = 12;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo14_Click(object sender, EventArgs e)
        {
            if (weightNo14_Button.Checked != true) return;
            //          if (!serMCU.IsOpen) return;
            _addMcu = 13;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo15_Click(object sender, EventArgs e)
        {
            if (weightNo15_Button.Checked != true) return;
            //           if (!serMCU.IsOpen) return;
            _addMcu = 14;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

        private void weightNo16_Click(object sender, EventArgs e)
        {
            if (weightNo16_Button.Checked != true) return;
            _addMcu = 15;
            WeightChooseFlagInit(_addMcu);
            if (!serMCU.IsOpen) return;
            SendCommand();
        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*------------------------------信息框显示----------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        private void MessageShow(String str)
        {
            funInfo.Text = str;
        }

        private void StandardValueView(bool str)
        {
            if (str)
            {
                measureEmpty.Text = _weightZero.ToString();
                measureFull.Text = _weightMax.ToString();
                standardEmpty.Text = _factWeightZero.ToString();
                standardFull.Text = _factWeightMax.ToString();
            }
            else
            {
                measureEmpty.Clear();
                measureFull.Clear();
                standardEmpty.Clear();
                standardFull.Clear();
            }
        }

/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*------------------------------称重数据显示--------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        private void DisplayData(float weight)
        {
            revText.Text = weight.ToString();
        }
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*------------------------------运行日志------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------------*/

        private void WriteLogFile(string input)
        {
        //    _checkProtExistTimer.Enabled = false;
            string fname = Directory.GetCurrentDirectory() + "\\" + "log" + "\\" + "CurrentLogText.txt";
            FileInfo finfo = new FileInfo(fname);
            lock (this)
            {
                try
                {
                    //指定日志文件的目录
                    /**/
                    //定义文件信息对象

                    if (!finfo.Exists)
                    {
                        File.Create(fname).Close();
                        finfo = new FileInfo(fname);
                    }

                    //创建只写文件流
                    //   using (FileStream fs = finfo.OpenWrite())
                    //  {
                    /**/
                    //根据上面创建的文件流创建写数据流
                    using (StreamWriter w = finfo.AppendText())
                    {
                        /**/
                        //设置写数据流的起始位置为文件流的末尾
                        //w.BaseStream.Seek(0, SeekOrigin.End);

                        w.Write("\n\r");
                        /**/
                        //写入当前系统时间并换行
                        w.Write("\n\r" + DateTime.Now.ToString("MM-dd-hh:mm:ss") + "\n\r");

                        /**/
                        //写入日志内容并换行
                        w.Write("\n\r" + input + "\n\r");


                        /**/
                        //清空缓冲区内容，并把缓冲区内容写入基础流
                        w.Flush();

                        /**/
                        //关闭写数据流
                        w.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageShow(ex.Message);
                    File.AppendAllText(fname, "\n\r" + "写入出错" + "\n\r" + ex.Message + "\n\r");
                }

                try
                {
                    if (finfo.Length > 1024 * 1024*10)
                    {
                        /**/
                        //文件超过10MB则重命名
                        if (
                            File.Exists(Directory.GetCurrentDirectory() + "\\" + "log" + "\\" +
                                        DateTime.Now.ToString("MM-dd-hh-mm") + ".txt"))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + "\\" + "log" + "\\" +
                                         DateTime.Now.ToString("MM-dd-hh-mm") + ".txt");
                        }
                        File.Move(fname, Directory.GetCurrentDirectory() + "\\" + "log" + "\\" + DateTime.Now.ToString("MM-dd-hh-mm") + ".txt");
                        /**/
                        //删除该文件
                        File.Delete(fname);
                        finfo.Delete();
                    }
                }
                catch (Exception ex)
                {

                    MessageShow(ex.Message);
                    File.AppendAllText(fname, "\n\r" + "替换文件出错" + "\n\r" + ex.Message + "\n\r");
                }
            }
           // _checkProtExistTimer.Enabled = true;

        }


        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_checkProtExistTimer.Enabled = false;
            WriteLogFile("程序结束");
            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "log" + "\\" + DateTime.Now.ToString("MM-dd-hh-mm") + ".txt"))
            {
                File.Move(Directory.GetCurrentDirectory() + "\\" + "log" + "\\" + "CurrentLogText.txt", Directory.GetCurrentDirectory() + "\\" + "log" + "\\" + DateTime.Now.ToString("MM-dd-hh-mm") + "(closing)" + ".txt");
            }
            File.Move(Directory.GetCurrentDirectory() + "\\" + "log" + "\\" + "CurrentLogText.txt", Directory.GetCurrentDirectory() + "\\" + "log" + "\\" + DateTime.Now.ToString("MM-dd-hh-mm") + ".txt");
            Environment.Exit(0);
        }

    }
}

