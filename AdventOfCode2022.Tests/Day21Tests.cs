using static AdventOfCode2022.Day21;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day21Tests
	{
		private static readonly string SampleInput =
			"""
			root: pppw + sjmn
			dbpl: 5
			cczh: sllz + lgvd
			zczc: 2
			ptdq: humn - dvpt
			dvpt: 3
			lfqf: 4
			humn: 5
			ljgn: 2
			sjmn: drzm * dbpl
			sllz: 4
			pppw: cczh / lfqf
			lgvd: ljgn * ptdq
			drzm: hmdt - zczc
			hmdt: 32
			""";

		[Test]
		public void Day21_Sample_Monkey_Named_Root_Yells_152()
		{
			var monkeys = ParseMonkeys(SampleInput.Split("\r\n"));
			var valueOfRootMonkey = EvaluateMonkey(monkeys, "root");

			Assert.That(valueOfRootMonkey, Is.EqualTo(152L));
		}

		[Test]
		public async Task Day21_Puzzle1_Monkey_Named_Root_Yells__155_708_040_358_220()
		{
			var monkeys = ParseMonkeys(await File.ReadAllLinesAsync("Day21.txt"));
			var valueOfRootMonkey = EvaluateMonkey(monkeys, "root");

			Assert.That(valueOfRootMonkey, Is.EqualTo(155_708_040_358_220L));
		}

		[Test]
		public void Day21_Sample_Human_Needs_To_Yell_301_To_Have_Root_Having_Equal_Values()
		{
			var monkeys = ParseMonkeys(SampleInput.Split("\r\n"));
			var valueOfHuman = FindValueOfHumanToHaveEqualValuesAtRoot(monkeys, 0);

			Assert.That(valueOfHuman, Is.EqualTo(301L));
		}

		[Test]
		public async Task Day21_Puzzle2_Human_Needs_To_Yell__3_342_154_812_537__To_Have_Root_Having_Equal_Values()
		{
			var monkeys = ParseMonkeys(await File.ReadAllLinesAsync("Day21.txt"));
			var valueOfHuman = FindValueOfHumanToHaveEqualValuesAtRoot(monkeys, 100_000_000);

			Assert.That(valueOfHuman, Is.EqualTo(3_342_154_812_537L));
		}
	}
}
