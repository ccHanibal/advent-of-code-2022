using System.Drawing;
using static AdventOfCode2022.Day14;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day14Tests
	{
		private static readonly string SampleInput =
			"""
			498,4 -> 498,6 -> 496,6
			503,4 -> 502,4 -> 502,9 -> 494,9
			""";

		[Test]
		[TestCase(0, 0, 498, 4)]
		[TestCase(0, 1, 498, 6)]
		[TestCase(0, 2, 496, 6)]
		public void Day14_Sample_ParseRock_HasPoint(int lineIndex, int pointIndex, int x, int y)
		{
			var rock = ParseRock(SampleInput.Split("\r\n")[lineIndex]).ToList();

			Assert.That(rock[pointIndex].X, Is.EqualTo(x));
			Assert.That(rock[pointIndex].Y, Is.EqualTo(y));
		}

		[Test]
		public void Day14_Sample_FillCaveWithSand_Has_24_CellsFilled()
		{
			var rocks = ParseRocks(SampleInput.Split("\r\n")).ToList();
			var cave = CreateCaveNoEnd(rocks);
			var numSandCells = FillCaveWithSand(cave);
			var countSand = cave.CountSand();

			Assert.That(numSandCells, Is.EqualTo(countSand));
			Assert.That(numSandCells, Is.EqualTo(24));
		}

		[Test]
		public async Task Day14_Puzzle1_Has_163_Rocks()
		{
			var rocks = ParseRocks(await File.ReadAllLinesAsync("Day14.txt")).ToList();

			Assert.That(rocks, Has.Count.EqualTo(163));
		}

		[Test]
		public async Task Day14_Puzzle1_FillCaveWithSand_Has_897_CellsFilled()
		{
			var rocks = ParseRocks(await File.ReadAllLinesAsync("Day14.txt")).ToList();
			var cave = CreateCaveNoEnd(rocks);
			var numSandCells = FillCaveWithSand(cave);
			var countSand = cave.CountSand();

			Assert.That(numSandCells, Is.EqualTo(countSand));
			Assert.That(numSandCells, Is.EqualTo(897));
		}

		[Test]
		public void Day14_Sample_FillCaveCompletelyWithSand_Has_93_CellsFilled()
		{
			var rocks = ParseRocks(SampleInput.Split("\r\n")).ToList();
			var cave = CreateCaveWithEnd(rocks);
			var numSandCells = FillCaveWithSand(cave);
			var countSand = cave.CountSand();

			Assert.That(numSandCells, Is.EqualTo(countSand));
			Assert.That(numSandCells, Is.EqualTo(93));
		}

		[Test]
		public async Task Day14_Puzzle2_FillCaveCompletelyWithSand_Has_26683_CellsFilled()
		{
			var rocks = ParseRocks(await File.ReadAllLinesAsync("Day14.txt")).ToList();
			var cave = CreateCaveWithEnd(rocks);
			var numSandCells = FillCaveWithSand(cave);
			var countSand = cave.CountSand();

			Assert.That(numSandCells, Is.EqualTo(countSand));
			Assert.That(numSandCells, Is.EqualTo(26_683));
		}
	}
}
