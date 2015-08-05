/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: 服务注册接口。注册时将搜索程序集中所有实现此接口的类，
 *		按Order规定的顺序优先注册优先级高的注册器
 *****************************************/
using Autofac;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 服务注册器接口
	/// </summary>
	public interface IDependencyRegistrar
	{
		/// <summary>
		/// 执行注册
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="typeFinder"></param>
		void Register(ContainerBuilder builder, ITypeFinder typeFinder);

		/// <summary>
		/// 注册器优先级
		/// </summary>
		int Order { get; }
	}
}
