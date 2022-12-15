using static AdventOfCode2022.Day02;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day02_Part1_Tests
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

		private readonly IRoundParser roundParser = new RoundParserPlayers();

		public Day02_Part1_Tests()
		{
			guideParser = new GuideParser(roundParser);
		}


		[Test]
		[TestCase(0, Choice.Paper, Choice.Rock)]
		[TestCase(1, Choice.Rock, Choice.Paper)]
		[TestCase(2, Choice.Scissors, Choice.Scissors)]
		public void Day02_Sample_RoundParserPlayers_Ok(int lineIdx, Choice you, Choice opponent)
		{
			var round = roundParser.Parse(SampleInput.Split("\r\n")[lineIdx]);

			Assert.That(round.You, Is.EqualTo(you));
			Assert.That(round.Opponent, Is.EqualTo(opponent));
		}

		[Test]
		public void Day02_Sample_GuideParserPlayers_Returns_As_ManyRounds_AsInput_HasLines()
		{
			var rounds = guideParser.Parse(SampleInput.Split("\r\n")).ToList();

			Assert.That(rounds, Has.Count.EqualTo(3));
		}

		[Test]
		[TestCase(0, Choice.Paper, Choice.Rock)]
		[TestCase(1, Choice.Rock, Choice.Paper)]
		[TestCase(2, Choice.Scissors, Choice.Scissors)]
		public void Day02_Sample_GuideParserPlayers_Returns_Rounds(int lineIdx, Choice you, Choice opponent)
		{
			var round = guideParser.Parse(SampleInput.Split("\r\n")).ToList()[lineIdx];

			Assert.That(round.You, Is.EqualTo(you));
			Assert.That(round.Opponent, Is.EqualTo(opponent));
		}

		[Test]
		[TestCase(Choice.Rock, Choice.Rock, Scores.Rock + Scores.Draw)]
		[TestCase(Choice.Rock, Choice.Paper, Scores.Rock + Scores.Lost)]
		[TestCase(Choice.Rock, Choice.Scissors, Scores.Rock + Scores.Win)]
		[TestCase(Choice.Paper, Choice.Rock, Scores.Paper + Scores.Win)]
		[TestCase(Choice.Paper, Choice.Paper, Scores.Paper + Scores.Draw)]
		[TestCase(Choice.Paper, Choice.Scissors, Scores.Paper + Scores.Lost)]
		[TestCase(Choice.Scissors, Choice.Rock, Scores.Scissors + Scores.Lost)]
		[TestCase(Choice.Scissors, Choice.Paper, Scores.Scissors + Scores.Win)]
		[TestCase(Choice.Scissors, Choice.Scissors, Scores.Scissors + Scores.Draw)]
		public void Day02_Round_CalcScore(Choice you, Choice opponent, int score)
		{
			var round = new Round(you, opponent);
			Assert.That(round.GetScore(), Is.EqualTo(score));
		}

		[Test]
		public void Day02_SampleInput_Part1_TotalScore_Is_15()
		{
			var rounds = guideParser.Parse(SampleInput.Split("\r\n"));
			Assert.That(Day02.GetYourScore(rounds), Is.EqualTo(15));
		}

		[Test]
		public async Task Day02_Puzzle1_TotalScore_Is_13809()
		{
			var rounds = guideParser.Parse(await File.ReadAllLinesAsync("Day02.txt"));
			Assert.That(Day02.GetYourScore(rounds), Is.EqualTo(13_809));
		}
	}
}
