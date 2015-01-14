namespace PCExpert.Core.Application
{
	public class TableParameters
	{
		public TableParameters(PagingParameters paging, OrderingParameters ordering)
		{
			PagingParameters = paging;
			OrderingParameters = ordering;
		}

		public OrderingParameters OrderingParameters { get; private set; }
		public PagingParameters PagingParameters { get; private set; }
	}
}