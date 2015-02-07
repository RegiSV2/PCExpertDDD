using System.Net.Http;

namespace PCExpert.Web.Api.Common
{
	public class CurrentRequestHandler : DelegatingHandler
	{
		protected async override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			var scope = request.GetDependencyScope();
			var currentRequest = (CurrentRequest)scope.GetService(typeof(CurrentRequest));
			currentRequest.Value = request;
			return await base.SendAsync(request, cancellationToken);
		}
	}
}