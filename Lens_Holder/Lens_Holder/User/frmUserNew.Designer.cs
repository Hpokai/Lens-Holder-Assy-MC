namespace casedefect
{
    partial class frmUserNew
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.TextBox_UserPassword = new System.Windows.Forms.TextBox();
            this.TextBox_UserName = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.TextBox_UserPassword2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(93, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 25);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TextBox_UserPassword
            // 
            this.TextBox_UserPassword.Location = new System.Drawing.Point(107, 31);
            this.TextBox_UserPassword.Name = "TextBox_UserPassword";
            this.TextBox_UserPassword.PasswordChar = '*';
            this.TextBox_UserPassword.Size = new System.Drawing.Size(70, 22);
            this.TextBox_UserPassword.TabIndex = 11;
            // 
            // TextBox_UserName
            // 
            this.TextBox_UserName.Location = new System.Drawing.Point(107, 6);
            this.TextBox_UserName.Name = "TextBox_UserName";
            this.TextBox_UserName.Size = new System.Drawing.Size(70, 22);
            this.TextBox_UserName.TabIndex = 10;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(12, 9);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(65, 12);
            this.Label6.TabIndex = 13;
            this.Label6.Text = "使用者名稱";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 34);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(29, 12);
            this.Label2.TabIndex = 14;
            this.Label2.Text = "密碼";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(5, 97);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 25);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // TextBox_UserPassword2
            // 
            this.TextBox_UserPassword2.Location = new System.Drawing.Point(107, 57);
            this.TextBox_UserPassword2.Name = "TextBox_UserPassword2";
            this.TextBox_UserPassword2.PasswordChar = '*';
            this.TextBox_UserPassword2.Size = new System.Drawing.Size(70, 22);
            this.TextBox_UserPassword2.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "再輸入一次密碼";
            // 
            // frmUserNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(188, 130);
            this.Controls.Add(this.TextBox_UserPassword2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.TextBox_UserPassword);
            this.Controls.Add(this.TextBox_UserName);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.btnOK);
            this.Name = "frmUserNew";
            this.Text = "frmUserNew";
            this.Load += new System.EventHandler(this.frmUserNew_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.TextBox TextBox_UserPassword;
        internal System.Windows.Forms.TextBox TextBox_UserName;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.TextBox TextBox_UserPassword2;
        internal System.Windows.Forms.Label label1;
    }
}