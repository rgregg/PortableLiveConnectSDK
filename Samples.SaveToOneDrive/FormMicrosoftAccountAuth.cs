using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneDriveSamples.SaveToOneDrive
{
    public partial class FormMicrosoftAccountAuth : Form
    {

        private string StartUrl { get; set; }
        private string EndUrl { get; set; }
        public AuthResult AuthResult { get; private set; }
        public FormMicrosoftAccountAuth(string startUrl, string endUrl)
        {
            InitializeComponent();

            this.StartUrl = startUrl;
            this.EndUrl = endUrl;
            this.FormClosing += FormMicrosoftAccountAuth_FormClosing;
        }


        void FormMicrosoftAccountAuth_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void FormMicrosoftAccountAuth_Load(object sender, EventArgs e)
        {
            this.webBrowser.Navigated += webBrowser_Navigated;
            this.webBrowser.Navigate(this.StartUrl);
        }

        void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (this.webBrowser.Url.AbsoluteUri.StartsWith(EndUrl))
            {
                this.AuthResult = new AuthResult(this.webBrowser.Url);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            this.Text = webBrowser.DocumentTitle;
        }
    }
}
