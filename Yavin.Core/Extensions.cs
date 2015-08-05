using System;
using System.Text.RegularExpressions;

namespace Yavin.Core
{
	public static class Extensions
	{
		/// <summary>
		/// 去掉字符串中间与两端的全部全角和半角空格
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static string ClearWriteSpace(this string src)
		{
			if (string.IsNullOrWhiteSpace(src))
				return src;
			return Regex.Replace(src, @"( |　)+", "");
		}

		/// <summary>
		/// 验证字符串是否严格是手机号码格式
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsStrictCellphone(this string src)
		{
			if (string.IsNullOrWhiteSpace(src)) return false;
			if (Regex.IsMatch(src, @"(\d)\1{6,}"))//相同数字连续7次以上就不是手机号码
				return false;
			if (Regex.IsMatch(src, @"^13[0-9]{1}[0-9]{8}$"))//130-139开头
				return true;
			if (Regex.IsMatch(src, @"^15[^4]{1}[0-9]{8}$"))//15开头，但不是154
				return true;
			if (Regex.IsMatch(src, @"^18[0-9]{1}[0-9]{8}$"))//180-189开头
				return true;
			if (Regex.IsMatch(src, @"^14[57]{1}[0-9]{8}$"))//140-149开头
				return true;
			if (Regex.IsMatch(src, @"^17[0678]{1}[0-9]{8}$"))//170、176、177、178开头
				return true;
			//不符合上面所有条件就不包含手机号码
			return false;
		}

		/// <summary>
		/// 验证字符串是否包含一个手机号码
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsCellphone(this string src)
		{
			if (string.IsNullOrWhiteSpace(src)) return false;
			if (Regex.IsMatch(src, @"13[0-9]{1}[0-9]{8}"))//130-139开头
				return true;
			if (Regex.IsMatch(src, @"15[^4]{1}[0-9]{8}"))//15开头，但不是154
				return true;
			if (Regex.IsMatch(src, @"18[0-9]{1}[0-9]{8}"))//180-189开头
				return true;
			if (Regex.IsMatch(src, @"14[57]{1}[0-9]{8}"))//140-149开头
				return true;
			if (Regex.IsMatch(src, @"17[0678]{1}[0-9]{8}"))//170、176、177、178开头
				return true;
			//不符合上面所有条件就不包含手机号码
			return false;
		}

		/// <summary>
		/// 验证字符串是否严格是固定电话号码
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsStrictTelephone(this string src)
		{
			if (string.IsNullOrWhiteSpace(src)) return false;
			if (Regex.IsMatch(src, @"^0{1}[1-9]{1}[0-9]{1,2}\-[1-9]{1}[0-9]{6,7}(((\-|转)[^(\-|转)].+)*)$"))
				return true;
			if (Regex.IsMatch(src, @"^\(0{1}[1-9]{1}[0-9]{1,2}\)[1-9]{1}[0-9]{6,7}(((\-|转)[^(\-|转)].+)*)$"))
				return true;
			if (Regex.IsMatch(src, @"^0{1}[1-9]{1}[0-9]{1,2}[1-9]{1}[0-9]{6,7}(((\-|转)[^(\-|转)].+)*)$") && !src.IsCellphone())
				return true;
			if (Regex.IsMatch(src, @"^400[0123456789-]+$"))
				return true;
			if (Regex.IsMatch(src, @"^800[0123456789-]+$"))
				return true;
			return false;
		}

		/// <summary>
		/// 验证字符串是否包含一个固定电话号码
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsTelephone(this string src)
		{
			if (string.IsNullOrWhiteSpace(src)) return false;
			if (Regex.IsMatch(src, @"0{1}[1-9]{1}[0-9]{1,2}\-[1-9]{1}[0-9]{6,7}"))
				return true;
			if (Regex.IsMatch(src, @"\(0{1}[1-9]{1}[0-9]{1,2}\)[1-9]{1}[0-9]{6,7}"))
				return true;
			if (Regex.IsMatch(src, @"0{1}[1-9]{1}[0-9]{1,2}[1-9]{1}[0-9]{6,7}") && !src.IsCellphone())
				return true;
			if (Regex.IsMatch(src, @"400[0123456789-]+"))
				return true;
			if (Regex.IsMatch(src, @"800[0123456789-]+"))
				return true;
			return false;
		}

