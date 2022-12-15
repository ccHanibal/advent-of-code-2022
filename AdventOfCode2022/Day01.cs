namespace AdventOfCode2022
{
	public class Day01
	{
		public sealed record Elf(long Calories);

		public static IEnumerable<Elf> CreateElves(IEnumerable<string> content)
		{
			var elves = new List<Elf>();
			long currentElfCalories = 0;

			foreach (var line in content)
			{
				if (string.IsNullOrEmpty(line))
				{
					elves.Add(new Elf(currentElfCalories));
					currentElfCalories = 0;
				}
				else
				{
					currentElfCalories += long.Parse(line);
				}
			}

			if (currentElfCalories > 0)
				elves.Add(new Elf(currentElfCalories));

			return elves;
		}

		public static long GetMaxCalories(IEnumerable<Elf> elves)
		{
			return elves
					.Select(e => e.Calories)
					.Max();
		}

		public static long GetSumMaxCaloriesTopThree(IEnumerable<Elf> elves)
		{
			return elves
					.OrderByDescending(e => e.Calories)
					.Select(e => e.Calories)
					.Take(3)
					.Sum();
		}
	}
}