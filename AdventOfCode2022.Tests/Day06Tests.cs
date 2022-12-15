using static AdventOfCode2022.Day06;

namespace AdventOfCode2022.Tests
{
	using FakeItEasy;

	[TestFixture]
	public class Day06Tests
	{
		private readonly IDifferenceFinder diffFinder;
		private readonly StartOfPaketMarkerFinder markerFinder;
		private readonly StartOfPaketMarkerFinder messageFinder;

		public Day06Tests()
		{
			diffFinder = new DifferenceFinder();
			markerFinder = new StartOfPaketMarkerFinder(diffFinder, 4);
			messageFinder = new StartOfPaketMarkerFinder(diffFinder, 14);
		}

		[Test]
		[TestCase("abcd")]
		[TestCase("flpq")]
		[TestCase("jpqm")]
		public void Day06_DifferenceFinder_AreAllDifferent_True(string data)
		{
			Assert.That(diffFinder.AreAllDifferent(data), Is.True);
		}

		[Test]
		[TestCase("abca")]
		[TestCase("flfq")]
		[TestCase("mjqj")]
		public void Day06_DifferenceFinder_AreAllDifferent_False(string data)
		{
			Assert.That(diffFinder.AreAllDifferent(data), Is.False);
		}

		[Test]
		public void Day06_StartOfPaketMarkerFinder_WhenLastFourCharsAreDifferent_ReturnsTrue()
		{
			var differ = A.Fake<IDifferenceFinder>();
			A.CallTo(() => differ.AreAllDifferent(A<string>._))
				.Returns(false).NumberOfTimes(7)
				.Then.Returns(true);

			Assert.That(new StartOfPaketMarkerFinder(differ, 4).FindPositionOfFirstStartOfPaketMarker("jhdfgjhsdfg"), Is.Not.Null);

			A.CallTo(() => differ.AreAllDifferent(A<string>._)).MustHaveHappened(8, Times.Exactly);
		}

		[Test]
		public void Day06_StartOfPaketMarkerFinder_WhenNoLastFourCharsAreDifferent_ReturnsNull()
		{
			var differ = A.Fake<IDifferenceFinder>();
			A.CallTo(() => differ.AreAllDifferent(A<string>._)).Returns(false);

			Assert.That(new StartOfPaketMarkerFinder(differ, 4).FindPositionOfFirstStartOfPaketMarker("jhdfgjhsdfg"), Is.Null);
		}

		[Test]
		[TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
		[TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
		[TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6)]
		[TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
		[TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
		public void Day06_Sample_PositionOfPacketMarker_Is(string data, int position)
		{
			Assert.That(markerFinder.FindPositionOfFirstStartOfPaketMarker(data), Is.EqualTo(position));
		}

		[Test]
		public async Task Day06_Puzzle1_PositionOfPacketMarker_Is_1198()
		{
			var data = await File.ReadAllTextAsync("Day06.txt");

			Assert.That(markerFinder.FindPositionOfFirstStartOfPaketMarker(data), Is.EqualTo(1198));
		}

		[Test]
		[TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
		[TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
		[TestCase("nppdvjthqldpwncqszvftbrmjlhg", 23)]
		[TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
		[TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
		public void Day06_Sample_PositionOfMessageMarker_Is(string data, int position)
		{
			Assert.That(messageFinder.FindPositionOfFirstStartOfPaketMarker(data), Is.EqualTo(position));
		}

		[Test]
		public async Task Day06_Puzzle2_PositionOfMessageMarker_Is_3120()
		{
			var data = await File.ReadAllTextAsync("Day06.txt");

			Assert.That(messageFinder.FindPositionOfFirstStartOfPaketMarker(data), Is.EqualTo(3120));
		}
	}
}
