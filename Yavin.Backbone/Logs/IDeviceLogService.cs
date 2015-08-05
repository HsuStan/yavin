using System;
using Yavin.Model.Common;
using Yavin.Model.Logs;

namespace Yavin.Backbone.Logs
{
	/// <summary>
	/// 设备日志基础数据服务接口
	/// </summary>
	public interface IDeviceLogService
	{
		/// <summary>
		/// 新增
		/// </summary>
		/// <param name="log"></param>
		void Insert(DeviceLog log);

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="id"></param>
		/// <param name="log"></param>
		void Update(long id, DeviceLog log);

		/// <summary>
		/// 根据搜索条件取得符合条件数据总数
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		int Count(Search search);

		/// <summary>
		/// 根据主键获取
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		DeviceLog Select(long id);

		/// <summary>
		/// 根据搜索条件分页获取，按创建时间倒序排列
		/// </summary>
		/// <param name="search"></param>
		/// <param name="page"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		Paging<DeviceLog> Select(Search search, int page, int size);
	}
}
