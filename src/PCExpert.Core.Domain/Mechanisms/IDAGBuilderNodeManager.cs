namespace PCExpert.Core.Domain.Mechanisms
{
	public interface IDagBuilderNodeManager<TNodeValue>
	{
		bool CanVisitNode(TNodeValue nodeValue);
		void OnNodeVisited(TNodeValue nodeValue);
		void OnNodeRemovedFromVisited(TNodeValue nodeValue);
		bool AreSame(TNodeValue val1, TNodeValue val2);
	}
}