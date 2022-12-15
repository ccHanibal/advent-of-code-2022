namespace AdventOfCode2022.Tests
{
	public class Day01Tests
	{
		private const string SampleInput =
			"""
			1000
			2000
			3000

			4000

			5000
			6000

			7000
			8000
			9000

			10000
			""";

		[Test]
		public void Day01_Sample_NumElfes_Is_Five()
		{
			var elves = Day01.CreateElves(SampleInput.Split("\r\n"));

			Assert.That(elves.Count(), Is.EqualTo(5));
		}

		[Test]
		[TestCase(0, 6_000)]
		[TestCase(1, 4_000)]
		[TestCase(2, 11_000)]
		[TestCase(3, 24_000)]
		[TestCase(4, 10_000)]
		public void Day01_Sample_Elf_Has_Calories(int elfIdx, int calories)
		{
			var elves = Day01.CreateElves(SampleInput.Split("\r\n"));

			Assert.That(elves.ElementAt(elfIdx).Calories, Is.EqualTo(calories));
		}

		[Test]
		public void Day01_Sample_MaxCalories_Is_24000()
		{
			var elves = Day01.CreateElves(SampleInput.Split("\r\n"));
			var maxCalories = Day01.GetMaxCalories(elves);

			Assert.That(maxCalories, Is.EqualTo(24_000));
		}

		[Test]
		public async Task Day01_Puzzle1_MaxCalories_Is_70116()
		{
			var elves = Day01.CreateElves(await File.ReadAllLinesAsync("Day01.txt"));
			var maxCalories = Day01.GetMaxCalories(elves);

			Assert.That(maxCalories, Is.EqualTo(70_116));
		}

		[Test]
		public void Day01_Sample_SumCalories_TopThree_Is_45000()
		{
			var elves = Day01.CreateElves(SampleInput.Split("\r\n"));
			var calories = Day01.GetSumMaxCaloriesTopThree(elves);

			Assert.That(calories, Is.EqualTo(45_000));
		}

		[Test]
		public async Task Day01_Puzzle2_SumCalories_TopThree_Is_206582()
		{
			var elves = Day01.CreateElves(await File.ReadAllLinesAsync("Day01.txt"));
			var calories = Day01.GetSumMaxCaloriesTopThree(elves);

			Assert.That(calories, Is.EqualTo(206_582));
		}
	}
}