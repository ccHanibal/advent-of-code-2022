using static AdventOfCode2022.Day13;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day13Tests
	{
		private static readonly string SampleInput =
            """
			[1,1,3,1,1]
			[1,1,5,1,1]

			[[1],[2,3,4]]
			[[1],4]

			[9]
			[[8,7,6]]

			[[4,4],4,4]
			[[4,4],4,4,4]

			[7,7,7,7]
			[7,7,7]

			[]
			[3]

			[[[]]]
			[[]]

			[1,[2,[3,[4,[5,6,7]]]],8,9]
			[1,[2,[3,[4,[5,6,0]]]],8,9]
			""";

		[Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(15)]
        [TestCase(16)]
        [TestCase(18)]
        [TestCase(19)]
        [TestCase(21)]
        [TestCase(22)]
        public void Day13_ParsePacket_CanParseLine(int lineIndex)
		{
			var packet = ParsePacket(SampleInput.Split("\r\n")[lineIndex]);

			Assert.That(packet, Is.Not.Null);
		}

        [Test]
        public void Day13_ParsePackets_HasCountOfPairs_8()
        {
            var packets = ParsePackets(SampleInput.Split("\r\n")).ToList();

            Assert.That(packets, Has.Count.EqualTo(8));
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, true)]
        [TestCase(5, false)]
        [TestCase(6, true)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        public void Day13_Sample_Paír_X_IsInOrder(int pairIndex, bool inOrder)
        {
            var packets = ParsePackets(SampleInput.Split("\r\n")).ToList();
            var leftPacket = packets[pairIndex - 1].Left;
            var rightPacket = packets[pairIndex - 1].Right;

            Assert.That(AreInOrder(leftPacket, rightPacket), Is.EqualTo(inOrder));
        }

        [Test]
        public void Day13_Sample_SumOfIndices_InOrder_Is_13()
        {
            var packets = ParsePackets(SampleInput.Split("\r\n")).ToArray();
            var sum = GetIndicesSumOfOrderedPackets(packets);

            Assert.That(sum, Is.EqualTo(13));
        }
        
        [Test]
        public async Task Day13_Puzzle1_SumOfIndices_InOrder_Is_5529()
        {
            var packets = ParsePackets(await File.ReadAllLinesAsync("Day13.txt")).ToArray();
            var sum = GetIndicesSumOfOrderedPackets(packets);

            Assert.That(sum, Is.EqualTo(5_529));
        }

        [Test]
        public void Day13_Sample_SortPackets_DividersAreAt_10_And_14()
        {
            var dividerPacktets = new[]
            {
                ParsePacket("[[2]]"),
                ParsePacket("[[6]]")
            };

            var packets = ParsePackets(SampleInput.Split("\r\n"))
                            .SelectMany(p => new[] { p.Left, p.Right })
                            .Concat(dividerPacktets)
                            .ToArray();

            var orderedPackets = OrderPackets(packets);

            Assert.That(FindPacketIndex(orderedPackets, dividerPacktets[0]), Is.EqualTo(10));
            Assert.That(FindPacketIndex(orderedPackets, dividerPacktets[1]), Is.EqualTo(14));
        }

        [Test]
        public async Task Day13_Puzzle2_SortPackets_DividersAreAt_130_And_213()
        {
            var dividerPacktets = new[]
            {
                ParsePacket("[[2]]"),
                ParsePacket("[[6]]")
            };

            var packets = ParsePackets(await File.ReadAllLinesAsync("Day13.txt"))
                            .SelectMany(p => new[] { p.Left, p.Right })
                            .Concat(dividerPacktets)
                            .ToArray();

            var orderedPackets = OrderPackets(packets);

            Assert.That(FindPacketIndex(orderedPackets, dividerPacktets[0]), Is.EqualTo(130));
            Assert.That(FindPacketIndex(orderedPackets, dividerPacktets[1]), Is.EqualTo(213));
        }
    }
}
