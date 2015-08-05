using System;
using System.Text;

namespace Yavin.Core.JSON
{
	/// <summary>
	/// 基础数据JSON格式转换器，兼容Newtonsoft.JSON格式
	/// </summary>
	public class Converter
	{
		public static string ToString(object item)
		{
			if (item == null)
			{
				return "null";
			}
			else
			{
				if (item.GetType().IsEnum)
				{
					item = Convert.ToInt64(item);
				}
				switch (item.GetType().ToString())
				{
					case "System.Boolean": return Converter.ToString(Convert.ToBoolean(item));
					case "System.Byte": return Converter.ToString(Convert.ToByte(item));
					case "System.SByte": return Converter.ToString(Convert.ToSByte(item));
					case "System.Char":
					case "System.String": return Converter.ToString(Convert.ToString(item));
					case "System.Decimal": return Converter.ToString(Convert.ToDecimal(item));
					case "System.Double": return Converter.ToString(Convert.ToDouble(item));
					case "System.DateTime": return Converter.ToString(Convert.ToDateTime(item));
					case "System.Single": return Converter.ToString(Convert.ToSingle(item));
					case "System.Int16": return Converter.ToString(Convert.ToInt16(item));
					case "System.UInt16": return Converter.ToString(Convert.ToUInt16(item));
					case "System.Int32": return Converter.ToString(Convert.ToInt32(item));
					case "System.UInt32": return Convert.ToString(Convert.ToUInt32(item));
					case "System.Int64": return Convert.ToString(Convert.ToInt64(item));
					case "System.UInt64": return Converter.ToString(Convert.ToUInt64(item));
					case "System.Guid": return Converter.ToString(new Guid(item.ToString()));
				}
			}
			return "Unknown data type";
		}

		internal static string ToString(bool item)
		{
			return item.ToString().ToLower();
		}
		internal static string ToString(byte item)
		{
			return item.ToString();
		}
		internal static string ToString(sbyte item)
		{
			return item.ToString();
		}
		internal static string ToString(char item)
		{
			return @"""" + item.ToString() + @"""";
		}
		internal static string ToString(decimal item)
		{
			return item.ToString();
		}
		internal static string ToString(double item)
		{
			return item.ToString();
		}
		internal static string ToString(DateTime item)
		{
			return @"""" + item.ToString("yyyy-MM-ddTHH:mm:ss") + @"""";
		}
		internal static string ToString(float item)
		{
			return @"""" + item.ToString() + @"""";
		}
		internal static string ToString(short item)
		{
			return item.ToString();
		}
		internal static string ToString(ushort item)
		{
			return item.ToString();
		}
		internal static string ToString(int item)
		{
			return item.ToString();
		}
		internal static string ToString(uint item)
		{
			return item.ToString();
		}
		internal static string ToString(long item)
		{
			return item.ToString();
		}
		internal static string ToString(ulong item)
		{
			return item.ToString();
		}
		internal static string ToString(string item)
		{
			if (item == null) return @"""""";
			else return @"""" + Converter.Cleaner(item) + @"""";
		}
		internal static string ToString(Guid item)
		{
			if (item == null) return @"""""";
			else return @"""" + item.ToString() + @"""";
		}
		internal static string Cleaner(string src)
		{
			if (src == null) return "";
			StringBuilder sb = new StringBuilder(src);
			sb.Replace(@"\", @"\\");
			//sb.Replace(@"'", @"\'");
			sb.Replace(@"""", @"\""");
			sb.Replace(Environment.NewLine, @"\n");	//替换连在一起的\r\n
			sb.Replace("\n", @"\n");				//单个替换
			sb.Replace("\r", @"\n");
			return sb.ToString();
		}
	}
}
