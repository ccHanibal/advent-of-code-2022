using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
	public static class Day21
	{
		public interface IMonkeyOperation
		{
			long Evaluate();
		}

		public class ConstantOperation : IMonkeyOperation
		{
			private readonly long value;

			public ConstantOperation(long value)
			{
				this.value = value;
			}

			public long Evaluate()
			{
				return value;
			}
		}
		public class VariableOperation : IMonkeyOperation
		{
			public long Value { get; set; }

			public long Evaluate()
			{
				return Value;
			}
		}
		public class BinaryOperation : IMonkeyOperation
		{
			private readonly Func<long, long, long> combinationOperation;

			public Monkey Monkey1 { get; }
			public Monkey Monkey2 { get; }

			public BinaryOperation(Monkey monkey1, Monkey monkey2, Func<long, long, long> combinationOperation)
			{
				this.Monkey1 = monkey1;
				this.Monkey2 = monkey2;
				this.combinationOperation = combinationOperation;
			}

			public long Evaluate()
			{
				return combinationOperation(Monkey1.Evaluate(), Monkey2.Evaluate());
			}
		}


		public class Monkey
		{
			public string Name { get; }
			public IMonkeyOperation? Operation { get; set; }

			public Monkey(string name)
			{
				this.Name = name;
			}

			public long Evaluate()
			{
				return Operation?.Evaluate() ?? throw new InvalidOperationException("Operation is null.");
			}
		}

		private static readonly Regex monkeyRegex = new Regex(@"([a-z]+):\s*(.*)", RegexOptions.IgnoreCase);
		private static readonly Regex monkeyBinOpRegex = new Regex(@"([a-z]+)\s*([+\-*/])\s*([a-z]+)", RegexOptions.IgnoreCase);

		public static IDictionary<string, Monkey> ParseMonkeys(IEnumerable<string> data)
		{
			var monkeyDict = new ConcurrentDictionary<string, Monkey>(StringComparer.OrdinalIgnoreCase);

			foreach (var line in data)
			{
				var monkeyMatch = monkeyRegex.Match(line);

				var monkey = monkeyDict.GetOrAdd(monkeyMatch.Groups[1].Value, n => new Monkey(n));

				var opText = monkeyMatch.Groups[2].Value;
				if (char.IsDigit(opText[0]))
				{
					monkey.Operation = new ConstantOperation(int.Parse(opText));
				}
				else
				{
					var opMatch = monkeyBinOpRegex.Match(opText);
					var monkey1Name = opMatch.Groups[1].Value;
					var monkey2Name = opMatch.Groups[3].Value;

					var monkey1 = monkeyDict.GetOrAdd(monkey1Name, n => new Monkey(n));
					var monkey2 = monkeyDict.GetOrAdd(monkey2Name, n => new Monkey(n));

					monkey.Operation = opMatch.Groups[2].Value.ToLower() switch
					{
						"+" => new BinaryOperation(monkey1, monkey2, (v1, v2) => v1 + v2),
						"-" => new BinaryOperation(monkey1, monkey2, (v1, v2) => v1 - v2),
						"*" => new BinaryOperation(monkey1, monkey2, (v1, v2) => v1 * v2),
						"/" => new BinaryOperation(monkey1, monkey2, (v1, v2) => v1 / v2),
						_ => throw new InvalidOperationException($"Unknown operator found: {opMatch.Groups[2].Value}")
					};
				}
			}

			return monkeyDict;
		}

		public static long EvaluateMonkey(IDictionary<string, Monkey> monkeys, string monkeyName)
		{
			return EvaluateMonkey(monkeys[monkeyName]);
		}
		public static long EvaluateMonkey(Monkey monkey)
		{
			return monkey.Evaluate();
		}
		public static long FindValueOfHumanToHaveEqualValuesAtRoot(IDictionary<string, Monkey> monkeys, long minValue)
		{
			var root = monkeys["root"];
			var human = monkeys["humn"];
			human.Operation = null;

			var leftRootMonkey = ((BinaryOperation)root.Operation)!.Monkey1;
			var rightRootMonkey = ((BinaryOperation)root.Operation)!.Monkey2;

			long constSideValue;
			Monkey monkeyWithHuman;

			long? leftValue = EvaluateOrDefault(leftRootMonkey);
			if (leftValue is null)
			{
				constSideValue = EvaluateOrDefault(rightRootMonkey) ?? throw new InvalidOperationException("Human is in both sides.");
				monkeyWithHuman = leftRootMonkey;
			}
			else
			{
				constSideValue = leftValue.Value;
				monkeyWithHuman = rightRootMonkey;
			}

			var humanOp = new VariableOperation();
			human.Operation = humanOp;

			double x = minValue;
			double i = 1_000_000_000_000.0;
			Func<double, double, double> differ = (x, y) => x - y;

			humanOp.Value = (long)x;
			double sideWithHumanValue = monkeyWithHuman.Evaluate();
			if (differ(constSideValue, sideWithHumanValue) < 0)
			{
				// das erste Diff darf nicht negativ sein
				differ = (x, y) => y - x;
			}

			var xHist = new List<double>();

			while (true)
			{
				humanOp.Value = (long)x;
				sideWithHumanValue = monkeyWithHuman.Evaluate();

				double diff = differ(constSideValue, sideWithHumanValue);
				if ((long)diff == 0L)
					return (long)x;

				if (diff < 0)
				{
					if (i == 1.0)
						throw new InvalidOperationException();

					i = Math.Max(Math.Ceiling(i / 100.0), 1.0);
					if (xHist.Count > 0)
					{
						x = xHist.Max();
					}
					else
					{
						x++;
					}

					xHist = new List<double> { x };
					continue;
				}

				xHist.Add(x);
				x += i;
			}

			throw new InvalidOperationException();

			static long? EvaluateOrDefault(Monkey m)
			{
				try
				{
					return m.Evaluate();
				}
				catch
				{
					return null;
				}
			}
		}
	}
}