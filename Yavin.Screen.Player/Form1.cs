using System;
using System.Configuration;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Yavin.Model.Device;

namespace Yavin.Screen.Player
{
	public partial class Form1 : Form
	{
		private string homeUrl = string.Empty;
		private string warnUrl = string.Empty;
		public Form1()
		{
			InitializeComponent();
			var path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"/");
			var dataPath = ConfigurationManager.AppSettings["DataPath"];
			var currentPath = ConfigurationManager.AppSettings["CurrentPath"];
			var startPage = ConfigurationManager.AppSettings["StartPage"];
			var warnPage = ConfigurationManager.AppSettings["WarnPage"];
			this.homeUrl = string.Format("file:///{0}{1}/{2}/{3}", path, dataPath, currentPath, startPage);
			this.warnUrl = string.Format("file:///{0}{1}/{2}/{3}", path, dataPath, currentPath, warnPage);
			webBrowser1.Url = new Uri(this.homeUrl);
			webBrowser1.IsWebBrowserContextMenuEnabled = false;
			webBrowser1.ScrollBarsEnabled = false;
			webBrowser1.WebBrowserShortcutsEnabled = false;
		}

		protected override void DefWndProc(ref Message m)
		{
			if (m.Msg == PreCommand.MSG_ID)
			{
				var cmd = (int)m.WParam;
				switch (cmd)
				{
					case PreCommand.GO_HOME:
						if (webBrowser1.Url.ToString() != this.homeUrl)
							webBrowser1.Url = new Uri(this.homeUrl);
						webBrowser1.Document.InvokeScript("Page_Init");
						break;
					case PreCommand.GO_WARN:
						if (webBrowser1.Url.ToString() != this.warnUrl)
							webBrowser1.Url = new Uri(this.warnUrl);
						break;
					case PreCommand.PAUSE:
						if (webBrowser1.Url.ToString() == this.homeUrl)
							webBrowser1.Document.InvokeScript("Page_Pause");
						break;
					case PreCommand.CONTINUE:
						if (webBrowser1.Url.ToString() == this.homeUrl)
							webBrowser1.Document.InvokeScript("Page_Continue");
						break;
					default:
						base.DefWndProc(ref m);
						break;
				}
			}
			else
			{
				base.DefWndProc(ref m);
			}
		}
	}
}
