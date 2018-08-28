using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using JGP_Splash;
using casedefect;
using Cognex.VisionPro.Blob;
using System.IO;
using Cognex.VisionPro;
using System.Threading;
namespace Lens_Holder
{
    public partial class Form1 : Form
    {

        #region 變數們
        #region 路徑們
        string LensPicPath = Application.StartupPath + "\\LensPic\\";
        string HolderPicPath = Application.StartupPath + "\\HolderPic\\";


        #endregion

        //要傳送給plc的變數
        public struct SendToPLC
        {

            /// <summary>Lens Tray標記尋找完成
            /// 
            /// </summary>
            public bool isSendLensTrayComplete;
            /// <summary>Holder Tray標記尋找完成
            /// 
            /// </summary>
            public bool isSendHolderTrayComplete;
            /// <summary>Lens Tray 執行ok
            /// 
            /// </summary>
            public bool isLensTrayOK;
            /// <summary>Holder Tray 執行ok
            /// 
            /// </summary>
            public bool isHolderTrayOK;
            /// <summary>機器手連結異常
            /// 
            /// </summary>
            public bool isRobotConnError;
            /// <summary>Lens Tray 標記搜尋結果
            /// 
            /// </summary>
            public string LensTrayResult1, LensTrayResult2;
            /// <summary>Holder Tray 標記搜尋結果
            /// 
            /// </summary>
            public string HolderTrayResult;
        }
        //從plc得到的變數
        public struct RecvFromPLC
        {
            /// <summary>LENS TRAY 1 不使用
            /// 
            /// </summary>
            public bool LensTrayOff1;
            /// <summary>LENS TRAY 2 不使用
            /// 
            /// </summary>
            public bool LensTrayOff2;
            /// <summary>HOLDER TRAY 1 不使用
            /// 
            /// </summary>
            public bool HolderTrayOff1;
            /// <summary>HOLDER TRAY 2 不使用
            /// 
            /// </summary>
            public bool HolderTrayOff2;

        }
        SendToPLC SPLC;
        RecvFromPLC RPLC;
        PLCConn PLC = new PLCConn("192.168.0.10", 2001);

        /// <summary>組裝offset
        /// 
        /// </summary>
        public XYUPos Offset1 = new XYUPos("Offset1"), Offset2 = new XYUPos("Offset2");

        /// <summary>允許組裝料件大小差異值
        /// 
        /// </summary>
        public XYUPos AllowDisSize = new XYUPos("AllowDisSize");

        /// <summary>第2支SCARA(HOLDER  和  LENS組裝)
        /// 
        /// 
        /// </summary>
        EpsonConn Epson = new EpsonConn("192.168.0.2", 2000);

        /// <summary>第3支SCARA(成品)
        /// 
        /// 
        /// </summary>
        EpsonConn Epson_OK = new EpsonConn("192.168.0.3", 2000);

        Splasher sp = new Splasher();

        //確認連線是否異常
        bool isPLCError, isRBError, isRB_OKTrayError;

        //執行緒變數
        Thread Thread_LensTray, Thread_HolderTray, Thread_Lens, Thread_Holder;

        //toolblock變數
        CogToolBlock tbLensTray = new CogToolBlock(),
                      tbHolderTray = new CogToolBlock(),
                      tbLens = new CogToolBlock(),
                      tbHolder = new CogToolBlock(),
                      tbAssyOK = new CogToolBlock(),
                      tbOKTray = new CogToolBlock();

        //toolblock 介面list
        List<CogToolBlock> List_tb;
        List<ToolStripMenuItem> List_tSBTBEdit;
        List<Button> List_Btn;
        List<CogRecordDisplay> List_cRD;
        string[] strtbName;


        
        #endregion

        public Form1()
        {

            sp.Show("準備中...");
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            #region List配置
            List_tSBTBEdit = new List<ToolStripMenuItem> { tSBLensTray, tSBHolderTray, tSBLens, tSBHolder1, tSBAssyOK, tSBOKTray };
            List_Btn = new List<Button> { btnRun_LensTray, btnRun_HolderTray, btnRun_Lens, btnRun_Holder, btnRun_AssyOK, btnRun_OKTray };
            strtbName = new string[] { "LensTray", "HolderTray", "Lens", "Holder", "AssyOK", "OKTray" };
            List_cRD = new List<CogRecordDisplay> { cRD_LensTray, cRD_HolderTray, cRD_Lens, cRD_Holder, cRD_AssyOK, cRD_OKTray };

            #endregion

            #region 讀取tb
            string s;
            sp.mStatus = "開啟Lens萃盤檢測工具...";
            s = string.Concat(System.IO.Directory.GetCurrentDirectory().ToString(), "\\TB\\" + "LensTray" + ".vpp");
            tbLensTray = (CogToolBlock)CogSerializer.LoadObjectFromFile(s);
            sp.mStatus = "開啟Holder萃盤檢測工具...";
            s = string.Concat(System.IO.Directory.GetCurrentDirectory().ToString(), "\\TB\\" + "HolderTray" + ".vpp");
            tbHolderTray = (CogToolBlock)CogSerializer.LoadObjectFromFile(s);
            sp.mStatus = "開啟Lens位置尋找工具...";
            s = string.Concat(System.IO.Directory.GetCurrentDirectory().ToString(), "\\TB\\" + "Lens" + ".vpp");
            tbLens = (CogToolBlock)CogSerializer.LoadObjectFromFile(s);
            sp.mStatus = "開啟Holder位置尋找工具...";
            s = string.Concat(System.IO.Directory.GetCurrentDirectory().ToString(), "\\TB\\" + "Holder" + ".vpp");
            tbHolder = (CogToolBlock)CogSerializer.LoadObjectFromFile(s);
            sp.mStatus = "開啟組裝完成檢測工具...";
            s = string.Concat(System.IO.Directory.GetCurrentDirectory().ToString(), "\\TB\\" + "AssyOK" + ".vpp");
            tbAssyOK = (CogToolBlock)CogSerializer.LoadObjectFromFile(s);
            sp.mStatus = "開啟成品萃盤位置尋找工具...";
            s = string.Concat(System.IO.Directory.GetCurrentDirectory().ToString(), "\\TB\\" + "OKTray" + ".vpp");
            tbOKTray = (CogToolBlock)CogSerializer.LoadObjectFromFile(s);

            List_tb = new List<CogToolBlock> { tbLensTray, tbHolderTray, tbLens, tbHolder, tbAssyOK, tbOKTray };
            #endregion

            #region 連線們
            sp.mStatus = "PLC連線...";
            if (!PLC.ConnectPLC())
            {
                isPLCError = true;
                MessageBox.Show("PLC連線異常，請確認連線後，重啟程式");
            }
            else
            {
                bgWPLC.RunWorkerAsync();
            }
            sp.mStatus = "機器手連線...";
            if (!Epson.ConnectEpson())
            {
                isRBError = true;
                MessageBox.Show("機器手(Lens Holder)連線異常，請確認連線後，重啟程式");
                SPLC.isRobotConnError = true;
            }
            else
            {
                bgW_Epson.RunWorkerAsync();
            }
            sp.mStatus = "機器手(成品萃盤)連線...";
            if (!Epson_OK.ConnectEpson())
            {
                isRB_OKTrayError = true;
                MessageBox.Show("機器手(成品萃盤)連線異常，請確認連線後，重啟程式");
                SPLC.isRobotConnError = true;
            }
            else
            {
                bgW_EpsonOK.RunWorkerAsync();
            }
            string SS = "";
            if (isPLCError)
                SS += "PLC連線異常   ";
            if (isRBError)
                SS += "機器手(Lens Holder)連線異常   ";
            if (isRB_OKTrayError)
                SS += "機器手(成品萃盤)連線異常";
            if (isPLCError | isRB_OKTrayError | isRBError)
            {
                tSSLConnStatus.Text = SS;
                tSSLConnStatusLamp.BackColor = Color.Red;
            }
            bgw_ReConn.RunWorkerAsync();
            #endregion

            #region 讀取組裝設定值
            Offset1.Load();
            Offset2.Load();
            AllowDisSize.Load();
            #endregion

            User_Set.ini();
            SetLimit();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            sp.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);//exit and close all threads and release all recources
        }

