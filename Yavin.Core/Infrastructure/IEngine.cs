/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: 组件装配引擎接口
 *****************************************/
using System;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 组件装配引擎接口
	/// </summary>
	public interface IEngine
	{
		/// <summary>
		/// 组件容器
		/// </summary>
		ContainerManager ContainerManager { get; }

		/// <summary>
		/// 根据基础配置初始化引擎
		/// </summary>
		/// <param name="config"></param>
		void Initialize(BaseConfig config);

		/// <summary>
		/// 解析型参服务的实现
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T Resolve<T>() where T : class;

		/// <summary>
		/// 解析指定类型服务的实现实例
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		object Resolve(Type type);
	}
}
