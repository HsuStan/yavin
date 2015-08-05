using System;

namespace Yavin.Model.Logs
{
	/// <summary>
	/// 日志基础数据搜索条件封装
	/// </summary>
	public class Search
	{
		/// <summary>
		/// 主键条件
		/// </summary>
		public long[] Ids { get; set; }

		/// <summary>
		/// 时间段开始条件
		/// </summary>
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// 时间段结束条件
		/// </summary>
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// 状态条件
		/// </summary>
		public string[] Status { get; set; }

		/// <summary>
		/// 设备条件
		/// </summary>
		public Device[] Devices { get; set; }

		/// <summary>
		/// 用户条件
		/// </summary>
		public User[] Users { get; set; }

		/// <summary>
		/// 是否可用条件
		/// </summary>
		public bool? Enabled { get; set; }
	}
}
