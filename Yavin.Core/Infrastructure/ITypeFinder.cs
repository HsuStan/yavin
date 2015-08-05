/*****************************************
 * Create On: 2014.05.17. By: 徐斌
 * For: 用于搜索系统程序集的方法接口
 *****************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 类型搜索器接口
	/// </summary>
	public interface ITypeFinder
	{
		/// <summary>
		/// 取得所有程序集文件
		/// </summary>
		/// <returns></returns>
		IList<Assembly> GetAssemblies();

		/// <summary>
		/// 查找指定类型
		/// </summary>
		/// <param name="assignTypeFrom"></param>
		/// <param name="onlyConcreteClasses"></param>
		/// <returns></returns>
		IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

		/// <summary>
		/// 查找指定程序集中的指定类型
		/// </summary>
		/// <param name="assignTypeFrom"></param>
		/// <param name="assemblies"></param>
		/// <param name="onlyConcreteClasses"></param>
		/// <returns></returns>
		IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

		/// <summary>
		/// 查找指定泛型的类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="onlyConcreteClasses"></param>
		/// <returns></returns>
		IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

		/// <summary>
		/// 查找指定程序集中的指定泛型类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assemblies"></param>
		/// <param name="onlyConcreteClasses"></param>
		/// <returns></returns>
		IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

		/// <summary>
		/// 查找继承了特性的程序集的指定泛型类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TAssemblyAttribute"></typeparam>
		/// <param name="onlyConcreteClasses"></param>
		/// <returns></returns>
		IEnumerable<Type> FindClassesOfType<T, TAssemblyAttribute>(bool onlyConcreteClasses = true) where TAssemblyAttribute : Attribute;

		/// <summary>
		/// 查找所有继承了特性的程序集
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IEnumerable<Assembly> FindAssembliesWithAttribute<T>();

		/// <summary>
		/// 在指定程序集中查找继承了特性的泛型类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assemblies"></param>
		/// <returns></returns>
		IEnumerable<Assembly> FindAssembliesWithAttribute<T>(IEnumerable<Assembly> assemblies);

		/// <summary>
		/// 在指定目录下查找程序集
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assemblyPath"></param>
		/// <returns></returns>
		IEnumerable<Assembly> FindAssembliesWithAttribute<T>(DirectoryInfo assemblyPath);
	}
}
