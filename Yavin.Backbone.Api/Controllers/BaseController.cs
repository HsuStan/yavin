using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Yavin.Model.Common;

namespace Yavin.Backbone.Api.Controllers
{
	[RoutePrefix("test")]
    public class BaseController : ApiController
    {
		[Route("first")]
		[HttpGet]
		public virtual Result Select()
		{
			var rm = Result.SUCCESS;
			return rm;
		}
    }
}
