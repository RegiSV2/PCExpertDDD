using System.Collections.Generic;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Model.Core
{
	public class ComponentInterfaceModel : ComponentInterfaceVO, ILinksContaining
	{
		public IList<WebLink> Links { get; set; }
	}
}