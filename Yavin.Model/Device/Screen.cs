using System;

namespace Yavin.Model.Device
{
	/// <summary>
	/// 表示一个显示屏
	/// </summary>
	public class Screen
	{
		/// <summary>
		/// 主键
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// 设备编号
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 设备安装位置
		/// </summary>
		public Address Address { get; set; }

		/// <summary>
		/// 显示区域宽度
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// 显示区域高度
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// 当前状态
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// 数据是否可用
		/// </summary>
		public bool Enabled { get; set; }
	}
}
