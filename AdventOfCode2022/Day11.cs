using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
	public static class Day11
	{
		public interface IItemOperation
		{
            long IncreaseWorryLevel(long level);
		}

        public class AddToWorryLevel : IItemOperation
        {
            private readonly long @operator;

            public AddToWorryLevel(long @operator)
            {
                this.@operator = @operator;
            }

            public long IncreaseWorryLevel(long level)
            {
                return level + @operator;
            }
        }

        public class MultiplyWorryLevel : IItemOperation
        {
            private readonly long @operator;

            public MultiplyWorryLevel(long @operator)
            {
                this.@operator = @operator;
            }

            public long IncreaseWorryLevel(long level)
            {
                return level * @operator;
            }
        }

        public class SquareWorryLevel : IItemOperation
        {
            public long IncreaseWorryLevel(long level)
            {
                return level * level;
            }
        }

        public interface IDecision
		{
			bool Test(long level);
		}

		public class DivisibleByDesision : IDecision
		{
            private readonly int divisor;

            public DivisibleByDesision(int divisor)
			{
                this.divisor = divisor;
            }

            public bool Test(long level)
            {
                return level % divisor == 0;
            }
        }

		public class Monkey
		{
            public static long Modulus = 1;

            private readonly IDecision decision;
            private readonly int falseMonkeyTargetIndex;
            private readonly IItemOperation itemOperation;
            private readonly int trueMonkeyTargetIndex;

            private readonly Queue<long> items = new();

            public long InspectionCount { get; private set; }

            public Monkey(IEnumerable<long> items, IItemOperation itemOperation, IDecision decision, int trueMonkeyTargetIndex, int falseMonkeyTargetIndex)
			{
                this.decision = decision;
                this.falseMonkeyTargetIndex = falseMonkeyTargetIndex;
                this.itemOperation = itemOperation;
                this.trueMonkeyTargetIndex = trueMonkeyTargetIndex;

                foreach (var item in items)
                {
                    this.items.Enqueue(item);
                }
            }

            public void CatchItem(long item)
			{
				items.Enqueue(item);
			}
            public bool HasItems()
            {
                return items.Any();
            }
			public void ThrowNextItem(Monkey[] monkeys, bool worryLevelDescreases)
			{
                InspectionCount++;

                long itemWorryLevel = items.Dequeue();
                long increasedItemWorryLevel = itemOperation.IncreaseWorryLevel(itemWorryLevel);
                long decreasedWorryLevel = worryLevelDescreases
                                                ? increasedItemWorryLevel / 3
                                                : increasedItemWorryLevel;

                decreasedWorryLevel = decreasedWorryLevel % Modulus;

                if (decision.Test(decreasedWorryLevel))
				{
					monkeys[trueMonkeyTargetIndex].CatchItem(decreasedWorryLevel);
				}
				else
                {
                    monkeys[falseMonkeyTargetIndex].CatchItem(decreasedWorryLevel);
                }
            }
		}

        public static Monkey[] ParseMonkeys(string[] data)
        {
            var operationRegex = new Regex(@"(old)\s*([+*])\s*(old|\d+)");

            var monkeys = new List<Monkey>(8);
            long modulus = 1;

            for (int monkeyIdx = 0; monkeyIdx * 7 < data.Length; monkeyIdx++)
            {
                var items = data[(monkeyIdx * 7) + 1]
                                .Substring("  Starting items: ".Length)
                                .Split(',')
                                .Select(p => long.Parse(p.Trim()))
                                .ToList();

                var operationText = data[(monkeyIdx * 7) + 2]
                                    .Substring("  Operation: new = ".Length);
                var operationMatch = operationRegex.Match(operationText);
                if (!operationMatch.Success)
                    throw new InvalidOperationException($"Opertaion {operationText} failed to match.");

                IItemOperation operation = operationMatch.Groups[2].Value switch
                {
                    "+" => new AddToWorryLevel(long.Parse(operationMatch.Groups[3].Value)),
                    "*" => operationMatch.Groups[3].Value == "old"
                                ? new SquareWorryLevel()
                                : new MultiplyWorryLevel(long.Parse(operationMatch.Groups[3].Value)),
                    _ => throw new InvalidOperationException("Opertion unkonwn.")
                };

                int decisionDivisor = int.Parse(data[(monkeyIdx * 7) + 3].Substring("  Test: divisible by ".Length));
                var decision = new DivisibleByDesision(decisionDivisor);
                var trueIndex = int.Parse(data[(monkeyIdx * 7) + 4].Last().ToString());
                var falseIndex = int.Parse(data[(monkeyIdx * 7) + 5].Last().ToString());

                monkeys.Add(new Monkey(items, operation, decision, trueIndex, falseIndex));
                modulus *= decisionDivisor;
            }

            Monkey.Modulus = modulus;

            return monkeys.ToArray();
        }

        public static void SimulateRounds(int rounds, Monkey[] monkeys, bool worryLevelDescreases)
        {
            /*
            == After round 1 ==
            Monkey 0 inspected items 2 times.
            Monkey 1 inspected items 4 times.
            Monkey 2 inspected items 3 times.
            Monkey 3 inspected items 6 times.
             */

            SimulateRound(monkeys, worryLevelDescreases);
            Print(1);

            for (int r = 2; r <= rounds; r++)
            {
                SimulateRound(monkeys, worryLevelDescreases);
            
                if (r == 20 || r % 1000 == 0)
                    Print(r);
            }

            void Print(int round)
            {
                Console.WriteLine("== After round {0} ==", round);
                for (int a = 0; a < monkeys.Length; a++)
                {
                    Console.WriteLine("Monkey {0} inspected items {1} times.", a, monkeys[a].InspectionCount);
                }

                Console.WriteLine();
            }
        }
        public static void SimulateRound(Monkey[] monkeys, bool worryLevelDescreases)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.HasItems())
                    monkey.ThrowNextItem(monkeys, worryLevelDescreases);
            }
        }

        public static long GetProductOfMostActiveMonkeys(IEnumerable<Monkey> monkeys)
        {
            return monkeys
                    .OrderByDescending(m => m.InspectionCount)
                    .Take(2)
                    .Aggregate(1, (long product, Monkey m) => product * m.InspectionCount);
        }
	}
}
