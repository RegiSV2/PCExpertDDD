namespace PCExpert.Core.Tests.Utils
{
	public static class NamesGenerator
	{
		private const string ComponentNameBase = "Test Component Name";
		private const string ComponentInterfaceNameBase = "Test ComponentInterface Name";
		private const string ConfigurationNameBase = "Test Configuration Name";

		public static string ComponentName()
		{
			return ComponentNameBase;
		}

		public static string ComponentName(int nameNumber)
		{
			return ComponentNameBase + " " + nameNumber;
		}

		public static string ComponentInterfaceName()
		{
			return ComponentInterfaceNameBase;
		}

		public static string ComponentInterfaceName(int nameNumber)
		{
			return ComponentInterfaceNameBase + " " + nameNumber;
		}

		public static string ConfigurationName()
		{
			return ConfigurationNameBase;
		}

		public static string ConfigurationName(int nameNumber)
		{
			return ConfigurationNameBase + " " + nameNumber;
		}
	}
}