using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 字中字
{
    public partial class Form1 : Form
    {
        System.Drawing.Bitmap bmp;
        int size = 32;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateBmp();
        }
        public void CreateBmp()
        {

            char c = textBox1.Text.ToCharArray()[0];
            Font f = new System.Drawing.Font(comboBox1.SelectedItem.ToString().Trim(), size/2, FontStyle.Regular);
            bmp = new System.Drawing.Bitmap(size * textBox1.Text.Trim().Length, size);
            //Image image = new System.Drawing.Bitmap(32,32);
            Graphics gra = Graphics.FromImage(bmp);
            System.Drawing.Drawing2D.LinearGradientBrush b = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, size * textBox1.Text.Trim().Length, size), Color.Black, Color.Black, 0, false);
            gra.DrawString( textBox1.Text.Trim(), f, b, 0, 0);
            pictureBox1.Height = size;
            pictureBox1.Width = size * textBox1.Text.Trim().Length;
            pictureBox1.Image = bmp;
            if (File.Exists("tmp.bmp"))
            {
                File.Delete("tmp.bmp");
            }
            bmp.Save("tmp.bmp", ImageFormat.Bmp);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public void GetChar()
        {

            Stream stream = File.OpenRead("tmp.bmp");  // 打开位图文件
            byte[] buffer = new byte[stream.Length - 54];  // 缓冲区，文件长度减去文件头和信息头的长度

            stream.Position = 54;  // 跳过文件头和信息头

            stream.Read(buffer, 0, buffer.Length);  // 读取位图数据，位图数据是颠倒的
            byte[] buf = new byte[buffer.Length / 4];
            for (int a = 3, i = 0; a < buffer.Length; a+=4,i++)
            {
                buf[i] = buffer[a];
            }
            StringBuilder[] sbArray = new StringBuilder[ size];
            StringBuilder sb = new StringBuilder();
            int count = -1;
            for (int a = 0; a < buf.Length; a++)
            {
                if (a % (buf.Length / size) == 0)
                {
                    count++;
                    sbArray[count] = new StringBuilder();
                }
                if (buf[a] == 255)
                {
                    sbArray[count].Append(textBox3.Text.ToCharArray().Length > 0 ? textBox3.Text.ToCharArray()[0].ToString() : "x");
                }
                else
                {
                    sbArray[count].Append(textBox4.Text.ToCharArray().Length > 0 ? textBox4.Text.ToCharArray()[0].ToString() : " ");
                }
            }
            for (int a = sbArray.Length - 1; a >= 0; a--)
            {
                textBox2.Text += sbArray[a].ToString();
                textBox2.Text += "\r\n";
            }
            stream.Close();
            stream.Dispose();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CreateBmp();
            GetChar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in fonts.Families)
            {
                comboBox1.Items.Add(family.Name);
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
    }
}
