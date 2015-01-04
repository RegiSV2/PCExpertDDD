namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     Result of detailed specification check
	/// </summary>
	/// <typeparam name="TCheckData">Type of additional check data</typeparam>
	public sealed class SpecificationDetailedCheckResult<TCheckData>
	{
		public SpecificationDetailedCheckResult(bool isSatisfied, TCheckData checkData)
		{
			IsSatisfied = isSatisfied;
			CheckData = checkData;
		}

		public bool IsSatisfied { get; private set; }
		public TCheckData CheckData { get; private set; }
	}
}