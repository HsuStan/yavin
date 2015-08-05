using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Yavin.Core.JSON
{
	/// <summary>
	/// JSON序列化工具
	/// </summary>
	public class JsonSerializer
	{
		protected int _scope;
		protected Dictionary<int, List<MemberItem>> _objectBox;
		protected List<string> _patternBox;
		protected StringBuilder _jsonToken;
		protected int _scopeIndex;
		protected string _rootName = "Root";

		public JsonSerializer() : this(5) { }

		public JsonSerializer(int scope)
		{
			this._scope = scope;
		}

		/// <summary>
		/// 最大深度
		/// </summary>
		public int Scope
		{
			get { return this._scope; }
			set { this._scope = value; }
		}

		/// <summary>
		/// 广度控制参数中间字符
		/// </summary>
		public const char SplitChar = '|';

		/// <summary>
		/// 通配符
		/// </summary>
		public const string ASTERISK = "*";

		/// <summary>
		/// 以指定广度、深度序列化对象实例，兼容以前版本
		/// </summary>
		/// <param name="target"></param>
		/// <param name="fields"></param>
		/// <returns></returns>
		public string Serialize(object target, params string[] fields)
		{
			if (fields == null)
			{
				return this.Serialize(target, string.Empty);
			}
			else
			{
				return this.Serialize(target, string.Join(JsonSerializer.SplitChar.ToString(), fields));
			}
		}

		/// <summary>
		/// 以指定广度、深度序列化对象实例
		/// </summary>
		/// <param name="target"></param>
		/// <param name="scope"></param>
		/// <param name="fields"></param>
		/// <returns></returns>
		public string Serialize(object target, string fields = null)
		{
			//简单对象直接返回
			if (TypeHelper.IsPlainValue(target)) return Converter.ToString(target);
			//初始化返回容器
			this._jsonToken = new StringBuilder("key");
			//初始化深度控制容器
			this._objectBox = new Dictionary<int, List<MemberItem>>();
			//深度指示器
			this._scopeIndex = 0;
			#region 用占位字符将根对象添加到深度控制器的第一级
			var key = Guid.NewGuid().ToString();
			this._jsonToken.Replace("key", key);
			this._objectBox.Add(this._scopeIndex, new List<MemberItem>());
			this._objectBox[this._scopeIndex].Add(new MemberItem(key, target, this._rootName));
			#endregion
			//处理广度控制参数
			if (!string.IsNullOrEmpty(fields))
			{
				this.PreparePattern(fields);
			}
			//开始深度控制下的对象分解循环
			while (this._scopeIndex <= this._scope && this._objectBox.ContainsKey(this._scopeIndex))
			{
				var itemList = this._objectBox[this._scopeIndex];
				this._scopeIndex++;
				for (var i = 0; i < itemList.Count; i++)
				{
					var json = string.Empty;
					//区分集合对象与普通对象
					if (itemList[i].Value.GetType().GetInterface("IEnumerable") != null)
					{
						json = this.TryIEnumerable((IEnumerable)itemList[i].Value, itemList[i].FullName);
					}
					else
					{
						json = this.TryPlainObject(itemList[i].Value, itemList[i].FullName);
					}
					this._jsonToken.Replace(itemList[i].Key, json);
				}
			}
			return this._jsonToken.ToString();
		}

		protected string TryPlainObject(object target, string pattern)
		{
			//从广度控制容器中取得当前对象的全部广度控制参数
			var elements = this._patternBox != null ? this._patternBox.FindAll(i => i.StartsWith(pattern)) : null;
			//根据当前对象的广度控制参数取得可以分解的对象
			var members = TypeHelper.GetMembers(target.GetType(),
				MemberTypes.Field | MemberTypes.Property,
				true,
				m =>
				{
					if (string.IsNullOrEmpty(pattern) || elements == null) return true;
					var suffix = string.Empty;
					foreach (var e in elements)
					{
						//取得e字符串中pattern+.之后到下一个.之间的字符（找不到.则取全部）
						var indexStart = e.IndexOf(pattern) + pattern.Length + 1;
						var dotIndex = e.IndexOf('.', indexStart);
						var part = dotIndex < 0 ? e.Substring(indexStart) : e.Substring(indexStart, e.IndexOf('.', indexStart) - indexStart);
						suffix = string.Format("{0},{1}", suffix, part);
					}
					return suffix.Contains(string.Format(",{0}", JsonSerializer.ASTERISK)) || suffix.Contains(string.Format(",{0}", m.Name));
				});
			var json = new StringBuilder("{");
			foreach (var m in members)
			{
				var value = TypeHelper.GetMemberValue(m, target);
				if (TypeHelper.IsPlainValue(value))
				{
					json.AppendFormat(",\"{0}\":{1}", m.Name, Converter.ToString(value));
				}
				else
				{
					if (this._scopeIndex <= this._scope)
					{
						var key = Guid.NewGuid().ToString();
						json.AppendFormat(",\"{0}\":{1}", m.Name, key);
						if (!this._objectBox.ContainsKey(this._scopeIndex))
						{
							this._objectBox.Add(this._scopeIndex, new List<MemberItem>());
						}
						var fields = string.Format("{0}.{1}", pattern, m.Name);
						this._objectBox[this._scopeIndex].Add(new MemberItem(key, value, fields));
					}
				}
			}
			if (json.Length > 1) json.Remove(1, 1);
			json.Append("}");
			return json.ToString();
		}

		protected string TryIEnumerable(IEnumerable target, string pattern)
		{
			var json = new StringBuilder("[");
			foreach (var item in target)
			{
				if (TypeHelper.IsPlainValue(item))
				{
					json.AppendFormat(",{0}", Converter.ToString(item));
				}
				else
				{
					json.AppendFormat(",{0}", this.TryPlainObject(item, pattern));
				}
			}
			if (json.Length > 1) json.Remove(1, 1);
			json.Append("]");
			return json.ToString();
		}

		//为根对象的各个层级上的每个属性准备广度控制参数
		protected void PreparePattern(string pattern)
		{
			//去掉不必要的空格
			pattern = pattern.Replace(" ", "");
			//特别处理根对象的属性，加入Root前缀
			this._patternBox = new List<string>(pattern.Split(JsonSerializer.SplitChar));
			this._patternBox = this._patternBox.OrderBy(i => i.Count(j => j == '.')).ToList();
			for (var i = 0; i < this._patternBox.Count; i++)
			{
				this._patternBox[i] = string.Format("{0}.{1}", this._rootName, this._patternBox[i]);
			}
		}

		protected class MemberItem
		{
			/// <summary>
			/// 对象占位符
			/// </summary>
			public string Key { get; protected set; }

			/// <summary>
			/// 对象实例
			/// </summary>
			public object Value { get; protected set; }

			/// <summary>
			/// 对象分解广度控制参数
			/// </summary>
			public string FullName { get; protected set; }

			public MemberItem(string key, object value, string fullName)
			{
				this.Key = key;
				this.Value = value;
				this.FullName = fullName;
			}
		}
	}
}
