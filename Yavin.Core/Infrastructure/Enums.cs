/*****************************************
 * Create On: 2014.05.19. By: 徐斌
 * For: 基础设施枚举定义
 *****************************************/
using System;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 组件方式
	/// </summary>
	public enum ComponentLifeStyle
	{
		Singleton = 0,
		Transient = 1,
		LifetimeScope = 2
	}
}
