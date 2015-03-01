using System;
using System.Diagnostics.Contracts;
using System.Web.Http.Routing;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Web.Api.Common.WebModel
{
	[ContractClass(typeof(LinkSetterContracts))]
	public interface ILinkSetter
	{
		Type ModelType { get; }
		void SetLinks(UrlHelper urlHelper, object model);
	}

	[ContractClassFor(typeof(ILinkSetter))]
	abstract class LinkSetterContracts : ILinkSetter
	{
		public Type ModelType
		{
			get
			{
				Contract.Ensures(Contract.Result<Type>() != null);
				return null;
			}
		}

		public void SetLinks(UrlHelper urlHelper, object model)
		{
			Argument.NotNull(urlHelper);
			Argument.NotNull(model);
		}
	}
}