using System.Collections.Generic;
using System.Linq;

namespace PCExpert.Core.Domain.Specifications
{
	internal class RequiredComponentTypesSetContraversions
	{
		public RequiredComponentTypesSetContraversions(List<ComponentType> requiredButNotAddedTypes, List<ComponentType> typesViolatedUniqueConstraint)
		{
			RequiredButNotAddedTypes = requiredButNotAddedTypes;
			TypesViolatedUniqueConstraint = typesViolatedUniqueConstraint;
		}

		public List<ComponentType> RequiredButNotAddedTypes { get; private set; }

		public List<ComponentType> TypesViolatedUniqueConstraint { get; private set; }

		public bool ContraversionsFound()
		{
			return RequiredButNotAddedTypes.Any() || TypesViolatedUniqueConstraint.Any();
		}
	}
}