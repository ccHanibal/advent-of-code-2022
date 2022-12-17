using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
	using MoreLinq;

	public static class Day16
	{
		public record Node(string Label, int FlowRate, string[] Neighbors)
		{
			public static Node Null { get; } = new Node("", 0, Array.Empty<string>());
		}

		private static readonly Regex valveRegex = new Regex(@"Valve ([A-Z]+) has flow rate=(\d+); tunnels? leads? to valves? ([A-Z,\s]+)");

		public static IDictionary<string, Node> ParseNodes(IEnumerable<string> data)
		{
			var nodes = new Dictionary<string, Node>(StringComparer.OrdinalIgnoreCase);

			foreach (var line in data)
			{
				var match = valveRegex.Match(line);

				var label = match.Groups[1].Value;
				int flowRate = int.Parse(match.Groups[2].Value);
				var neighbors = match.Groups[3].Value.Split(", ");

				nodes.Add(label, new Node(label, flowRate, neighbors));
			}

			return nodes;
		}

		public static int FindMaxPreasureToRelease(IDictionary<string, Node> nodes)
		{
			int maxReleasedPressure = 0;

			foreach (var path in FindPaths(nodes))
			{
				var releasedPresureThisPath = GetReleasedPressure(path.ToList());
				if (releasedPresureThisPath > maxReleasedPressure)
					maxReleasedPressure = releasedPresureThisPath;
			}

			return maxReleasedPressure;
		}

		public static int GetReleasedPressure(IEnumerable<Node> path)
		{
			int releasedPresure = 0;
			int flowRate = 0;

			Node lastNode = path.First();

			foreach (var currentNode in path.Skip(1))
			{
				releasedPresure += flowRate;

				if (lastNode == currentNode && currentNode.FlowRate > 0)
				{
					flowRate += currentNode.FlowRate;
				}

				lastNode = currentNode;
			}

			return releasedPresure;
		}

		public static IEnumerable<IEnumerable<Node>> FindPaths(IDictionary<string, Node> nodes)
		{
			int numReleasableNodes = nodes.Values.Count(n => n.FlowRate > 0);

			return FindPathsImpl(new Node[] { nodes["AA"] }, Array.Empty<Node>(), 30, Node.Null, nodes["AA"]);

			IEnumerable<IEnumerable<Node>> FindPathsImpl(IEnumerable<Node> path, IEnumerable<Node> releasedNodes, int length, Node lastVisitedNode, Node currentNode)
			{
				if (length <= 0)
				{
					yield return path;
					yield break;
				}

				if (releasedNodes.Count() >= numReleasableNodes)
				{
					yield return path.Concat(Enumerable.Repeat(Node.Null, length));
					yield break;
				}

				if (currentNode.FlowRate > 0 && !releasedNodes.Contains(currentNode))
				{
					foreach (var subPath in FindPathsImpl(path.Append(currentNode), releasedNodes.Append(currentNode), length - 1, currentNode, currentNode))
					{
						yield return subPath;
					}
				}

				var posibleNextNodeLabels = currentNode.Neighbors.Where(l => l != lastVisitedNode.Label);

				foreach (var nextNodeLabel in posibleNextNodeLabels)
				{
					var nextNode = nodes[nextNodeLabel];

					foreach (var subPath in FindPathsImpl(path.Append(nextNode), releasedNodes, length - 1, currentNode, nextNode))
					{
						yield return subPath;
					}
				}
			}
		}
	}
}
