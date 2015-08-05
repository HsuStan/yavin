using System;

namespace Yavin.Model.Device
{
	/// <summary>
	/// 表示一个设备的安装位置
	/// </summary>
	public class Address
	{
		/// <summary>
		/// 区域编码
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 区域名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 详细地址
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 坐标点
		/// </summary>
		public Point Point { get; set; }
	}
}
