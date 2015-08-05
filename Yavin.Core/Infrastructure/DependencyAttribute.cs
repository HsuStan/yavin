/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: 自动通过IoC注册服务特性类
 *****************************************/
using System;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 自动通过IoC注册服务特性类
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class DependencyAttribute : Attribute
	{
		public DependencyAttribute(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.LifeStyle = lifeStyle;
		}

		public DependencyAttribute(Type serviceType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			this.LifeStyle = lifeStyle;
			this.ServiceType = serviceType;
		}

		/// <summary>
		/// 通过特性指定的服务类型
		/// </summary>
		public Type ServiceType { get; protected set; }

		/// <summary>
		/// 通过特性指定的组件生存周期
		/// </summary>
		public ComponentLifeStyle LifeStyle { get; protected set; }

		/// <summary>
		/// 查找实现类型的KEY
		/// </summary>
		public string Key { get; set; }

		public string Configuration { get; set; }

		public virtual void RegisterService(AttributeInfo<DependencyAttribute> attributeInfo, ContainerManager container)
		{
			Type serviceType = attributeInfo.Attribute.ServiceType ?? attributeInfo.DecoratedType;
			container.AddComponent(serviceType, 
				attributeInfo.DecoratedType, 
				attributeInfo.Attribute.Key ?? attributeInfo.DecoratedType.FullName, 
				attributeInfo.Attribute.LifeStyle);
		}
	}
}
