using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Yavin.Model.Device;

namespace Yavin.Screen.Command
{
	class Win32API
	{
		/// <summary>
		/// 根据程序类名或窗体名查询窗体句柄
		/// </summary>
		/// <param name="lpClassName"></param>
		/// <param name="lpWindowName"></param>
		/// <returns></returns>
		[DllImport("user32.dll", EntryPoint = "FindWindow")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		/// <summary>
		/// 设置窗体位置、尺寸与Z轴顺序
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="hWndInsertAfter"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int width, int height, int flags);

		/// <summary>
		/// 将指定句柄的窗体置为最前端窗体
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// 向指定窗口发送消息
		/// </summary>
		/// <param name="hWnd">窗口句柄</param>
		/// <param name="msg">消息ID</param>
		/// <param name="wparam">参数1</param>
		/// <param name="lparam">参数2</param>
		/// <returns></returns>
		[DllImport("user32.dll", EntryPoint = "PostMessage")]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, int wparam, int lparam);

		/// <summary>
		/// 将指定名称的窗口切换到最顶层
		/// </summary>
		/// <param name="windowName"></param>
		public static void SetToTopmost(string windowName)
		{
			var hwnd = Win32API.FindWindow(null, windowName);
			Win32API.SetToTopmost(hwnd);
		}

		/// <summary>
		/// 将指定句柄的窗体切换到最顶层
		/// </summary>
		/// <param name="hWnd"></param>
		public static void SetToTopmost(IntPtr hWnd)
		{
			if (hWnd == null || hWnd == IntPtr.Zero)
				return;
			Win32API.SetForegroundWindow(hWnd);
		}

		/// <summary>
		/// 向指定名称的窗口发送文本消息
		/// </summary>
		/// <param name="windowName"></param>
		/// <param name="command"></param>
		public static void PostMessage(string windowName, int command)
		{
			var hwnd = Win32API.FindWindow(null, windowName);
			Win32API.PostMessage(hwnd, command);
		}

		/// <summary>
		/// 向指定的应用程序发送指定消息
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="command"></param>
		public static void PostMessage(IntPtr hWnd, int command)
		{
			if (hWnd == null || hWnd == IntPtr.Zero)
				return;
			var rst = Win32API.PostMessage(hWnd, PreCommand.MSG_ID, command, 0);
		}
	}
}
