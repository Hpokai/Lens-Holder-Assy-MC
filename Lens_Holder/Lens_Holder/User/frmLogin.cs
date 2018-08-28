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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btn_LogIn_Click(object sender, EventArgs e)
        {
            if (TextBox_UserName.Text != "" & TextBox_UserPassword.Text != "")
            {
                if (TextBox_UserName.Text == User_Set.Me.UserName & TextBox_UserPassword.Text == User_Set.Me.PassWord)
                {
                    User_Set.NowUser = User_Set.Me;
                    MessageBox.Show("登入成功", "登入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }

                User uu=new User();
                if (User_Set.LoadUser(ref uu, TextBox_UserName.Text))
                {
                    User_Set.NowUser = uu;
                    MessageBox.Show("登入成功", "登入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
                MessageBox.Show("請輸入帳號及密碼", "登入資訊不完全", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_LogCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox_UserPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_LogIn_Click(null, null);
        }
    }
}
