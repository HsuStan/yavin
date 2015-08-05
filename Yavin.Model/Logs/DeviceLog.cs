using System;

namespace Yavin.Model.Logs
{
	/// <summary>
	/// 表示一个设备事件日志记录
	/// </summary>
	public class DeviceLog
	{
		/// <summary>
		/// 主键
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// 事件类别
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 事件名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 记录时间
		/// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 事件状态
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// 设备
		/// </summary>
		public Device Device { get; set; }

		/// <summary>
		/// 事件内容
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 数据是否可用
		/// </summary>
		public bool Enabled { get; set; }
	}
}
