using System;

namespace Yavin.Model.Device
{
	/// <summary>
	/// 设备数据搜索条件封装
	/// </summary>
	public class Search
	{
		/// <summary>
		/// 主键条件
		/// </summary>
		public long[] Ids { get; set; }

		/// <summary>
		/// 编号条件
		/// </summary>
		public string[] Codes { get; set; }

		/// <summary>
		/// 状态条件
		/// </summary>
		public string[] Status { get; set; }

		/// <summary>
		/// 中心点条件，必须配合Radius条件才生效
		/// </summary>
		public Point Center { get; set; }

		/// <summary>
		/// 半径条件，单位：米
		/// </summary>
		public float Radius { get; set; }

		/// <summary>
		/// 是否可用条件
		/// </summary>
		public bool? Enabled { get; set; }
	}
}
