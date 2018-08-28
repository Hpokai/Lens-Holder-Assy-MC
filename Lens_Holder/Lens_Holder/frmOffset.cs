using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lens_Holder
{
    public partial class frmOffset : Form
    {
        Form1 fm1;
        public frmOffset(Form1 fm)
        {
            fm1 = fm;
            InitializeComponent();
        }
        
        private void frmOffset_Load(object sender, EventArgs e)
        {
            txtX1.Text = fm1.Offset1.X.ToString();
            txtY1.Text = fm1.Offset1.Y.ToString();
            txtU1.Text = fm1.Offset1.U.ToString();
            txtX2.Text = fm1.Offset2.X.ToString();
            txtY2.Text = fm1.Offset2.Y.ToString();
            txtU2.Text = fm1.Offset2.U.ToString();
            txtAllowDisSize.Text = fm1.AllowDisSize.U.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            double X1, Y1, U1, X2, Y2, U2,A1;
            bool isOK;
            isOK = double.TryParse(txtX1.Text, out X1) &
                    double.TryParse(txtY1.Text, out Y1) &
                    double.TryParse(txtU1.Text, out U1) &
                    double.TryParse(txtX2.Text, out X2) &
                    double.TryParse(txtY2.Text, out Y2) &
                    double.TryParse(txtU2.Text, out U2) &
                    double.TryParse(txtAllowDisSize.Text  ,out A1);
            if (isOK)
            {

                fm1.Offset1.setPos(X1, Y1, U1);
                fm1.Offset2.setPos(X2, Y2, U2);

                fm1.Offset1.Save();
                fm1.Offset2.Save();

                fm1.AllowDisSize.U = A1;
                fm1.AllowDisSize.Save();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Input Number"); 
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
