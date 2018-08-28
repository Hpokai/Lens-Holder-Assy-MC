using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;

namespace Lens_Holder
{

    public partial class frmTB : Form
    {
        //CogToolBlock Form2TB = new CogToolBlock();
        static String strPath;

        private CogToolBlock EditTB;

        public CogToolBlock ResultTB
        {
            get{return EditTB ;}
            set {EditTB =value;}
        }

        public frmTB(CogToolBlock tb, String path  )
        {
            InitializeComponent();
            EditTB =new CogToolBlock ();
            EditTB = (CogToolBlock)CogSerializer.DeepCopyObject(tb);
            cogToolBlockEditV21.Subject = EditTB; 
            strPath = path;
            this.Text = path;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = MessageBox.Show("SAVE", "SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (DialogResult == DialogResult.Yes)
            {
                EditTB = cogToolBlockEditV21.Subject;
                CogSerializer.SaveObjectToFile(EditTB, strPath);
            }
            else if(DialogResult==DialogResult.Cancel )
                e.Cancel =true;
        }

        private void frmTB_Load(object sender, EventArgs e)
        {

        }
    }
}
