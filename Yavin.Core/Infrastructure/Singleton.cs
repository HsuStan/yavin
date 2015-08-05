/*****************************************
 * Create On: 2014.05.17. By: 徐斌
 * For: 程序内单例容器
 *****************************************/
using System;
using System.Collections.Generic;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 单例基类
	/// </summary>
	public class Singleton
	{
		private static readonly IDictionary<Type, object> _singletons;

		static Singleton()
		{
			Singleton._singletons = new Dictionary<Type, object>();
		}

		/// <summary>
		/// 系统类全部单例类的唯一实例
		/// </summary>
		public static IDictionary<Type, object> All
		{
			get { return Singleton._singletons; }
		}
	}
	
	/// <summary>
	/// 泛型单例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> : Singleton
	{
		private static T _instance;

		/// <summary>
		/// 取得指定类型的单例对象
		/// </summary>
		public static T Instance
		{
			get { return Singleton<T>._instance; }
			set
			{
				Singleton<T>._instance = value;
				Singleton.All[typeof(T)] = value;
			}
		}
	}
}
