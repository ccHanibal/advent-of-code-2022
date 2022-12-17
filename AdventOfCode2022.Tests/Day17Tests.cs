using static AdventOfCode2022.Day17;

namespace AdventOfCode2022.Tests
{
    [TestFixture]
	public class Day17Tests
	{
		private static readonly string SampleInput = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

		[Test]
		public void Day17_Sample_HeightAfter_2022_Rocks_Is_3068()
		{
			var rocks = ParseMovement(SampleInput);
			var heightAfter2022Rocks = SimulateFallingRocks(rocks, 2_022L);

			Assert.That(heightAfter2022Rocks, Is.EqualTo(3_068));
		}

		[Test]
		public async Task Day17_Puzzle1_HeightAfter_2022_Rocks_Is_3090()
		{
			var movement = ParseMovement(await File.ReadAllTextAsync("Day17.txt"));
            var heightAfter2022Rocks = SimulateFallingRocks(movement, 2_022L);

            Assert.That(heightAfter2022Rocks, Is.EqualTo(3_090));
		}

        [Test, Explicit]
        public void Day17_Sample_HeightAfter__1_000_000_000_000__Rocks_Is__1_514_285_714_288()
        {
            var movement = ParseMovement(SampleInput);
            var heightAfterNRocks = SimulateFallingRocks(movement, 1_000_000_000_000L);

            Assert.That(heightAfterNRocks, Is.EqualTo(3_068));
        }

        [Test, Explicit]
        public async Task Day17_Puzzle2_HeightAfter__1_000_000_000_000__Rocks_Is__1_530_057_803_453()
        {
            var movement = ParseMovement(await File.ReadAllTextAsync("Day17.txt"));
            var heightAfter2022Rocks = SimulateFallingRocks(movement, 1_000_000_000_000L);

            Assert.That(heightAfter2022Rocks, Is.EqualTo(1_530_057_803_453));
        }
    }
}
