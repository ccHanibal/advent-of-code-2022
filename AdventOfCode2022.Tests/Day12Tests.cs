using static AdventOfCode2022.Day12;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day12Tests
	{
		private static readonly string SampleInput =
			"""
			Sabqponm
			abcryxxl
			accszExk
			acctuvwj
			abdefghi
			""";

		[Test]
		public void Day12_Sample_FindShortestPath_HasSteps_31()
		{
			var numSteps = FindShortestPath(SampleInput.Split("\r\n"));

			Assert.That(numSteps, Is.EqualTo(31));
		}

		[Test]
		public async Task Day12_Puzzle1_FindShortestPath_HasSteps_420()
		{
			var numSteps = FindShortestPath(await File.ReadAllLinesAsync("Day12.txt"));

			Assert.That(numSteps, Is.EqualTo(420));
		}

		[Test]
		public void Day12_Sample_FindShortestPath_AnyLowestPoint_HasSteps_29()
		{
			var numSteps = FindShortestPathFromAnyLowestPoint(SampleInput.Split("\r\n"));

			Assert.That(numSteps, Is.EqualTo(29));
		}

		[Test, Explicit("Takes to long.")]
		public async Task Day12_Puzzle1_FindShortestPath_AnyLowestPoint_HasSteps_414()
		{
			var numSteps = FindShortestPathFromAnyLowestPoint(await File.ReadAllLinesAsync("Day12.txt"));

			Assert.That(numSteps, Is.EqualTo(414));
		}
	}
}
