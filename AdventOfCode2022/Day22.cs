using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using static AdventOfCode2022.Day22;

namespace AdventOfCode2022
{
	public static class Day22
	{
		private const int TurnLeft = -1;
		private const int TurnRight = 1;

		public record Instruction(Point Movement, int Count, int Facing);

		private static readonly Point[] movement = new[]
		{
			new Point(1, 0),
			new Point(0, 1),
			new Point(-1, 0),
			new Point(0, -1)
		};

		public static char[][] ParseField(IEnumerable<string> data)
		{
			var fieldData = data.TakeWhile(l => l.Length > 0)
								.ToList();
			var maxFieldLength = fieldData.Select(l => l.Length)
											.Max();

			return fieldData.Select(l => l.PadRight(maxFieldLength, ' ').ToCharArray())
							.ToArray();
		}
		public static IEnumerable<Instruction> ParseInstructions(IEnumerable<string> data)
		{
			var instructionsText = data.Last();

			return ParseInstructionsImpl().ToList();

			IEnumerable<Instruction> ParseInstructionsImpl()
			{
				int index = 0;
				var rotationSymbols = "RL".ToCharArray();
				var facingIndex = 0;

				while (index < instructionsText.Length)
				{
					var nextRotationIndex = instructionsText.IndexOfAny(rotationSymbols, index);
					var steps = GetNumber();

					yield return new Instruction(movement[facingIndex], steps, facingIndex);

					if (nextRotationIndex == -1)
						yield break;

					index = nextRotationIndex + 1;

					var turnOffset = instructionsText[nextRotationIndex] switch
					{
						'R' => TurnRight,
						'L' => TurnLeft,
						_ => throw new InvalidOperationException($"Unknown rotation symbol: {instructionsText[nextRotationIndex]}")
					};

					facingIndex = (facingIndex + turnOffset + movement.Length) % movement.Length;

					int GetNumber()
					{
						if (nextRotationIndex >= 0)
							return int.Parse(instructionsText.Substring(index, nextRotationIndex - index));

						return int.Parse(instructionsText.Substring(index));
					}
				}
			}
		}

		public static (Point Position, int Facing) Walk(char[][] field, IEnumerable<Instruction> instructions)
		{
			int currentX = new string(field[0]).IndexOf('.');
			int currentY = 0;
			int facing = 0;

			int fieldHeight = field.Length;
			int fieldWidth = field[0].Length;

			Console.WriteLine("Starting at: {0}, {1}", currentX + 1, currentY + 1);

			foreach (var instruction in instructions)
			{
				facing = instruction.Facing;

				for (int a = 0; a < instruction.Count; a++)
				{
					int nextX = FindNextFreeIndexX();
					int nextY = FindNextFreeIndexY();

					if (nextX == -1 || nextY == -1)
						break;

					currentX = nextX;
					currentY = nextY;

					field[currentY][currentX] = facing switch
					{
						0 => '>',
						1 => 'v',
						2 => '<',
						3 => '^',
						_ => throw new InvalidOperationException()
					};
				}

				Console.WriteLine("After {0} {1}: {2}, {3}", instruction.Count, facing switch
				{
					0 => "right",
					1 => "down",
					2 => "left",
					3 => "up",
					_ => throw new InvalidOperationException()
				}, currentX + 1, currentY + 1);

				int FindNextFreeIndexX()
				{
					if (instruction.Movement.X == 0)
						return currentX;

					for (int x = (currentX + instruction.Movement.X + fieldWidth) % fieldWidth; x != currentX; x = (x + instruction.Movement.X + fieldWidth) % fieldWidth)
					{
						if (".<>.^".Contains(field[currentY][x]))
							return x;

						if (field[currentY][x] == '#')
							return -1;
					}

					throw new InvalidOperationException("X");
				}

				int FindNextFreeIndexY()
				{
					if (instruction.Movement.Y == 0)
						return currentY;

					for (int y = (currentY + instruction.Movement.Y + fieldHeight) % fieldHeight; y != currentY; y = (y + instruction.Movement.Y + fieldHeight) % fieldHeight)
					{
						if (".<>.^".Contains(field[y][currentX]))
							return y;

						if (field[y][currentX] == '#')
							return -1;
					}

					throw new InvalidOperationException("Y");
				}
			}

			return (new Point(currentX, currentY), facing);
		}

		public static int GetPassword((Point Position, int Facing) finish)
		{
			return (finish.Position.Y + 1) * 1_000 +
					(finish.Position.X + 1) * 4 +
					finish.Facing;
		}
	}
}