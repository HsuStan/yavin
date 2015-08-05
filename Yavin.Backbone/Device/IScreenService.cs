using System;
using Yavin.Model.Common;
using Yavin.Model.Device;

namespace Yavin.Backbone.Device
{
	/// <summary>
	/// 显示屏基础数据服务接口
	/// </summary>
	public interface IScreenService
	{
		/// <summary>
		/// 新增
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		void Insert(Screen screen);

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="id"></param>
		/// <param name="screen"></param>
		/// <returns></returns>
		void Update(long id, Screen screen);

		/// <summary>
		/// 根据主键获取
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Screen Select(long id);

		/// <summary>
		/// 根据编号获取
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		Screen Select(string code);

		/// <summary>
		/// 根据搜索条件获取符合条件的屏幕的数量
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		int Count(Search search);

		/// <summary>
		/// 根据条件分页获取
		/// </summary>
		/// <param name="search"></param>
		/// <param name="page"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		Paging<Screen> Select(Search search, int page, int size);

		/// <summary>
		/// 根据中心点与半径获取范围内的全部屏幕数据
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		Screen[] Select(Point center, float radius);
	}
}
