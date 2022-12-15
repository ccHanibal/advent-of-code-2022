namespace AdventOfCode2022
{
	public static class Day02
	{
		public enum Choice
		{
			Rock,
			Paper,
			Scissors
		}

		public record Round(Choice You, Choice Opponent)
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

			public long GetScore()
			{
				return (You, Opponent) switch
				{
					(Choice.Rock, Choice.Rock) => Scores.Rock + Scores.Draw,
					(Choice.Rock, Choice.Paper) => Scores.Rock + Scores.Lost,
					(Choice.Rock, Choice.Scissors) => Scores.Rock + Scores.Win,
					(Choice.Paper, Choice.Rock) => Scores.Paper + Scores.Win,
					(Choice.Paper, Choice.Paper) => Scores.Paper + Scores.Draw,
					(Choice.Paper, Choice.Scissors) => Scores.Paper + Scores.Lost,
					(Choice.Scissors, Choice.Rock) => Scores.Scissors + Scores.Lost,
					(Choice.Scissors, Choice.Paper) => Scores.Scissors + Scores.Win,
					(Choice.Scissors, Choice.Scissors) => Scores.Scissors + Scores.Draw,
					_ => throw new InvalidOperationException("Unkown combination.")
				};
			}
		}

		public interface IRoundParser
		{
			Round Parse(string line);
		}

		public class RoundParserPlayers : IRoundParser
		{
			public Round Parse(string line)
			{
				var parts = line.Split(' ');
				return new Round(ParseChoice(parts[1]), ParseChoice(parts[0]));

				static Choice ParseChoice(string item)
				{
					return item switch
					{
						"A" => Choice.Rock,
						"B" => Choice.Paper,
						"C" => Choice.Scissors,
						"X" => Choice.Rock,
						"Y" => Choice.Paper,
						"Z" => Choice.Scissors,
						_ => throw new InvalidOperationException($"Unkown item found: {item}")
					};
				}
			}
		}

		public class RoundParserResult : IRoundParser
		{
			public Round Parse(string line)
			{
				var parts = line.Split(' ');
				var opponent = ParseChoice(parts[0]);
				return new Round(ParseChoiceByResult(opponent, parts[1]), opponent);

				static Choice ParseChoice(string item)
				{
					return item switch
					{
						"A" => Choice.Rock,
						"B" => Choice.Paper,
						"C" => Choice.Scissors,
						_ => throw new InvalidOperationException($"Unkown item found: {item}")
					};
				}

				static Choice ParseChoiceByResult(Choice opponentChoice, string item)
				{
					return (opponentChoice, item) switch
					{
						(Choice.Rock, "X") => Choice.Scissors,
						(Choice.Rock, "Y") => Choice.Rock,
						(Choice.Rock, "Z") => Choice.Paper,
						(Choice.Paper, "X") => Choice.Rock,
						(Choice.Paper, "Y") => Choice.Paper,
						(Choice.Paper, "Z") => Choice.Scissors,
						(Choice.Scissors, "X") => Choice.Paper,
						(Choice.Scissors, "Y") => Choice.Scissors,
						(Choice.Scissors, "Z") => Choice.Rock,
						_ => throw new InvalidOperationException($"Unkown combination found for item: {item}")
					};
				}
			}
		}

		public class GuideParser
		{
			private readonly IRoundParser roundParser;

			public GuideParser(IRoundParser roundParser)
			{
				this.roundParser = roundParser;
			}

			public IEnumerable<Round> Parse(IEnumerable<string> content)
			{
				foreach (var line in content)
				{
					yield return roundParser.Parse(line);
				}
			}
		}

		public static long GetYourScore(IEnumerable<Round> rounds)
		{
			return rounds
					.Select(r => r.GetScore())
					.Sum();
		}
	}
}
