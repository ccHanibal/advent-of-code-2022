using MoreLinq;
using System.Collections;
using System.Text.Json;

namespace AdventOfCode2022
{
	public static class Day13
	{
		public static object ParsePacket(string data)
		{
			var packet = JsonSerializer.Deserialize<JsonElement>(data);
			return ParseValue(packet);

			static object ParseValue(JsonElement ele)
			{
				if (ele.ValueKind == JsonValueKind.Number)
					return ele.GetInt32();

				if (ele.ValueKind == JsonValueKind.Array)
				{
					return ele.Deserialize<List<JsonElement>>()!
								.Select(ParseValue)
								.ToArray();
				}

				throw new InvalidDataException();
			}
		}
		public static IEnumerable<(object Left, object Right)> ParsePackets(string[] data)
		{
			for (int a = 0; a < data.Length; a += 3)
			{
				var left = ParsePacket(data[a]);
				var right = ParsePacket(data[a + 1]);

				yield return (left, right);
            }
		}

		public static bool? AreInOrder(object left, object right)
		{
			return (left, right) switch
			{
				(int leftNum, int rightNum) => leftNum == rightNum ? null : leftNum < rightNum,
				(int leftNum, object[]) => AreInOrder(new object[] { leftNum }, right),
				(object[], int rightNum) => AreInOrder(left, new object[] { rightNum }),
				(object[] leftArray, object[] rightArray) => AreInOrderArray(leftArray, rightArray),
				(_, _) => throw new InvalidDataException()
			};
        }
		private static bool? AreInOrderArray(object[] left, object[] right)
		{
			int index = 0;

			for (; index < left.Length && index < right.Length; index++)
			{
				var areInOrder = AreInOrder(left[index], right[index]);
				if (areInOrder == true)
					return true;

				if (areInOrder == false)
					return false;
			}

			if (left.Length < right.Length)
				return true;

			if (left.Length > right.Length)
				return false;

			return null;
        }

		public static int GetIndicesSumOfOrderedPackets((object Left, object Right)[] packets)
		{
			int sumOfOrderedPairIndicesBase1 = 0;

			for (int a = 0; a < packets.Length; a++)
			{
				if (AreInOrder(packets[a].Left, packets[a].Right) == true)
					sumOfOrderedPairIndicesBase1 += a + 1;
			}

			return sumOfOrderedPairIndicesBase1;
		}

		public static object[] OrderPackets(object[] packets)
		{
			return packets.Order(new PacketComparator())
							.ToArray();
		}

        public static int FindPacketIndex(object[] packets, object searchPacket)
        {
            return packets
					.Index()
					.First(kvp => AreInOrder(kvp.Value, searchPacket) == null)
					.Key + 1;
        }

        private sealed class PacketComparator : IComparer<object>
        {
            public int Compare(object? x, object? y)
            {
				return AreInOrder(x, y) switch
				{
					true => -1,
					null => 0,
					false => 1,
				};
            }
        }
    }
}
