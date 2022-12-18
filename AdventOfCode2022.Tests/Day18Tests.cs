using static AdventOfCode2022.Day18;

namespace AdventOfCode2022.Tests
{
    [TestFixture]
	public class Day18Tests
	{
		private static readonly string SimpleSampleInput =
            """
            1,1,1
            2,1,1
            """;

        private static readonly string SampleInput =
            """
            2,2,2
            1,2,2
            3,2,2
            2,1,2
            2,3,2
            2,2,1
            2,2,3
            2,2,4
            2,2,6
            1,2,5
            3,2,5
            2,1,5
            2,3,5
            """;

        [Test]
		public void Day18_SimpleSample_SurfaceArea_Is_10()
		{
			var cubes = ParsePoints(SimpleSampleInput.Split("\r\n")).ToHashSet();
			var surfaceArea = GetSurfaceArea(cubes, false);

			Assert.That(surfaceArea, Is.EqualTo(10));
		}

        [Test, Explicit]
        [TestCaseSource(nameof(AllSampleLineIndices))]
        public void Day18_Sample_Parser_IsCorrect_ForLine(int lineIdx)
        {
            var inputLines = SampleInput.Split("\r\n");

            var cubes = ParsePoints(inputLines);
            Assert.That($"{cubes[lineIdx].X},{cubes[lineIdx].Y},{cubes[lineIdx].Z}", Is.EqualTo(inputLines[lineIdx]));
        }
        public static IEnumerable<TestCaseData> AllSampleLineIndices
        {
            get
            {
                var inputLines = SampleInput.Split("\r\n");

                for (int a = 0; a < inputLines.Length; a++)
                {
                    yield return new TestCaseData(a);
                }
            }
        }

        [Test, Explicit]
        [TestCaseSource(nameof(AllPuzzle1LineIndices))]
        public void Day18_Puzzle1_Parser_IsCorrect_ForLine(int lineIdx)
        {
            var inputLines = File.ReadAllLines("Day18.txt");

            var cubes = ParsePoints(inputLines);
            Assert.That($"{cubes[lineIdx].X},{cubes[lineIdx].Y},{cubes[lineIdx].Z}", Is.EqualTo(inputLines[lineIdx]));
        }
        public static IEnumerable<TestCaseData> AllPuzzle1LineIndices
        {
            get
            {
                var inputLines = File.ReadAllLines("Day18.txt");

                for (int a = 0; a < inputLines.Length; a++)
                {
                    yield return new TestCaseData(a);
                }
            }
        }

        [Test]
        public void Day18_Sample_SurfaceArea_Is_64()
        {
            var cubes = ParsePoints(SampleInput.Split("\r\n")).ToHashSet();
            var surfaceArea = GetSurfaceArea(cubes, false);

            Assert.That(surfaceArea, Is.EqualTo(64));
        }

        [Test]
		public async Task Day18_Puzzle1_SurfaceArea_Is_4418()
		{
			var cubes = ParsePoints(await File.ReadAllLinesAsync("Day18.txt")).ToHashSet();
			var surfaceArea = GetSurfaceArea(cubes, false);

            Assert.That(surfaceArea, Is.EqualTo(4_418));
        }

        [Test]
        public void Day18_Sample_RealSurfaceArea_Is_58()
        {
            var cubes = ParsePoints(SampleInput.Split("\r\n")).ToHashSet();
            var surfaceArea = GetSurfaceArea(cubes, true);

            Assert.That(surfaceArea, Is.EqualTo(58));
        }

        [Test, Explicit]
        public async Task Day18_Puzzle1_RealSurfaceArea_Is_NotSolved()
        {
            var cubes = ParsePoints(await File.ReadAllLinesAsync("Day18.txt")).ToHashSet();
            var surfaceArea = GetSurfaceArea(cubes, true);

            Assert.That(surfaceArea, Is.EqualTo(-1));
        }
    }
}
