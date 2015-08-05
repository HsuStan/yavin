using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yavin.Model.Device
{
	/// <summary>
	/// 表示一个坐标点
	/// </summary>
	public class Point
	{
		/// <summary>
		/// 经度
		/// </summary>
		public double Longitude { get; set; }

		/// <summary>
		/// 续度
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// 坐标点类型
		/// </summary>
		public string CoordinateType { get; set; }

		/// <summary>
		/// 坐标的Geohash值
		/// </summary>
		public string Geohash { get; set; }
	}
}
