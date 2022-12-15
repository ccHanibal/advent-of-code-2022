using static AdventOfCode2022.Day05;

namespace AdventOfCode2022.Tests
{
	using AdventOfCode2022.Tests.Day05Ext;
	using CrateStacks = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Stack<char>>;

	[TestFixture]
	public class Day05Tests
	{


		private const string SampleInput =
			"""
			    [D]
			[N] [C]
			[Z] [M] [P]
			 1   2   3

			move 1 from 2 to 1
			move 3 from 1 to 3
			move 2 from 2 to 1
			move 1 from 1 to 2
			""";

		private readonly StackParser stackParser;
		private readonly IInstructionParser instructionParser9000;
		private readonly IInstructionParser instructionParser9001;

		private readonly InstructionsParser instructionsParser9000;
		private readonly InstructionsParser instructionsParser9001;

		public Day05Tests()
		{
			stackParser = new StackParser();
			instructionParser9000 = new InstructionParserCrateMover9000();
			instructionParser9001 = new InstructionParserCrateMover9001();

			instructionsParser9000 = new InstructionsParser(instructionParser9000);
			instructionsParser9001 = new InstructionsParser(instructionParser9001);
		}

		[Test]
		public void Day05_Sample_StackParser_Has_Three_Stacks()
		{
			var stacks = stackParser.Parse(SampleInput.Split("\r\n"), 3);

			Assert.That(stacks, Has.Count.EqualTo(3));
		}

		[Test]
		[TestCase(1, "NZ")]
		[TestCase(2, "DCM")]
		[TestCase(3, "P")]
		public void Day05_Sample_StackParser_NthStack_HasItems(int stackIdx, string items)
		{
			var stacks = stackParser.Parse(SampleInput.Split("\r\n"), 3);
			Assume.That(stacks, Contains.Key(stackIdx));

			Assert.That(stacks[stackIdx].GetAllItems(), Is.EqualTo(items.ToCharArray()));
		}

		[Test]
		[TestCase(1, 1, 2, 1)]
		[TestCase(2, 3, 1, 3)]
		[TestCase(3, 2, 2, 1)]
		[TestCase(4, 1, 1, 2)]
		public void Day05_Sample_InstructionParser_Ok(int lineIdx, int numberOfInstructions, int fromStack, int toStack)
		{
			var instructions = instructionParser9000.Parse(SampleInput.Split("\r\n")[lineIdx + 4]).ToList();

			Assert.That(instructions, Has.Count.EqualTo(numberOfInstructions));
			Assert.That(instructions.Select(i => i.FromStack).ToList(), Has.All.EqualTo(fromStack));
			Assert.That(instructions.Select(i => i.ToStack).ToList(), Has.All.EqualTo(toStack));
		}

		[Test]
		public void Day05_Sample_InstructionsParser_Ok()
		{
			var instructions = instructionsParser9000.Parse(SampleInput.Split("\r\n")).ToList();

			Assert.That(instructions, Has.Count.EqualTo(7));
			Assert.That(instructions.Select(i => i.FromStack).ToList(), Is.EqualTo(new[] { 2, 1, 1, 1, 2, 2, 1 }));
			Assert.That(instructions.Select(i => i.ToStack).ToList(), Is.EqualTo(new[] { 1, 3, 3, 3, 1, 1, 2 }));
		}

		[Test]
		public void Day05_Sample_ApplyInstuction_MovesTopItem_FromSource()
		{
			var stacks = new CrateStacks
			{
				{ 1, "NZ".ToStack() },
				{ 2, "DCM".ToStack() },
				{ 3, "P".ToStack() }
			};

			var instruction = new MoveInstruction(1, 2, 1);

			ApplyInstructions(stacks, new[] { instruction });

			Assert.That(stacks[2].GetAllItems(), Is.EqualTo("CM".ToCharArray()));
		}

