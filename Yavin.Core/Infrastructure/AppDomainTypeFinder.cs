/*****************************************
 * Create On: 2014.05.19. By: 徐斌
 * For: 用于搜索系统普通应用程序集的方法接口实现
 *****************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 应用程序集搜索方法接口的实现
	/// </summary>
	public class AppDomainTypeFinder : ITypeFinder
	{
		#region 字段
		private bool _loadAppDomainAssemblies = true;
		//以下字符开头的程序集将跳过
		private string _assemblyIgnorePattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^ComponentArt|^MvcContrib|^AjaxControlToolkit|^Antlr3|^Remotion|^Recaptcha";
		private string _assemblyRestrictPattern = ".*";
		private IList<string> _assemblyNames = new List<string>();
		private readonly List<Type> _assemblyAttributesSearched = new List<Type>();
		private readonly List<AttributedAssembly> _attributedAssemblies = new List<AttributedAssembly>();
		#endregion

		#region 构造
		public AppDomainTypeFinder() { }
		#endregion

		#region 属性
		/// <summary>
		/// 当前应用程序域
		/// </summary>
		public virtual AppDomain App
		{
			get { return AppDomain.CurrentDomain; }
		}

		/// <summary>
		/// 是否载入程序集
		/// </summary>
		public virtual bool LoadAppDomainAssemblies
		{
			get { return this._loadAppDomainAssemblies; }
			set { this._loadAppDomainAssemblies = value; }
		}

		/// <summary>
		/// 所有已载入程序集名称
		/// </summary>
		public virtual IList<string> AssemblyNames
		{
			get { return this._assemblyNames; }
			set { this._assemblyNames = value; }
		}

		/// <summary>
		/// 不会被查找的程序集名称模式
		/// </summary>
		public virtual string AssemblyIgnorePattern
		{
			get { return this._assemblyIgnorePattern; }
			set { this._assemblyIgnorePattern = value; }
		}

		/// <summary>
		/// 将被查找程序集名称模式
		/// </summary>
		public virtual string AssemblyRestrictPattern
		{
			get { return this._assemblyRestrictPattern; }
			set { this._assemblyRestrictPattern = value; }
		}
		#endregion

		#region 私有
		private class AttributedAssembly
		{
			internal Assembly Assembly { get; set; }
			internal Type PluginAttributeType { get; set; }
		}
		/// <summary>
		/// 将应用程序域中的满足名称匹配模式的程序集加入集合
		/// </summary>
		/// <param name="addedAssemblyNames"></param>
		/// <param name="assemblies"></param>
		private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (this.Matches(assembly.FullName))
				{
					if (!addedAssemblyNames.Contains(assembly.FullName))
					{
						assemblies.Add(assembly);
						addedAssemblyNames.Add(assembly.FullName);
					}
				}
			}
		}
		#endregion

		#region 保护
		/// <summary>
		/// 检查指定的程序集名称是否匹配指定的名称模式
		/// </summary>
		/// <param name="assemblyFullName"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		protected virtual bool Matches(string assemblyFullName, string pattern)
		{
			return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		/// <summary>
		/// 将指定程序集名称集合中的程序集加入程序集集合
		/// </summary>
		/// <param name="addedAssemblyNames"></param>
		/// <param name="assemblies"></param>
		protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
		{
			foreach (string assemblyName in AssemblyNames)
			{
				Assembly assembly = Assembly.Load(assemblyName);
				if (!addedAssemblyNames.Contains(assembly.FullName))
				{
					assemblies.Add(assembly);
					addedAssemblyNames.Add(assembly.FullName);
				}
			}
		}

		protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
		{
			try
			{
				var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
				foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
				{
					if (!implementedInterface.IsGenericType)
					{
						continue;
					}
					var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
					return isMatch;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		protected virtual void LoadMatchingAssemblies(string directoryPath)
		{
			var loadedAssemblyNames = new List<string>();
			foreach (Assembly a in GetAssemblies())
			{
				loadedAssemblyNames.Add(a.FullName);
			}

			if (!Directory.Exists(directoryPath))
			{
				return;
			}

			foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
			{
				try
				{
					var an = AssemblyName.GetAssemblyName(dllPath);
					if (this.Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
					{
						App.Load(an);
					}
				}
				catch (BadImageFormatException ex)
				{
					Trace.TraceError(ex.ToString());
				}
			}
		}
		#endregion

		#region 公共
		/// <summary>
		/// 检查指定程序集名称是否匹配名称模式
		/// </summary>
		/// <param name="assemblyFullName"></param>
		/// <returns></returns>
		public virtual bool Matches(string assemblyFullName)
		{
			//不是忽略的名称且是要求的名称
			return !this.Matches(assemblyFullName, this.AssemblyIgnorePattern)
				   && this.Matches(assemblyFullName, this.AssemblyRestrictPattern);
		}
		#endregion

		#region ITypeFinder 成员

		public virtual IList<Assembly> GetAssemblies()
		{
			var foundAssemblyNames = new List<string>();
			var assemblies = new List<Assembly>();

			if (this.LoadAppDomainAssemblies)
			{
				this.AddAssembliesInAppDomain(foundAssemblyNames, assemblies);
			}
			this.AddConfiguredAssemblies(foundAssemblyNames, assemblies);
			return assemblies;
		}

		public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
		{
			return this.FindClassesOfType(assignTypeFrom, this.GetAssemblies(), onlyConcreteClasses);
		}

		public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
		{
			var result = new List<Type>();
			try
			{
				foreach (var a in assemblies)
				{
					foreach (var t in a.GetTypes())
					{
						if (assignTypeFrom.IsAssignableFrom(t) ||
							(assignTypeFrom.IsGenericTypeDefinition && this.DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
						{
							if (!t.IsInterface)
							{
								if (onlyConcreteClasses)
								{
									if (t.IsClass && !t.IsAbstract)
									{
										result.Add(t);
									}
								}
								else
								{
									result.Add(t);
								}
							}
						}
					}
				}
			}
			catch (ReflectionTypeLoadException ex)
			{
				var msg = string.Empty;
				foreach (var e in ex.LoaderExceptions)
				{
					msg += e.Message + Environment.NewLine;
				}
				var fail = new Exception(msg, ex);
				Debug.WriteLine(fail.Message, fail);
				throw fail;
			}
			return result;
		}

		public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
		{
			return this.FindClassesOfType(typeof(T), onlyConcreteClasses);
		}

		public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
		{
			return this.FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
		}

		public IEnumerable<Type> FindClassesOfType<T, TAssemblyAttribute>(bool onlyConcreteClasses = true) where TAssemblyAttribute : Attribute
		{
			var found = this.FindAssembliesWithAttribute<TAssemblyAttribute>();
			return this.FindClassesOfType<T>(found, onlyConcreteClasses);
		}

		public IEnumerable<Assembly> FindAssembliesWithAttribute<T>()
		{
			return this.FindAssembliesWithAttribute<T>(this.GetAssemblies());
		}

		public IEnumerable<Assembly> FindAssembliesWithAttribute<T>(IEnumerable<Assembly> assemblies)
		{
			if (!this._assemblyAttributesSearched.Contains(typeof(T)))
			{
				var foundAssemblies = (from assembly in assemblies
									   let customAttributes = assembly.GetCustomAttributes(typeof(T), false)
									   where customAttributes.Any()
									   select assembly).ToList();
				this._assemblyAttributesSearched.Add(typeof(T));
				foreach (var a in foundAssemblies)
				{
					this._attributedAssemblies.Add(new AttributedAssembly { Assembly = a, PluginAttributeType = typeof(T) });
				}
			}

			return this._attributedAssemblies
				.Where(x => x.PluginAttributeType.Equals(typeof(T)))
				.Select(x => x.Assembly)
				.ToList();
		}

		public IEnumerable<Assembly> FindAssembliesWithAttribute<T>(DirectoryInfo assemblyPath)
		{
			var assemblies = (from f in Directory.GetFiles(assemblyPath.FullName, "*.dll")
							  select Assembly.LoadFrom(f)
								  into assembly
								  let customAttributes = assembly.GetCustomAttributes(typeof(T), false)
								  where customAttributes.Any()
								  select assembly).ToList();
			return this.FindAssembliesWithAttribute<T>(assemblies);
		}

		#endregion
	}
}
