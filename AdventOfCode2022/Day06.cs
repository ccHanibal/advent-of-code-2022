namespace AdventOfCode2022
{
	public static class Day06
	{
		public interface IDifferenceFinder
		{
			bool AreAllDifferent(string chars);
		}

		public class DifferenceFinder : IDifferenceFinder
		{
			public bool AreAllDifferent(string chars)
			{
				var knownChars = new HashSet<char>(chars.Length);

				foreach (var character in chars)
				{
					if (!knownChars.Add(character))
						return false;
				}

				return true;
			}
		}

		public class StartOfPaketMarkerFinder
		{
			private readonly IDifferenceFinder differenceFinder;

			private readonly int lengthOfMarker;

			public StartOfPaketMarkerFinder(IDifferenceFinder differenceFinder, int lengthOfMarker)
			{
				this.differenceFinder = differenceFinder;

				this.lengthOfMarker = lengthOfMarker;
			}

			public int? FindPositionOfFirstStartOfPaketMarker(string data)
			{
				for (int a = lengthOfMarker -1; a < data.Length; a++)
				{
					if (differenceFinder.AreAllDifferent(data.Substring(a - lengthOfMarker + 1, lengthOfMarker)))
						return a + 1;
				}

				return null;
			}
		}
	}
}
