using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
	public static class Day15
	{
		private static readonly Regex sensorRegex = new(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

		public record Sensor(Point Location, Point NeareastBeaconLocation);

		public static Sensor ParseSensor(string data)
		{
			var match = sensorRegex.Match(data);
			return new Sensor(
						new Point(
							int.Parse(match.Groups[1].Value),
							int.Parse(match.Groups[2].Value)),
						new Point(
							int.Parse(match.Groups[3].Value),
							int.Parse(match.Groups[4].Value)));
		}

		public static IEnumerable<Sensor> ParseSensors(IEnumerable<string> data)
		{
			return data.Select(ParseSensor)
						.ToList();
		}

		public static Tunnels CreateTunnels(IEnumerable<Sensor> sensors)
		{
			var tunnels = new Tunnels(sensors);
			return tunnels;
		}

		public class Tunnels
		{
			private sealed record SensorDistance(Sensor Sensor, int ManhattenDistanceNearestBeacon)
			{
				public Rectangle Rectangle
				{
					get
					{
						return new Rectangle(
									Sensor.Location.X - ManhattenDistanceNearestBeacon,
									Sensor.Location.Y - ManhattenDistanceNearestBeacon,
									2 * ManhattenDistanceNearestBeacon + 1,
									2 * ManhattenDistanceNearestBeacon + 1);
					}
				}

				public IEnumerable<Point> GetEdgePointsOutside()
				{
					var points = new List<Point>();

					for (int posX = -ManhattenDistanceNearestBeacon - 1; posX <= ManhattenDistanceNearestBeacon + 1; posX++)
					{
						var rangeY = ManhattenDistanceNearestBeacon + 1 - Math.Abs(posX);

						points.Add(new Point(Sensor.Location.X + posX, Sensor.Location.Y - rangeY));
						if (rangeY != 0)
							points.Add(new Point(Sensor.Location.X + posX, Sensor.Location.Y + rangeY));
					}

					return points;
				}
			}

			private static int CalcManhattenDistance(Point start, Point end)
			{
				int diffX = Math.Abs(start.X - end.X);
				int diffY = Math.Abs(start.Y - end.Y);
				return diffX + diffY;
			}

			private readonly IEnumerable<Point> allBeaconPoints;
			private readonly int minX;
			private readonly int maxX;
			private readonly IEnumerable<SensorDistance> sensors;

			public Tunnels(IEnumerable<Sensor> sensors)
			{
				this.sensors = sensors.Select(s => new SensorDistance(s, CalcManhattenDistance(s.Location, s.NeareastBeaconLocation)))
										.ToList();

				var rects = this.sensors.Select(s => s.Rectangle)
										.ToList();

				minX = rects.Select(p => p.Left).Min();
				maxX = rects.Select(p => p.Right).Max();

				allBeaconPoints = sensors.Select(s => s.NeareastBeaconLocation)
											.ToList();
			}

			public int CountNoBeaconCells(int rowIndex)
			{
				var cellLocation = new Point(minX, rowIndex);

				int count = 0;

				for (; cellLocation.X <= maxX; cellLocation.Offset(1, 0))
				{
					if (IsNearerToAnyThanItsNearestBeacon(cellLocation, false))
						count++;
				}

				return count;
			}
			public long GetUndetectedBeaconInRange(int maxXY)
			{
				var rect = new Rectangle(0, 0, maxXY, maxXY);

				foreach (var sensor in sensors)
				{
					var pointsOnEdge = sensor.GetEdgePointsOutside();
					var pointsOnEdgeInRange = pointsOnEdge.Where(rect.Contains).ToList();

					foreach (var point in pointsOnEdgeInRange)
					{
						if (!IsNearerToAnyThanItsNearestBeacon(point, true))
							return point.X * 4_000_000L + point.Y;
					}
				}

				throw new InvalidDataException();
			}

			public bool IsNearerToAnyThanItsNearestBeacon(Point cell, bool beaconResult)
			{
				if (allBeaconPoints.Contains(cell))
					return beaconResult;

				foreach (var sensor in sensors)
				{
					var manHattDistToCell = CalcManhattenDistance(sensor.Sensor.Location, cell);
					if (manHattDistToCell <= sensor.ManhattenDistanceNearestBeacon)
						return true;
				}

				return false;
			}
		}

		public static IEnumerable<Point> GetDiffs(int radius)
		{
			for (int posX = -radius; posX <= radius; posX++)
			{
				var rangeY = radius - Math.Abs(posX);

				for (int posY = -rangeY; posY <= rangeY; posY++)
				{
					yield return new Point(posX, posY);
				}
			}
		}
	}
}
