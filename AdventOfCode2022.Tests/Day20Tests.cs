using static AdventOfCode2022.Day20;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day20Tests
	{
		private static readonly string SampleInput =
			"""
			1
			2
			-3
			3
			-2
			0
			4
			""";

		[Test]
		public void Day20_Sample_Mixing_Sum_Is_3()
		{
			var values = ParseInput(SampleInput.Split("\r\n"), 1);
			MixValues(values);
			var sumOfCoords = GetSum(values);

			Assert.That(sumOfCoords, Is.EqualTo(3L));
		}

		[Test]
		public async Task Day20_Puzzle1_Mixing_Sum_Is_988()
		{
			var values = ParseInput(await File.ReadAllLinesAsync("Day20.txt"), 1);
			MixValues(values);
			var sumOfCoords = GetSum(values);

			Assert.That(sumOfCoords, Is.EqualTo(988L));
		}

		[Test]
		public void Day20_Sample_Multiplied_Mixing_10_Times_Sum_Is__811_589_153()
		{
			var values = ParseInput(SampleInput.Split("\r\n"), 811_589_153L);
			MixValues(values, 10);
			var sumOfCoords = GetSum(values);

			Assert.That(sumOfCoords, Is.EqualTo(1_623_178_306L));
		}

		[Test]
		public async Task Day20_Puzzle1_Multiplied_Mixing_10_Times_Sum_Is_7_768_531_372_516()
		{
			var values = ParseInput(await File.ReadAllLinesAsync("Day20.txt"), 811_589_153L);
			MixValues(values, 10);
			var sumOfCoords = GetSum(values);

			Assert.That(sumOfCoords, Is.EqualTo(7_768_531_372_516L));
		}
	}
}
