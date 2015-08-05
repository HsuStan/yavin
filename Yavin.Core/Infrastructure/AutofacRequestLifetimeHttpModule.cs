/*****************************************
 * Create On: 2014.05.19. By: 徐斌
 * For: Autofac的HttpModule用于取得组件在web应用程序上下文的生存周期
 *****************************************/
using System;
using System.Web;
using Autofac;

namespace Yavin.Core.Infrastructure
{
	public class AutofacRequestLifetimeHttpModule : IHttpModule
	{
		/// <summary>
		/// Tag used to identify registrations that are scoped to the HTTP request level.
		/// </summary>
		public static readonly object HttpRequestTag = "AutofacWebRequest";

		#region IHttpModule 成员

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the 
		/// methods, properties, and events common to all application objects within an ASP.NET application</param>
		public void Init(HttpApplication context)
		{
			context.EndRequest += AutofacRequestLifetimeHttpModule.ContextEndRequest;
		}

		#endregion

		#region static
		/// <summary>
		/// Gets a nested lifetime scope that services can be resolved from.
		/// </summary>
		/// <param name="container">The parent container.</param>
		/// <param name="configurationAction">Action on a <see cref="ContainerBuilder"/>
		/// that adds component registations visible only in nested lifetime scopes.</param>
		/// <returns>A new or existing nested lifetime scope.</returns>
		public static ILifetimeScope GetLifetimeScope(ILifetimeScope container, Action<ContainerBuilder> configurationAction)
		{
			//little hack here to get dependencies when HttpContext is not available
			if (HttpContext.Current != null)
			{
				return AutofacRequestLifetimeHttpModule.LifetimeScope ??
					(AutofacRequestLifetimeHttpModule.LifetimeScope = AutofacRequestLifetimeHttpModule.InitializeLifetimeScope(configurationAction, container));
			}
			else
			{
				//throw new InvalidOperationException("HttpContextNotAvailable");
				return AutofacRequestLifetimeHttpModule.InitializeLifetimeScope(configurationAction, container);
			}
		}

		static ILifetimeScope LifetimeScope
		{
			get
			{
				return (ILifetimeScope)HttpContext.Current.Items[typeof(ILifetimeScope)];
			}
			set
			{
				HttpContext.Current.Items[typeof(ILifetimeScope)] = value;
			}
		}

		static void ContextEndRequest(object sender, EventArgs e)
		{
			ILifetimeScope lifetimeScope = LifetimeScope;
			if (lifetimeScope != null)
				lifetimeScope.Dispose();
		}

		static ILifetimeScope InitializeLifetimeScope(Action<ContainerBuilder> configurationAction, ILifetimeScope container)
		{
			return (configurationAction == null)
				? container.BeginLifetimeScope(HttpRequestTag)
				: container.BeginLifetimeScope(HttpRequestTag, configurationAction);
		}
		#endregion
	}
}
