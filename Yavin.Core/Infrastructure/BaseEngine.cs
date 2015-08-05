/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: 引擎接口基础实现
 *****************************************/
using System;
using System.Configuration;
using Autofac;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 引擎接口基础实现
	/// </summary>
	public class BaseEngine : IEngine
	{
		#region 字段
		private ContainerManager _containerManager;
		#endregion

		#region 构造
		public BaseEngine()
			: this(EventBroker.Instance, new ContainerConfigurer())
		{
		}

		public BaseEngine(EventBroker broker, ContainerConfigurer configurer)
		{
			var config = ConfigurationManager.GetSection("BaseConfig") as BaseConfig;
			this.InitializeContainer(configurer, broker, config);
		}
		#endregion

		#region 私有
		/// <summary>
		/// 初始化容器
		/// </summary>
		/// <param name="configurer"></param>
		/// <param name="broker"></param>
		/// <param name="config"></param>
		private void InitializeContainer(ContainerConfigurer configurer, EventBroker broker, BaseConfig config)
		{
			var builder = new ContainerBuilder();
			this._containerManager = new ContainerManager(builder.Build());
			configurer.Configure(this, _containerManager, broker, config);
		}

		/// <summary>
		/// 执行系统启动任务
		/// </summary>
		private void RunStartupTasks()
		{
			
		}
		#endregion

		#region IEngine 成员
		/// <summary>
		/// 组件容器
		/// </summary>
		public virtual ContainerManager ContainerManager
		{
			get { return this._containerManager; }
		}

		/// <summary>
		/// 根据基础配置初始化 
		/// </summary>
		/// <param name="config"></param>
		public virtual void Initialize(BaseConfig config)
		{
			this.RunStartupTasks();
		}

		/// <summary>
		/// 返回指定服务型参的实现
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual T Resolve<T>() where T : class
		{
			return this._containerManager.Resolve<T>();
		}

		/// <summary>
		/// 返回指定服务类型的实现实例
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual object Resolve(Type type)
		{
			return this._containerManager.Resolve(type);
		}

		#endregion
	}
}