        #region PLC控制
        public delegate void LensTrayCCDResultdelegate(string ms);
        public delegate void HolderTrayCCDResultdelegate(string ms);
        private void bgWPLC_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 從plc讀m3000~m3008，依m點執行動作
            bool isRunLensTray, isRunHolderTray, isRunHolderTray2;
            string M08 = PLC.ReadM(3000, 8);
            if (M08 == "")
                return;
            isRunLensTray = M08[0] == '1';
            isRunHolderTray = M08[1] == '1';
            isRunHolderTray2 = M08[3] == '1';
            RPLC.LensTrayOff1  = M08[4] == '1' ;
            RPLC.LensTrayOff2 = M08[5] == '1';
            RPLC.HolderTrayOff1 = M08[6] == '1';
            RPLC.HolderTrayOff2 = M08[7] == '1';

            if (isRunLensTray)
                RunLensTray();

            if (isRunHolderTray|isRunHolderTray2)
                RunHolderTray();

            #endregion

            #region 若robot連線異常，則告知plc
            if (isRB_OKTrayError)
            {
                PLC.WriteM(3010, "1");
                SPLC.isRobotConnError = false;
            }
            if (isRBError)
            {
                PLC.WriteM(3011, "1");
                SPLC.isRobotConnError = false;
            }
            #endregion

            #region 傳送結果
            if (SPLC.isSendLensTrayComplete)
            {
                if (SPLC.isLensTrayOK)
                    PLC.WriteM(3099, "0" + SPLC.LensTrayResult1 + SPLC.LensTrayResult2);
                else
                    PLC.WriteM(3099, "1");
                SPLC.isSendLensTrayComplete = false;
                PLC.WriteM(3000, "0");
            }

            if (SPLC.isSendHolderTrayComplete)
            {
                if (isRunHolderTray)
                {
                    if (SPLC.isHolderTrayOK)
                        PLC.WriteM(3299, "0" + SPLC.HolderTrayResult);
                    else
                        PLC.WriteM(3299, "1");
                    SPLC.isSendHolderTrayComplete = false;
                    PLC.WriteM(3001, "0");
                }
                if (isRunHolderTray2)
                {
                    if (SPLC.isHolderTrayOK)                    
                        PLC.WriteM(3372, SPLC.HolderTrayResult+"0" );
                    else
                        PLC.WriteM(3444, "1");
                    SPLC.isSendHolderTrayComplete = false;
                    PLC.WriteM(3003, "0");
                }
            }

            #endregion

