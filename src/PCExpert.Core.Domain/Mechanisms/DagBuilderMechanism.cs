using System.Collections.Generic;
using System.Linq;

namespace PCExpert.Core.Domain.Mechanisms
{
	/// <summary>
	///     Checks if directed acyclic graph can be build on the specified directed graph
	/// </summary>
	public class DagBuilderMechanism<TNode>
		where TNode : class
	{
		private readonly IDagBuilderNodeManager<TNode> _builderNodeManager;
		private readonly Stack<Stack<LinkedListNode<TNode>>> _candidatesToVisit;
		private readonly LinkedList<TNode> _notVisitedNodes;
		private readonly Stack<LinkedListNode<TNode>> _visitedNodes;

		public DagBuilderMechanism(IDagBuilderNodeManager<TNode> builderNodeManager, LinkedList<TNode> notVisitedNodes)
		{
			_visitedNodes = new Stack<LinkedListNode<TNode>>();
			_candidatesToVisit = new Stack<Stack<LinkedListNode<TNode>>>();
			_notVisitedNodes = notVisitedNodes;
			_builderNodeManager = builderNodeManager;
		}

		public bool CanConnectNodesToDag()
		{
			PushVisitableNodes();
			while (_notVisitedNodes.Any())
			{
				var currentLevelCandidates = _candidatesToVisit.Peek();
				if (currentLevelCandidates.Any())
				{
					var componentToPlug = currentLevelCandidates.Pop();
					VisitNode(componentToPlug);
					PushVisitableNodes();
				}
				else
				{
					_candidatesToVisit.Pop();
					if (_visitedNodes.Any())
						RemoveVisitedNode();
					else
						return false;
				}
			}
			return true;
		}

		private void RemoveVisitedNode()
		{
			var failedToVisitNode = _visitedNodes.Pop();
			_notVisitedNodes.AddFirst(failedToVisitNode);
			_builderNodeManager.OnNodeRemovedFromVisited(failedToVisitNode.Value);
		}

		private void VisitNode(LinkedListNode<TNode> componentToPlug)
		{
			_notVisitedNodes.Remove(componentToPlug);
			_visitedNodes.Push(componentToPlug);
			_builderNodeManager.OnNodeVisited(componentToPlug.Value);
		}

		private void PushVisitableNodes()
		{
			_candidatesToVisit.Push(PeekVisitableNodes());
		}

		private Stack<LinkedListNode<TNode>> PeekVisitableNodes()
		{
			var result = new Stack<LinkedListNode<TNode>>();

			var candidateNode = _notVisitedNodes.First;
			while (candidateNode != null)
			{
				if (_builderNodeManager.CanVisitNode(candidateNode.Value))
				{
					result.Push(candidateNode);
				}
				candidateNode = candidateNode.Next;
			}

			return result;
		}
	}
}