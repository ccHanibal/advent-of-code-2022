using static AdventOfCode2022.Day22;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day22Tests
	{
		private static readonly string SampleInput =
			"""
			        ...#
			        .#..
			        #...
			        ....
			...#.......#
			........#...
			..#....#....
			..........#.
			        ...#....
			        .....#..
			        .#......
			        ......#.

			10R5L5R10L4R5L5
			""";

		[Test]
		public void Day22_Sample_Walk_Finishes_At_Row_5()
		{
			var field = ParseField(SampleInput.Split("\r\n"));
			var instruction = ParseInstructions(SampleInput.Split("\r\n"));

			var finish = Walk(field, instruction);

			Assert.That(finish.Position.Y, Is.EqualTo(5));
		}

		[Test]
		public void Day22_Sample_Walk_Finishes_At_Column_7()
		{
			var field = ParseField(SampleInput.Split("\r\n"));
			var instruction = ParseInstructions(SampleInput.Split("\r\n"));

			var finish = Walk(field, instruction);

			Assert.That(finish.Position.X, Is.EqualTo(7));
		}

		[Test]
		public void Day22_Sample_Walk_Finishes_Facing_Right()
		{
			var field = ParseField(SampleInput.Split("\r\n"));
			var instruction = ParseInstructions(SampleInput.Split("\r\n"));

			var finish = Walk(field, instruction);

			Assert.That(finish.Facing, Is.EqualTo(0));
		}

		[Test]
		public void Day22_Sample_Walk_Password_Is_6032()
		{
			var field = ParseField(SampleInput.Split("\r\n"));
			var instruction = ParseInstructions(SampleInput.Split("\r\n"));

			var finish = Walk(field, instruction);

			Assert.That(GetPassword(finish), Is.EqualTo(6_032));
		}

		[Test]
		public async Task Day22_Puzzle1_Walk_FinishesAt_X_21_Y_158_Facing_Left()
		{
			var input = await File.ReadAllLinesAsync("Day22.txt");

			var field = ParseField(input);
			var instruction = ParseInstructions(input);

			var finish = Walk(field, instruction);

			Assert.That(finish.Position.X, Is.EqualTo(21), "X");
			Assert.That(finish.Position.Y, Is.EqualTo(158), "Y");
			Assert.That(finish.Facing, Is.EqualTo(2), "Facing");
		}

		[Test]
		public async Task Day22_Puzzle1_Walk_Password_Is_xxx()
		{
			var input = await File.ReadAllLinesAsync("Day22.txt");

			var field = ParseField(input);
			var instruction = ParseInstructions(input);

			var finish = Walk(field, instruction);

			Assert.That(GetPassword(finish), Is.EqualTo(1));
		}
	}
}
