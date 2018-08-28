using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace casedefect
{
    public partial class frmSplash : Form
    {
        private int count = 0;
        private String strStatus;
        delegate void delStatus();
        string[] strRnd = {
                          "在圖面上雙擊滑鼠左鍵，可以將圖面容器放大或縮小",
                          "在圖面上滑動滑鼠滾輪，可縮放圖面",
                          "在圖面上按右鍵，可顯示圖面選項"};

        public frmSplash()
        {
            InitializeComponent();
        }

        public string Status
        {
            get 
            { 
                return strStatus; 
            }
            set
            {
                strStatus = value;
                ChangeText();
            }
        }

        private void ChangeText()
        {
            if (this.InvokeRequired)
            {
                delStatus m = new delStatus(ChangeText);
                this.Invoke(m);
            }
            else
                lblStatus.Text = strStatus;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value >= progressBar1.Minimum & progressBar1.Value < progressBar1.Maximum)
                count++;
            else
                count = 1;
            progressBar1.Value = count;
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            this.Text = "";
            Random Rnd = new Random();
            label1.Text=strRnd[Rnd.Next(strRnd.Length)];
            timer1.Enabled = true;
        }

        private void frmSplash_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
