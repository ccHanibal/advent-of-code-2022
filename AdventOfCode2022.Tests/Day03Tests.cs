using static AdventOfCode2022.Day03;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day03Tests
	{
		private const string SampleInput =
			"""
			vJrwpWtwJgWrhcsFMMfFFhFp
			jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
			PmmdzqPrVvPwwTWBwg
			wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
			ttgJtRGJQctTZtZT
			CrZsJsPPZsGzwwsLwLmpwMDw
			""";

		private readonly BackpacksParser backpacksParser;

		private readonly IBackpackParser backpackParser = new BackpackParser();

		public Day03Tests()
		{
			backpacksParser = new BackpacksParser(backpackParser);
		}


		[Test]
		[TestCase(0, "vJrwpWtwJgWr", "hcsFMMfFFhFp")]
		[TestCase(1, "jqHRNqRjqzjGDLGL", "rsFMfFZSrLrFZsSL")]
		[TestCase(2, "PmmdzqPrV", "vPwwTWBwg")]
		[TestCase(3, "wMqvLMZHhHMvwLH", "jbvcjnnSBnvTQFn")]
		[TestCase(4, "ttgJtRGJ", "QctTZtZT")]
		[TestCase(5, "CrZsJsPPZsGz", "wwsLwLmpwMDw")]
		public void Day03_Sample_BackpackParser_Ok(int lineIdx, string part1, string part2)
		{
			var backpack = backpackParser.Parse(SampleInput.Split("\r\n")[lineIdx]);

			Assert.That(backpack.Part1, Is.EqualTo(part1));
			Assert.That(backpack.Part2, Is.EqualTo(part2));
		}

		[Test]
		[TestCase("vJrwpWtwJgWr", "hcsFMMfFFhFp")]
		[TestCase("jqHRNqRjqzjGDLGL", "rsFMfFZSrLrFZsSL")]
		[TestCase("PmmdzqPrV", "vPwwTWBwg")]
		[TestCase("wMqvLMZHhHMvwLH", "jbvcjnnSBnvTQFn")]
		[TestCase("ttgJtRGJ", "QctTZtZT")]
		[TestCase("CrZsJsPPZsGz", "wwsLwLmpwMDw")]
		public void Day03_Backpack_AllItems_Part1PlusPart2(string part1, string part2)
		{
			var backpack = new Backpack(part1, part2);

			Assert.That(backpack.AllItems, Is.EqualTo(part1 + part2));
		}

		[Test]
		[TestCase(0, 'p')]
		[TestCase(1, 'L')]
		[TestCase(2, 'P')]
		[TestCase(3, 'v')]
		[TestCase(4, 't')]
		[TestCase(5, 's')]
		public void Day03_Sample_Backpack_CalculateCommonChar(int lineIdx, char commonItemType)
		{
			var backpack = backpackParser.Parse(SampleInput.Split("\r\n")[lineIdx]);

			Assert.That(backpack.GetCommonItemType(), Is.EqualTo(commonItemType));
		}

		[Test]
		public void Day03_Sample_BackpacksParser_Has_Count_Six()
		{
			var backpacks = backpacksParser.Parse(SampleInput.Split("\r\n")).ToList();

			Assert.That(backpacks, Has.Count.EqualTo(6));
		}

		[Test]
		[TestCase(0, "vJrwpWtwJgWr", "hcsFMMfFFhFp")]
		[TestCase(1, "jqHRNqRjqzjGDLGL", "rsFMfFZSrLrFZsSL")]
		[TestCase(2, "PmmdzqPrV", "vPwwTWBwg")]
		[TestCase(3, "wMqvLMZHhHMvwLH", "jbvcjnnSBnvTQFn")]
		[TestCase(4, "ttgJtRGJ", "QctTZtZT")]
		[TestCase(5, "CrZsJsPPZsGz", "wwsLwLmpwMDw")]
		public void Day03_Sample_BackpacksParser_Returns_Backpacks(int lineIdx, string part1, string part2)
		{
			var backpack = backpacksParser.Parse(SampleInput.Split("\r\n")).ToList()[lineIdx];

			Assert.That(backpack.Part1, Is.EqualTo(part1));
			Assert.That(backpack.Part2, Is.EqualTo(part2));
		}

		[Test]
		[TestCaseSource(nameof(ItemTypeToPriorityTestCases))]
		public void Day03_ItemType_CalcPriority(char itemType, int priority)
		{
			Assert.That(GetItemTypePriority(itemType), Is.EqualTo(priority));
		}
		public static IEnumerable<TestCaseData> ItemTypeToPriorityTestCases
		{
			get
			{
				for (char c = 'a'; c <= 'z'; c++)
				{
					yield return new TestCaseData(c, c - 'a' + 1);
				}

				for (char c = 'A'; c <= 'Z'; c++)
				{
					yield return new TestCaseData(c, c - 'A' + 1 + 26);
				}
			}
		}

		[Test]
		public void Day03_SampleInput_SumPriorities_Is_157()
		{
			var backpacks = backpacksParser.Parse(SampleInput.Split("\r\n"));
			Assert.That(SumOfCommonItemTypePriorities(backpacks), Is.EqualTo(157));
		}

		[Test]
		public async Task Day03_Puzzle1_SumPriorities_Is_8072()
		{
			var backpacks = backpacksParser.Parse(await File.ReadAllLinesAsync("Day03.txt"));
			Assert.That(SumOfCommonItemTypePriorities(backpacks), Is.EqualTo(8_072));
		}

		[Test]
		public void Day03_SampleInput_Grouping_Produces_Two_Groups()
		{
			var backpacks = backpacksParser.Parse(SampleInput.Split("\r\n"));
			var groups = GroupBackpacksByThree(backpacks).ToList();

			Assert.That(groups, Has.Count.EqualTo(2));
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		public void Day03_SampleInput_Grouping_EachGroup_Has_Size_Three(int groupIdx)
		{
			var backpacks = backpacksParser.Parse(SampleInput.Split("\r\n"));
			var groups = GroupBackpacksByThree(backpacks).ToList();

			Assert.That(groups[groupIdx].Count(), Is.EqualTo(3));
		}

		[Test]
		[TestCase(0, 'r')]
		[TestCase(1, 'Z')]
		public void Day03_SampleInput_CalcBadge(int groupIdx, char groupBadge)
		{
			var backpacks = backpacksParser.Parse(SampleInput.Split("\r\n"));
			var groups = GroupBackpacksByThree(backpacks).ToList();

			Assert.That(GetBadgeItemType(groups[groupIdx]), Is.EqualTo(groupBadge));
		}

		[Test]
		public void Day03_SampleInput_SumGroupBadges_Is_70()
		{
			var backpacks = backpacksParser.Parse(SampleInput.Split("\r\n"));
			Assert.That(SumOfBadgeItemTypePriorities(backpacks), Is.EqualTo(70));
		}

		[Test]
		public async Task Day03_Puzzle2_SumGroupBadges_Is_2567()
		{
			var backpacks = backpacksParser.Parse(await File.ReadAllLinesAsync("Day03.txt"));
			Assert.That(SumOfBadgeItemTypePriorities(backpacks), Is.EqualTo(2_567));
		}
	}
}
