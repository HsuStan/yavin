using System;

namespace Yavin.Model.Common
{
	/// <summary>
	/// 表示两个对象之间的关联关系
	/// </summary>
	public class Correlation
	{
		/// <summary>
		/// 关系的种类名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 关系中主体对象的主键
		/// </summary>
		public string MasterKey { get; set; }

		/// <summary>
		/// 关系中主体对象的类型
		/// </summary>
		public string MasterType { get; set; }

		/// <summary>
		/// 关系中主体对象的数据快照
		/// </summary>
		public string MasterData { get; set; }

		/// <summary>
		/// 关系中从属对象的主键
		/// </summary>
		public string SlaveKey { get; set; }

		/// <summary>
		/// 关系中从属对象的类型
		/// </summary>
		public string SlaveType { get; set; }

		/// <summary>
		/// 关系中从属对象的数据快照
		/// </summary>
		public string SlaveData { get; set; }

		/// <summary>
		/// 关系创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 是否有效
		/// </summary>
		public bool Enabled { get; set; }
	}
}