            Thread.Sleep(1);
        }
        //顯示lens tray結果
        void ShowLensTrayResult(string ms)
        {
            if (InvokeRequired == true)
            {
                LensTrayCCDResultdelegate m = new LensTrayCCDResultdelegate(ShowLensTrayResult);
                object[] eventargs = { ms };
                BeginInvoke(m, eventargs);
            }
            else
            {
                txtLensTrayResult.Text = ms;
            }
        }
        //顯示Holder tray結果
        void ShowHolderTrayResult(string ms)
        {
            if (InvokeRequired == true)
            {
                HolderTrayCCDResultdelegate m = new HolderTrayCCDResultdelegate(ShowHolderTrayResult);
                object[] eventargs = { ms };
                BeginInvoke(m, eventargs);
            }
            else
            {
                txtHolderTrayResult.Text = ms;
            }
        }

        void RunLensTray()
        {
            if (Thread_LensTray == null)
            {
                Thread_LensTray = new Thread(_RunLensTray);
                Thread_LensTray.Start();
            }
            else
            {
                if (!Thread_LensTray.IsAlive)
                {
                    Thread_LensTray = new Thread(_RunLensTray);
                    Thread_LensTray.Start();
                }
            }

        }
        void _RunLensTray()
        {
            
            //清除原有圖片
            cRD_LensTray.Image = null;
            cRD_LensTray.StaticGraphics.Clear();
            cRD_LensTray.InteractiveGraphics.Clear();


            int LensTrayNum = RPLC.LensTrayOff1 | RPLC.LensTrayOff2 ? 1 : 2;

            //執行結果
            ICogRecord rd=null, rd2;


            //取像
            CogAcqFifoTool ff = (CogAcqFifoTool)tbLensTray.Tools["CogAcqFifoTool1"];
            ff.Run();
            CogImage8Grey im = (CogImage8Grey)ff.OutputImage;

            //im = (CogImage8Grey)(((CogAcqFifoTool)BBBB.Tools["CogAcqFifoTool1"]).OutputImage);

            //丟給PMAlign
            Cognex.VisionPro.PMAlign.CogPMAlignTool PMA = (Cognex.VisionPro.PMAlign.CogPMAlignTool)tbLensTray.Tools["CogPMAlignTool1"];
            PMA.InputImage = im;
            PMA.Run();
            int PMAResultCount = PMA.Results.Count;

            if (PMA.RunStatus.Result == CogToolResultConstants.Accept)
            {
                bool RB=false ,RB2=false ;
                string R, RS="",R2,RS2="";
                CogToolBlock stb = (CogToolBlock)tbLensTray.Tools["TB"];
                switch (PMAResultCount)
                {
                    case 0://沒找到
                        //執行失敗
                        rd = tbLensTray.CreateLastRunRecord().SubRecords[0];
                        SPLC.isLensTrayOK = false;
                        SPLC.isSendLensTrayComplete = true;
                        break;
                    case 1://找到一ㄍ
                        stb.Inputs["InputImage"].Value = im;
                        stb.Inputs["GetPose"].Value = PMA.Results[0].GetPose();
                        stb.Run();
                        R = (string)stb.Outputs["strResult"].Value;
                        RS = (string)stb.Outputs["strPos"].Value;
                        RB = stb.RunStatus.Result == CogToolResultConstants.Accept;
                        rd = stb.CreateLastRunRecord().SubRecords[1];
                        if (RB)
                        {
                            //回傳與顯示執行結果
                            SPLC.LensTrayResult1 = SPLC.LensTrayResult2 = R;
                            ShowLensTrayResult(RS);

                            //確認找到ㄉ是哪ㄍ,找錯則回傳執行失敗
                            if (RPLC.LensTrayOff1)
                            {
                                if (PMA.Results[0].GetPose().TranslationX <= 1296)
                                    SPLC.isLensTrayOK = false;
                                else
                                    SPLC.isLensTrayOK = true;
                                SPLC.isSendLensTrayComplete = true;
                            }
                            else if (RPLC.LensTrayOff2)
                            {
                                if (PMA.Results[0].GetPose().TranslationX <= 1296)
                                    SPLC.isLensTrayOK = false;
                                else
                                    SPLC.isLensTrayOK = true;
                                SPLC.isSendLensTrayComplete = true;
                            }
                            else
                            {//應該要找到2ㄍ
                                //執行失敗
                                SPLC.isLensTrayOK = false;
                                SPLC.isSendLensTrayComplete = true;
                            }
                        }
                        else
                        {
                            //執行失敗
                            rd = stb.CreateLastRunRecord().SubRecords[1];
                            SPLC.isLensTrayOK = false;
                            SPLC.isSendLensTrayComplete = true;
                        }

                        break;
                    case 2:
                        string aa = "111111111111111111111111111111111111111111111111111111111111111111111111";
                        stb.Inputs["InputImage"].Value = im;
                        Cognex.VisionPro.CogTransform2DLinear P1, P2;
                        if (PMA.Results[1].GetPose().TranslationX > PMA.Results[0].GetPose().TranslationX)
                        {
                            P1 = PMA.Results[0].GetPose();
                            P2 = PMA.Results[1].GetPose();
                        }
                        else
                        {
                            P1 = PMA.Results[1].GetPose();
                            P2 = PMA.Results[0].GetPose();
                        }

                        if (!RPLC.LensTrayOff1)
                        {
                            stb.Inputs["GetPose"].Value = P1;
                            stb.Run();
                            R = (string)stb.Outputs["strResult"].Value;
                            RS = (string)stb.Outputs["strPos"].Value;
                            RB = stb.RunStatus.Result == CogToolResultConstants.Accept;
                            rd = stb.CreateLastRunRecord().SubRecords[1];
                        }
                        else
                        {
                            R = aa;
                        }
                        if (!RPLC.LensTrayOff2)
                        {
                            stb.Inputs["GetPose"].Value = P2;
                            stb.Run();
                            R2 = (string)stb.Outputs["strResult"].Value;
                            RS2 = (string)stb.Outputs["strPos"].Value;
                            RB2 = stb.RunStatus.Result == CogToolResultConstants.Accept;
                            if (RPLC.LensTrayOff1)
                            {
                                rd = stb.CreateLastRunRecord().SubRecords[1];
                            }
                            else
                            {
                                rd2 = stb.CreateLastRunRecord().SubRecords[1];
                                rd.SubRecords.Add(rd2);
                            }
                        }
                        else
                        {
                            R2 = aa;
                        }

                        SPLC.LensTrayResult1 = R;
                        SPLC.LensTrayResult2 = R2;
                        ShowLensTrayResult(1+Environment.NewLine + RS+Environment.NewLine +
                            2+Environment.NewLine +RS2);

                        if (RPLC.LensTrayOff1)
                        {
                            SPLC.isLensTrayOK = RB2;
                        }
                        else if (RPLC.LensTrayOff2)
                        {
                            SPLC.isLensTrayOK = RB;
                        }
                        else
                        {
                            SPLC.isLensTrayOK = RB & RB2;
                        }
                        SPLC.isSendLensTrayComplete = true;
                        break;


                }

                cRD_LensTray.Record = rd;
                cRD_LensTray.Fit();
            }
            else
            {
                //執行失敗
                SPLC.isLensTrayOK = false;
                SPLC.isSendLensTrayComplete = true;
            }



        }
 
        void RunHolderTray()
        {
            if (Thread_HolderTray == null)
            {
                Thread_HolderTray = new Thread(_RunHolderTray);
                Thread_HolderTray.Start();
            }
            else
            {
                if (!Thread_HolderTray.IsAlive)
                {
                    Thread_HolderTray = new Thread(_RunHolderTray);
                    Thread_HolderTray.Start();
                }
            }

        }
        void _RunHolderTray()
        {

            //清除原有圖片
            cRD_HolderTray.Image = null;
            cRD_HolderTray.StaticGraphics.Clear();
            cRD_HolderTray.InteractiveGraphics.Clear();

            tbHolderTray.Run();
             if (tbHolderTray.RunStatus.Result == CogToolResultConstants.Accept)
            {
                SPLC.HolderTrayResult = (string)tbHolderTray.Outputs["strResult"].Value;
                ShowHolderTrayResult((string)tbHolderTray.Outputs["strPos"].Value);
                SPLC.isHolderTrayOK = true;
                SPLC.isSendHolderTrayComplete = true;
                cRD_HolderTray.Record = tbHolderTray.CreateLastRunRecord().SubRecords[0];
            }
            else
            {
                //執行失敗
                SPLC.isHolderTrayOK = false;
                SPLC.isSendHolderTrayComplete = true;
                cRD_HolderTray.Record = tbHolderTray.CreateLastRunRecord().SubRecords[0];
            }           




            //顯示結果

            cRD_HolderTray.Fit();




        }


        private void bgWPLC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgWPLC.RunWorkerAsync();
        }
        #endregion

        #region Scara
        public delegate void CommandFromRobotdelegate(string ms);
        bool isHolderRunOK;
        bool isLensOK1, isLensOK2, isHolderOK1, isHolderOK2;
        double Lens1Size, Lens2Size, Holder1Size, Holder2Size;
        //所有計算會用到的位置
        RobotVisionPos.PositionAngle Holder1, Lens1, Assy1, Holder2, Lens2, Assy2, Robot;
        private void bgW_Epson_DoWork(object sender, DoWorkEventArgs e)
        {
            
            string str = Epson.ReadData();
            CommandFromRobot(str);
        }

        private void bgW_Epson_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgW_Epson.RunWorkerAsync();
        }

        void CommandFromRobot(string str)
        {

            if (InvokeRequired == true)
            {
                CommandFromRobotdelegate m = new CommandFromRobotdelegate(CommandFromRobot);
                object[] eventargs = { str };
                BeginInvoke(m, eventargs);
            }
            else
            {
                string[] SS = str.Split(',');
                switch (SS[0])
                {
                    case "C1":
                        _RunLens();
                        _RunHolder();
                        ////while (Thread_Lens.IsAlive | Thread_Holder1.IsAlive )
                        ////{
                        ////    Thread.Sleep(101);
                        ////}


                        //Epson.WriteData("OK,OK,-162.399,284.198,-81.966,-125.399,273.198,-119.966" + "\r\n");

                        SetPosition(double.Parse(SS[1]), double.Parse(SS[2]), double.Parse(SS[3]));

                        break;
                    case "C2":
                        //清除原有圖片
                        cRD_AssyOK.Image = null;
                        cRD_AssyOK.StaticGraphics.Clear();
                        cRD_AssyOK.InteractiveGraphics.Clear();

                        tbAssyOK.Run();
                        cRD_AssyOK.Record = tbAssyOK.CreateLastRunRecord().SubRecords[0];;
                        cRD_AssyOK.Fit();
                        if (tbAssyOK.RunStatus.Result == CogToolResultConstants.Accept)
                        {
                            string aa = (int)tbAssyOK.Outputs["Results_Count1"].Value == 1 ? "OK" : "NG";
                            string bb = (int)tbAssyOK.Outputs["Results_Count2"].Value == 1 ? "OK" : "NG";
                            if (checkBox1.Checked)
                                aa = bb = "OK";
                            Epson.WriteData( aa+ "," +bb + "\r\n");
                        }
                        else
                        {
                            if (checkBox1.Checked)
                                Epson.WriteData("OK,OK" + "\r\n");
                            else
                                Epson.WriteData("NG,NG" + "\r\n");
                        }
                        break;
                }
            }

        }
        //以執行緒lens tb
        void RunLens()
        {
            if (Thread_Lens == null)
            {
                Thread_Lens = new Thread(_RunLens);
                Thread_Lens.Start();
            }
            else
            {
                if (!Thread_Lens.IsAlive)
                    Thread_Lens.Start();
            }

        }
        //執行與顯示LENS tb
        void _RunLens()
        {
            //清除原有圖片
            cRD_Lens.Image = null;
            cRD_Lens.StaticGraphics.Clear();
            cRD_Lens.InteractiveGraphics.Clear();

            //執行結果
            ICogRecord rd, rd2;

            //取像
            tbLens.Run();
            CogAcqFifoTool ff = (CogAcqFifoTool)tbLens.Tools["CogAcqFifoTool1"];
            //ff.Run();
            CogImage8Grey im = (CogImage8Grey)ff.OutputImage;


            //丟給PMAlign
            Cognex.VisionPro.PMAlign.CogPMAlignTool PMA = (Cognex.VisionPro.PMAlign.CogPMAlignTool)tbLens.Tools["CogPMAlignTool1"];
            //PMA.InputImage = im;
            //PMA.Run();

            if (PMA.RunStatus.Result == CogToolResultConstants.Accept)
            {
                CogTransform2DLinear Pose1 = new CogTransform2DLinear(), Pose2 = new CogTransform2DLinear();

                #region 判斷PMA抓到的特徵位置
                if (PMA.Results.Count == 0)
                {
                    isLensOK1 = false;
                    isLensOK2 = false;
                }
                else if (PMA.Results.Count == 1)
                {
                    //若抓到ㄉ點在上邊  代表抓到ㄉ是料件A
                    if (PMA.Results[0].GetPose().TranslationY < 295)
                    {
                        Pose1 = PMA.Results[0].GetPose();
                        isLensOK2 = false;
                        isLensOK1 = true;
                    }
                    else
                    {
                        Pose2 = PMA.Results[0].GetPose();
                        isLensOK1 = false;
                        isLensOK2 = true;
                    }
                }
                else if (PMA.Results.Count == 2)
                {
                    isLensOK1 = true;
                    isLensOK2 = true;
                    //看哪ㄍ在上面  上面為料件A
                    if (PMA.Results[0].GetPose().TranslationY < PMA.Results[1].GetPose().TranslationY)
                    {
                        Pose1 = PMA.Results[0].GetPose();
                        Pose2 = PMA.Results[1].GetPose();
                    }
                    else
                    {
                        Pose1 = PMA.Results[1].GetPose();
                        Pose2 = PMA.Results[0].GetPose();
                    }
                }
                #endregion

                CogToolBlock stb = (CogToolBlock)tbLens.Tools["TB"];
               //stb.Inputs["InputImage"].Value = im;


                #region 若特徵點抓取ok，則執行位置記憶
                if (isLensOK1)
                {
                    //執行內部tb                    
                    stb.Inputs["GetPose"].Value = Pose1;
                    stb.Run();

                    Lens1.X = (double)stb.Outputs["X"].Value;
                    Lens1.Y = (double)stb.Outputs["Y"].Value;
                    Lens1.Angle = (double)stb.Outputs["U"].Value * 180 / Math.PI;
                    Lens1Size = (double)stb.Outputs["Size"].Value;

                    rd = stb.CreateLastRunRecord().SubRecords[1];


                    isLensOK1 = stb.RunStatus.Result == CogToolResultConstants.Accept & (bool )stb.Outputs["CornerResult"].Value ;
                }
                else
                {
                    rd = PMA.CreateLastRunRecord().SubRecords[0];
                }
                if (isLensOK2)
                {
                    //執行內部tb                    
                    stb.Inputs["GetPose"].Value = Pose2;
                    stb.Run();

                    Lens2.X = (double)stb.Outputs["X"].Value;
                    Lens2.Y = (double)stb.Outputs["Y"].Value;
                    Lens2.Angle = (double)stb.Outputs["U"].Value * 180 / Math.PI;
                    Lens2Size = (double)stb.Outputs["Size"].Value;

                    rd2 = stb.CreateLastRunRecord().SubRecords[1];
                    isLensOK2 = stb.RunStatus.Result == CogToolResultConstants.Accept & (bool)stb.Outputs["CornerResult"].Value;
                }
                else
                {
                    rd2 = PMA.CreateLastRunRecord().SubRecords[0];
                }
                rd.SubRecords.Add(rd2);
                cRD_Lens.Record = rd;
                cRD_Lens.Fit();
                #endregion
            }
            else
            {
                //執行失敗
                isLensOK1 = false;
                isLensOK2 = false;

            }

            lblLensResult1.BackColor = isLensOK1 ? Color.Green : Color.Red;
            lblLensResult1.Text = isLensOK1 ? "OK" : "NG";
            lblLensSize1.Text = Lens1Size.ToString() + "mm";
            lblLensPos1.Text = Lens1.ToString();

            lblLensResult2.BackColor = isLensOK2 ? Color.Green : Color.Red;
            lblLensResult2.Text = isLensOK2 ? "OK" : "NG";
            lblLensSize2.Text = Lens2Size.ToString() + "mm";
            lblLensPos2.Text = Lens2.ToString();
        }

        //以執行緒holder tb
        void RunHolder()
        {
            if (Thread_Holder == null)
            {
                Thread_Holder = new Thread(_RunHolder);
                Thread_Holder.Start();
            }
            else
            {
                if (!Thread_Holder.IsAlive)
                    Thread_Holder.Start();
            }

        }
        //執行與顯示holder tb
        void _RunHolder()
        {

            //清除原有圖片
            cRD_Holder.Image = null;
            cRD_Holder.StaticGraphics.Clear();
            cRD_Holder.InteractiveGraphics.Clear();

            //執行結果
            ICogRecord rd, rd2;

            //取像
            tbHolder.Run();
            CogAcqFifoTool ff = (CogAcqFifoTool)tbHolder.Tools["CogAcqFifoTool1"];
            //ff.Run();
            CogImage8Grey im = (CogImage8Grey)ff.OutputImage;


            //丟給PMAlign
            Cognex.VisionPro.PMAlign.CogPMAlignTool PMA = (Cognex.VisionPro.PMAlign.CogPMAlignTool)tbHolder.Tools["CogPMAlignTool1"];
            //PMA.InputImage = im;
            //PMA.Run();

            if (PMA.RunStatus.Result == CogToolResultConstants.Accept)
            {
                CogTransform2DLinear Pose1 = new CogTransform2DLinear(), Pose2 = new CogTransform2DLinear();

                #region 判斷PMA抓到的特徵位置
                if (PMA.Results.Count == 0)
                {
                    isHolderOK1 = false;
                    isHolderOK2 = false;
                }
                else if (PMA.Results.Count == 1)
                {
                    if (PMA.Results[0].GetPose().TranslationY < 320)
                    {
                        Pose1 = PMA.Results[0].GetPose();
                        isHolderOK2 = false;
                        isHolderOK1 = true;
                    }
                    else
                    {
                        Pose2 = PMA.Results[0].GetPose();
                        isHolderOK1 = false;
                        isHolderOK2 = true;
                    }
                }
                else if (PMA.Results.Count == 2)
                {
                    isHolderOK1 = true;
                    isHolderOK2 = true;
                    if (PMA.Results[0].GetPose().TranslationY < PMA.Results[1].GetPose().TranslationY)
                    {
                        Pose1 = PMA.Results[0].GetPose();
                        Pose2 = PMA.Results[1].GetPose();
                    }
                    else
                    {
                        Pose1 = PMA.Results[1].GetPose();
                        Pose2 = PMA.Results[0].GetPose();
                    }
                }
                #endregion

                #region 將目前圖像丟入內部tb
                CogToolBlock stb = (CogToolBlock)tbHolder.Tools["TB"];
                //stb.Inputs["InputImage"].Value = im;
                #endregion

                #region 若特徵點抓取ok，則執行位置記憶
                if (isHolderOK1)
                {
                    //執行內部tb                    
                    stb.Inputs["GetPose"].Value = Pose1;
                    stb.Run();

                    Holder1.X = (double)stb.Outputs["X"].Value;
                    Holder1.Y = (double)stb.Outputs["Y"].Value;
                    Holder1.Angle = (double)stb.Outputs["U"].Value * 180 / Math.PI;
                    Holder1Size = (double)stb.Outputs["Size"].Value;

                    rd = stb.CreateLastRunRecord().SubRecords[1];
                    isHolderOK1 = stb.RunStatus.Result == CogToolResultConstants.Accept & (bool)stb.Outputs["CornerResult"].Value;
                }
                else
                {
                    rd = PMA.CreateLastRunRecord().SubRecords[0];
                }
                if (isHolderOK2)
                {
                    //執行內部tb                    
                    stb.Inputs["GetPose"].Value = Pose2;
                    stb.Run();

                    Holder2.X = (double)stb.Outputs["X"].Value;
                    Holder2.Y = (double)stb.Outputs["Y"].Value;
                    Holder2.Angle = (double)stb.Outputs["U"].Value * 180 / Math.PI;
                    Holder2Size = (double)stb.Outputs["Size"].Value;

                    rd2 = stb.CreateLastRunRecord().SubRecords[1];
                    isHolderOK2 = stb.RunStatus.Result == CogToolResultConstants.Accept & (bool)stb.Outputs["CornerResult"].Value;
                }
                else
                {
                    rd2 = PMA.CreateLastRunRecord().SubRecords[0];
                }
                rd.SubRecords.Add(rd2);
                cRD_Holder.Record = rd;
                cRD_Holder.Fit();
                #endregion
            }
            else
            {
                //執行失敗
                isHolderOK1 = false;
                isHolderOK2 = false;

            }
            lblHolderResult1.BackColor = isHolderOK1 ? Color.Green : Color.Red;
            lblHolderResult1.Text = isHolderOK1 ? "OK": "NG";
            lblHolderPos1.Text = Holder1.ToString();
            lblHolderSize1.Text = Holder1Size.ToString() + "mm";
            lblHolderResult2.BackColor = isHolderOK2 ? Color.Green : Color.Red;
            lblHolderResult2.Text = isHolderOK2 ? "OK" : "NG";
            lblHolderPos2.Text = Holder2.ToString();
            lblHolderSize2.Text = Holder2Size.ToString() + "mm";
        }

        /// <summary>計算位置，並回傳給robot
        /// 
        /// </summary>
        /// <param name="RX">robot 目前位置X</param>
        /// <param name="RY">robot 目前位置Y</param>
        /// <param name="RU">robot 目前位置U</param>
        void SetPosition(double RX, double RY, double RU)
        {
            #region 設定所有計算會用到的位置
            Robot.X = RX;
            Robot.Y = RY;
            Robot.Angle = RU;
            #endregion

            //計算並回傳組裝位
            if (isLensOK1 & isHolderOK1)
            {
                //Lens1.Angle = 92.845;
                //Holder1.Angle = 92.22;
                Assy1 = CaulAssy(Holder1, Lens1, Robot);
                Assy1.X = Assy1.X + Offset1.X;
                Assy1.Y = Assy1.Y + Offset1.Y;
                Assy1.Angle = Assy1.Angle + Offset1.U;
            }
            
                        File.AppendAllText  (Application.StartupPath+"\\abc.txt",  Lens1 .ToString ()+"   " +
                Holder1.ToString() + "   " +
                Assy1.ToString()+Environment .NewLine );



            if (isLensOK2 & isHolderOK2)
            {
                //Lens2.Angle = -90.648;
                //Holder2.Angle = -94.286;
                Assy2 = CaulAssy(Holder2, Lens2, Robot);
                Assy2.X = Assy2.X + Offset2.X;
                Assy2.Y = Assy2.Y + Offset2.Y;
                Assy2.Angle = Assy2.Angle + Offset2.U;
            }

            #region 確認組裝物與被組裝物大小是否超出spec
            //判斷大小是否可正常組入與tb執行是否正常
            string L1= isLensOK1 ? "OK" : "NG",
                L2=isLensOK2 ? "OK" : "NG",
            H1 = isHolderOK1 ? "OK" : "NG", 
            H2=isHolderOK2 ? "OK" : "NG";
            if (isLensOK1 & isHolderOK1)
            {
                if ((Holder1Size - Lens1Size) < AllowDisSize.U)
                    L1 = H1 = "NG";
                
            }
            if (isLensOK2 & isHolderOK2)
            {
                if ((Holder2Size - Lens2Size) < AllowDisSize.U)
                    L2 = H2 = "NG";               
            }
            #endregion

            //傳送檢測結果與組裝位置
            string report = L1 + "," + L2 + "," + H1 + "," + H2 + "," + Assy1.ToString() + "," + Assy2.ToString() + "\r\n";


            //string report = "NG" + "," + "NG" + "," + Assy1.ToString() + "," + Assy2.ToString() + "\r\n";
            //回應組裝座標
            Epson.WriteData(report);

            #region 顯示所有位置

            lblRobotPos.Text = Robot.ToString();

            lblAssyPos1.Text = Assy1.ToString();

            lblAssyPos2.Text = Assy2.ToString();
            #endregion

        }
        /// <summary>計算組裝位置
        /// 
        /// </summary>
        /// <param name="SamplePisitionAngle">組裝物</param>
        /// <param name="AssemblyPisitionAngle">被組裝物</param>
        /// <param name="RobotPoint_CCD1">組裝物照到下ccd的機械手位置</param>
        /// <returns></returns>
        RobotVisionPos.PositionAngle CaulAssy(RobotVisionPos.PositionAngle SamplePisitionAngle,
            RobotVisionPos.PositionAngle AssemblyPisitionAngle,
            RobotVisionPos.PositionAngle RobotPoint_CCD1)
        {
            RobotVisionPos.PositionAngle Result;

            RobotVisionPos josephclass = new RobotVisionPos(0, 0, 0, 0);    //初始化(基準偏移角度, 實際測試的Offset(X Y R 先設為0))
            Result = josephclass.Position_RobotAssembly(RobotPoint_CCD1, SamplePisitionAngle, AssemblyPisitionAngle);

            return Result;
        }
        /// <summary>套用99+勝郁
        /// 
        /// </summary>
        public class RobotVisionPos
        {

            public struct PositionAngle  //機器手  與  CCD位置(給定角度)
            {
                public double X;      //X座標
                public double Y;      //Y座標
                public double Angle;  //角度

                public override string ToString()
                {
                    string str = X.ToString("0.000") + "," + Y.ToString("0.000") + "," + Angle.ToString("0.000");


                    return str;
                }

            }

            public struct PositionTwoPoint  //CCD位置(給定角度算兩點)
            {
                public double X;         //X座標
                public double Y;         //Y座標
                public double StartX;    //開始X座標(用於算角度)
                public double StartY;    //開始Y座標
                public double EndX;      //結束X座標(用於算角度)
                public double EndY;      //結束Y座標

                public override string ToString()
                {
                    string str = X.ToString("0.000") + "," + Y.ToString("0.000") + "," + StartX.ToString("0.000") + "," + StartY.ToString("0.000") + "," + EndX.ToString("0.000") + "," + EndY.ToString("0.000");


                    return str;
                }
            }

            private double Angle_RefOffset; //基準線偏移角度  

            private double Angle_ActOffset; //實際測試偏移角度(當實際組裝時有一固定量Offset用)
            private double X_ActOffset; //實際測試偏移位置X(當實際組裝時有一固定量Offset用)
            private double Y_ActOffset; //實際測試偏移位置Y(當實際組裝時有一固定量Offset用)

            //初始化(基準線偏移角度、實際測試的Offset)
            public RobotVisionPos(double _Angle_RefOffset, double _X_ActOffset, double _Y_ActOffset, double _Angle_ActOffset)
            {
                Angle_RefOffset = _Angle_RefOffset; //基準線偏移角度

                X_ActOffset = _X_ActOffset;         //實際測試偏移位置X(當實際組裝時有一固定量Offset用、正負號以絕對座標系為主)
                Y_ActOffset = _Y_ActOffset;         //實際測試偏移位置Y(當實際組裝時有一固定量Offset用、正負號以絕對座標系為主)
                Angle_ActOffset = _Angle_ActOffset; //實際測試偏移角度(當實際組裝時有一固定量Offset用、正負號以絕對座標系為主)
            }

            //計算機器手組裝的座標(角度已給定)
            public PositionAngle Position_RobotAssembly(
                PositionAngle Position_Robot, PositionAngle Position_Assembly, PositionAngle Position_Assembled)
            {
                double Radius_AsseblyToZ;   //組裝物相對於Z軸中心的距離 R
                double Angle_AsseblyToZ;    //組裝物相對於Z軸中心的角度 θ0 
                double Angle_Delta;         //偏移角度      Δθ
                double TotalAngle;          //總計算角度    θ0 + Δθ
                double Displacement_X;      //偏移位置      ΔX
                double Displacement_Y;      //偏移位置      ΔY

                //計算組裝物相對於Z軸中心的半徑R
                Radius_AsseblyToZ = Math.Pow((
                    Math.Pow((Position_Assembly.X - Position_Robot.X), 2) +
                    Math.Pow((Position_Assembly.Y - Position_Robot.Y), 2)), 0.5);

                //計算組裝物相對於Z軸中心的角度θ0 
                Angle_AsseblyToZ = Angle_AtanDetermine(
                    Position_Robot.X, Position_Robot.Y, Position_Assembly.X, Position_Assembly.Y);
                //偏移角度      Δθ
                Angle_Delta = Position_Assembled.Angle - Position_Assembly.Angle + Angle_RefOffset + Angle_ActOffset;//Offset Angle
                //計算總計算角度θ0 + Δθ
                TotalAngle = Angle_AsseblyToZ + Angle_Delta;
                //計算偏移量ΔX
                Displacement_X = Radius_AsseblyToZ * Math.Cos(TotalAngle * (Math.PI / 180));
                //計算偏移量ΔY
                Displacement_Y = Radius_AsseblyToZ * Math.Sin(TotalAngle * (Math.PI / 180));

                //計算機器手組裝的座標
                PositionAngle Position_Result;
                Position_Result.X = Position_Assembled.X - Displacement_X + X_ActOffset;    //Offset X
                Position_Result.Y = Position_Assembled.Y - Displacement_Y + Y_ActOffset;    //Offset Y
                Position_Result.Angle = Position_Robot.Angle + Angle_Delta;

                //回傳座標
                return Position_Result;
            }


            //計算機器手組裝的座標(不給角度，僅給定計算角度的兩點位置)
            public PositionAngle Position_RobotAssembly(
                PositionAngle Position_Robot, PositionTwoPoint PositionTwo_Assembly, PositionTwoPoint PositionTwo_Assembled)
            {
                //組裝物與被組裝物的參數
                PositionAngle Position_Assembly;
                PositionAngle Position_Assembled;

                //組裝物與被組裝物的XY
                Position_Assembly.X = PositionTwo_Assembly.X;
                Position_Assembly.Y = PositionTwo_Assembly.Y;
                Position_Assembled.X = PositionTwo_Assembled.X;
                Position_Assembled.Y = PositionTwo_Assembled.Y;

                //計算組裝物偏差角度(回傳-180~180)
                Position_Assembly.Angle = Angle_AtanDetermine(
                    PositionTwo_Assembly.StartX, PositionTwo_Assembly.StartY, PositionTwo_Assembly.EndX, PositionTwo_Assembly.EndY);

                //計算被組裝物偏差角度(回傳-180~180)
                Position_Assembled.Angle = Angle_AtanDetermine(
                    PositionTwo_Assembled.StartX, PositionTwo_Assembled.StartY, PositionTwo_Assembled.EndX, PositionTwo_Assembled.EndY);

                //計算機器手組裝的座標
                PositionAngle Position_Result;

                //計算機器手組裝座標值
                Position_Result = Position_RobotAssembly(Position_Robot, Position_Assembly, Position_Assembled);

                //回傳座標
                return Position_Result;
            }

            //計算θ0所在的象限位置(回傳-180~+180)
            public static double Angle_AtanDetermine(double Start_X, double Start_Y, double End_X, double End_Y)
            {
                double _Angle = 0;
                double Atan_X = End_X - Start_X;
                double Atan_Y = End_Y - Start_Y;

                _Angle = Math.Atan((Atan_Y) / (Atan_X)) / Math.PI * 180;

                if (Atan_X > 0)
                {
                    if (Atan_Y >= 0) //第一象限
                    { _Angle = _Angle + 0; }
                    else //第四象限
                    { _Angle = _Angle + 0; }
                }
                else if (Atan_X < 0)
                {
                    if (Atan_Y >= 0) //第二象限
                    { _Angle = _Angle + 180; }
                    else //第三象限
                    { _Angle = _Angle - 180; }
                }
                else if (Atan_X == 0)
                {
                    if (Atan_Y > 0)     //在正Y軸上
                    { _Angle = 90; }
                    else if (Atan_Y < 0)//在負Y軸上
                    { _Angle = 270; }
                    else if (Atan_Y == 0)
                    { System.Windows.Forms.MessageBox.Show("定位點在Z軸中心上，請重新確認"); }
                }

                return _Angle;
            }

        }
        #endregion

        #region Scara_OKTray

        public delegate void CommandFromRobot_OKdelegate(string ms);

        //成品萃的robot指令動作
        void CommandFromRobot_OK(string str)
        {

            if (InvokeRequired == true)
            {
                CommandFromRobot_OKdelegate m = new CommandFromRobot_OKdelegate(CommandFromRobot_OK);
                object[] eventargs = { str };
                BeginInvoke(m, eventargs);
            }
            else
            {
                //string ssss = str.Trim();
                string[] SS = str.Split(',');
                if (SS[0] == "")
                    return;
                if (SS[0] == "C")
                {
                    cRD_OKTray.Image = null;
                    cRD_OKTray.StaticGraphics.Clear();
                    cRD_OKTray.InteractiveGraphics.Clear();
                    tbOKTray.Run();
                    cRD_OKTray.Record = tbOKTray.CreateLastRunRecord().SubRecords[0];
                    cRD_OKTray.Fit();
                    Cognex.VisionPro.PMAlign.CogPMAlignTool PMA = (Cognex.VisionPro.PMAlign.CogPMAlignTool)tbOKTray.Tools["CogPMAlignTool1"];
                    if (PMA.Results.Count  == 1)
                    {
                        double XX = (double)tbOKTray.Outputs["X"].Value;
                        double YY = (double)tbOKTray.Outputs["Y"].Value;
                        ClassXYPos PP = new ClassXYPos(XX, YY);
                        //int idx = int.Parse(SS[0].Substring(1, 1)) - 1;

                        Epson_OK.WriteData("OK," + XX.ToString()+","+YY.ToString() + "\r\n");
                        lblXY1.Text = XX.ToString("0.000") + "," + YY.ToString("0.000");
                    }
                    else
                    {
                        Epson_OK.WriteData("NG" + ",0,0" + "\r\n");
                        return;
                    }

                }
            }

        }

        private void bgW_EpsonOK_DoWork(object sender, DoWorkEventArgs e)
        {
            string str = Epson_OK.ReadData();
            CommandFromRobot_OK(str);
        }

        private void bgW_EpsonOK_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgW_EpsonOK.RunWorkerAsync();
        }



        #endregion

        #region User
        private void tSBLogin_Click(object sender, EventArgs e)
        {
            frmLogin f = new frmLogin();
            f.ShowDialog();
            SetLimit();
        }

        private void tSBUser_Click(object sender, EventArgs e)
        {
            frmUserConfig f = new frmUserConfig();
            f.ShowDialog();
            SetLimit();
        }

        private void tSBLogOut_Click(object sender, EventArgs e)
        {
            User_Set.NowUser = User_Set.NoLogIn;
            SetLimit();
        }
        void SetLimit()
        {
            tSBUser.Enabled = User_Set.NowUser.Limit_ConfigUser;
            tsbTBConfig.Enabled = User_Set.NowUser.Limit_ConfigTB;
            tSBConfig.Enabled = User_Set.NowUser.Limit_ConfigNormal;
        }
        #endregion

        //所有tb的編輯
        private void tSBLensTray_Click(object sender, EventArgs e)
        {
            Splasher Sp = new Splasher();
            Sp.Show("開啟中");
            ToolStripMenuItem s = (ToolStripMenuItem)sender;
            frmTB f = null;
            for (int i = 0; i < List_tSBTBEdit.Count; i++)
            {
                if (s == List_tSBTBEdit[i])
                {
                    f = new frmTB(List_tb[i], string.Concat(System.IO.Directory.GetCurrentDirectory().ToString(), "\\TB\\" + strtbName[i] + ".vpp"));
                    Sp.Close();
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.Yes)
                    {
                        switch (s.Name)
                        {
                            case "tSBLensTray":
                                tbLensTray = (CogToolBlock)CogSerializer.DeepCopyObject(f.ResultTB);
                                break;
                            case "tSBHolderTray":
                                tbHolderTray = (CogToolBlock)CogSerializer.DeepCopyObject(f.ResultTB);
                                break;
                            case "tSBLens":
                                tbLens = (CogToolBlock)CogSerializer.DeepCopyObject(f.ResultTB);
                                break;
                            case "tSBHolder1":
                                tbHolder = (CogToolBlock)CogSerializer.DeepCopyObject(f.ResultTB);
                                break;
                            case "tSBOKTray":
                                tbOKTray = (CogToolBlock)CogSerializer.DeepCopyObject(f.ResultTB);
                                break;
                            case "tSBAssyOK":
                                tbAssyOK = (CogToolBlock)CogSerializer.DeepCopyObject(f.ResultTB);
                                break;
                        }
                        List_tb = new List<CogToolBlock> { tbLensTray, tbHolderTray, tbLens, tbHolder, tbAssyOK, tbOKTray };
                    }
                    break;
                }
            }
            f.Dispose();

        }

        //顯示組裝設定
        private void tSBOffset_Click(object sender, EventArgs e)
        {
            frmOffset f = new frmOffset(this);
            f.ShowDialog();
        }

        //執行tb，並顯示
        private void btnRun_Holder_Click(object sender, EventArgs e)
        {
            Button s = (Button)sender;
            for (int i = 0; i < List_Btn.Count; i++)
            {
                //除了lens萃盤和holder萃盤要另外run與另外取得結果圖像，其他皆run和顯示
                if (List_Btn[i] == s)
                {
                    if (List_Btn[i] == btnRun_LensTray)
                        _RunLensTray();
                    else if (List_Btn[i] == btnRun_HolderTray)
                        _RunHolderTray();
                    else if (List_Btn[i] == btnRun_Holder)
                        _RunHolder();
                    else if (List_Btn[i] == btnRun_Lens)
                        _RunLens();
                    else
                    {
                        List_cRD[i].Image = null;
                        List_cRD[i].StaticGraphics.Clear();
                        List_cRD[i].InteractiveGraphics.Clear();
                        List_tb[i].Run();
                        List_cRD[i].Record = List_tb[i].CreateLastRunRecord().SubRecords[0];
                        List_cRD[i].Fit();
                    }
                }
            }

        }

        private void cRD_HolderTray_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Splasher s = new Splasher();
            s.Show("Connecting");
            bgW_Epson.CancelAsync();
            Epson.DisConnect();
            if (Epson.ConnectEpson()) 
            {
                if (!bgW_Epson.IsBusy)
                    bgW_Epson.RunWorkerAsync();
                isRBError = false;
            }
            else
            {
            isRBError=true;
            }
            Thread.Sleep(100);
            s.Close();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string SS = "";
            if (isPLCError)
                SS += "PLC連線異常";
            if (isRBError)
                SS += "機器手(Lens Holder)連線異常   ";
            if (isRB_OKTrayError)
                SS += "機器手(成品萃盤)連線異常";

            isRBError = !Epson.isConnect;
            isPLCError = !PLC.isConnect;
            isRB_OKTrayError = !Epson_OK.isConnect;
            tSSLConnStatus.Text = SS;
            tSSLConnStatusLamp.BackColor = Color.Red;

        }



        private void bgw_ReConn_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                #region EPSON(Lens Holder)連線
                if (!Epson.isConnect)
                {
                    bgW_Epson.CancelAsync();
                    Epson.DisConnect();
                    if (Epson.ConnectEpson())
                    {
                        if (!bgW_Epson.IsBusy)
                            bgW_Epson.RunWorkerAsync();
                        isRBError = false;
                    }
                    else
                    {
                        isRBError = true;
                    }
                }
                #endregion

                #region EPSON(成品)連線
                if (!Epson_OK.isConnect)
                {
                    bgW_EpsonOK.CancelAsync();
                    Epson_OK.DisConnect();
                    if (Epson_OK.ConnectEpson())
                    {
                        if (!bgW_EpsonOK.IsBusy)
                            bgW_EpsonOK.RunWorkerAsync();
                        isRB_OKTrayError = false;
                    }
                    else
                    {
                        isRB_OKTrayError = true;
                    }
                }
                #endregion

                #region PLC連線
                if (!PLC.isConnect)
                {
                    bgWPLC.CancelAsync();
                    PLC.DisConnect();
                    if (PLC.ConnectPLC())
                    {
                        if (!bgWPLC.IsBusy)
                            bgWPLC.RunWorkerAsync();
                        isPLCError = false;
                    }
                    else
                    {
                        isPLCError = true;
                    }
                
                }
                #endregion

                Thread.Sleep(100);
            }
        }

        private void btnSaveHolder_Click(object sender, EventArgs e)
        {
            DateTime dd = DateTime.Now;

            Image AA = cRD_Holder.CreateContentBitmap(Cognex.VisionPro.Display.CogDisplayContentBitmapConstants.Image);
            string s = DateTime.Now.ToString();
            s=s.Replace(':', '.');
            s=s.Replace('/', '.');
            AA.Save (Application.StartupPath +"\\SavePic\\"+ s+".bmp");
            this.Text = (DateTime.Now - dd).TotalMilliseconds.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetPosition(2.584, 318.904, -39.713);
        }

    }
}