		/// <summary>
		/// 验证字符串是否包含一个电话号码（手机或座机）
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsPhoneNumber(this string src)
		{
			return src.IsCellphone() || src.IsTelephone();
		}

		/// <summary>
		/// 验证字符串是否严格是一个邮箱地址
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsStrictEmail(this string src)
		{
			if (string.IsNullOrWhiteSpace(src)) return false;
			//return Regex.IsMatch(src, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
			return Regex.IsMatch(src, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(((([0-9a-zA-Z][-\w]*[0-9a-zA-Z])|([0-9a-zA-Z]+))\.)+[a-zA-Z]{2,6}))$");
		}

		/// <summary>
		/// 验证字符串是否严格是一串中文
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsStrictCHN(this string src)
		{
			if (string.IsNullOrWhiteSpace(src)) return false;
			return Regex.IsMatch(src, @"^[\u4e00-\u9fa5]+$");
		}

		#region 身份证号码格式验证
		/// <summary>
		/// 验证字符串是否严格是一个身份证号码
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsStrictIDNumber(this string src)
		{
			if (src.Length == 15)
			{
				return checkIDNumber15(src);
			}
			else if (src.Length == 18)
			{
				return checkIDNumber18(src);
			}
			return false;
		}
		//验证15位身份证号码
		private static bool checkIDNumber15(string src)
		{
			long n = 0;
			if (long.TryParse(src, out n) == false || n < Math.Pow(10, 14))
			{
				return false;//数字验证
			}
			string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (address.IndexOf(src.Remove(2)) == -1)
			{
				return false;//省份验证
			}
			string birth = src.Substring(6, 6).Insert(4, "-").Insert(2, "-");
			DateTime time = new DateTime();
			if (DateTime.TryParse(birth, out time) == false)
			{
				return false;//生日验证
			}
			return true;//符合15位身份证标准
		}
		//验证18位身份证号码
		private static bool checkIDNumber18(string src)
		{
			long n = 0;
			if (long.TryParse(src.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(src.Replace('x', '0').Replace('X', '0'), out n) == false)
			{
				return false;//数字验证
			}
			string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (address.IndexOf(src.Remove(2)) == -1)
			{
				return false;//省份验证
			}
			string birth = src.Substring(6, 8).Insert(6, "-").Insert(4, "-");
			DateTime time = new DateTime();
			if (DateTime.TryParse(birth, out time) == false)
			{
				return false;//生日验证
			}
			string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
			string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
			char[] Ai = src.Remove(17).ToCharArray();
			int sum = 0;
			for (int i = 0; i < 17; i++)
			{
				sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
			}
			int y = -1;
			Math.DivRem(sum, 11, out y);
			if (arrVarifyCode[y] != src.Substring(17, 1).ToLower())
			{
				return false;//校验码验证
			}
			return true;//符合GB11643-1999标准
		}
		#endregion

		/// <summary>
		/// 验证字符串是否严格是银行帐号格式
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsStrictBankCard(this string src)
		{
			if (string.IsNullOrWhiteSpace(src)) return false;
			//清除空格
			src = Regex.Replace(src, @" ", "");
			//去掉检验位
			string code = src.Substring(0, src.Length - 1);
			if (!Regex.IsMatch(code, @"^\d+$")) return false;
			char[] chs = code.ToCharArray();
			int luhmSum = 0;
			for (int i = chs.Length - 1, j = 0; i >= 0; i--, j++)
			{
				int k = chs[i] - '0';
				if (j % 2 == 0)
				{
					k *= 2;
					k = k / 10 + k % 10;
				}
				luhmSum += k;
			}
			char r = (luhmSum % 10 == 0) ? '0' : (char)((10 - luhmSum % 10) + '0');
			return r.ToString() == src.Substring(src.Length - 1, 1);
		}
	}
}
