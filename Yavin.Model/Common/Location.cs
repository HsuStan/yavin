using System;

namespace Yavin.Model.Common
{
	/// <summary>
	/// 行政区域对象
	/// </summary>
	public class Location
	{
		/// <summary>
		/// 编码
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 级别
		/// Province=省份
		/// City=地市
		/// County=区县
		/// Village=乡镇
		/// </summary>
		public string Level { get; set; }

		/// <summary>
		/// 上级编码
		/// </summary>
		public string ParentCode { get; set; }

		/// <summary>
		/// 全编码, 从最上级到当前级行政区域编码, 每级之间用|分隔, 最上级区域的, 该值等于编码
		/// </summary>
		public string FullCode { get; set; }

		/// <summary>
		/// 全名称, 从最上级到当前级行政区域名称, 每级之间用|分隔, 最上级区域的, 该值等于名称
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// 经度
		/// </summary>
		public double Longitude { get; set; }

		/// <summary>
		/// 纬度
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// 车牌前缀
		/// </summary>
		public string PlatePrefix { get; set; }

		/// <summary>
		/// 行政区域Geohash值
		/// </summary>
		public string Geohash { get; set; }
	}
}
