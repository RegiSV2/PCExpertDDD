using System.Collections.Generic;

namespace PCExpert.Core.Application.ViewObjects
{
	public class PagedResult<T>
	{
		public PagedResult(PagingParameters parameters, int countTotal, IList<T> items)
		{
			PagingParameters = parameters;
			CountTotal = countTotal;
			Items = items;
		}

		public PagingParameters PagingParameters { get; private set; }
		public int CountTotal { get; private set; }
		public IList<T> Items { get; private set; }
	}
}