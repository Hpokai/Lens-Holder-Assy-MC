namespace casedefect
{
    partial class frmUserConfig
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
            this.chkConfigTB = new System.Windows.Forms.CheckBox();
            this.chkConfigUser = new System.Windows.Forms.CheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkConfigNormal = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnChangeUser = new System.Windows.Forms.Button();
            this.btnDelUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkConfigTB
            // 
            this.chkConfigTB.AutoSize = true;
            this.chkConfigTB.Location = new System.Drawing.Point(18, 31);
            this.chkConfigTB.Name = "chkConfigTB";
            this.chkConfigTB.Size = new System.Drawing.Size(117, 16);
            this.chkConfigTB.TabIndex = 1;
            this.chkConfigTB.Text = "編輯TOOLBLOCK";
            this.chkConfigTB.UseVisualStyleBackColor = true;
            this.chkConfigTB.Click += new System.EventHandler(this.chkConfigUser_Click);
            // 
            // chkConfigUser
            // 
            this.chkConfigUser.AutoSize = true;
            this.chkConfigUser.Location = new System.Drawing.Point(18, 76);
            this.chkConfigUser.Name = "chkConfigUser";
            this.chkConfigUser.Size = new System.Drawing.Size(84, 16);
            this.chkConfigUser.TabIndex = 3;
            this.chkConfigUser.Text = "使用者設定";
            this.chkConfigUser.UseVisualStyleBackColor = true;
            this.chkConfigUser.Click += new System.EventHandler(this.chkConfigUser_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(11, 21);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(157, 148);
            this.listBox1.TabIndex = 4;
            this.listBox1.Click += new System.EventHandler(this.listBox1_SelectedValueChanged);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(196, 249);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(279, 249);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkConfigNormal);
            this.groupBox1.Controls.Add(this.chkConfigTB);
            this.groupBox1.Controls.Add(this.chkConfigUser);
            this.groupBox1.Location = new System.Drawing.Point(204, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 212);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "權限設定";
            // 
            // chkConfigNormal
            // 
            this.chkConfigNormal.AutoSize = true;
            this.chkConfigNormal.Location = new System.Drawing.Point(18, 131);
            this.chkConfigNormal.Name = "chkConfigNormal";
            this.chkConfigNormal.Size = new System.Drawing.Size(72, 16);
            this.chkConfigNormal.TabIndex = 4;
            this.chkConfigNormal.Text = "一般設定";
            this.chkConfigNormal.UseVisualStyleBackColor = true;
            this.chkConfigNormal.Click += new System.EventHandler(this.chkConfigUser_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnChangeUser);
            this.groupBox2.Controls.Add(this.btnDelUser);
            this.groupBox2.Controls.Add(this.btnAddUser);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Location = new System.Drawing.Point(12, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(174, 212);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "使用者";
            // 
            // btnChangeUser
            // 
            this.btnChangeUser.Location = new System.Drawing.Point(121, 176);
            this.btnChangeUser.Name = "btnChangeUser";
            this.btnChangeUser.Size = new System.Drawing.Size(47, 30);
            this.btnChangeUser.TabIndex = 7;
            this.btnChangeUser.Text = "修改";
            this.btnChangeUser.UseVisualStyleBackColor = true;
            this.btnChangeUser.Click += new System.EventHandler(this.btnChangeUser_Click);
            // 
            // btnDelUser
            // 
            this.btnDelUser.Location = new System.Drawing.Point(68, 176);
            this.btnDelUser.Name = "btnDelUser";
            this.btnDelUser.Size = new System.Drawing.Size(47, 30);
            this.btnDelUser.TabIndex = 6;
            this.btnDelUser.Text = "刪除";
            this.btnDelUser.UseVisualStyleBackColor = true;
            this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(15, 176);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(47, 30);
            this.btnAddUser.TabIndex = 5;
            this.btnAddUser.Text = "新增";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // frmUserConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 284);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "frmUserConfig";
            this.Text = "UserConfig";
            this.Load += new System.EventHandler(this.UserConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkConfigTB;
        private System.Windows.Forms.CheckBox chkConfigUser;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnChangeUser;
        private System.Windows.Forms.Button btnDelUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.CheckBox chkConfigNormal;
    }
}