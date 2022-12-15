using static AdventOfCode2022.Day11;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day11Tests
	{
		private static readonly string SampleInput =
            """
			Monkey 0:
			  Starting items: 79, 98
			  Operation: new = old * 19
			  Test: divisible by 23
			    If true: throw to monkey 2
			    If false: throw to monkey 3

			Monkey 1:
			  Starting items: 54, 65, 75, 74
			  Operation: new = old + 6
			  Test: divisible by 19
			    If true: throw to monkey 2
			    If false: throw to monkey 0

			Monkey 2:
			  Starting items: 79, 60, 97
			  Operation: new = old * old
			  Test: divisible by 13
			    If true: throw to monkey 1
			    If false: throw to monkey 3

			Monkey 3:
			  Starting items: 74
			  Operation: new = old + 3
			  Test: divisible by 17
			    If true: throw to monkey 0
			    If false: throw to monkey 1
			""";

		[Test]
		public void Day11_Sample_SimulateRounds_ProductMostActive_Is__10_605()
		{
			var monkeys = ParseMonkeys(SampleInput.Split("\r\n"));
			SimulateRounds(20, monkeys, true);
			var product = GetProductOfMostActiveMonkeys(monkeys);

			Assert.That(product, Is.EqualTo(10_605));
		}

		[Test]
		public async Task Day11_Puzzle1_SimulateRounds_ProductMostActive_Is__316_888()
		{
            var monkeys = ParseMonkeys(await File.ReadAllLinesAsync("Day11.txt"));
            SimulateRounds(20, monkeys, true);
            var product = GetProductOfMostActiveMonkeys(monkeys);

            Assert.That(product, Is.EqualTo(316_888));
        }

        [Test]
        public void Day11_Sample_Part2_SimulateRounds_ProductMostActive_Is__2_713_310_158()
        {
            var monkeys = ParseMonkeys(SampleInput.Split("\r\n"));
            SimulateRounds(10_000, monkeys, false);
            var product = GetProductOfMostActiveMonkeys(monkeys);

            Assert.That(product, Is.EqualTo(2_713_310_158.0));
        }

        [Test]
        public async Task Day11_Puzzle2_SimulateRounds_ProductMostActive_Is__35_270_398_814()
        {
            var monkeys = ParseMonkeys(await File.ReadAllLinesAsync("Day11.txt"));
            SimulateRounds(10_000, monkeys, false);
            var product = GetProductOfMostActiveMonkeys(monkeys);

            Assert.That(product, Is.EqualTo(35_270_398_814));
        }
    }
}
