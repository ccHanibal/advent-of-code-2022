using MoreLinq;

namespace AdventOfCode2022
{
	public static class Day03
	{
		public record Backpack(string Part1, string Part2)
		{
			public string AllItems => Part1 + Part2;

			public char GetCommonItemType()
			{
				return Part1.Intersect(Part2).First();
			}
		}

		public interface IBackpackParser
		{
			Backpack Parse(string line);
		}

		public class BackpackParser : IBackpackParser
		{
			public Backpack Parse(string line)
			{
				var sizeOfParts = line.Length / 2;

				return new Backpack(line[..^sizeOfParts], line[sizeOfParts..]);
			}
		}

		public class BackpacksParser
		{
			private readonly IBackpackParser backpackParser;

			public BackpacksParser(IBackpackParser backpackParser)
			{
				this.backpackParser = backpackParser;
			}

			public IEnumerable<Backpack> Parse(IEnumerable<string> content)
			{
				foreach (var line in content)
				{
					yield return backpackParser.Parse(line);
				}
			}
		}

		public static int GetItemTypePriority(char itemType)
		{
			const string itemTypes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

			return itemTypes.IndexOf(itemType) + 1;
		}

		public static int SumOfCommonItemTypePriorities(IEnumerable<Backpack> backpacks)
		{
			return backpacks
					.Select(b => b.GetCommonItemType())
					.Select(GetItemTypePriority)
					.Sum();
		}

		public static IEnumerable<IEnumerable<Backpack>> GroupBackpacksByThree(IEnumerable<Backpack> backpacks)
		{
			return backpacks
					.Batch(3)
					.ToList();
		}

		public static char GetBadgeItemType(IEnumerable<Backpack> backpacks)
		{
			return backpacks
					.Aggregate(
						backpacks.First().AllItems.AsEnumerable(),
						(b1, b2) => b1.Intersect(b2.AllItems))
					.First();
		}

		public static int SumOfBadgeItemTypePriorities(IEnumerable<Backpack> backpacks)
		{
			return GroupBackpacksByThree(backpacks)
					.Select(GetBadgeItemType)
					.Select(GetItemTypePriority)
					.Sum();
		}
	}
}
