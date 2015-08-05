using System;

namespace Yavin.Model.Device
{
	/// <summary>
	/// 设备操作指令
	/// </summary>
	public class PreCommand
	{
		/// <summary>
		/// 默认的消息ID，必须大于0x400
		/// </summary>
		public const int MSG_ID = 0x0500;

		/// <summary>
		/// 回到主页
		/// </summary>
		public const int GO_HOME = 0x100;

		/// <summary>
		/// 停止播放
		/// </summary>
		public const int PAUSE = 0x110;

		/// <summary>
		/// 转到警示页
		/// </summary>
		public const int GO_WARN = 0x120;

		/// <summary>
		/// 继续播放
		/// </summary>
		public const int CONTINUE = 0x130;
	}
}