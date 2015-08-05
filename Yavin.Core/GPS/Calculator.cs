using System;

namespace Yavin.Core.GPS
{
	/// <summary>
	/// 定位有关数据计算器
	/// </summary>
	public class Calculator
	{
		/// <summary>
		/// 根据出发/目的点的坐标计算直线距离, 单位：千米
		/// 参考http://blog.csdn.net/lfqsy/article/details/6750537
		/// </summary>
		/// <param name="startLng"></param>
		/// <param name="startLat"></param>
		/// <param name="destLng"></param>
		/// <param name="destLat"></param>
		/// <returns></returns>
		public static double GetDistance(double startLat, double startLng, double destLat, double destLng)
		{
			var radius = 6378.137;		//地球半径
			var lngFrom = startLng * Math.PI / 180.0;
			var latFrom = startLat * Math.PI / 180.0;
			var lngTo = destLng * Math.PI / 180.0;
			var latTo = destLat * Math.PI / 180.0;
			var a = latFrom - latTo;
			var b = lngFrom - lngTo;
			var distance = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(latFrom) * Math.Cos(latTo) * Math.Pow(Math.Sin(b / 2), 2)));
			distance = distance * radius;
			//根据IEEE754标准, 浮点数可能出现特殊值, 这里需要判断, 否则程序将出现异常
			//参考: http://zh.wikipedia.org/zh-cn/IEEE_754#.E7.89.B9.E6.AE.8A.E5.80.BC
			if (double.IsNaN(distance) || double.IsInfinity(distance))
			{
				return default(double);
			}
			return Math.Round(distance * 10000) / 10000;
		}
	}
}
