using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;


namespace Reddit_Monitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string url = "https://www.reddit.com/domain/i.redd.it/new/";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var newestPost = doc.DocumentNode.SelectSingleNode("/html/body/div[4]/div/div[1]");
            string imageUrl = newestPost.GetAttributeValue("data-url", "").ToString();
            textBox1.Text = imageUrl;

            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(imageUrl);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

            pictureBox1.Image = img;
        }
    }
}
