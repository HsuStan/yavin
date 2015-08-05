/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: 对引擎的引用
 *****************************************/
using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 对引擎的引用
	/// </summary>
	public class EngineContext
	{
		/// <summary>
		/// 初始化组件引擎
		/// </summary>
		/// <param name="forceRecreate"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static IEngine Initialize(bool forceRecreate)
		{
			if (Singleton<IEngine>.Instance == null || forceRecreate)
			{
				var config = ConfigurationManager.GetSection("BaseConfig") as BaseConfig;
				Singleton<IEngine>.Instance = CreateEngineInstance(config);
				Singleton<IEngine>.Instance.Initialize(config);
			}
			return Singleton<IEngine>.Instance;
		}

		private static IEngine CreateEngineInstance(BaseConfig config)
		{
			if (config != null && !string.IsNullOrEmpty(config.EngineType))
			{
				var engineType = Type.GetType(config.EngineType);
				if (engineType == null)
					throw new ConfigurationErrorsException("类型 '" + engineType + "' 没有找到.");
				if (!typeof(IEngine).IsAssignableFrom(engineType))
					throw new ConfigurationErrorsException("类型 '" + engineType + "' 没有实现IEngine.");
				return Activator.CreateInstance(engineType) as IEngine;
			}

			return new BaseEngine();
		}

		/// <summary>
		/// 其它启动任务
		/// </summary>
		public static void MissionToStart()
		{

		}

		/// <summary>
		/// 运行时替换引擎
		/// </summary>
		/// <param name="engine"></param>
		public static void Replace(IEngine engine)
		{
			Singleton<IEngine>.Instance = engine;
		}

		/// <summary>
		/// 当前引擎实例
		/// </summary>
		public static IEngine Current
		{
			get
			{
				if (Singleton<IEngine>.Instance == null)
				{
					EngineContext.Initialize(false);
				}
				return Singleton<IEngine>.Instance;
			}
		}
	}
}