		[Test]
		public void Day05_Sample_ApplyInstuction_MovesTopItem_ToDestination()
		{
			var stacks = new CrateStacks
			{
				{ 1, "NZ".ToStack() },
				{ 2, "DCM".ToStack() },
				{ 3, "P".ToStack() }
			};

			var instruction = new MoveInstruction(1, 2, 1);

			ApplyInstructions(stacks, new[] { instruction });

			Assert.That(stacks[1].GetAllItems(), Is.EqualTo("DNZ".ToCharArray()));
		}

		[Test]
		public void Day05_Sample_ApplyInstuction_Multiple()
		{
			var stacks = new CrateStacks
			{
				{ 1, "NZ".ToStack() },
				{ 2, "DCM".ToStack() },
				{ 3, "P".ToStack() }
			};

			var instructions = new[]
			{
				new MoveInstruction(1, 2, 1),
				new MoveInstruction(1, 1, 3),
				new MoveInstruction(1, 1, 3),
				new MoveInstruction(1, 1, 3)
			};

			ApplyInstructions(stacks, instructions);

			Assert.That(stacks[1].GetAllItems(), Is.Empty);
			Assert.That(stacks[2].GetAllItems(), Is.EqualTo("CM".ToCharArray()));
			Assert.That(stacks[3].GetAllItems(), Is.EqualTo("ZNDP".ToCharArray()));
		}

		[Test]
		public void Day05_Sample_ApplyInstuction_9001_MovesAllAtOnce()
		{
			var stacks = new CrateStacks
			{
				{ 1, "NZ".ToStack() },
				{ 2, "DCM".ToStack() },
				{ 3, "P".ToStack() }
			};

			var instructions = new[]
			{
				new MoveInstruction(2, 1, 3),
			};

			ApplyInstructions(stacks, instructions);

			Assert.That(stacks[1].GetAllItems(), Is.Empty);
			Assert.That(stacks[2].GetAllItems(), Is.EqualTo("DCM".ToCharArray()));
			Assert.That(stacks[3].GetAllItems(), Is.EqualTo("NZP".ToCharArray()));
		}

		[Test]
		public void Day05_Sample_GetTopItems_9000_Is_NDP()
		{
			var stacks = new CrateStacks
			{
				{ 1, "NZ".ToStack() },
				{ 2, "DCM".ToStack() },
				{ 3, "P".ToStack() }
			};

			Assert.That(GetTopItems(stacks), Is.EqualTo("NDP"));
		}

		[Test]
		[TestCase(1, 1, 2, 1)]
		[TestCase(2, 3, 1, 3)]
		[TestCase(3, 2, 2, 1)]
		[TestCase(4, 1, 1, 2)]
		public void Day05_Sample_InstructionParser9001_Ok(int lineIdx, int numberOfInstructions, int fromStack, int toStack)
		{
			var instructions = instructionParser9001.Parse(SampleInput.Split("\r\n")[lineIdx + 4]).ToList();

			Assert.That(instructions, Has.Count.EqualTo(1));
			Assert.That(instructions[0].NumCrates, Is.EqualTo(numberOfInstructions));
			Assert.That(instructions[0].FromStack, Is.EqualTo(fromStack));
			Assert.That(instructions[0].ToStack, Is.EqualTo(toStack));
		}

		[Test]
		public async Task Day05_Puzzle2_GetTopItems_Is_RMHFJNVFP()
		{
			var stacks = stackParser.Parse(await File.ReadAllLinesAsync("Day05.txt"), 9);
			var instructions = instructionsParser9001.Parse(await File.ReadAllLinesAsync("Day05.txt")).ToList();

			ApplyInstructions(stacks, instructions);

			Assert.That(GetTopItems(stacks), Is.EqualTo("RMHFJNVFP"));
		}
	}

	namespace Day05Ext
	{
		public static class Ext
		{
			public static Stack<char> ToStack(this string items)
			{
				var stack = new Stack<char>(items.Length);
				for (int a = items.Length - 1; a >= 0; a--)
				{
					stack.Push(items[a]);
				}

				return stack;
			}

			public static IEnumerable<char> GetAllItems(this Stack<char> stack)
			{
				return stack.ToList();
			}
		}
	}
}
