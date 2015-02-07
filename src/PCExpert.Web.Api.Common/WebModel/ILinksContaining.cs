using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PCExpert.Web.Api.Common.WebModel
{
	/// <summary>
	/// Mixes web links into classes
	/// </summary>
	public interface ILinksContaining
	{
		IList<WebLink> Links { get; set; }
	}
}