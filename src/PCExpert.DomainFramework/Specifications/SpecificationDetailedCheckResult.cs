namespace PCExpert.DomainFramework.Specifications
{
	/// <summary>
	///     Result of detailed specification check
	/// </summary>
	/// <typeparam name="TCheckData">Type of additional check data</typeparam>
	public sealed class SpecificationDetailedCheckResult<TCheckData>
	{
		public SpecificationDetailedCheckResult(bool isSatisfied, TCheckData failureDetails)
		{
			IsSatisfied = isSatisfied;
			FailureDetails = failureDetails;
		}

		public bool IsSatisfied { get; private set; }
		public TCheckData FailureDetails { get; private set; }
	}
}