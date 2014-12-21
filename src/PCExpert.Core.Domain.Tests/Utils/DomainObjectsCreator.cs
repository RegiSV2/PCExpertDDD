namespace PCExpert.Core.Domain.Tests.Utils
{
	public static class DomainObjectsCreator
	{
		public static PCComponent CreateComponent(int componentNameValue)
		{
			return new PCComponent(NamesGenerator.ComponentName(componentNameValue));
		}

		public static ComponentInterface CreateInterface(int componentNameValue)
		{
			return new ComponentInterface(NamesGenerator.ComponentName(componentNameValue));
		}
	}
}