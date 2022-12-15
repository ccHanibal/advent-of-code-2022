using static AdventOfCode2022.Day02;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day02_Part2_Tests
	{
		static class Scores
		{
			public const int Lost = 0;
			public const int Draw = 3;
			public const int Win = 6;

			public const int Rock = 1;
			public const int Paper = 2;
			public const int Scissors = 3;
		}

		private const string SampleInput =
			"""
			A Y
			B X
			C Z
			""";

		private readonly GuideParser guideParser;

		private readonly IRoundParser roundParser = new RoundParserResult();

		public Day02_Part2_Tests()
		{
			guideParser = new GuideParser(roundParser);
		}

		[Test]
		[TestCase(0, Choice.Rock, Choice.Rock)]
		[TestCase(1, Choice.Rock, Choice.Paper)]
		[TestCase(2, Choice.Rock, Choice.Scissors)]
		public void Day02_Sample_RoundParserResult_Ok(int lineIdx, Choice you, Choice opponent)
		{
			var round = roundParser.Parse(SampleInput.Split("\r\n")[lineIdx]);

			Assert.That(round.You, Is.EqualTo(you));
			Assert.That(round.Opponent, Is.EqualTo(opponent));
		}

		[Test]
		public void Day02_Sample_GuideParserResult_Returns_As_ManyRounds_AsInput_HasLines()
		{
			var rounds = guideParser.Parse(SampleInput.Split("\r\n")).ToList();

			Assert.That(rounds, Has.Count.EqualTo(3));
		}

		[Test]
		[TestCase(0, Choice.Rock, Choice.Rock)]
		[TestCase(1, Choice.Rock, Choice.Paper)]
		[TestCase(2, Choice.Rock, Choice.Scissors)]
		public void Day02_Sample_GuideParserResult_Returns_Rounds(int lineIdx, Choice you, Choice opponent)
		{
			var round = guideParser.Parse(SampleInput.Split("\r\n")).ToList()[lineIdx];

			Assert.That(round.You, Is.EqualTo(you));
			Assert.That(round.Opponent, Is.EqualTo(opponent));
		}

		[Test]
		public void Day02_SampleInput_Part2_TotalScore_Is_12()
		{
			var rounds = guideParser.Parse(SampleInput.Split("\r\n"));
			Assert.That(Day02.GetYourScore(rounds), Is.EqualTo(12));
		}

		[Test]
		public async Task Day02_Puzzle2_TotalScore_Is_12316()
		{
			var rounds = guideParser.Parse(await File.ReadAllLinesAsync("Day02.txt"));
			Assert.That(Day02.GetYourScore(rounds), Is.EqualTo(12_316));
		}
	}
}
