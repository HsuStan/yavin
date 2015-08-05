/*****************************************
 * Create On: 2014.05.21. By: 徐斌
 * For: 应用程序级事件代理
 *****************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Yavin.Core.Infrastructure
{
	/// <summary>
	/// 事件代理
	/// </summary>
	public class EventBroker
	{
		static EventBroker()
		{
			Instance = new EventBroker();
		}

		public static EventBroker Instance
		{
			get { return Singleton<EventBroker>.Instance; }
			protected set { Singleton<EventBroker>.Instance = value; }
		}

		public virtual void Attach(HttpApplication application)
		{
			Trace.WriteLine("EventBroker: Attaching to " + application);

			application.BeginRequest += Application_BeginRequest;
			application.AuthorizeRequest += Application_AuthorizeRequest;

			application.PostResolveRequestCache += Application_PostResolveRequestCache;
			application.PostMapRequestHandler += Application_PostMapRequestHandler;

			application.AcquireRequestState += Application_AcquireRequestState;
			application.Error += Application_Error;
			application.EndRequest += Application_EndRequest;

			application.Disposed += Application_Disposed;
		}

		public EventHandler<EventArgs> BeginRequest;
		public EventHandler<EventArgs> AuthorizeRequest;
		public EventHandler<EventArgs> PostResolveRequestCache;
		public EventHandler<EventArgs> AcquireRequestState;
		public EventHandler<EventArgs> PostMapRequestHandler;
		public EventHandler<EventArgs> Error;
		public EventHandler<EventArgs> EndRequest;

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (this.BeginRequest != null)
			{
				Debug.WriteLine("Application_BeginRequest");
				this.BeginRequest(sender, e);
			}
		}

		protected void Application_AuthorizeRequest(object sender, EventArgs e)
		{
			if (this.AuthorizeRequest != null)
			{
				Debug.WriteLine("Application_AuthorizeRequest");
				this.AuthorizeRequest(sender, e);
			}
		}

		private void Application_PostResolveRequestCache(object sender, EventArgs e)
		{
			if (this.PostResolveRequestCache != null)
			{
				Debug.WriteLine("Application_PostResolveRequestCache");
				this.PostResolveRequestCache(sender, e);
			}
		}

		private void Application_PostMapRequestHandler(object sender, EventArgs e)
		{
			if (this.PostMapRequestHandler != null)
			{
				Debug.WriteLine("Application_PostMapRequestHandler");
				this.PostMapRequestHandler(sender, e);
			}
		}

		protected void Application_AcquireRequestState(object sender, EventArgs e)
		{
			if (this.AcquireRequestState != null)
			{
				Debug.WriteLine("Application_AcquireRequestState");
				this.AcquireRequestState(sender, e);
			}
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			if (this.Error != null)
				this.Error(sender, e);
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			if (this.EndRequest != null)
				this.EndRequest(sender, e);
		}

		void Application_Disposed(object sender, EventArgs e)
		{
			Trace.WriteLine("EventBroker: Disposing " + sender);
		}
	}
}
