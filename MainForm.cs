using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Test_Program4
{
    class MainForm : Form
    {
        int fNumber;
        string fPath;
        FileStream fStream;
        Image loadImage;
        PictureBox[] pbList;
        MenuStrip menuStrip;
        OpenFileDialog ofd;
        List<string> filesList;

        Stopwatch sw_all;
        Stopwatch sw;

        public MainForm()
        {
            Load += new EventHandler(MainForm_Load);
            menuStrip = new MenuStrip();
            Controls.Add(menuStrip);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            ToolStripMenuItem menuFile = new ToolStripMenuItem();
            menuFile.Text = "ファイル(&F)";
            menuStrip.Items.Add(menuFile);

            ToolStripMenuItem menuFileOpen = new ToolStripMenuItem();

            menuFileOpen.Text = "開く(&O)";
            menuFileOpen.Click += new EventHandler(Open_Click);
            menuFile.DropDownItems.Add(menuFileOpen);
            ToolStripMenuItem menuFileEnd = new ToolStripMenuItem();

            menuFileEnd.Text = "終了(&X)";
            menuFileEnd.Click += new EventHandler(Close_Click);
            menuFile.DropDownItems.Add(menuFileEnd);
        }

        void Open_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            ofd.Filter = "Image File(*.bmp,*.jpg,*.png)|*.bmp;*.jpg;*.png|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine($"読み込みファイル:{ofd.FileName}");
                string folderPath = Path.GetDirectoryName(ofd.FileName);
                IEnumerable<string> files = Directory.EnumerateFiles(folderPath).Where(str => str.EndsWith(".bmp") || str.EndsWith(".jpg") || str.EndsWith(".png"));
                filesList = files.ToList();
                var sortQuery = filesList.OrderBy(s => s.Length);
                filesList = sortQuery.ToList();
                Console.WriteLine($"{filesList.Count}");
                fNumber = filesList.IndexOf(ofd.FileName);
                pbList = new PictureBox[filesList.Count];
                Img_DrawTest1();
            }
        }

        void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //ロードする画像の高速化のテスト
        void Img_DrawTest1()
        {
            sw_all = Stopwatch.StartNew();
            for(int i = 0; i < filesList.Count; i++)
            {
                pbList[i] = new PictureBox();
                Controls.Add(pbList[i]);
            }
            sw_all.Stop();
            Console.WriteLine(sw_all.ElapsedMilliseconds + "ミリ秒");

            sw_all = Stopwatch.StartNew();
            for (int i = 0; i < filesList.Count; i++)
            {
                sw = Stopwatch.StartNew();
                fStream = File.OpenRead(filesList[i]);
                loadImage = Image.FromStream(fStream, false, false);
                pbList[i].Size = new Size(loadImage.Width, loadImage.Height);
                Bitmap b = new Bitmap(loadImage.Width, loadImage.Height);
                Graphics g = Graphics.FromImage(b);

                //DrawImageが遅い

                //g.DrawImage(loadImage, 0, 0, loadImage.Width, loadImage.Height);
                //g.DrawImage(loadImage, 0, 0);

                loadImage.Dispose();
                g.Dispose();
                pbList[i].Image = b;
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds + $"ミリ秒");
            }
            fStream.Dispose();
            sw_all.Stop();
            Console.WriteLine(sw_all.ElapsedMilliseconds + $"ミリ秒");

            /*
            sw = Stopwatch.StartNew();
            for (int i = 0; i < filesList.Count; i++)
            {

            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + $"ミリ秒");
            */
        }

    }
}
