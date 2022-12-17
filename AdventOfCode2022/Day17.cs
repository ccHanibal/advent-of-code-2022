using MoreLinq;

namespace AdventOfCode2022
{
    public static class Day17
    {
        public static long[] ParseMovement(string data)
        {
            return data.Select(MapToMovement)
                        .ToArray();

            static long MapToMovement(char c)
            {
                return c switch
                {
                    '>' => 1,
                    '<' => -1,
                    _ => throw new InvalidDataException()
                };
            }
        }

        public static long SimulateFallingRocks(long[] horizontalMovement, long numRocks)
        {
            var rockGen = new RockGenerator();
            var otherRocks = new HashSet<PointLong>(40000);

            long height = -1;
            int moveIdx = 0;



            for (long rocksFallen = 0; rocksFallen < numRocks; rocksFallen++)
            {
                AddRock();
            }

            return height + 1;

            void AddRock()
            {
                var rock = rockGen.CreateNextRock(height);

                while (true)
                {
                    rock.MoveHorizontally(horizontalMovement[moveIdx], otherRocks);
                    moveIdx = (moveIdx + 1) % horizontalMovement.Length;

                    if (!rock.MoveDown(otherRocks))
                    {
                        var topOfRock = rock.GetTopOfRock();
                        if (height < topOfRock)
                            height = topOfRock;

                        rock.Cells.ForEach(c => otherRocks.Add(c));

                        otherRocks.RemoveWhere(p => p.Y < topOfRock - 100);

                        foreach (var rowIdx in rock.Cells.Select(c => c.Y).Distinct().OrderByDescending(y => y))
                        {
                            if (otherRocks.Count(p => p.Y == rowIdx) == 7)
                            {
                                otherRocks.RemoveWhere(p => p.Y < rowIdx);
                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }

        public class RockGenerator
        {
            private static readonly PointLong[] rockHorizontalLine = new[]
            {
                new PointLong(2, 0),
                new PointLong(3, 0),
                new PointLong(4, 0),
                new PointLong(5, 0)
            };
            private static readonly PointLong[] rockCross = new[]
            {
                new PointLong(3, 2),
                new PointLong(2, 1),
                new PointLong(3, 1),
                new PointLong(4, 1),
                new PointLong(3, 0)
            };
            private static readonly PointLong[] rockMirroredL = new[]
            {
                new PointLong(4, 2),
                new PointLong(4, 1),
                new PointLong(2, 0),
                new PointLong(3, 0),
                new PointLong(4, 0)
            };
            private static readonly PointLong[] rockVerticalLine = new[]
            {
                new PointLong(2, 3),
                new PointLong(2, 2),
                new PointLong(2, 1),
                new PointLong(2, 0)
            };
            private static readonly PointLong[] rockBlock = new[]
            {
                new PointLong(2, 1),
                new PointLong(3, 1),
                new PointLong(2, 0),
                new PointLong(3, 0)
            };

            private static IEnumerable<PointLong[]> CreateInfiniteRockSequence()
            {
                yield return rockHorizontalLine;
                yield return rockCross;
                yield return rockMirroredL;
                yield return rockVerticalLine;
                yield return rockBlock;
            }

            private readonly List<PointLong[]> rocks = CreateInfiniteRockSequence().ToList();

            private int nextIndex = 0;

            public Rock CreateNextRock(long highestBlockedRow)
            {
                var rock = rocks[nextIndex];
                nextIndex = (nextIndex + 1) % rocks.Count;

                return new Rock(rock.Select(p => new PointLong(p.X, p.Y + 4 + highestBlockedRow)).ToArray());
            }
        }

        public sealed class Rock
        {
            private PointLong[] cells;

            public IEnumerable<PointLong> Cells => cells;

            public Rock(PointLong[] cells)
            {
                this.cells = cells;
            }

            public long GetTopOfRock()
            {
                return cells.Select(c => c.Y)
                            .Max();
            }

            public void MoveHorizontally(long step, HashSet<PointLong> otherRocks)
            {
                var movedCells = new PointLong[cells.Length];

                for (int cIdx = 0; cIdx < cells.Length; cIdx++)
                {
                    var cell = cells[cIdx];

                    var newX = cell.X + step;
                    if (newX < 0 || newX > 6)
                    {
                        // movement into a wall (left/right)
                        return;
                    }

                    movedCells[cIdx] = new PointLong(newX, cell.Y);
                }

                PerformMoveCollisionCheck(movedCells, otherRocks);
            }
            public bool MoveDown(HashSet<PointLong> otherRocks)
            {
                var movedCells = new PointLong[cells.Length];

                for (int cIdx = 0; cIdx < cells.Length; cIdx++)
                {
                    var cell = cells[cIdx];

                    var newY = cell.Y - 1;
                    if (newY < 0)
                    {
                        // movement into a wall (bottom)
                        return false;
                    }

                    movedCells[cIdx] = new PointLong(cell.X, newY);
                }

                return PerformMoveCollisionCheck(movedCells, otherRocks);
            }

            private bool PerformMoveCollisionCheck(PointLong[] movedCells, HashSet<PointLong> otherRocks)
            {
                for (int i = 0; i < movedCells.Length; i++)
                {
                    if (otherRocks.Contains(movedCells[i]))
                        return false;
                }

                this.cells = movedCells;
                return true;
            }
        }

        public sealed record PointLong(long X, long Y);
    }
}