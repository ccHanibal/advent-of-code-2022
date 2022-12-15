using System.Drawing;

namespace AdventOfCode2022
{
	public static class Day09
	{
		public record Step(int X, int Y, int NumberOfSteps);

		public interface IStepParser
		{
			Step Parse(string data);
		}

		public class StepParser : IStepParser
		{
			public Step Parse(string data)
			{
				var parts = data.Split(" ");
				return new Step(GetX(), GetY(), int.Parse(parts[1]));

				int GetX()
				{
					return parts[0] switch
					{
						"L" => -1,
						"R" => 1,
						"U" => 0,
						"D" => 0,
						_ => throw new InvalidDataException(),
					};
				}
				int GetY()
				{
					return parts[0] switch
					{
						"L" => 0,
						"R" => 0,
						"U" => -1,
						"D" => 1,
						_ => throw new InvalidDataException(),
					};
				}
			}
		}

		public class MovementParser
		{
			private readonly IStepParser stepParser;

			public MovementParser(IStepParser stepParser)
			{
				this.stepParser = stepParser;
			}

			public IEnumerable<Step> Parse(IEnumerable<string> data)
			{
				return data.Select(stepParser.Parse)
							.ToList();
			}
		}

		public static (Point Head, Point Tail) TakeStep(Point head, Point tail, Step step)
		{
			head.Offset(step.X, step.Y);

			int tailDiffX = head.X - tail.X;
			int tailDiffY = head.Y - tail.Y;

			if (tailDiffX == 0)
			{
				if (tailDiffY > 1)
					tail.Offset(0, 1);
				else if (tailDiffY < -1)
					tail.Offset(0, -1);
			}
			else if (tailDiffY == 0)
			{
				if (tailDiffX > 1)
					tail.Offset(1, 0);
				else if (tailDiffX < -1)
					tail.Offset(-1, 0);
			}
			else
			{
				if (Math.Abs(tailDiffX) > 1 || Math.Abs(tailDiffY) > 1)
				{
					tail.Offset(tailDiffX / Math.Abs(tailDiffX), tailDiffY / Math.Abs(tailDiffY));
				}
			}

			return (head, tail);
		}

		public static int SimulateStepsRops2Knots(IEnumerable<Step> steps)
		{
			// special case
			var head = Point.Empty;
			var tail = Point.Empty;

			var positonsTailOnce = new HashSet<Point>(1000)
			{
				tail
			};

			foreach (var step in steps)
			{
				for (int a = 0; a < step.NumberOfSteps; a++)
				{
					(head, tail) = TakeStep(head, tail, step);
					positonsTailOnce.Add(tail);
				}
			}

			return positonsTailOnce.Count;
		}

		public static int SimulateStepsRopeNKnots(IEnumerable<Step> steps, int numKnots)
		{
			var noStep = new Step(0, 0, 0);

			// head == 0
			// tail == numKnots - 1
			var knots = Enumerable.Repeat(Point.Empty, numKnots).ToArray();

			var positonsTailOnce = new HashSet<Point>(1000)
			{
				knots[numKnots - 1]
			};

			foreach (var step in steps)
			{
				for (int a = 0; a < step.NumberOfSteps; a++)
				{
					(knots[0], knots[1]) = TakeStep(knots[0], knots[1], step);

					for (int knotIdx = 1; knotIdx < knots.Length - 1; knotIdx++)
					{
						(knots[knotIdx], knots[knotIdx + 1]) = TakeStep(knots[knotIdx], knots[knotIdx + 1], noStep);
					}

					positonsTailOnce.Add(knots[numKnots - 1]);
				}
			}

			return positonsTailOnce.Count;
		}
	}
}
