using System.Collections.Generic;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Api.Common.Impl.Tests
{
	public class FakeLinkContaining : ILinksContaining
	{
		public IList<WebLink> Links { get; set; }
	}
}