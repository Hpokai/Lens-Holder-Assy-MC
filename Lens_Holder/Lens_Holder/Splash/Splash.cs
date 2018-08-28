//using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using casedefect;

namespace JGP_Splash
{
    public class Splasher
    {
        private frmSplash MySplashForm;

        private Thread MySplashThread;
        private void ShowThread()
        {
            Application.Run(MySplashForm);
        }

        public string mStatus
        {
            get
            {
                if (MySplashForm == null)
                    return "";
                else
                    return MySplashForm.Status;
            }
            set
            {
                if (MySplashForm == null)
                    return;
                MySplashForm.Status = value;
            }
        }

        public void Show(string strName)
        {
            if (MySplashThread != null)
                return;


            MySplashForm = new frmSplash();
            this.mStatus = strName;

            MySplashThread = new Thread(new ThreadStart(ShowThread));
            MySplashThread.IsBackground = true;
            MySplashThread.SetApartmentState(ApartmentState.STA);
            MySplashThread.Start();
        }

        public void Close()
        {
            if (MySplashThread == null)
                return;

            if (MySplashForm == null)
                return;

            try
            {
                MySplashForm.Invoke(new MethodInvoker(MySplashForm.Close));
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            MySplashForm = null;
            MySplashThread = null;
        }
    }
}