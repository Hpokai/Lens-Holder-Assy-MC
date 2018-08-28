using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace casedefect
{
    class User_Set
    {
        public static User Me;
        public static User NoLogIn;
        public static User NowUser;
        public static string UserFolder = Application.StartupPath + "\\User\\";
        public static void ini()
        {
            NoLogIn = new User();
            NoLogIn.UserName = "未登入";

            NowUser = NoLogIn;

            Me=new User();
            Me.UserName = "Jabil";
            Me.PassWord = "7316";
            Me.Limit_ConfigNormal  = true;
            Me.Limit_ConfigTB = true;
            Me.Limit_ConfigUser = true;

            //創建user資料夾
            if (!Directory.Exists(User_Set.UserFolder))
                Directory.CreateDirectory(User_Set.UserFolder);
        }
        public static bool SaveUser(User SU)
        {
            try
            {
                string SS;
                SS = SU.PassWord + "\r\n"
                   + SU.Limit_ConfigNormal + "\r\n"
                   + SU.Limit_ConfigUser + "\r\n"
                   + SU.Limit_ConfigTB + "\r\n";
                File.WriteAllText(UserFolder+SU.UserName +".user",SS);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "設定失敗");
                return false;
            }
        }
        public static bool LoadUser(ref User SU,string UserName)
        {
            try
            {
                string LP = UserFolder + UserName + ".user";
                string[] LoadStr;
                if(File.Exists (LP))
                    LoadStr = File.ReadAllLines(LP);
                else
                {
                    MessageBox.Show ("使用者名稱錯誤");
                    return false;
                }
                if (LoadStr != null)
                    if (LoadStr.Length >= 4)
                    {
                        SU.UserName = UserName;
                        SU.PassWord = LoadStr[0];
                        SU.Limit_ConfigNormal  = LoadStr[1] == "True";
                        SU.Limit_ConfigUser = LoadStr[2] == "True";
                        SU.Limit_ConfigTB = LoadStr[3] == "True";
                    }
                    else
                        return false;
                else
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public static bool DelUser(User SU)
        {
            string FP = UserFolder + SU.UserName + ".user";
            try
            {
                if (File.Exists(FP))
                {
                    File.Delete(FP);
                    return true;
                }
                else
                {
                    MessageBox.Show("使用者不存在", "使用者不存在", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message , ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
