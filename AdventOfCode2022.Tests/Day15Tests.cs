using System.Drawing;
using static AdventOfCode2022.Day15;

namespace AdventOfCode2022.Tests
{
	[TestFixture]
	public class Day15Tests
	{
		private static readonly string SampleInput =
			"""
			Sensor at x=2, y=18: closest beacon is at x=-2, y=15
			Sensor at x=9, y=16: closest beacon is at x=10, y=16
			Sensor at x=13, y=2: closest beacon is at x=15, y=3
			Sensor at x=12, y=14: closest beacon is at x=10, y=16
			Sensor at x=10, y=20: closest beacon is at x=10, y=16
			Sensor at x=14, y=17: closest beacon is at x=10, y=16
			Sensor at x=8, y=7: closest beacon is at x=2, y=10
			Sensor at x=2, y=0: closest beacon is at x=2, y=10
			Sensor at x=0, y=11: closest beacon is at x=2, y=10
			Sensor at x=20, y=14: closest beacon is at x=25, y=17
			Sensor at x=17, y=20: closest beacon is at x=21, y=22
			Sensor at x=16, y=7: closest beacon is at x=15, y=3
			Sensor at x=14, y=3: closest beacon is at x=15, y=3
			Sensor at x=20, y=1: closest beacon is at x=15, y=3
			""";

		[Test]
		[TestCase(0, 2, 18)]
		[TestCase(1, 9, 16)]
		[TestCase(2, 13, 2)]
		public void Day15_Sample_ParseSensor_HasLocation(int lineIndex, int x, int y)
		{
			var sensor = ParseSensor(SampleInput.Split("\r\n")[lineIndex]);

			Assert.That(sensor.Location.X, Is.EqualTo(x));
			Assert.That(sensor.Location.Y, Is.EqualTo(y));
		}

		[Test]
		[TestCase(0, -2, 15)]
		[TestCase(1, 10, 16)]
		[TestCase(2, 15, 3)]
		public void Day15_Sample_ParseSensor_HasBeaconLocation(int lineIndex, int x, int y)
		{
			var sensor = ParseSensor(SampleInput.Split("\r\n")[lineIndex]);

			Assert.That(sensor.NeareastBeaconLocation.X, Is.EqualTo(x));
			Assert.That(sensor.NeareastBeaconLocation.Y, Is.EqualTo(y));
		}

		[Test]
		public void Day15_Sample_ParseSensors_SensorCount_14()
		{
			var sensors = ParseSensors(SampleInput.Split("\r\n")).ToList();

			Assert.That(sensors, Has.Count.EqualTo(14));
		}

		[Test]
		[TestCaseSource(nameof(AllSampleCells))]
		public void Day15_Sample_CellAt_Is_PossibleBeaconCell(int colIdx, int rowIdx, bool isPossible)
		{
			var sensors = ParseSensors(SampleInput.Split("\r\n"));
			var tunnels = CreateTunnels(sensors);
			var isPossibleBeacon = tunnels.IsNearerToAnyThanItsNearestBeacon(new Point(colIdx, rowIdx), false);

			Assert.That(isPossibleBeacon, Is.EqualTo(isPossible));
		}
		public static IEnumerable<TestCaseData> AllSampleCells
		{
			get
			{
				var points = Enumerable.Range(0, 22)
								.SelectMany(y => Enumerable.Range(-2, 28)
															.Select(x => new Point(x, y)))
								.ToList();

				var falseyPoints = new[]
				{
					new Point(15, 3),
					new Point(25, 4),
					new Point(24, 5),
					new Point(25, 5),
					new Point(23, 6),
					new Point(24, 6),
					new Point(25, 6),
					new Point(-2, 7),
					new Point(22, 7),
					new Point(23, 7),
					new Point(24, 7),
					new Point(25, 7),
					new Point(-2, 8),
					new Point(-1, 8),
					new Point(23, 8),
					new Point(24, 8),
					new Point(25, 8),
					new Point(-2, 9),
					new Point(24, 9),
					new Point(25, 9),
					new Point(2, 10),
					new Point(25, 10),
					new Point(14, 11),
					// nix in 12
					new Point(-2, 13),
					new Point(-2, 14),
					new Point(-2, 15),
					new Point(10, 16),
					new Point(25, 17),
					new Point(25, 18),
					new Point(24, 19),
					new Point(25, 19),
					new Point(24, 20),
					new Point(25, 20),
					new Point(23, 21),
					new Point(24, 21),
					new Point(25, 21),
					new Point(-2, 22),
					new Point(6, 22),
					new Point(7, 22),
					new Point(21, 22),
					new Point(22, 22),
					new Point(23, 22),
					new Point(24, 22),
					new Point(25, 22),
				};

				// line 0
				foreach (var p in points)
				{
					yield return new TestCaseData(p.X, p.Y, !falseyPoints.Contains(p));
				}
			}
		}

		[Test]
		public void Day15_Sample_CountNoBeaconCells_InLine_10_Is_26()
		{
			var sensors = ParseSensors(SampleInput.Split("\r\n"));
			var tunnels = CreateTunnels(sensors);
			var numNoBeasonCells = tunnels.CountNoBeaconCells(10);

			Assert.That(numNoBeasonCells, Is.EqualTo(26));
		}

		[Test, Explicit]
		public async Task Day15_Puzzle1_CountNoBeaconCells_InLine__2_000_000__Is__5_144_286()
		{
			var sensors = ParseSensors(await File.ReadAllLinesAsync("Day15.txt"));
			var tunnels = CreateTunnels(sensors);
			var numNoBeasonCells = tunnels.CountNoBeaconCells(2_000_000);

			Assert.That(numNoBeasonCells, Is.EqualTo(5_144_286));
		}

		[Test]
		public void Day15_Sample_UndetectedBeacon_Frequency_In_Range_Is__56_000_011()
		{
			var sensors = ParseSensors(SampleInput.Split("\r\n"));
			var tunnels = CreateTunnels(sensors);
			var tuningFrequencyOfUndetectedBeacon = tunnels.GetUndetectedBeaconInRange(20);

			Assert.That(tuningFrequencyOfUndetectedBeacon, Is.EqualTo(56_000_011L));
		}

		[Test, Explicit]
		public async Task Day15_Puzzle2_UndetectedBeacon_Frequency_In_Range_Is__10_229_191_267_339()
		{
			var sensors = ParseSensors(await File.ReadAllLinesAsync("Day15.txt"));
			var tunnels = CreateTunnels(sensors);
			var tuningFrequencyOfUndetectedBeacon = tunnels.GetUndetectedBeaconInRange(4_000_000);

			Assert.That(tuningFrequencyOfUndetectedBeacon, Is.EqualTo(10_229_191_267_339L));
		}
	}
}
