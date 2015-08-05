/*****************************************
 * Create On: 2014.05.19. By: 徐斌
 * For: 组件容器管理器，用于注册服务及其实现，并在需要时从上下文中取得服务实现的实例
 *****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Builder;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// Autofac封装
	/// </summary>
	public class ContainerManager
	{
		#region 字段
		private readonly IContainer _container;
		#endregion

		#region 构造
		public ContainerManager(IContainer container)
		{
			this._container = container;
		}
		#endregion
		/// <summary>
		/// 表示装载组件的容器接口
		/// </summary>
		public IContainer Container
		{
			get { return this._container; }
		}
		
		/// <summary>
		/// 以指定的key与组件生存周期添加指定的组件
		/// </summary>
		/// <typeparam name="TService">组件型参</typeparam>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.AddComponent<TService, TService>(key, lifeStyle);
		}

		/// <summary>
		/// 以指定的key与组件生存周期添加指定的组件
		/// </summary>
		/// <param name="service"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.AddComponent(service, service, key, lifeStyle);
		}

		/// <summary>
		/// 以指定的key与生存周期为指定的服务添加指定的实现
		/// </summary>
		/// <typeparam name="TService">服务</typeparam>
		/// <typeparam name="TImplementation">实现</typeparam>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.AddComponent(typeof(TService), typeof(TImplementation), key, lifeStyle);
		}

		/// <summary>
		/// 以指定的key与生存周期为指定的服务类型添加指定的实现类型
		/// </summary>
		/// <param name="service">服务类型</param>
		/// <param name="implementation">实现类型</param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.UpdateContainer(x => {
				var serviceTypes = new List<Type> { service };

				if (service.IsGenericType)
				{
					var temp = x.RegisterGeneric(implementation).As(
						serviceTypes.ToArray()).PerLifeStyle(lifeStyle);
					if (!string.IsNullOrEmpty(key))
					{
						temp.Keyed(key, service);
					}
				}
				else
				{
					var temp = x.RegisterType(implementation).As(
						serviceTypes.ToArray()).PerLifeStyle(lifeStyle);
					if (!string.IsNullOrEmpty(key))
					{
						temp.Keyed(key, service);
					}
				}
			});
		}

		/// <summary>
		/// 以指定的key和组件生存周期为指定的服务添加实现的实例
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <param name="instance"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentInstance<TService>(object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.AddComponentInstance(typeof(TService), instance, key, lifeStyle);
		}

		/// <summary>
		/// 以指定的key和组件生存周期为指定的服务添加实现的实例
		/// </summary>
		/// <param name="service"></param>
		/// <param name="instance"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentInstance(Type service, object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.UpdateContainer(x => {
				var registration = x.RegisterInstance(instance).Keyed(key, service).As(service).PerLifeStyle(lifeStyle);
			});
		}

		/// <summary>
		/// 以指定的key和组件生存周期为指定的服务添加实现的实例
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentInstance(object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.AddComponentInstance(instance.GetType(), instance, key, lifeStyle);
		}

		/// <summary>
		/// 以带参数的形式用指定的key与生存周期为指定的服务型参添加指定的实现型参
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <typeparam name="TImplementation"></typeparam>
		/// <param name="properties"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentWithParameters<TService, TImplementation>(IDictionary<string, string> properties, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.AddComponentWithParameters(typeof(TService), typeof(TImplementation), properties, key, lifeStyle);
		}

		/// <summary>
		/// 以带参数的形式用指定的key与生存周期为指定的服务类型添加指定的实现类型
		/// </summary>
		/// <param name="service"></param>
		/// <param name="implementation"></param>
		/// <param name="properties"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentWithParameters(Type service, Type implementation, IDictionary<string, string> properties, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.UpdateContainer(x => {
				var serviceTypes = new List<Type> { service };

				var temp = x.RegisterType(implementation).As(serviceTypes.ToArray()).
					WithParameters(properties.Select(y => new NamedParameter(y.Key, y.Value)));
				if (!string.IsNullOrEmpty(key))
				{
					temp.Keyed(key, service);
				}
			});
		}

		/// <summary>
		/// 从组件上下文中解析指定的服务型参，返回服务的实现
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Resolve<T>(string key = "") where T : class
		{
			if (string.IsNullOrEmpty(key))
			{
				return this.Scope().Resolve<T>();
			}
			return this.Scope().ResolveKeyed<T>(key);
		}

		/// <summary>
		/// 从组件上下文中解析指定的服务类型对象实例
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object Resolve(Type type)
		{
			return this.Scope().Resolve(type);
		}

		/// <summary>
		/// 从组件上下文中解析指定的服务型参，返回服务的实现集合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T[] ResolveAll<T>(string key = "")
		{
			if (string.IsNullOrEmpty(key))
			{
				return Scope().Resolve<IEnumerable<T>>().ToArray();
			}
			return Scope().ResolveKeyed<IEnumerable<T>>(key).ToArray();
		}

		/// <summary>
		/// 解析未注册指定型参的实现实例
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T ResolveUnregistered<T>() where T : class
		{
			return this.ResolveUnregistered(typeof(T)) as T;
		}

		/// <summary>
		/// 解析指定类型的实例
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object ResolveUnregistered(Type type)
		{
			var constructors = type.GetConstructors();
			foreach (var constructor in constructors)
			{
				try
				{
					var parameters = constructor.GetParameters();
					var parameterInstances = new List<object>();
					foreach (var parameter in parameters)
					{
						var service = this.Resolve(parameter.ParameterType);
						if (service == null) throw new Exception("Unkown dependency");
						parameterInstances.Add(service);
					}
					return Activator.CreateInstance(type, parameterInstances.ToArray());
				}
				catch (Exception)
				{

				}
			}
			throw new Exception("No contructor was found that had all the dependencies satisfied.");
		}

		/// <summary>
		/// 尝试解析指定类型，解析成功返回true，失败返回false
		/// </summary>
		/// <param name="serviceType"></param>
		/// <param name="instance"></param>
		/// <returns></returns>
		public bool TryResolve(Type serviceType, out object instance)
		{
			return this.Scope().TryResolve(serviceType, out instance);
		}

		/// <summary>
		/// 判断指定类型是否已注册
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public bool IsRegistered(Type serviceType)
		{
			return this.Scope().IsRegistered(serviceType);
		}

		/// <summary>
		/// 解析指定服务类型的实现，未注册类型返回null
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public object ResolveOptional(Type serviceType)
		{
			return this.Scope().ResolveOptional(serviceType);
		}

		/// <summary>
		/// 更新组件容器
		/// </summary>
		/// <param name="action"></param>
		public void UpdateContainer(Action<ContainerBuilder> action)
		{
			var builder = new ContainerBuilder();
			action.Invoke(builder);
			builder.Update(_container);
		}

		/// <summary>
		/// 取得组件生存周期范围
		/// </summary>
		/// <returns></returns>
		public ILifetimeScope Scope()
		{
			try
			{
				return AutofacRequestLifetimeHttpModule.GetLifetimeScope(this.Container, null);
			}
			catch
			{
				return this.Container;
			}
		}
	}
	/// <summary>
	/// 组件生存周期配置扩展
	/// </summary>
	public static class ContainerManagerExtensions
	{
		public static Autofac.Builder.IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> PerLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder, ComponentLifeStyle lifeStyle)
		{
			switch (lifeStyle)
			{
				case ComponentLifeStyle.LifetimeScope:
					return HttpContext.Current != null ? builder.InstancePerRequest() : builder.InstancePerLifetimeScope();
				case ComponentLifeStyle.Transient:
					return builder.InstancePerDependency();
				case ComponentLifeStyle.Singleton:
					return builder.SingleInstance();
				default:
					return builder.SingleInstance();
			}
		}
	}
}
