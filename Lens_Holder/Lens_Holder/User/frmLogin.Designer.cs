namespace casedefect
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_LogCancel = new System.Windows.Forms.Button();
            this.TextBox_UserPassword = new System.Windows.Forms.TextBox();
            this.TextBox_UserName = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.btn_LogIn = new System.Windows.Forms.Button();
            this.Label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_LogCancel
            // 
            this.btn_LogCancel.Location = new System.Drawing.Point(93, 59);
            this.btn_LogCancel.Name = "btn_LogCancel";
            this.btn_LogCancel.Size = new System.Drawing.Size(84, 25);
            this.btn_LogCancel.TabIndex = 9;
            this.btn_LogCancel.Text = "取消";
            this.btn_LogCancel.UseVisualStyleBackColor = true;
            this.btn_LogCancel.Click += new System.EventHandler(this.btn_LogCancel_Click);
            // 
            // TextBox_UserPassword
            // 
            this.TextBox_UserPassword.Location = new System.Drawing.Point(71, 31);
            this.TextBox_UserPassword.Name = "TextBox_UserPassword";
            this.TextBox_UserPassword.PasswordChar = '*';
            this.TextBox_UserPassword.Size = new System.Drawing.Size(70, 22);
            this.TextBox_UserPassword.TabIndex = 1;
            this.TextBox_UserPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_UserPassword_KeyDown);
            
            // 
            // TextBox_UserName
            // 
            this.TextBox_UserName.Location = new System.Drawing.Point(71, 6);
            this.TextBox_UserName.Name = "TextBox_UserName";
            this.TextBox_UserName.Size = new System.Drawing.Size(70, 22);
            this.TextBox_UserName.TabIndex = 0;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 34);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(48, 12);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "Password";
            // 
            // btn_LogIn
            // 
            this.btn_LogIn.Location = new System.Drawing.Point(5, 59);
            this.btn_LogIn.Name = "btn_LogIn";
            this.btn_LogIn.Size = new System.Drawing.Size(82, 25);
            this.btn_LogIn.TabIndex = 3;
            this.btn_LogIn.Text = "登入";
            this.btn_LogIn.UseVisualStyleBackColor = true;
            this.btn_LogIn.Click += new System.EventHandler(this.btn_LogIn_Click);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(12, 9);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(53, 12);
            this.Label6.TabIndex = 6;
            this.Label6.Text = "UserName";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(189, 93);
            this.Controls.Add(this.btn_LogCancel);
            this.Controls.Add(this.TextBox_UserPassword);
            this.Controls.Add(this.TextBox_UserName);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.btn_LogIn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "使用者登入介面";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btn_LogCancel;
        internal System.Windows.Forms.TextBox TextBox_UserPassword;
        internal System.Windows.Forms.TextBox TextBox_UserName;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button btn_LogIn;
        internal System.Windows.Forms.Label Label6;
    }
}