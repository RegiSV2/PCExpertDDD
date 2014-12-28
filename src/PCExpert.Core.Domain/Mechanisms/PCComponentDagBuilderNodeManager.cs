using System.Collections.Generic;
using System.Linq;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain.Mechanisms
{
	public class PCComponentDagBuilderNodeManager : IDagBuilderNodeManager<PCComponent>
	{
		private readonly Dictionary<ComponentInterface, int> _availableInterfaces;

		public PCComponentDagBuilderNodeManager(IEnumerable<PCComponent> components)
		{
			Argument.NotNull(components);

			_availableInterfaces = new Dictionary<ComponentInterface, int>();
			PutInterfacesInDictionary(components
				.Where(x => !x.PlugSlots.Any())
				.SelectMany(GetComponentInterfaces));
		}

		public bool CanVisitNode(PCComponent nodeValue)
		{
			return nodeValue.PlugSlots.GroupBy(x => x, new EntityEqualityComparer<ComponentInterface>())
				.All(x => _availableInterfaces.ContainsKey(x.Key)
				          && _availableInterfaces[x.Key] >= x.Count());
		}

		public void OnNodeVisited(PCComponent nodeValue)
		{
			PopInterfacesFromDictionary(nodeValue.PlugSlots);
			PutInterfacesInDictionary(GetComponentInterfaces(nodeValue));
		}

		public void OnNodeRemovedFromVisited(PCComponent nodeValue)
		{
			PopInterfacesFromDictionary(GetComponentInterfaces(nodeValue));
			PutInterfacesInDictionary(nodeValue.PlugSlots);
		}

		public bool AreSame(PCComponent val1, PCComponent val2)
		{
			return val1.SameIdentityAs(val2);
		}

		private void PutInterfacesInDictionary(IEnumerable<ComponentInterface> availableInterfacesEnum)
		{
			foreach (var availableInterface in availableInterfacesEnum)
			{
				if (!_availableInterfaces.ContainsKey(availableInterface))
					_availableInterfaces.Add(availableInterface, 0);
				_availableInterfaces[availableInterface] += 1;
			}
		}

		private void PopInterfacesFromDictionary(IEnumerable<ComponentInterface> compInterfaces)
		{
			foreach (var compInt in compInterfaces)
			{
				_availableInterfaces[compInt] -= 1;
				if (_availableInterfaces[compInt] == 0)
					_availableInterfaces.Remove(compInt);
			}
		}

		private static IEnumerable<ComponentInterface> GetComponentInterfaces(PCComponent component)
		{
			foreach (var componentInterface in component.ContainedSlots)
				yield return componentInterface;
			foreach (var componentInterface in component.ContainedComponents.SelectMany(GetComponentInterfaces))
				yield return componentInterface;
		}
	}
}