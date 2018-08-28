using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//for running threads
using System.Net.Sockets;//use this namespace for sockets
namespace Lens_Holder
{
    class EpsonConn
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public EpsonConn(string _IP, int _Port)
        {
            IP = _IP;
            Port = _Port;
        }


        //宣告網路資料流變數2
        NetworkStream myNetworkStream;
        //宣告 Tcp 用戶端物件
        TcpClient myTcpClient;
        Thread Thread_ConnectState;
        public bool isConnect; 
        public string RecieveData;
        public bool ConnectEpson()
        {
            myTcpClient = new TcpClient();
            try
            {
                //測試連線至遠端主機
                myTcpClient.Connect(IP, Port);
                //建立網路資料流
                myNetworkStream = myTcpClient.GetStream();
                isConnect = true;
                Thread_ConnectState = new Thread(ConnectState);
                Thread_ConnectState.Start();
                return true;
            }
            catch
            {
                isConnect = false ;
                myTcpClient.Close();
                return false;
            }
        }

        void ConnectState()
        {
            byte[] testRecByte = new byte[1];
            try
            {
                while (myTcpClient.Connected)
                {
                    if (myTcpClient.Client.Receive(testRecByte, SocketFlags.Peek) == 0)
                        break;
                    Thread.Sleep(100);
                    isConnect = true;
                }
                isConnect = false;
            }
            catch
            {
                isConnect = false;
            }
            

        }

        //讀取資料
        public string ReadData()
        {
            try
            {
                //從網路資料流讀取資料
                int bufferSize = myTcpClient.ReceiveBufferSize;
                byte[] myBufferBytes = new byte[bufferSize];
                myNetworkStream.Read(myBufferBytes, 0, bufferSize);
                //取得資料並且解碼文字
                return (Encoding.ASCII.GetString(myBufferBytes, 0, bufferSize));
            }
            catch
            {
                return "";
            }
        }
        //寫入資料
        public void WriteData(string SSS)
        {

            //將字串轉 byte 陣列，使用 ASCII 編碼
            Byte[] myBytes = Encoding.ASCII.GetBytes(SSS);
            try
            {
                myNetworkStream.Write(myBytes, 0, myBytes.Length);
            }
            catch (Exception ex)
            {
            }
        }
        public void DisConnect()
        {
            
            if(myNetworkStream!=null )
                myNetworkStream.Close();
            myTcpClient.Close();

        }

    }
}
