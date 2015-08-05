/*****************************************
 * Create On: 2014.05.19. By: 徐斌
 * For: 系统基础配置对象
 *****************************************/
using System;
using System.Configuration;
using System.Xml;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 系统基础配置对象
	/// </summary>
	public class BaseConfig : IConfigurationSectionHandler
	{
		#region IConfigurationSectionHandler 成员
		/// <summary>
		/// 创建一个配置节点
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			var config = new BaseConfig();
			var dynamicDiscoveryNode = section.SelectSingleNode("DynamicDiscovery");
			if (dynamicDiscoveryNode != null && dynamicDiscoveryNode.Attributes != null)
			{
				var attribute = dynamicDiscoveryNode.Attributes["Enabled"];
				if (attribute != null)
					config.DynamicDiscovery = Convert.ToBoolean(attribute.Value);
			}

			var engineNode = section.SelectSingleNode("Engine");
			if (engineNode != null && engineNode.Attributes != null)
			{
				var attribute = engineNode.Attributes["Type"];
				if (attribute != null)
					config.EngineType = attribute.Value;
			}

			return config;
		}

		#endregion

		/// <summary>
		/// 是否在bin目录加载组件
		/// </summary>
		public bool DynamicDiscovery { get; set; }

		/// <summary>
		/// 自定义Engine类型取代BaseEngine
		/// </summary>
		public string EngineType { get; set; }
	}
}
