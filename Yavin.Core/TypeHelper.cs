using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Yavin.Core
{
	/// <summary>
	/// 类型处理实用工具，不能继承此类
	/// </summary>
	public sealed class TypeHelper
	{
		//缓存
		private static readonly ConcurrentDictionary<string, IEnumerable<MemberInfo>> MemberCache = new ConcurrentDictionary<string, IEnumerable<MemberInfo>>();

		/// <summary>
		/// 从缓存中取得指定类型的全部成员
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static IEnumerable<MemberInfo> GetMembers(Type type)
		{
			if (!TypeHelper.MemberCache.ContainsKey(type.FullName))
			{
				var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				TypeHelper.MemberCache.AddOrUpdate(type.FullName, members, (t, m) => m);
			}
			return TypeHelper.MemberCache[type.FullName];
		}

		/// <summary>
		/// 取得指定类型的指定成员类型的公共实例成员集合
		/// </summary>
		/// <param name="type">类型</param>
		/// <param name="memberType">成员类型</param>
		/// <param name="inherited">是否取得继承的成员</param>
		/// <returns></returns>
		public static IEnumerable<MemberInfo> GetMembers(Type type, MemberTypes memberType, bool inherited = false, Func<MemberInfo, bool> filter = null)
		{
			var members = new List<MemberInfo>();
			if (inherited)
			{
				var types = TypeHelper.GetBaseTypes(type);
				foreach (var t in types)
				{
					members.AddRange(TypeHelper.GetMembers(t));
				}
			}
			members.AddRange(TypeHelper.GetMembers(type));
			members = members.FindAll(m => (m.MemberType & memberType) > 0);
			if (filter != null)
			{
				members = members.FindAll(m => filter(m));
			}
			return members;
		}

		/// <summary>
		/// 取得指定类型的全部父类型
		/// </summary>
		/// <param name="type">类型</param>
		/// <returns></returns>
		public static IEnumerable<Type> GetBaseTypes(Type type)
		{
			var types = new List<Type>();
			var parentType = type.BaseType;
			while (parentType != null)
			{
				types.Add(parentType);
				parentType = parentType.BaseType;
			}
			return types;
		}

		/// <summary>
		/// 是否基础数据类型
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsPlainValue(object value)
		{
			if (null == value) return true;
			if (value.GetType().IsEnum) return true;
			switch (value.GetType().ToString())
			{
				case "System.Boolean": return true;
				case "System.Byte": return true;
				case "System.SByte": return true;
				case "System.Char": return true;
				case "System.Decimal": return true;
				case "System.Double": return true;
				case "System.DateTime": return true;
				case "System.Single": return true;
				case "System.Int16": return true;
				case "System.UInt16": return true;
				case "System.Int32": return true;
				case "System.UInt32": return true;
				case "System.Int64": return true;
				case "System.UInt64": return true;
				case "System.String": return true;
				case "System.Guid": return true;
			}
			return false;
		}

		/// <summary>
		/// 取指定对象的指定成员的值
		/// </summary>
		/// <param name="member"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static object GetMemberValue(MemberInfo member, object target, params object[] args)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Property: return target.GetType().GetProperty(member.Name).GetValue(target, null);
				case MemberTypes.Field: return target.GetType().GetField(member.Name).GetValue(target);
				case MemberTypes.Method: return target.GetType().GetMethod(member.Name).Invoke(target, args);
			}
			return null;
		}
	}
}
