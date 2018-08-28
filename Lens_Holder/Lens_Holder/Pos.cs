using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace Lens_Holder
{
    public class XYUPos
    {

        public string posname { get; set; }
        public XYUPos(string PosName)
        {
            posname = PosName;
        }

        public XYUPos(double _X, double _Y, double _U)
        {
            X = _X;
            Y = _Y;
            U = _U;
        }
        public static XYUPos operator +(XYUPos a, XYUPos b)
        {
            return new XYUPos(a.X + b.X, a.Y + b.Y, a.U + b.U);
        }
        public double X;
        public double Y;
        public double U;

        string PosFolder = Application.StartupPath + "\\Pos\\";
        /// <summary>確認位置是否相等
        /// 
        /// </summary>
        /// <param name="P">位置</param>
        /// <returns></returns>
        public bool PosEqual(XYUPos P)
        {
            return X == P.X & Y == P.Y  & U == P.U;
        }
        /// <summary>確認位置是否相等
        /// 
        /// </summary>
        /// <param name="PX">X</param>
        /// <param name="PY">Y</param>
        /// <param name="PU">U</param>
        /// <returns></returns>
        public bool PosEqual(double PX, double PY,  double PU)
        {
            return X == PX & Y == PY & U == PU;
        }
        public bool Save()
        {
            try
            {
                if (!Directory.Exists(PosFolder))
                    Directory.CreateDirectory(PosFolder);
                string SP = PosFolder + posname + ".Pos";
                string SD = X.ToString() + Environment.NewLine +
                    Y.ToString() + Environment.NewLine +
                    U.ToString() + Environment.NewLine;
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
                    if (LoadStr.Length >= 3)
                    {
                        if (!double.TryParse(LoadStr[0], out X))
                            return false;
                        if (!double.TryParse(LoadStr[1], out Y))
                            return false;

                        if (!double.TryParse(LoadStr[2], out U))
                            return false;
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
            string str = X.ToString() + "," + Y.ToString() + "," + U.ToString();
            return str;
        }
        public static XYUPos operator -(XYUPos a, XYUPos b)
        {
            return new XYUPos(a.X + b.X, a.Y + b.Y, a.U + b.U);
        }
        public XYUPos setPos(double PX, double PY, double PU)
        {
            XYUPos P = new XYUPos("iii");
            X = PX;
            Y = PY;
            U = PU;
            return P;
        }

    }

}
