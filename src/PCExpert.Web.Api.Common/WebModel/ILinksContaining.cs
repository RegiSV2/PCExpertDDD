using System.Collections.Generic;

namespace PCExpert.Web.Api.Common.WebModel
{
	/// <summary>
	///     Mixes web links into classes
	/// </summary>
	public interface ILinksContaining
	{
		IList<WebLink> Links { get; set; }
	}
}