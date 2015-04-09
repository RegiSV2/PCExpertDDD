using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using PCExpert.Web.Api.Resources;

namespace PCExpert.Web.Api.Filters
{
	public class PCExpertExceptionFilter : ExceptionFilterAttribute, System.Web.Mvc.IExceptionFilter
	{
		public override void OnException(HttpActionExecutedContext context)
		{
			var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
			{
				Content = new StringContent(Messages.UnexpectedErrorOccured),
				ReasonPhrase = Messages.UnexpectedErrorOccured,
				StatusCode = HttpStatusCode.InternalServerError
			};
			context.Response = msg;
		}

		public void OnException(ExceptionContext filterContext)
		{
			filterContext.Result = new HttpStatusCodeResult(
				HttpStatusCode.InternalServerError,
				Messages.UnexpectedErrorOccured);
		}
	}
}