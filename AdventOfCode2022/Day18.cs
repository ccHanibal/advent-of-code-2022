using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode2022
{
    public static class Day18
    {
        [DebuggerDisplay("X: {X}, Y: {Y}, Z: {Z}")]
        public sealed class Point3D : IEquatable<Point3D>
        {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public Point3D(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public Point3D WithOffset(Point3D offset)
            {
                return new Point3D(X + offset.X, Y + offset.Y, Z + offset.Z);
            }
            public Point3D WithOffset(int offsetX, int offsetY, int offsetZ)
            {
                return new Point3D(X + offsetX, Y + offsetY, Z + offsetZ);
            }

            public override bool Equals(object? obj)
            {
                return Equals(obj as Point3D);
            }
            public override int GetHashCode()
            {
                return X ^ Y ^ Z;
            }

            public bool Equals(Point3D? other)
            {
                if (ReferenceEquals(this, other))
                    return true;

                if (other == null)
                    return false;

                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            }
        }
        public static List<Point3D> ParsePoints(IEnumerable<string> data)
        {
            return data.Select(ParsePoint3D)
                        .ToList();

            static Point3D ParsePoint3D(string line)
            {
                var parts = line.Split(',');
                return new Point3D(
                            int.Parse(parts[0]),
                            int.Parse(parts[1]),
                            int.Parse(parts[2]));
            }
        }

        public static int GetSurfaceArea(HashSet<Point3D> points, bool includeEnclosedCubeSides)
        {
            int surfaceArea = 0;

            var offsets = new Point3D[]
            {
                new Point3D(0, 0, 1),
                new Point3D(0, 0, -1),
                new Point3D(0, 1, 0),
                new Point3D(0, -1, 0),
                new Point3D(1, 0, 0),
                new Point3D(-1, 0, 0)
            };

            foreach (var point in points)
            {
                foreach (var offset in offsets)
                {
                    var p = point.WithOffset(offset);

                    var cubeFilled = points.Contains(p);
                    if (!cubeFilled && (!includeEnclosedCubeSides || !IsEnclosedByCubes(p)))
                        surfaceArea++;
                }
            }

            bool IsEnclosedByCubes(Point3D testCube)
            {
                foreach (var offset in offsets)
                {
                    var p = testCube.WithOffset(offset);

                    var cubeFilled = points.Contains(p);
                    if (!cubeFilled)
                        return false;
                }

                return true;
            }

            return surfaceArea;
        }
    }
}