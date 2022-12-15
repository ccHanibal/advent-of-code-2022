using static AdventOfCode2022.Day04;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day04Tests
	{
		private const string SampleInput =
			"""
			2-4,6-8
			2-3,4-5
			5-7,7-9
			2-8,3-7
			6-6,4-6
			2-6,4-8
			""";

		private readonly ISectionParser sectionParser;
		private readonly IElfPairParser pairParser;

		private readonly CleaningParser cleaningParser;

		public Day04Tests()
		{
			sectionParser = new SectionParser();
			pairParser = new ElfPairParser(sectionParser);

			cleaningParser = new CleaningParser(pairParser);
		}

		[Test]
		[TestCase(0, 2, 4)]
		[TestCase(1, 2, 3)]
		[TestCase(2, 5, 7)]
		[TestCase(3, 2, 8)]
		[TestCase(4, 6, 6)]
		[TestCase(5, 2, 6)]
		public void Day04_Sample_SectionParser_FirstSection_Ok(int lineIdx, int lower, int higher)
		{
			var section = sectionParser.Parse(SampleInput.Split("\r\n")[lineIdx].Split(",")[0]);

			Assert.That(section.FirstId, Is.EqualTo(lower));
			Assert.That(section.LastId, Is.EqualTo(higher));
		}

		[Test]
		[TestCase(0, 6, 8)]
		[TestCase(1, 4, 5)]
		[TestCase(2, 7, 9)]
		[TestCase(3, 3, 7)]
		[TestCase(4, 4, 6)]
		[TestCase(5, 4, 8)]
		public void Day04_Sample_SectionParser_LastSection_Ok(int lineIdx, int lower, int higher)
		{
			var section = sectionParser.Parse(SampleInput.Split("\r\n")[lineIdx].Split(",")[1]);

			Assert.That(section.FirstId, Is.EqualTo(lower));
			Assert.That(section.LastId, Is.EqualTo(higher));
		}

		[Test]
		[TestCase(0, 2, 4, 6, 8)]
		[TestCase(1, 2, 3, 4, 5)]
		[TestCase(2, 5, 7, 7, 9)]
		[TestCase(3, 2, 8, 3, 7)]
		[TestCase(4, 6, 6, 4, 6)]
		[TestCase(5, 2, 6, 4, 8)]
		public void Day04_Sample_PairParser_HasSections_Ok(int lineIdx, int lower1, int higher1, int lower2, int higher2)
		{
			var pair = pairParser.Parse(SampleInput.Split("\r\n")[lineIdx]);

			Assert.That(pair.Elf1.FirstId, Is.EqualTo(lower1));
			Assert.That(pair.Elf1.LastId, Is.EqualTo(higher1));

			Assert.That(pair.Elf2.FirstId, Is.EqualTo(lower2));
			Assert.That(pair.Elf2.LastId, Is.EqualTo(higher2));
		}

		[Test]
		public void Day04_Sample_CleaningParser_Has_Count_Six()
		{
			var pairs = cleaningParser.Parse(SampleInput.Split("\r\n")).ToList();

			Assert.That(pairs, Has.Count.EqualTo(6));
		}

		[Test]
		[TestCase(0, false)]
		[TestCase(1, false)]
		[TestCase(2, false)]
		[TestCase(3, true)]
		[TestCase(4, true)]
		[TestCase(5, false)]
		public void Day04_Sample_Pair_HasFullOverlap(int lineIdx, bool overlap)
		{
			var pair = pairParser.Parse(SampleInput.Split("\r\n")[lineIdx]);

			Assert.That(pair.HasFullOverlap(), Is.EqualTo(overlap));
		}

		[Test]
		public void Day04_SampleInput_TotalNumberOverlaps_Is_Two()
		{
			var pairs = cleaningParser.Parse(SampleInput.Split("\r\n"));
			Assert.That(GetNumberOfFullOverlappingPairs(pairs), Is.EqualTo(2));
		}

		[Test]
		public async Task Day04_Puzzle1_TotalNumberFullOverlaps_Is_532()
		{
			var pairs = cleaningParser.Parse(await File.ReadAllLinesAsync("Day04.txt"));
			Assert.That(GetNumberOfFullOverlappingPairs(pairs), Is.EqualTo(532));
		}

		[Test]
		[TestCase(0, false)]
		[TestCase(1, false)]
		[TestCase(2, true)]
		[TestCase(3, true)]
		[TestCase(4, true)]
		[TestCase(5, true)]
		public void Day04_Sample_Pair_HasAnyOverlap(int lineIdx, bool overlap)
		{
			var pair = pairParser.Parse(SampleInput.Split("\r\n")[lineIdx]);

			Assert.That(pair.HasAnyOverlap(), Is.EqualTo(overlap));
		}

		[Test]
		public void Day04_SampleInput_TotalNumberAnyOverlaps_Is_Four()
		{
			var pairs = cleaningParser.Parse(SampleInput.Split("\r\n"));
			Assert.That(GetNumberOfAnyOverlappingPairs(pairs), Is.EqualTo(4));
		}

		[Test]
		public async Task Day04_Puzzle2_TotalNumberAnyOverlaps_Is_854()
		{
			var pairs = cleaningParser.Parse(await File.ReadAllLinesAsync("Day04.txt"));
			Assert.That(GetNumberOfAnyOverlappingPairs(pairs), Is.EqualTo(854));
		}
	}
}
