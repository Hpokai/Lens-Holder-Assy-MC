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
    public partial class frmUserNew : Form
    {
        public string UserName, Password;
        public frmUserNew(string UN, string PW, string title)
        {
            InitializeComponent();
            this.Text = title;
            UserName = UN;
            Password = PW;
            TextBox_UserName.Text = UN;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (TextBox_UserName.Text == "" | TextBox_UserPassword.Text == "" | TextBox_UserPassword2.Text == "")
            {
                MessageBox.Show("請輸入完整資訊", "資料缺失", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (TextBox_UserPassword.Text != TextBox_UserPassword.Text)
                {
                    MessageBox.Show("密碼相異", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    UserName = TextBox_UserName.Text;
                    Password = TextBox_UserPassword.Text;
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUserNew_Load(object sender, EventArgs e)
        {

        }


    }
}
