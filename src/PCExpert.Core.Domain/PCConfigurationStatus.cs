namespace PCExpert.Core.Domain
{
	public enum PCConfigurationStatus
	{
		/// <summary>
		/// Not specified, invalid
		/// </summary>
		Undefined = 0,

		/// <summary>
		/// Configuration is available only for it's author.
		/// Other users won't see it
		/// </summary>
		Personal = 1,

		/// <summary>
		/// Configuration is publicly available
		/// </summary>
		Published = 2
	}
}