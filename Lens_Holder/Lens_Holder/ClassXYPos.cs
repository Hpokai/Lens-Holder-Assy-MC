using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Lens_Holder
{
    public class ClassXYPos
    {
        public string posname { get; set; }
        public string PosText;
        public ClassXYPos(string PosName)
        {
            posname = PosName;
            PosText = PosName;
        }
        public ClassXYPos(string PosName,string postext)
        {
            posname = PosName;
            PosText = postext;
        }
        public ClassXYPos()
        { }
        public ClassXYPos(double _X, double _Y)
        {
            X = _X;
            Y = _Y;
        }
        public static ClassXYPos operator +(ClassXYPos a, ClassXYPos b)
        {
            return new ClassXYPos(a.X + b.X, a.Y + b.Y);
        }
        public static ClassXYPos operator -(ClassXYPos a, ClassXYPos b)
        {
            return new ClassXYPos(a.X - b.X, a.Y - b.Y);
        }
        public double  Caul_Dis(ClassXYPos PP)
        {
            return  Math.Pow( Math.Pow ((PP.X - X) , 2) + Math.Pow ((PP.Y - Y) , 2),0.5);
        }
        public double X{ get; set; }
        public double Y{ get; set; }
        
        string PosFolder = Application.StartupPath + "\\XYPos\\";

        public bool Save()
        {
            try
            {
                if (!Directory.Exists(PosFolder))
                    Directory.CreateDirectory(PosFolder);
                string SP = PosFolder + posname + ".Pos";
                string SD = X.ToString() + Environment.NewLine +
                    Y.ToString() + Environment.NewLine;
                File.WriteAllText(SP, SD);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Load()
        {
            try
            {
                string LP = PosFolder + posname + ".Pos";
                string[] LoadStr;
                if (File.Exists(LP))
                    LoadStr = File.ReadAllLines(LP);
                else
                    return false;

                if (LoadStr != null)
                    if (LoadStr.Length >= 2)
                    {
                        double XX, YY;
                        if (!double.TryParse(LoadStr[0], out XX))
                            return false;
                        if (!double.TryParse(LoadStr[1], out YY))
                            return false;
                        X = XX;
                        Y = YY;
                    }
                    else
                        return false;
                else
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString();
        }

    }
}
