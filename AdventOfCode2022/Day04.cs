namespace AdventOfCode2022
{
	public static class Day04
	{
		public record Section(int FirstId, int LastId);

		public record ElfPair(Section Elf1, Section Elf2)
		{
			public bool HasFullOverlap()
			{
				if (Elf1.FirstId <= Elf2.FirstId && Elf1.LastId >= Elf2.LastId)
					return true;

				if (Elf1.FirstId >= Elf2.FirstId && Elf1.LastId <= Elf2.LastId)
					return true;

				return false;
			}

			public bool HasAnyOverlap()
			{
				return RangeOfSections(Elf1)
						.Intersect(RangeOfSections(Elf2))
						.Any();

				static IEnumerable<int> RangeOfSections(Section section)
				{
					return Enumerable.Range(section.FirstId, section.LastId - section.FirstId + 1);
				}
			}
		}

		public interface ISectionParser
		{
			Section Parse(string item);
		}

		public class SectionParser : ISectionParser
		{
			public Section Parse(string item)
			{
				var parts = item.Split("-");
				return new Section(int.Parse(parts[0]), int.Parse(parts[1]));
			}
		}

		public interface IElfPairParser
		{
			ElfPair Parse(string line);
		}

		public class ElfPairParser : IElfPairParser
		{
			private readonly ISectionParser sectionParser;

			public ElfPairParser(ISectionParser sectionParser)
			{
				this.sectionParser = sectionParser;
			}

			public ElfPair Parse(string line)
			{
				var parts = line.Split(",");
				return new ElfPair(sectionParser.Parse(parts[0]), sectionParser.Parse(parts[1]));
			}
		}

		public class CleaningParser
		{
			private readonly IElfPairParser elfPairParser;

			public CleaningParser(IElfPairParser elfPairParser)
			{
				this.elfPairParser = elfPairParser;
			}

			public IEnumerable<ElfPair> Parse(IEnumerable<string> content)
			{
				foreach (var line in content)
				{
					yield return elfPairParser.Parse(line);
				}
			}
		}

		public static int GetNumberOfFullOverlappingPairs(IEnumerable<ElfPair> pairs)
		{
			return pairs
					.Where(p => p.HasFullOverlap())
					.Count();
		}

		public static int GetNumberOfAnyOverlappingPairs(IEnumerable<ElfPair> pairs)
		{
			return pairs
					.Where(p => p.HasAnyOverlap())
					.Count();
		}
	}
}
