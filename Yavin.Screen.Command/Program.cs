using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yavin.Model.Device;

namespace Yavin.Screen.Command
{
	class Program
	{
		static void Main(string[] args)
		{
			var cmd = string.Empty;
			if (args != null && args.Length > 0)
			{
				cmd = args[0];
			}
			switch (cmd.ToLower())
			{
				case "start":	//启动播放
					Start();
					break;
				case "close":	//关闭（关闭播放程序）
					Close();
					break;
				case "home":	//强制切换到主页
					GotoHome();
					break;
				case "warn":	//强制切换到警示程序
					GotoWarn();
					break;
				case "pause":	//停止（有界面，但不能交互）
					Pause();
					break;
				case "continue"://恢复
					Continue();
					break;
				case "skip":	//停止某一广告的播放（需要第二参数，待定）

					break;
				case "black":	//关屏（不关程序）

					break;
			}
		}

		static void Start()
		{
			var appName = ConfigurationManager.AppSettings["AppName"];
			var processes = Process.GetProcessesByName(appName);
			if (processes != null && processes.Length > 0)
			{
				var p = processes[0];
				Win32API.SetToTopmost(p.MainWindowHandle);
				return;
			}
			var process = new Process();
			process.StartInfo.FileName = string.Format(@"{0}\{1}.exe", ConfigurationManager.AppSettings["AppPath"], appName);
			process.Start();
		}

		static void Close()
		{
			var appName = ConfigurationManager.AppSettings["AppName"];
			var processes = Process.GetProcessesByName(appName);
			if (processes != null && processes.Length > 0)
			{
				foreach (var p in processes)
				{
					p.Kill();
				}
			}
		}

		static void GotoHome()
		{
			var appName = ConfigurationManager.AppSettings["AppName"];
			var processes = Process.GetProcessesByName(appName);
			if (processes != null && processes.Length > 0)
			{
				var p = processes[0];
				Win32API.PostMessage(p.MainWindowHandle, PreCommand.GO_HOME);
			}
		}

		static void GotoWarn()
		{
			var appName = ConfigurationManager.AppSettings["AppName"];
			var processes = Process.GetProcessesByName(appName);
			if (processes != null && processes.Length > 0)
			{
				var p = processes[0];
				Win32API.PostMessage(p.MainWindowHandle, PreCommand.GO_WARN);
			}
		}

		static void Pause()
		{
			var appName = ConfigurationManager.AppSettings["AppName"];
			var processes = Process.GetProcessesByName(appName);
			if (processes != null && processes.Length > 0)
			{
				var p = processes[0];
				Win32API.PostMessage(p.MainWindowHandle, PreCommand.PAUSE);
			}
		}

		static void Continue()
		{
			var appName = ConfigurationManager.AppSettings["AppName"];
			var processes = Process.GetProcessesByName(appName);
			if (processes != null && processes.Length > 0)
			{
				var p = processes[0];
				Win32API.PostMessage(p.MainWindowHandle, PreCommand.CONTINUE);
			}
		}
	}
}
