using System;
using System.Linq;

namespace Yavin.ORM
{
	/// <summary>
	/// 数据访问接口
	/// </summary>
	public interface IDataAccess<TEntity> where TEntity : BaseMeta
	{
		/// <summary>
		/// 新增
		/// </summary>
		/// <param name="entity"></param>
		void Insert(TEntity entity);

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="entity"></param>
		void Delete(TEntity entity);

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity"></param>
		void Update(TEntity entity);

		/// <summary>
		/// 根据主键取得
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		TEntity GetByID(object id);

		/// <summary>
		/// 对表的引用
		/// </summary>
		IQueryable<TEntity> Table { get; }
	}
}
