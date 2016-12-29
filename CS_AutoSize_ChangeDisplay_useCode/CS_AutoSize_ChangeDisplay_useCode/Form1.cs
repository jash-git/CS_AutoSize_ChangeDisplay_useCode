using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CS_AutoSize_ChangeDisplay_useCode
{
    public partial class Form1 : Form
    {
        public Display_API m_Display_API;
        //--
        //step_01
        private float m_fltResolution_W = 1920;//系統預設解析度-寬度
        private float m_fltResolution_H = 1080;//系統預設解析度-高度
        private float m_fltRFactor_W = 1;//解析度放大倍率因子-寬度
        private float m_fltRFactor_H = 1;//解析度放大倍率因子-高度
        
        private float m_fltDpi_W = 96;//預設DPI-寬度
        private float m_fltDpi_H = 96;//預設DPI-高度
        private float m_fltDFactor_W = 1;//DPI放大倍率因子-寬度
        private float m_fltDFactor_H = 1;//DPI放大倍率因子-高度

        private float m_fltSysFactor_W = 1;
        private float m_fltSysFactor_H = 1;

        
        private Single m_currentSiz;
        //--
        public Form1()
        {
            InitializeComponent();
            showMsg();
            setTag(this);
            setControls_Position_Size((1 / m_fltSysFactor_W), (1 / m_fltSysFactor_H), this);
            this.Width = (int)(this.Width * (1 / m_fltSysFactor_W));
            this.Height = (int)(this.Height * (1 / m_fltSysFactor_W));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        //--
        private void setTag(Control cons)
        {
            //string data = cons.Tag.ToString();
            //string[] mytag = cons.Tag.ToString().Split(new char[] { ':' });//獲取控制項的Tag屬性值，並分割後存儲字元串數組
            //m_currentSiz = System.Convert.ToSingle(mytag[4]);

            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                m_currentSiz = con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        private void setControls_Position_Size(float newx, float newy, Control cons)
        {
            //遍歷窗體中的控制項，重新設置控制項的值
            foreach (Control con in cons.Controls)
            {
                richTextBox1.Text += "\n--------------------\n";
                string data = con.Tag.ToString();
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//獲取控制項的Tag屬性值，並分割後存儲字元串數組
                float a = System.Convert.ToSingle(mytag[0]) * newx;//根據窗體縮放比例確定控制項的值，寬度
                richTextBox1.Text += "\n元件原本寬度 = " + System.Convert.ToSingle(mytag[0])+"\t";
                con.Width = (int)a;//寬度
                richTextBox1.Text += "元件之後寬度 = " + con.Width + "\n";
                a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                richTextBox1.Text += "元件原本高度 = " + System.Convert.ToSingle(mytag[1]) + "\t";
                con.Height = (int)(a);
                richTextBox1.Text += "元件之後高度 = " + con.Height + "\n";
                a = System.Convert.ToSingle(mytag[2]) * newx;//左邊距離
                richTextBox1.Text += "元件原本X = " + System.Convert.ToSingle(mytag[2]) + "\t";
                con.Left = (int)(a);
                richTextBox1.Text += "元件之後X = " + con.Left + "\n";
                a = System.Convert.ToSingle(mytag[3]) * newy;//上邊緣距離
                richTextBox1.Text += "元件原本Y = " + System.Convert.ToSingle(mytag[3]) + "\t";
                con.Top = (int)(a);
                richTextBox1.Text += "元件之後Y = " + con.Top + "\n";
                Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字體大小
                richTextBox1.Text += "元件原本字型大小 = " + System.Convert.ToSingle(mytag[4]) + "\t";
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                richTextBox1.Text += "元件之後字型大小 = " + currentSize + "\t";
                if (con.Controls.Count > 0)
                {
                    setControls_Position_Size(newx, newy, con);
                }
            }
        }
        //--
        //--
        private void showMsg()
        {
            m_Display_API = new Display_API();
            m_Display_API.getDisplaySetting();
            richTextBox1.Text = "\n";

            richTextBox1.Text += "預設螢幕解析度 = " + m_fltResolution_W + " * " + m_fltResolution_H + "\n";
            richTextBox1.Text += "目前系統解析度 = " + m_Display_API.m_intWidth + " * " + m_Display_API.m_intHeight + "\n";
            m_fltRFactor_W = m_fltResolution_W / m_Display_API.m_intWidth;//計算解析度放大倍率因子-寬度
            m_fltRFactor_H = m_fltResolution_H / m_Display_API.m_intHeight;//計算解析度放大倍率因子-高度
            richTextBox1.Text += "目前解析度放大倍率因子 = " + m_fltRFactor_W + " * " + m_fltRFactor_H + "\n";
            richTextBox1.Text += "\n--------------------\n";

            richTextBox1.Text += "\n目前系等本身放大因子(實際解析度/邏輯解析度) = " + m_Display_API.m_fltScreenScalingFactor_W + " * " + m_Display_API.m_fltScreenScalingFactor_H + "\n";
            richTextBox1.Text += "\n--------------------\n";

            Graphics graphics = this.CreateGraphics();
            float fltDpi_W, fltDpi_H;
            fltDpi_W = graphics.DpiX;
            fltDpi_H = graphics.DpiY;
            richTextBox1.Text += "\n預設螢幕Dpi = " + m_fltDpi_W + " * " + m_fltDpi_H + "\n";
            richTextBox1.Text += "目前系統Dpi = " + fltDpi_W + " * " + fltDpi_H + "\n";
            m_fltDFactor_W = fltDpi_W / m_fltDpi_W;//計算DPI度放大倍率因子-寬度
            m_fltDFactor_H = fltDpi_H / m_fltDpi_H;//計算DPI放大倍率因子-高度
            richTextBox1.Text += "目前Dpi放大倍率因子 = " + m_fltDFactor_W + " * " + m_fltDFactor_H + "\n";
            richTextBox1.Text += "\n--------------------\n";

            m_fltSysFactor_W = m_fltRFactor_W * m_Display_API.m_fltScreenScalingFactor_W * m_fltDFactor_W;
            m_fltSysFactor_H = m_fltRFactor_H * m_Display_API.m_fltScreenScalingFactor_H * m_fltDFactor_H;
            richTextBox1.Text += "\n目前放大倍率因子(Sys) = " + m_fltSysFactor_W + " * " + m_fltSysFactor_H + "\n";


        }
        //--
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
