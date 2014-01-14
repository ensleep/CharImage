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
        int imgh = 32;
        public Form1()
        {
            InitializeComponent();
            label5.Text = "CopyRight@  张峻崎 2014";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            CreateBmp();
        }
        public void CreateBmp()
        {
            if(textBox1.Text.Trim()=="")
            {
                MessageBox.Show("请输入要转换的字符哦~~不然，怎么转换呢?");
            }
            else
            {
                Font f = new System.Drawing.Font(comboBox1.SelectedItem.ToString().Trim(), imgh / 2, FontStyle.Regular);
                bmp = new System.Drawing.Bitmap(imgh * textBox1.Text.Trim().Length, imgh);
                //Image image = new System.Drawing.Bitmap(32,32);
                Graphics gra = Graphics.FromImage(bmp);
                System.Drawing.Drawing2D.LinearGradientBrush b = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, imgh * textBox1.Text.Trim().Length, imgh), Color.Black, Color.Black, 0, false);
                gra.DrawString(textBox1.Text.Trim(), f, b, 0, 0);
                pictureBox1.Height = imgh;
                pictureBox1.Width = imgh * textBox1.Text.Trim().Length;
                pictureBox1.Image = bmp;
                if (File.Exists("tmp.bmp"))
                {
                    File.Delete("tmp.bmp");
                }
                bmp.Save("tmp.bmp", ImageFormat.Bmp);
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public void GetChar(string path)
        {
            try
            {
                Stream stream = File.OpenRead(path);  // 打开位图文件
                byte[] buffer = new byte[stream.Length - 54];  // 缓冲区，文件长度减去文件头和信息头的长度

                stream.Position = 54;  // 跳过文件头和信息头

                stream.Read(buffer, 0, buffer.Length);  // 读取位图数据，位图数据是颠倒的
                byte[] buf = new byte[buffer.Length / 4];
                Image img = Bitmap.FromStream(stream);
                imgh = img.Height;
                for (int a = 3, i = 0; a < buffer.Length; a += 4, i++)
                {
                    buf[i] = buffer[a];
                }
                StringBuilder[] sbArray = new StringBuilder[imgh];
                StringBuilder sb = new StringBuilder();
                int count = -1;
                for (int a = 0; a < buf.Length; a++)
                {
                    if (a % (buf.Length / imgh) == 0)
                    {
                        count++;
                        sbArray[count] = new StringBuilder();
                    }
                    if (buf[a] == 255)
                    {
                        sbArray[count].Append(GetSafeString(textBox3.Text.ToCharArray().Length > 0 ? textBox3.Text.ToCharArray()[0].ToString() : "x"));
                    }
                    else
                    {
                        sbArray[count].Append(GetSafeString(textBox4.Text.ToCharArray().Length > 0 ? textBox4.Text.ToCharArray()[0].ToString() : " "));
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CreateBmp();
            GetChar("tmp.bmp");
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
        public string GetSafeString(string str)
        {
            // 半角转全角：
            char c = str.ToCharArray()[0];
            if (c == 32)
                c = (char)12288;
            if (c < 127)
                c = (char)(c + 65248);
            return c.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".bmp";
            openFileDialog1.CheckPathExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                if (path.Split('.').LastOrDefault() == "bmp")
                {
                    GetChar(path);
                }
                else
                {
                    MessageBox.Show("打开的图片格式不正确");
                }
            }
        }
    }
}
