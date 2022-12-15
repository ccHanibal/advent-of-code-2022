using QuikGraph;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

namespace AdventOfCode2022
{
	public static class Day12
	{
		public static int FindShortestPath(string[] data)
		{
			var vertexByIndex = string.Join("", data);
			var startVertex = vertexByIndex.IndexOf("S");

			return FindShortestPath(data, startVertex);
		}
		public static int FindShortestPath(string[] data, int startVertex)
		{
			int CalcVertexIndex(int rowIndex, int columnIndex)
			{
				return rowIndex * data[0].Length + columnIndex;
			}

			var graph = new AdjacencyGraph<int, Edge<int>>();
			graph.AddVertexRange(Enumerable.Range(0, data.Length * data[0].Length));

			var vertexByIndex = string.Join("", data);
			var endVertex = vertexByIndex.IndexOf("E");
			data = data.Select(l => l.Replace("S", "a").Replace("E", "z"))
						.ToArray();

			for (int rowIdx = 0; rowIdx < data.Length; rowIdx++)
			{
				for (int colIdx = 0; colIdx < data[rowIdx].Length; colIdx++)
				{
					var myHeight = data[rowIdx][colIdx];

					if (rowIdx - 1 >= 0 && data[rowIdx - 1][colIdx] - myHeight < 2)
					{
						graph.AddEdge(new Edge<int>(CalcVertexIndex(rowIdx, colIdx), CalcVertexIndex(rowIdx - 1, colIdx)));
					}

					if (rowIdx + 1 < data.Length && data[rowIdx + 1][colIdx] - myHeight < 2)
					{
						graph.AddEdge(new Edge<int>(CalcVertexIndex(rowIdx, colIdx), CalcVertexIndex(rowIdx + 1, colIdx)));
					}

					if (colIdx - 1 >= 0 && data[rowIdx][colIdx - 1] - myHeight < 2)
					{
						graph.AddEdge(new Edge<int>(CalcVertexIndex(rowIdx, colIdx), CalcVertexIndex(rowIdx, colIdx - 1)));
					}

					if (colIdx + 1 < data[rowIdx].Length && data[rowIdx][colIdx + 1] - myHeight < 2)
					{
						graph.AddEdge(new Edge<int>(CalcVertexIndex(rowIdx, colIdx), CalcVertexIndex(rowIdx, colIdx + 1)));
					}
				}
			}

			double EdgeCost(Edge<int> edge) => 1;

			var dijkstra = new DijkstraShortestPathAlgorithm<int, Edge<int>>(graph, EdgeCost);

			// Attach a Vertex Predecessor Recorder Observer to give us the paths
			var predecessor = new VertexPredecessorRecorderObserver<int, Edge<int>>();
			using (predecessor.Attach(dijkstra))
			{
				// Run the algorithm with A set to be the source
				dijkstra.Compute(startVertex);
			}

			if (predecessor.TryGetPath(endVertex, out var path))
				return path.Count();

			return -1;
		}

		public static int FindShortestPathFromAnyLowestPoint(string[] data)
		{
			int knownShortestPathNumSteps = int.MaxValue;

			var vertexByIndex = string.Join("", data);

			for (int vertexIndex = 0; vertexIndex < vertexByIndex.Length; vertexIndex++)
			{
				if (vertexByIndex[vertexIndex] == 'S' || vertexByIndex[vertexIndex] == 'a')
				{
					var numSteps = FindShortestPath(data, vertexIndex);
					if (numSteps > 0 && numSteps < knownShortestPathNumSteps)
						knownShortestPathNumSteps = numSteps;
				}
			}

			return knownShortestPathNumSteps;
		}
	}
}
