namespace AdventOfCode2022
{
	public static class Day20
	{
		public record Data(int Index, long Value);

		public static List<Data> ParseInput(IEnumerable<string> data, long m)
		{
			return data.Select((v, idx) => new Data(idx, long.Parse(v) * m)).ToList();
		}

		public static long GetSum(List<Data> values)
		{
			return Enumerate(0, 1000, 2000, 3000)
						.Sum(i => i);

			IEnumerable<long> Enumerate(long targetValue, int index1, int index2, int index3)
			{
				int targetIndex = values.FindIndex(v => v.Value == targetValue);

				yield return values[(targetIndex + index1) % values.Count].Value;
				yield return values[(targetIndex + index2) % values.Count].Value;
				yield return values[(targetIndex + index3) % values.Count].Value;
			}
		}

		public static List<Data> MixValues(List<Data> values, int times)
		{
			for (int a = 0; a < times; a++)
			{
				MixValues(values);
			}

			return values;
		}
		public static List<Data> MixValues(List<Data> values)
		{
			var mod = values.Count - 1;

			for (int idx = 0; idx < values.Count; idx++)
			{
				var index = values.FindIndex(v => v.Index == idx);
				var num = values[index];
				if (num.Value == 0)
					continue;

				var targetIndex = (index + num.Value) % mod;
				while (targetIndex < 0)
					targetIndex += mod;

				values.RemoveAt(index);
				values.Insert((int)targetIndex, num);
			}

			return values;
		}
	}
}