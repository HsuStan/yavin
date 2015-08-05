using System;

namespace Yavin.Core.GPS
{
	/// <summary>
	/// Geohash算法实现
	/// 来源: http://www.cnblogs.com/dengxinglin/archive/2012/12/14/2817761.html#d
	/// </summary>
	public class Geohash
	{
		public enum Direction
		{
			Top = 0,
			Right = 1,
			Bottom = 2,
			Left = 3
		}
		private const string Base32 = "0123456789bcdefghjkmnpqrstuvwxyz";
		private static readonly int[] Bits = new[] { 16, 8, 4, 2, 1 };
		private static readonly string[][] Neighbors = 
		{
			new[]
			{
				"p0r21436x8zb9dcf5h7kjnmqesgutwvy", // Top
				"bc01fg45238967deuvhjyznpkmstqrwx", // Right
				"14365h7k9dcfesgujnmqp0r2twvyx8zb", // Bottom
				"238967debc01fg45kmstqrwxuvhjyznp", // Left
			}, 
			new[]
			{
				"bc01fg45238967deuvhjyznpkmstqrwx", // Top
				"p0r21436x8zb9dcf5h7kjnmqesgutwvy", // Right
				"238967debc01fg45kmstqrwxuvhjyznp", // Bottom
				"14365h7k9dcfesgujnmqp0r2twvyx8zb", // Left
			}
		};

		private static readonly string[][] Borders = 
		{
			new[] {"prxz", "bcfguvyz", "028b", "0145hjnp"},
			new[] {"bcfguvyz", "prxz", "0145hjnp", "028b"}
		};

		private static string CalculateAdjacent(string hash, Direction direction)
		{
			hash = hash.ToLower();

			char lastChr = hash[hash.Length - 1];
			int type = hash.Length % 2;
			var dir = (int)direction;
			string nHash = hash.Substring(0, hash.Length - 1);

			if (Borders[type][dir].IndexOf(lastChr) != -1)
			{
				nHash = Geohash.CalculateAdjacent(nHash, (Direction)dir);
			}
			return nHash + Base32[Neighbors[type][dir].IndexOf(lastChr)];
		}

		private static void RefineInterval(ref double[] interval, int cd, int mask)
		{
			if ((cd & mask) != 0)
			{
				interval[0] = (interval[0] + interval[1]) / 2;
			}
			else
			{
				interval[1] = (interval[0] + interval[1]) / 2;
			}
		}

		/// <summary>
		/// 根据Geohash值还原经纬度值
		/// </summary>
		/// <param name="geohash"></param>
		/// <returns>
		/// 数组包含2个元素, 0=纬度, 1=经度
		/// </returns>
		public static double[] Decode(string geohash)
		{
			bool even = true;
			double[] lat = { -90.0, 90.0 };
			double[] lon = { -180.0, 180.0 };

			foreach (char c in geohash)
			{
				int cd = Base32.IndexOf(c);
				for (int j = 0; j < 5; j++)
				{
					int mask = Bits[j];
					if (even)
					{
						Geohash.RefineInterval(ref lon, cd, mask);
					}
					else
					{
						Geohash.RefineInterval(ref lat, cd, mask);
					}
					even = !even;
				}
			}

			return new[] { (lat[0] + lat[1]) / 2, (lon[0] + lon[1]) / 2 };
		}

		/// <summary>
		/// 根据经纬度数据和指定的精度, 取得Geohash值
		/// </summary>
		/// <param name="latitude">纬度</param>
		/// <param name="longitude">经度</param>
		/// <param name="precision">精度</param>
		/// <returns></returns>
		public static string Encode(double latitude, double longitude, int precision = 12)
		{
			bool even = true;
			int bit = 0;
			int ch = 0;
			string geohash = "";

			double[] lat = { -90.0, 90.0 };
			double[] lon = { -180.0, 180.0 };

			if (precision < 1 || precision > 20) precision = 12;

			while (geohash.Length < precision)
			{
				double mid;

				if (even)
				{
					mid = (lon[0] + lon[1]) / 2;
					if (longitude > mid)
					{
						ch |= Bits[bit];
						lon[0] = mid;
					}
					else
						lon[1] = mid;
				}
				else
				{
					mid = (lat[0] + lat[1]) / 2;
					if (latitude > mid)
					{
						ch |= Bits[bit];
						lat[0] = mid;
					}
					else
						lat[1] = mid;
				}

				even = !even;
				if (bit < 4)
					bit++;
				else
				{
					geohash += Base32[ch];
					bit = 0;
					ch = 0;
				}
			}
			return geohash;
		}

		/// <summary>
		/// 取得用于在指定位置编码附近搜索的GeoHash字符串数组,
		/// 该数组第一个元素为参数本身, 其余元素为参数块上/右/下/左4个块
		/// </summary>
		/// <param name="geohash"></param>
		/// <returns></returns>
		public static string[] GetNearbyRange(string geohash)
		{
			var range = new string[]
			{
				geohash,
				Geohash.CalculateAdjacent(geohash, Direction.Top),
				Geohash.CalculateAdjacent(geohash, Direction.Right),
				Geohash.CalculateAdjacent(geohash, Direction.Bottom),
				Geohash.CalculateAdjacent(geohash, Direction.Left)
			};
			return range;
		}

		/// <summary>
		/// 根据半径的长度取得Geohash值需要比较的位数,
		/// 计算依据: http://en.wikipedia.org/wiki/Geohash
		/// </summary>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static int GetQueryLength(float radius)
		{
			if (radius >= 2500) return 1;
			if (radius >= 630) return 2;
			if (radius >= 78) return 3;
			if (radius >= 20) return 4;
			if (radius >= 2.4) return 5;
			if (radius >= 0.61) return 6;
			if (radius >= 0.076) return 7;
			if (radius >= 0.019) return 8;
			return 9;
		}
	}
}
