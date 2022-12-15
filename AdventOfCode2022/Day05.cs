namespace AdventOfCode2022
{
	using ICrateStacks = System.Collections.Generic.IDictionary<int, System.Collections.Generic.Stack<char>>;
	using CrateStacks = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Stack<char>>;

	using System.Text.RegularExpressions;
	using MoreLinq;
	using System.Text;

	public static class Day05
	{
		public record MoveInstruction(int NumCrates, int FromStack, int ToStack);

		public class StackParser
		{
			public ICrateStacks Parse(IEnumerable<string> content, int numStacks)
			{
				var stacks = new CrateStacks();
				for (int idx = 1; idx <= numStacks; idx++)
				{
					stacks.Add(idx, new Stack<char>());
				}

				var regex = new Regex(string.Join("", Enumerable.Repeat(@"(\[[A-Z]\]|\s{3})?\s?", numStacks)));

				foreach (var line in content.TakeUntil(l => string.IsNullOrEmpty(l)).Reverse().Skip(2))
				{
					var match = regex.Match(line);

					for (int grpNum = 1; grpNum <= numStacks; grpNum++)
					{
						if (match.Groups.Count >= grpNum && match.Groups[grpNum].Success && !string.IsNullOrWhiteSpace(match.Groups[grpNum].Value))
							stacks[grpNum].Push(match.Groups[grpNum].Value[1]);
					}
				}

				return stacks;
			}
		}

		public interface IInstructionParser
		{
			IEnumerable<MoveInstruction> Parse(string line);
		}

		public class InstructionParserCrateMover9000 : IInstructionParser
		{
			private static readonly Regex instrRegex = new Regex(@"move (\d+) from (\d+) to (\d+)", RegexOptions.IgnoreCase);

			public IEnumerable<MoveInstruction> Parse(string line)
			{
				var matches = instrRegex.Match(line);
				var numInstructionsToGenerate = GetNumberFromMatchedGroup(1);
				var instruction = new MoveInstruction(1, GetNumberFromMatchedGroup(2), GetNumberFromMatchedGroup(3));

				return Enumerable.Repeat(instruction, numInstructionsToGenerate);

				int GetNumberFromMatchedGroup(int groupNr)
				{
					return int.Parse(matches.Groups[groupNr].Value);
				}
			}
		}

		public class InstructionParserCrateMover9001 : IInstructionParser
		{
			private static readonly Regex instrRegex = new Regex(@"move (\d+) from (\d+) to (\d+)", RegexOptions.IgnoreCase);

			public IEnumerable<MoveInstruction> Parse(string line)
			{
				var matches = instrRegex.Match(line);
				var numInstructionsToGenerate = GetNumberFromMatchedGroup(1);
				var instruction = new MoveInstruction(numInstructionsToGenerate, GetNumberFromMatchedGroup(2), GetNumberFromMatchedGroup(3));

				return Enumerable.Repeat(instruction, 1);

				int GetNumberFromMatchedGroup(int groupNr)
				{
					return int.Parse(matches.Groups[groupNr].Value);
				}
			}
		}

		public class InstructionsParser
		{
			private readonly IInstructionParser instructionParser;

			public InstructionsParser(IInstructionParser instructionParser)
			{
				this.instructionParser = instructionParser;
			}

			public IEnumerable<MoveInstruction> Parse(IEnumerable<string> content)
			{
				foreach (var line in content.SkipUntil(l => string.IsNullOrEmpty(l)))
				{
					foreach (var instr in instructionParser.Parse(line))
					{
						yield return instr;
					}
				}
			}
		}

		public static void ApplyInstructions(ICrateStacks stacks, IEnumerable<MoveInstruction> instructions)
		{
			var tmp = new Stack<char>();

			foreach (var instruction in instructions)
			{
				tmp.Clear();

				for (int numItem = 0; numItem < instruction.NumCrates; numItem++)
				{
					tmp.Push(stacks[instruction.FromStack].Pop());
				}

				while (tmp.Count > 0)
				{
					stacks[instruction.ToStack].Push(tmp.Pop());
				}
			}
		}

		public static string GetTopItems(ICrateStacks stacks)
		{
			var sb = new StringBuilder();

			foreach (var stackIdx in stacks.Keys.OrderBy(x => x))
			{
				sb.Append(stacks[stackIdx].Peek());
			}

			return sb.ToString();
		}
	}
}
