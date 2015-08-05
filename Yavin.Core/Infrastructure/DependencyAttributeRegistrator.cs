/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: 将使用特性表明了依赖关系的类型通过IoC注册服务
 *****************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 将使用特性表明了依赖关系的类型通过IoC注册服务
	/// </summary>
	public class DependencyAttributeRegistrator
	{
		private readonly ITypeFinder _finder;
		private readonly IEngine _engine;

		public DependencyAttributeRegistrator(ITypeFinder finder, IEngine engine)
		{
			this._finder = finder;
			this._engine = engine;
		}

		public virtual IEnumerable<AttributeInfo<DependencyAttribute>> FindServices()
		{
			foreach (Type type in this._finder.FindClassesOfType<object>())
			{
				var attributes = type.GetCustomAttributes(typeof(DependencyAttribute), false);
				foreach (DependencyAttribute attribute in attributes)
				{
					yield return new AttributeInfo<DependencyAttribute> { Attribute = attribute, DecoratedType = type };
				}
			}
		}

		public virtual void RegisterServices(IEnumerable<AttributeInfo<DependencyAttribute>> services)
		{
			foreach (var info in services)
			{
				info.Attribute.RegisterService(info, this._engine.ContainerManager);
			}
		}

		public virtual IEnumerable<AttributeInfo<DependencyAttribute>> FilterServices(IEnumerable<AttributeInfo<DependencyAttribute>> services, params string[] configurationKeys)
		{
			return services.Where(s => s.Attribute.Configuration == null || configurationKeys.Contains(s.Attribute.Configuration));
		}
	}
}
