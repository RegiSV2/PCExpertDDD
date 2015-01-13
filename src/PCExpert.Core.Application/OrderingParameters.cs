namespace PCExpert.Core.Application
{
	public sealed class OrderingParameters
	{
		public OrderingParameters(string orderBy, SortDirection direction)
		{
			OrderBy = orderBy;
			Direction = direction;
		}

		public string OrderBy { get; private set; }

		public SortDirection Direction { get; private set; }
	}
}