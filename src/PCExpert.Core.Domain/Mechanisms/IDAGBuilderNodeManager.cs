using System.Collections.Generic;

namespace PCExpert.Core.Domain.Specifications
{
	public interface IDagBuilderNodeManager<TNodeValue>
	{
		bool CanVisitNode(TNodeValue nodeValue);

		void OnNodeVisited(TNodeValue nodeValue);

		void OnNodeRemovedFromVisited(TNodeValue nodeValue);

		bool AreSame(TNodeValue val1, TNodeValue val2);
	}
}