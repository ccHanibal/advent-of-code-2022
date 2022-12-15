using FakeItEasy;
using System.Drawing;
using static AdventOfCode2022.Day09;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day09Tests
	{
		private static readonly string SampleInput =
			"""
			R 4
			U 4
			L 3
			D 1
			R 4
			D 1
			L 5
			R 2
			""";

		private static readonly string SampleInput2 =
			"""
			R 5
			U 8
			L 8
			D 3
			R 17
			D 10
			L 25
			U 20
			""";

		[Test]
		[TestCase("R 4", 1, 0, 4)]
		[TestCase("U 3", 0, -1, 3)]
		[TestCase("D 2", 0, 1, 2)]
		[TestCase("L 1", -1, 0, 1)]
		public void Day09_StepParser_Parse(string data, int stepX, int stepY, int numOfSteps)
		{
			var parser = new StepParser();

			var step = parser.Parse(data);

			Assert.That(step.X, Is.EqualTo(stepX));
			Assert.That(step.Y, Is.EqualTo(stepY));
			Assert.That(step.NumberOfSteps, Is.EqualTo(numOfSteps));
		}

		[Test]
		public void Day09_Sample_MovementParser_ParsesEachLine_UsingStepParser()
		{
			var stepParser = A.Fake<IStepParser>();
			var parser = new MovementParser(stepParser);

			_ = parser.Parse(SampleInput.Split("\r\n"));

			A.CallTo(() => stepParser.Parse(A<string>._)).MustHaveHappened(8, Times.Exactly);
		}

		[Test]
		public void Day09_Sample_MovementParser_NumberOfInputLines_EqualsOutputCount()
		{
			var stepParser = A.Fake<IStepParser>();
			var parser = new MovementParser(stepParser);

			var steps = parser.Parse(SampleInput.Split("\r\n"));

			Assert.That(steps.Count(), Is.EqualTo(8));
		}

		[Test]
		[TestCase(0, 1, 0, 4)]
		[TestCase(1, 0, -1, 4)]
		[TestCase(2, -1, 0, 3)]
		[TestCase(3, 0, 1, 1)]
		[TestCase(4, 1, 0, 4)]
		[TestCase(5, 0, 1, 1)]
		[TestCase(6, -1, 0, 5)]
		[TestCase(7, 1, 0, 2)]
		public void Day09_Sample_MovementParser_HasStepAtIndex(int lineIdx, int stepX, int stepY, int numOfSteps)
		{
			var parser = new MovementParser(new StepParser());

			var steps = parser.Parse(SampleInput.Split("\r\n"));
			var step = steps.ElementAt(lineIdx);

			Assert.That(step.X, Is.EqualTo(stepX));
			Assert.That(step.Y, Is.EqualTo(stepY));
			Assert.That(step.NumberOfSteps, Is.EqualTo(numOfSteps));
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(0, -1)]
		[TestCase(1, 0)]
		[TestCase(-1, 0)]
		public void Day09_TakeStep_HeadMovesInDirection(int stepX, int stepY)
		{
			var (head, _) = TakeStep(Point.Empty, Point.Empty, new Step(stepX, stepY, 1));
			Assert.That(head.X, Is.EqualTo(stepX));
			Assert.That(head.Y, Is.EqualTo(stepY));
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(0, -1)]
		[TestCase(1, 0)]
		[TestCase(-1, 0)]
		public void Day09_TakeStep_TailNotMoved_WhenHeadAndTailSamePosition(int stepX, int stepY)
		{
			var (_, tail) = TakeStep(Point.Empty, Point.Empty, new Step(stepX, stepY, 1));
			Assert.That(tail.X, Is.Zero);
			Assert.That(tail.Y, Is.Zero);
		}

		[Test]
		public void Day09_TakeStep_TailMovedRightOnce_WhenXDiff_Two_AfterStep()
		{
			var (_, tail) = TakeStep(new Point(1, 0), new Point(0, 0), new Step(1, 0, 1));
			Assert.That(tail.X, Is.EqualTo(1));
			Assert.That(tail.Y, Is.Zero);
		}

		[Test]
		public void Day09_TakeStep_TailMovedLeft_WhenXDiff_Two_AfterStep()
		{
			var (_, tail) = TakeStep(new Point(1, 0), new Point(2, 0), new Step(-1, 0, 1));
			Assert.That(tail.X, Is.EqualTo(1));
			Assert.That(tail.Y, Is.Zero);
		}

		[Test]
		public void Day09_TakeStep_TailMovedUpOnce_WhenYDiff_Two_AfterStep()
		{
			var (_, tail) = TakeStep(new Point(0, -1), new Point(0, 0), new Step(0, -1, 1));
			Assert.That(tail.X, Is.Zero);
			Assert.That(tail.Y, Is.EqualTo(-1));
		}

		[Test]
		public void Day09_TakeStep_TailDownLeft_WhenYDiff_Two_AfterStep()
		{
			var (_, tail) = TakeStep(new Point(0, 1), new Point(0, 0), new Step(0, 1, 1));
			Assert.That(tail.X, Is.Zero);
			Assert.That(tail.Y, Is.EqualTo(1));
		}

		[Test]
		[TestCase(-1, -1)]
		[TestCase(1, -1)]
		[TestCase(-1, 1)]
		[TestCase(1, 1)]
		public void Day09_TailMovesDiagonally_WhenXAndY_Diff2_AfterStep(int stepX, int stepY)
		{
			var (_, tail1) = TakeStep(new Point(stepX, stepY), new Point(0, 0), new Step(stepX, 0, 1));
			var (_, tail2) = TakeStep(new Point(stepX, stepY), new Point(0, 0), new Step(0, stepY, 1));

			Assert.That(tail1.X, Is.EqualTo(stepX));
			Assert.That(tail1.Y, Is.EqualTo(stepY));
			Assert.That(tail2.X, Is.EqualTo(stepX));
			Assert.That(tail2.Y, Is.EqualTo(stepY));
		}

		[Test]
		public void Day09_Sample_SimulateSteps_Returns_13()
		{
			var parser = new MovementParser(new StepParser());

			var steps = parser.Parse(SampleInput.Split("\r\n"));
			int numPositionsTailWasAtleastOnce = SimulateStepsRopeNKnots(steps, 2);

			Assert.That(numPositionsTailWasAtleastOnce, Is.EqualTo(13));
		}

		[Test]
		public async Task Day09_Puzzle1_SimulateSteps_Returns_6376()
		{
			var parser = new MovementParser(new StepParser());

			var steps = parser.Parse(await File.ReadAllLinesAsync("Day09.txt"));
			int numPositionsTailWasAtleastOnce = SimulateStepsRopeNKnots(steps, 2);

			Assert.That(numPositionsTailWasAtleastOnce, Is.EqualTo(6_376));
		}

		[Test]
		public void Day09_Sample2_SimulateSteps_10Knots_Returns_36()
		{
			var parser = new MovementParser(new StepParser());

			var steps = parser.Parse(SampleInput2.Split("\r\n"));
			int numPositionsTailWasAtleastOnce = SimulateStepsRopeNKnots(steps, 10);

			Assert.That(numPositionsTailWasAtleastOnce, Is.EqualTo(36));
		}

		[Test]
		public async Task Day09_Puzzle1_SimulateSteps_10Knots_Returns_2607()
		{
			var parser = new MovementParser(new StepParser());

			var steps = parser.Parse(await File.ReadAllLinesAsync("Day09.txt"));
			int numPositionsTailWasAtleastOnce = SimulateStepsRopeNKnots(steps, 10);

			Assert.That(numPositionsTailWasAtleastOnce, Is.EqualTo(2_607));
		}
	}
}
