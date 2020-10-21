using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Reddit_Monitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            postTitleLabel.Text = "";
            postSubredditLabel.Text = "";
            refreshTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Load newest images page from Reddit
            string url = "https://www.reddit.com/domain/i.redd.it/new/";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // Extract newest image post
            var newestPost = doc.DocumentNode.SelectSingleNode("/html/body/div[4]/div/div[1]");

            // Extract details from post
            string imageUrl = newestPost.GetAttributeValue("data-url", "").ToString();
            string subreddit = newestPost.GetAttributeValue("data-subreddit", "").ToString();
            string title = doc.DocumentNode.SelectSingleNode("/html/body/div[4]/div/div[1]/div[2]/div[1]/p[1]/a").InnerText;

            // Download image
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(imageUrl);
            MemoryStream ms = new MemoryStream(bytes);
            Image img = Image.FromStream(ms);

            // Update display
            pictureBox1.Image = img;
            postTitleLabel.Text = title;
            postSubredditLabel.Text = "/r/" + subreddit;

            // Write to log file
            using (StreamWriter log = new StreamWriter(@"log.txt", true))
            {
                log.Write(DateTime.Now + " | ");
                log.Write(imageUrl + " | ");
                log.Write("/r/" + subreddit + " | ");
                log.Write(title + "\n");
            }
        }
    }
}
