using System;
using PCExpert.Core.Domain;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Application.ViewObjects
{
	public sealed class ComponentInterfaceVO
	{
		public ComponentInterfaceVO()
		{
		}

		public ComponentInterfaceVO(ComponentInterface domainObject)
		{
			Argument.NotNull(domainObject);
			Id = domainObject.Id;
			Name = domainObject.Name;
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}