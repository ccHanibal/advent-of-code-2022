using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using QuikGraph;

namespace AdventOfCode2022
{
	public static class Day14
	{
		public static IEnumerable<Point[]> ParseRocks(IEnumerable<string> data)
		{
			return data.Select(l => ParseRock(l).ToArray())
						.ToList();
		}
		public static IEnumerable<Point> ParseRock(string data)
		{
			var pointTexts = data.Split(" -> ");

			foreach (var text in pointTexts)
			{
				var parts = text.Split(',');
				yield return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
			}
		}
		public static Cave CreateCaveNoEnd(IEnumerable<Point[]> rocks)
		{
			return CreateCave(rocks, false);
		}
		public static Cave CreateCaveWithEnd(IEnumerable<Point[]> rocks)
		{
			return CreateCave(rocks, true);
		}
		public static Cave CreateCave(IEnumerable<Point[]> rocks, bool withEnd)
		{
			var allPoints = rocks.SelectMany(r => r)
								.Concat(new[] { new Point(500, 0) })
								.ToList();

			var minX = allPoints.Select(p => p.X).Min();
			var maxX = allPoints.Select(p => p.X).Max();
			var minY = allPoints.Select(p => p.Y).Min();
			var maxY = allPoints.Select(p => p.Y).Max();

			var endRocks = new Point[0];

			if (withEnd)
			{
				maxY += 2;
				minX -= 1000;
				maxX += 1000;

				endRocks = new[]
				{
					new Point(minX, maxY),
					new Point(maxX, maxY)
				};
			}

			var cave = new Cave(maxX - minX + 1, maxY - minY + 1, minX, minY);

			foreach (var rock in rocks.Concat(new[] { endRocks }))
			{
				for (int pIdx = 0; pIdx < rock.Length - 1; pIdx++)
				{
					cave.MarkRock(rock[pIdx], rock[pIdx + 1]);
				}

				//Console.WriteLine("After rock: {0}", string.Join(" -> ", rock.Select(r => $"{r.X},{r.Y}")));
				//cave.PrintCave();
			}

			return cave;
		}

		public static int FillCaveWithSand(Cave cave)
		{
			for (int numSand = 0; ; numSand++)
			{
				if (!cave.SimulateSand())
					return numSand;

				if (cave.IsSandOriginBlocked())
					return numSand + 1;
			}
		}

		public class Cave
		{
			private readonly int offsetX;
			private readonly int offsetY;

			private readonly char[][] cave;

			private readonly Point sandOrigin;

			public Cave(int sizeX, int sizeY, int offsetX, int offsetY)
			{
				this.offsetX = offsetX;
				this.offsetY = offsetY;

				this.cave = new char[sizeY][];
				for (int caveRowIdx = 0; caveRowIdx < cave.Length; caveRowIdx++)
				{
					cave[caveRowIdx] = new string('.', sizeX).ToCharArray();
				}

				this.sandOrigin = new Point(500, 0);
				this.sandOrigin.Offset(-offsetX, -offsetY);
			}

			public void MarkRock(Point start, Point end)
			{
				start.Offset(-offsetX, -offsetY);
				end.Offset(-offsetX, -offsetY);

				if (start.Y == end.Y)
				{
					var gradient = end.X - start.X > 0 ? 1 : -1;

					for (int x = start.X; x != end.X; x += gradient)
					{
						cave[start.Y][x] = '#';
					}
				}
				else
				{
					var gradient = end.Y - start.Y > 0 ? 1 : -1;

					for (int y = start.Y; y != end.Y; y += gradient)
					{
						cave[y][start.X] = '#';
					}
				}

				cave[end.Y][end.X] = '#';
			}
			public char GetCell(int x, int y)
			{
				return cave[y][x];
			}

			public void PrintCave()
			{
				int maxLineIndex = cave.Select((l, i) => l.Contains('o') || l.Contains('O') ? i : -1)
										.Where(i => i >= 0)
										.DefaultIfEmpty(1000)
										.Max();
				maxLineIndex = Math.Min(cave.Length, maxLineIndex + 4);

				for (int a = 0; a < maxLineIndex; a++)
				{
					Console.WriteLine(cave[a]);
				}
			}

			public bool SimulateSand()
			{
				//foreach (var line in cave)
				//{
				//	for (int a = 0; a < line.Length; a++)
				//	{
				//		if (line[a] == 'x')
				//			line[a] = '.';
				//		else if (line[a] == 'O')
				//			line[a] = 'o';
				//	}
				//}

				var sand = sandOrigin;

				while (true)
				{
					//if (IsValid(sand))
					//	cave[sand.Y][sand.X] = 'x';

					var sandDown = sand;
					sandDown.Offset(0, 1);

					if (!IsValid(sandDown))
						return false;

					if (IsFree(sandDown))
					{
						sand = sandDown;
						continue;
					}

					var sandLeftDown = sand;
					sandLeftDown.Offset(-1, 1);

					if (!IsValid(sandLeftDown))
						return false;

					if (IsFree(sandLeftDown))
					{
						sand = sandLeftDown;
						continue;
					}

					var sandRightDown = sand;
					sandRightDown.Offset(1, 1);

					if (!IsValid(sandRightDown))
						return false;

					if (IsFree(sandRightDown))
					{
						sand = sandRightDown;
						continue;
					}

					cave[sand.Y][sand.X] = 'O';
					return true;
				}
			}

			private bool IsBlocked(Point point)
			{
				char cellValue = cave[point.Y][point.X];
				return cellValue == '#' || cellValue == 'o' || cellValue == 'O';
			}
			private bool IsFree(Point point)
			{
				return !IsBlocked(point);
			}
			private bool IsValid(Point point)
			{
				return point.Y >= 0 && point.Y < cave.Length &&
						point.X >= 0 && point.X < cave[point.Y].Length;
			}

			public int CountSand()
			{
				return cave.SelectMany(c => c)
							.Count(c => c == 'o' || c == 'O');
			}
			public bool IsSandOriginBlocked()
			{
				return IsBlocked(sandOrigin);
			}
		}
	}
}
