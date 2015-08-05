/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: IoC容器配置生成器
 *****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 配置IoC容器
	/// </summary>
	public class ContainerConfigurer
	{
		/// <summary>
		/// 配置项Key常量
		/// </summary>
		public static class ConfigurationKeys
		{
			public const string MediumTrust = "MediumTrust";
			public const string FullTrust = "FullTrust";
		}

		/// <summary>
		/// 执行配置
		/// </summary>
		/// <param name="engine"></param>
		/// <param name="containerManager"></param>
		/// <param name="broker"></param>
		/// <param name="configuration"></param>
		public virtual void Configure(IEngine engine, ContainerManager containerManager, EventBroker broker, BaseConfig configuration)
		{
			//基础组件
			//添加基础配置的实例
			containerManager.AddComponentInstance<BaseConfig>(configuration, "Platform.Configuration");
			//添加引擎的实现
			containerManager.AddComponentInstance<IEngine>(engine, "Platform.Engine");
			//添加组件容器配置服务的实例
			containerManager.AddComponentInstance<ContainerConfigurer>(this, "Platform.ContainerConfigurer");
			//添加类型查找器服务的实现
			containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("Platform.TypeFinder");

			//注册的其他组件提供的依赖
			var typeFinder = containerManager.Resolve<ITypeFinder>();
			containerManager.UpdateContainer(x => {
				var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
				var drInstances = new List<IDependencyRegistrar>();
				foreach (var drType in drTypes)
				{
					drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
				}
				//排序
				drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
				foreach (var dependencyRegistrar in drInstances)
				{
					dependencyRegistrar.Register(x, typeFinder);
				}
			});

			//事件代理
			containerManager.AddComponentInstance(broker);

			//添加通过特性配置的依赖关系
			containerManager.AddComponent<DependencyAttributeRegistrator>("Platform.DependencyRegistrator");
			var registrator = containerManager.Resolve<DependencyAttributeRegistrator>();
			var services = registrator.FindServices();
			var configurations = this.GetComponentConfigurations(configuration);
			services = registrator.FilterServices(services, configurations);
			registrator.RegisterServices(services);
		}

		protected virtual string[] GetComponentConfigurations(BaseConfig configuration)
		{
			var configurations = new List<string>();
			var trustConfiguration = (this.GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
				? ConfigurationKeys.FullTrust
				: ConfigurationKeys.MediumTrust;
			configurations.Add(trustConfiguration);
			return configurations.ToArray();
		}

		private static AspNetHostingPermissionLevel? _trustLevel = null;
		private AspNetHostingPermissionLevel GetTrustLevel()
		{
			if (!_trustLevel.HasValue)
			{
				_trustLevel = AspNetHostingPermissionLevel.None;
				foreach (AspNetHostingPermissionLevel trustLevel in
						new AspNetHostingPermissionLevel[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
				{
					try
					{
						new AspNetHostingPermission(trustLevel).Demand();
						_trustLevel = trustLevel;
						break;
					}
					catch (SecurityException)
					{
						continue;
					}
				}
			}
			return _trustLevel.Value;
		}
	}
}
