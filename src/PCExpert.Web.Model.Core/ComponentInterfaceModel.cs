using System.Collections.Generic;
using System.Security.Policy;
using System.Web.Http.Routing;
using AutoMapper;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Model.Core
{
	public class ComponentInterfaceModel : ComponentInterfaceVO, ILinksContaining
	{
		public IList<WebLink> Links { get; set; }
	}
}