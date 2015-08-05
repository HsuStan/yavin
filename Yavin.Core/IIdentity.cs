using System;

namespace Yavin.Core
{
	/// <summary>
	/// 具有主键ID值的接口
	/// </summary>
	public interface IIdentity
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		Guid Id { get; }
	}
}
