using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022
{
	[SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "Quick and dirty")]
	public static class Day08
	{
		public static int[][] GetHeightMap(string[] content)
		{
			int[][] grid = new int[content[0].Length + 2][];
			int[] prePostFix = new[] { -1 };

			grid[0] = Enumerable.Repeat(-1, content[0].Length + 2).ToArray();
			grid[grid.Length - 1] = grid[0];

			for (int rowIdx = 0; rowIdx < content.Length; rowIdx++)
			{
				grid[rowIdx + 1] = prePostFix
										.Concat(content[rowIdx].Select(c => c - 48))
										.Concat(prePostFix)
										.ToArray();
			}

			return grid;
		}

		public static IEnumerable<Cell> FindVisibleCells(int[][] grid)
		{
			int gridSize = grid.Length;

			var visibleCells = new HashSet<Cell>(gridSize * gridSize);
			int lastKnownMaxHeight;

			for (int rowIdx = 1; rowIdx < gridSize - 1; rowIdx++)
			{
				lastKnownMaxHeight = -1;

				for (int colIdx = 1; colIdx < gridSize - 1; colIdx++)
				{
					if (grid[rowIdx][colIdx] > lastKnownMaxHeight)
					{
						visibleCells.Add(new Cell(rowIdx, colIdx));
						lastKnownMaxHeight = grid[rowIdx][colIdx];
					}
				}

				lastKnownMaxHeight = -1;

				for (int colIdx = gridSize - 2; colIdx > 0; colIdx--)
				{
					if (grid[rowIdx][colIdx] > lastKnownMaxHeight)
					{
						visibleCells.Add(new Cell(rowIdx, colIdx));
						lastKnownMaxHeight = grid[rowIdx][colIdx];
					}
				}
			}

			for (int colIdx = 1; colIdx < gridSize - 1; colIdx++)
			{
				lastKnownMaxHeight = -1;

				for (int rowIdx = 1; rowIdx < gridSize - 1; rowIdx++)
				{
					if (grid[rowIdx][colIdx] > lastKnownMaxHeight)
					{
						visibleCells.Add(new Cell(rowIdx, colIdx));
						lastKnownMaxHeight = grid[rowIdx][colIdx];
					}
				}

				lastKnownMaxHeight = -1;

				for (int rowIdx = gridSize - 2; rowIdx > 0; rowIdx--)
				{
					if (grid[rowIdx][colIdx] > lastKnownMaxHeight)
					{
						visibleCells.Add(new Cell(rowIdx, colIdx));
						lastKnownMaxHeight = grid[rowIdx][colIdx];
					}
				}
			}

			return visibleCells;
		}

		public static int GetScenicScore(int[][] grid, int rowIndex, int columnIndex)
		{
			int gridSize = grid.Length;

			int numVisibleLeft = 0;
			int numVisibleRight = 0;
			int numVisibleUp = 0;
			int numVisibleDown = 0;

			int capHeight = grid[rowIndex][columnIndex];

			for (int rowIdx = rowIndex + 1; rowIdx < gridSize - 1; rowIdx++)
			{
				numVisibleDown++;

				if (grid[rowIdx][columnIndex] >= capHeight)
					break;
			}

			for (int rowIdx = rowIndex - 1; rowIdx > 0; rowIdx--)
			{
				numVisibleUp++;

				if (grid[rowIdx][columnIndex] >= capHeight)
					break;
			}

			for (int colIdx = columnIndex + 1; colIdx < gridSize - 1; colIdx++)
			{
				numVisibleRight++;

				if (grid[rowIndex][colIdx] >= capHeight)
					break;
			}

			for (int colIdx = columnIndex - 1; colIdx > 0; colIdx--)
			{
				numVisibleLeft++;

				if (grid[rowIndex][colIdx] >= capHeight)
					break;
			}

			return numVisibleLeft * numVisibleDown * numVisibleRight * numVisibleUp;
		}

		public static int GetOptimalPositionScenicScore(int[][] grid)
		{
			int gridSize = grid.Length;
			var scenicScores = new List<int>(gridSize * gridSize);

			for (int rowIdx = 1; rowIdx < gridSize - 1; rowIdx++)
			{
				for (int colIdx = 1; colIdx < gridSize - 1; colIdx++)
				{
					scenicScores.Add(GetScenicScore(grid, rowIdx, colIdx));
				}
			}

			return scenicScores.Max();
		}

		public record Cell(int RowIndex, int ColumnIndex);
	}
}
