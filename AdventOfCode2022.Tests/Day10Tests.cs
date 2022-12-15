using static AdventOfCode2022.Day10;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day10Tests
	{
		private static readonly string SampleInput =
			"""
			addx 15
			addx -11
			addx 6
			addx -3
			addx 5
			addx -1
			addx -8
			addx 13
			addx 4
			noop
			addx -1
			addx 5
			addx -1
			addx 5
			addx -1
			addx 5
			addx -1
			addx 5
			addx -1
			addx -35
			addx 1
			addx 24
			addx -19
			addx 1
			addx 16
			addx -11
			noop
			noop
			addx 21
			addx -15
			noop
			noop
			addx -3
			addx 9
			addx 1
			addx -3
			addx 8
			addx 1
			addx 5
			noop
			noop
			noop
			noop
			noop
			addx -36
			noop
			addx 1
			addx 7
			noop
			noop
			noop
			addx 2
			addx 6
			noop
			noop
			noop
			noop
			noop
			addx 1
			noop
			noop
			addx 7
			addx 1
			noop
			addx -13
			addx 13
			addx 7
			noop
			addx 1
			addx -33
			noop
			noop
			noop
			addx 2
			noop
			noop
			noop
			addx 8
			noop
			addx -1
			addx 2
			addx 1
			noop
			addx 17
			addx -9
			addx 1
			addx 1
			addx -3
			addx 11
			noop
			noop
			addx 1
			noop
			addx 1
			noop
			noop
			addx -13
			addx -19
			addx 1
			addx 3
			addx 26
			addx -30
			addx 12
			addx -1
			addx 3
			addx 1
			noop
			noop
			noop
			addx -9
			addx 18
			addx 1
			addx 2
			noop
			noop
			addx 9
			noop
			noop
			noop
			addx -1
			addx 2
			addx -37
			addx 1
			addx 3
			noop
			addx 15
			addx -21
			addx 22
			addx -6
			addx 1
			noop
			addx 2
			addx 1
			noop
			addx -10
			noop
			noop
			addx 20
			addx 1
			addx 2
			addx 2
			addx -6
			addx -11
			noop
			noop
			noop
			""";

		[Test]
		public void Day10_Sample_SimulateProgram_Sum_Is_13140()
		{
			int sum = GetSumOfXAtCycles(SampleInput.Split("\r\n"));

			Assert.That(sum, Is.EqualTo(13_140));
		}

		[Test]
		public async Task Day10_Puzzle1_SimulateProgram_Sum_Is_15120()
		{
			int sum = GetSumOfXAtCycles(await File.ReadAllLinesAsync("Day10.txt"));

			Assert.That(sum, Is.EqualTo(15_120));
		}

		[Test]
		public void Day10_Sample_SimulateDrawing_FullScreen()
		{
			var screenRows = GetOutput(SampleInput.Split("\r\n"));

			foreach (var row in screenRows)
			{
				Console.WriteLine(row);
			}

			Assert.Pass();
		}

		[Test]
		[TestCase(0, "##..##..##..##..##..##..##..##..##..##..")]
		[TestCase(1, "###...###...###...###...###...###...###.")]
		[TestCase(2, "####....####....####....####....####....")]
		[TestCase(3, "#####.....#####.....#####.....#####.....")]
		[TestCase(4, "######......######......######......####")]
		[TestCase(5, "#######.......#######.......#######.....")]
		public void Day10_Sample_SimulateDrawing_Row_X_Is(int rowIndex, string output)
		{
			var screenRows = GetOutput(SampleInput.Split("\r\n"));

			Assert.That(screenRows, Has.ItemAt(rowIndex).EqualTo(output));
		}


		[Test]
		public async Task Day10_Puzzle1_SimulateDrawing_FullScreen()
		{
			var screenRows = GetOutput(await File.ReadAllLinesAsync("Day10.txt"));

			foreach (var row in screenRows)
			{
				Console.WriteLine(row);
			}

			Assert.Pass();
		}

		[Test]
		[TestCase(0, "###..#..#.###....##.###..###..#.....##..")]
		[TestCase(1, "#..#.#.#..#..#....#.#..#.#..#.#....#..#.")]
		[TestCase(2, "#..#.##...#..#....#.###..#..#.#....#..#.")]
		[TestCase(3, "###..#.#..###.....#.#..#.###..#....####.")]
		[TestCase(4, "#.#..#.#..#....#..#.#..#.#....#....#..#.")]
		[TestCase(5, "#..#.#..#.#.....##..###..#....####.#..#.")]
		public async Task Day10_Puzzle1_SimulateDrawing__Row_X_Is(int rowIndex, string output)
		{
			var screenRows = GetOutput(await File.ReadAllLinesAsync("Day10.txt"));

			Assert.That(screenRows, Has.ItemAt(rowIndex).EqualTo(output));
		}
	}
}
