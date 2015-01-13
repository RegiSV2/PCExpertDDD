using System.Data.SqlClient;

namespace PCExpert.Core.Application
{
	public sealed class PagingParameters
	{
		public PagingParameters(int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
		/// <summary>
		/// Zero-based number of requested page
		/// </summary>
		public int PageNumber { get; private set; }

		public int PageSize { get; private set; }
	}
}