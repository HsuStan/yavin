using System;

namespace Yavin.Model.Logs
{
	/// <summary>
	/// 表示一个指令事件日志
	/// </summary>
	public class CommandLog
	{
		/// <summary>
		/// 主键
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// 接收设备
		/// </summary>
		public Device Device { get; set; }

		/// <summary>
		/// 指令创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 指令创建者
		/// </summary>
		public User Creator { get; set; }

		/// <summary>
		/// 指令内容
		/// </summary>
		public string Statement { get; set; }

		/// <summary>
		/// 指令状态
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// 数据是否可用
		/// </summary>
		public bool Enabled { get; set; }
	}
}
