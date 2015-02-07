using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace PCExpert.Web.Api.Filters
{
	public class PCExpertExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext context)
		{
#if !DEBUG
			var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
			{
				Content = new StringContent("An unexpected error occured"),
				ReasonPhrase = "An unexpected error occured",
				StatusCode = HttpStatusCode.InternalServerError
			};
			context.Response = msg;
#endif
		}
	}
}