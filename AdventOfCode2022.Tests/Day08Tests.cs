using static AdventOfCode2022.Day08;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day08Tests
	{
		private static readonly string SampleInput =
			"""
			30373
			25512
			65332
			33549
			35390
			""";

		[Test]
		public void Day08_Sample_GetHeightMap_HasOneMoreRowOnEachEnd()
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));

			Assert.That(grid.Length, Is.EqualTo(7));
		}

		[Test]
		public void Day08_Sample_GetHeightMap_HasOneMoreColumnOnEachEnd()
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));

			foreach (var row in grid)
			{
				Assert.That(row.Length, Is.EqualTo(7));
			}
		}

		[Test]
		[TestCase(0, 0, -1)]
		[TestCase(0, 1, -1)]
		[TestCase(0, 2, -1)]
		[TestCase(0, 3, -1)]
		[TestCase(0, 4, -1)]
		[TestCase(0, 5, -1)]
		[TestCase(0, 6, -1)]

		[TestCase(1, 0, -1)]
		[TestCase(1, 1, 3)]
		[TestCase(1, 2, 0)]
		[TestCase(1, 3, 3)]
		[TestCase(1, 4, 7)]
		[TestCase(1, 5, 3)]
		[TestCase(1, 6, -1)]

		[TestCase(2, 0, -1)]
		[TestCase(2, 1, 2)]
		[TestCase(2, 2, 5)]
		[TestCase(2, 3, 5)]
		[TestCase(2, 4, 1)]
		[TestCase(2, 5, 2)]
		[TestCase(2, 6, -1)]

		[TestCase(3, 0, -1)]
		[TestCase(3, 1, 6)]
		[TestCase(3, 2, 5)]
		[TestCase(3, 3, 3)]
		[TestCase(3, 4, 3)]
		[TestCase(3, 5, 2)]
		[TestCase(3, 6, -1)]

		[TestCase(4, 0, -1)]
		[TestCase(4, 1, 3)]
		[TestCase(4, 2, 3)]
		[TestCase(4, 3, 5)]
		[TestCase(4, 4, 4)]
		[TestCase(4, 5, 9)]
		[TestCase(4, 6, -1)]


		[TestCase(5, 0, -1)]
		[TestCase(5, 1, 3)]
		[TestCase(5, 2, 5)]
		[TestCase(5, 3, 3)]
		[TestCase(5, 4, 9)]
		[TestCase(5, 5, 0)]
		[TestCase(5, 6, -1)]


		[TestCase(6, 0, -1)]
		[TestCase(6, 1, -1)]
		[TestCase(6, 2, -1)]
		[TestCase(6, 3, -1)]
		[TestCase(6, 4, -1)]
		[TestCase(6, 5, -1)]
		[TestCase(6, 6, -1)]
		public void Day08_Sample_GetHeightMap_Cell_HasHeight(int rowIdx, int colIdx, int height)
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));
			Assert.That(grid[rowIdx][colIdx], Is.EqualTo(height));
		}

		[Test]
		public void Day08_Sample_FindVisibleCells_DoesNotContain_DummyRowsAndColumns()
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));
			var visibleCells = FindVisibleCells(grid);

			Assert.That(visibleCells.Select(c => c.RowIndex).ToList(), Is.All.Not.EqualTo(0));
			Assert.That(visibleCells.Select(c => c.RowIndex).ToList(), Is.All.Not.EqualTo(6));

			Assert.That(visibleCells.Select(c => c.ColumnIndex).ToList(), Is.All.Not.EqualTo(0));
			Assert.That(visibleCells.Select(c => c.ColumnIndex).ToList(), Is.All.Not.EqualTo(6));
		}

		[Test]
		public void Day08_Sample_FindVisibleCells_Has_21_VisibleCells()
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));
			var visibleCells = FindVisibleCells(grid);

			Assert.That(visibleCells.Count(), Is.EqualTo(21));
		}

		[Test]
		[TestCase(1, 1)]
		[TestCase(1, 2)]
		[TestCase(1, 3)]
		[TestCase(1, 4)]
		[TestCase(1, 5)]
		[TestCase(2, 1)]
		[TestCase(2, 2)]
		[TestCase(2, 3)]
		[TestCase(2, 5)]
		[TestCase(3, 1)]
		[TestCase(3, 2)]
		[TestCase(3, 4)]
		[TestCase(3, 5)]
		[TestCase(4, 1)]
		[TestCase(4, 3)]
		[TestCase(4, 5)]
		[TestCase(5, 1)]
		[TestCase(5, 2)]
		[TestCase(5, 3)]
		[TestCase(5, 4)]
		[TestCase(5, 5)]
		public void Day08_Sample_FindVisibleCells_HasCell(int rowIdx, int colIdx)
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));
			var visibleCells = FindVisibleCells(grid);

			Assert.That(visibleCells.FirstOrDefault(c => c.RowIndex == rowIdx && c.ColumnIndex == colIdx), Is.Not.Null);
		}

		[Test]
		public async Task Day08_Puzzle1_FindVisibleCells_HasCount_1543()
		{
			var grid = GetHeightMap(await File.ReadAllLinesAsync("Day08.txt"));
			var visibleCells = FindVisibleCells(grid);

			Assert.That(visibleCells.Count(), Is.EqualTo(1543));
		}

		[Test]
		[TestCase(2, 3, 4)]
		[TestCase(4, 3, 8)]
		public void Day08_Sample_GetScenicScore_OfCell_Is(int rowIdx, int colIdx, int scenicScore)
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));
			var score = GetScenicScore(grid, rowIdx, colIdx);

			Assert.That(score, Is.EqualTo(scenicScore));
		}

		[Test]
		public void Day08_Sample_GetOptimalPositionScenicScore_Is_8()
		{
			var grid = GetHeightMap(SampleInput.Split("\r\n"));
			var score = GetOptimalPositionScenicScore(grid);

			Assert.That(score, Is.EqualTo(8));
		}

		[Test]
		public async Task Day08_Puzzle2_GetOptimalPositionScenicScore_Is_595080()
		{
			var grid = GetHeightMap(await File.ReadAllLinesAsync("Day08.txt"));
			var score = GetOptimalPositionScenicScore(grid);

			Assert.That(score, Is.EqualTo(595080));
		}
	}
}
