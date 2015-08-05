/*****************************************
 * Create On: 2014.05.19. By: 徐斌
 * For: 用于搜索Web应用程序集的方法接口实现
 *****************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// Web应用程序集搜索方法接口的实现
	/// </summary>
	public class WebAppTypeFinder : AppDomainTypeFinder
	{
		#region 字段
		private bool _ensureBinFolderAssembliesLoaded = true;
		private bool _binFolderAssembliesLoaded = false;
		#endregion

		#region 构造
		public WebAppTypeFinder(BaseConfig config)
		{
			this._ensureBinFolderAssembliesLoaded = config.DynamicDiscovery;
		}
		#endregion

		#region 属性
		/// <summary>
		/// 是否载入Bin文件夹下程序集
		/// </summary>
		public bool EnsureBinFolderAssembliesLoaded
		{
			get { return this._ensureBinFolderAssembliesLoaded; }
			set { this._ensureBinFolderAssembliesLoaded = value; }
		}
		#endregion

		#region 私有
		/// <summary>
		/// 返回Bin目录的物理路径
		/// </summary>
		protected virtual string GetBinDirectory()
		{
			if (HostingEnvironment.IsHosted)
			{
				return HttpRuntime.BinDirectory;
			}
			else
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}
		#endregion

		#region override

		public override IList<Assembly> GetAssemblies()
		{
			if (this.EnsureBinFolderAssembliesLoaded && !this._binFolderAssembliesLoaded)
			{
				_binFolderAssembliesLoaded = true;
				string binPath = this.GetBinDirectory();
				base.LoadMatchingAssemblies(binPath);
			}
			return base.GetAssemblies();
		}
		#endregion
	}
}
