using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace casedefect
{
    public partial class frmUserConfig : Form
    {
        public frmUserConfig()
        {
            InitializeComponent();
        }
        User[] AllUser;
        private void UserConfig_Load(object sender, EventArgs e)
        {
            string[] txtFiles = Directory.EnumerateFiles(User_Set.UserFolder, "*.user").ToArray();
            string fileName;
            AllUser = new User[txtFiles.Length];
            listBox1.Items.Clear();

            User LU=new User();
            int ii=0;
            for (int i = 0; i < txtFiles.Length; i++)
            {
                fileName = txtFiles[i].Substring(User_Set.UserFolder.Length);
                fileName = fileName.Substring(0, fileName.Length - 5);
                AllUser[ii] = new User();
                if (User_Set.LoadUser(ref AllUser[ii], fileName))
                {
                    listBox1.Items.Add(fileName);
                    ii++;
                }
            }

            Array.Resize(ref  AllUser, ii);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmUserNew fu = new frmUserNew("","", "新增");
            fu.ShowDialog();
            int aa = 0;
            if (fu.UserName != "")
            {
                if (listBox1.Items.IndexOf(fu.UserName) != -1)
                {
                    MessageBox.Show("使用者名稱已存在", "已存在", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    aa = AllUser.Length;
                    Array.Resize(ref  AllUser, aa + 1);
                    AllUser[aa] = new User();
                    AllUser[aa].UserName = fu.UserName;
                    AllUser[aa].PassWord = fu.Password;
                    listBox1.Items.Add(fu.UserName);
                }
            }
        }

        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            int LSI=listBox1.SelectedIndex;
            if (LSI ==-1 )
                return;
            
            frmUserNew fu = new frmUserNew(AllUser[LSI].UserName, AllUser[LSI].PassWord, "修改");
            fu.ShowDialog();
            AllUser[LSI].UserName = fu.UserName;
            AllUser[LSI].PassWord = fu.Password;
            listBox1.Items[LSI] = fu.UserName;
        }
        private void btnDelUser_Click(object sender, EventArgs e)
        {
            int LSI=listBox1.SelectedIndex;
            if (LSI == -1)
                return;
            listBox1.Items.RemoveAt(LSI);

            User[] NU;
            if (AllUser.Length == 1)
                AllUser = new User[0];
            else
            {
                NU = new User[AllUser.Length - 1];
                int j=0;
                for (int i = 0; i < AllUser.Length; i++)
                {
                    if (i != LSI)
                    {
                        NU[j] = AllUser[i];
                        j++;
                    }
                }
                AllUser = NU;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            string[] txtFiles = Directory.EnumerateFiles(User_Set.UserFolder, "*.user").ToArray();
  

            foreach (string item in txtFiles)
            {
                File.Delete(item);
            }


            for (int i = 0; i < AllUser.Length; i++)
            {
                User_Set.SaveUser(AllUser[i]);
                this.Close();
            }
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            int LSI = listBox1.SelectedIndex;
            if (LSI != -1)
            {

                chkConfigTB.Checked = AllUser[LSI].Limit_ConfigTB;
                chkConfigUser.Checked = AllUser[LSI].Limit_ConfigUser;
                chkConfigNormal.Checked = AllUser[LSI].Limit_ConfigNormal ;
            }
        }

        private void chkConfigUser_Click(object sender, EventArgs e)
        {
            int LSI = listBox1.SelectedIndex;
            if (LSI != -1)
            {
                AllUser[LSI].Limit_ConfigTB = chkConfigTB.Checked;
                AllUser[LSI].Limit_ConfigUser = chkConfigUser.Checked;
                AllUser[LSI].Limit_ConfigNormal = chkConfigNormal.Checked;
            }
        }



    }
}
