using System;
using System.Runtime.Serialization;

namespace Yavin.Core
{
	/// <summary>
	/// 自定义异常基类
	/// </summary>
	public class BaseException : Exception
	{
		public BaseException()
		{
		}

		public BaseException(string message)
			: base(message)
		{
		}

		public BaseException(string messageFormat, params object[] args)
			: base(string.Format(messageFormat, args))
		{
		}

		protected BaseException(SerializationInfo
			info, StreamingContext context)
			: base(info, context)
		{
		}

		public BaseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
