using System.Text;

namespace AdventOfCode2022
{
	public static class Day10
	{
		public static int GetSumOfXAtCycles(string[] data)
		{
			int x = 1;
			int cycle = 0;
			int sum = 0;

			foreach (var line in data)
			{
				if (line == "noop")
				{
					ExecuteCycle();
				}
				else if (line.StartsWith("addx "))
				{
					var param = int.Parse(line.Substring(5));

					ExecuteCycle();
					ExecuteCycle();

					x += param;
				}
			}

			void ExecuteCycle()
			{
				cycle++;

				if ((cycle + 20) % 40 == 0)
				{
					int signalStrength = cycle * x;
					sum += signalStrength;
					Console.WriteLine("Cycle: {0}, X={1}, Str={2}, Sum={3}", cycle, x, signalStrength, sum);
				}
			}

			return sum;
		}

		public static string[] GetOutput(string[] data)
		{
			int x = 1;
			int cycle = 0;
			var screenRows = Enumerable
								.Range(0, 6)
								.Select(_ => new StringBuilder(40))
								.ToList();

			foreach (var line in data)
			{
				if (line == "noop")
				{
					ExecuteCycle();
				}
				else if (line.StartsWith("addx "))
				{
					var param = int.Parse(line.Substring(5));

					ExecuteCycle();
					ExecuteCycle();

					x += param;

					//Console.WriteLine("End of cycle {0:D2}: finish executing {1} (Register X is now {2})", cycle, line, x);
				}

				void ExecuteCycle()
				{
					cycle++;

					int rowCycle = (cycle - 1) % 40;
					int screenRowIdx = (cycle - 1) / 40;

					//Console.WriteLine();

					//Console.WriteLine("Start cycle {0:D3}: begin executing {1}", cycle, line);

					//Console.WriteLine("During cycle {0:D2}: CRT draws pixel in position {1} in row {2}", cycle, screenRows[screenRowIdx].Length, screenRowIdx);

					if (rowCycle == x - 1 || rowCycle == x || rowCycle == x + 1)
					{
						screenRows[screenRowIdx].Append('#');
					}
					else
					{
						screenRows[screenRowIdx].Append('.');
					}

					//Console.WriteLine("Current CRT row: {0}", screenRows[screenRowIdx].ToString());
				}
			}

			return screenRows
					.Select(sb => sb.ToString())
					.ToArray();
		}
	}
}
