using PCExpert.Core.Domain;

namespace PCExpert.Core.Tests.Utils
{
	public static class DomainObjectsCreator
	{
		public static PCComponent CreateComponent(int componentNameValue, ComponentType type)
		{
			return new PCComponent(NamesGenerator.ComponentName(componentNameValue), type);
		}

		public static ComponentInterface CreateInterface(int componentNameValue)
		{
			return new ComponentInterface(NamesGenerator.ComponentName(componentNameValue));
		}
	}
}