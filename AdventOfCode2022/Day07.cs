using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
	public static class Day07
	{
		public record ElfFile(string FilePath, int Size);

		public interface IListContentOutputLineParser
		{
			ElfFile? Parse(string currentDirectory, string output);
		}

		public class ListContentOutputLineParser : IListContentOutputLineParser
		{
			private static readonly Regex fileRexex = new Regex(@"(\d+)\s+([^\s]+)");

			public ElfFile? Parse(string currentDirectory, string output)
			{
				var match = fileRexex.Match(output);
				if (!match.Success)
					return null;

				return new ElfFile(Path.Combine(currentDirectory, match.Groups[2].Value).Replace("\\", "/"), int.Parse(match.Groups[1].Value));
			}
		}

		public interface ICommandParser
		{
			IConsoleCommand Parse(string line);
		}

		public interface IConsoleCommand
		{
			string Execute(string currentDirectory);
			IEnumerable<ElfFile> ParseOutput(string currentDirectory, IEnumerable<string> output);
		}

		public class ChangeDirectoryCommand : IConsoleCommand
		{
			private readonly string dirParam;

			public ChangeDirectoryCommand(string dirParam)
			{
				this.dirParam = dirParam;
			}

			public string Execute(string currentDirectory)
			{
				if (dirParam == "/")
					return "/";

				if (dirParam == "..")
					return Path.GetDirectoryName(currentDirectory)?.Replace("\\", "/") ?? "/";

				return Path.Combine(currentDirectory, dirParam).Replace("\\", "/");
			}

			public IEnumerable<ElfFile> ParseOutput(string currentDirectory, IEnumerable<string> output)
			{
				yield break;
			}
		}

		public class ListContentCommand : IConsoleCommand
		{
			private readonly IListContentOutputLineParser singleLineParser;

			public ListContentCommand(IListContentOutputLineParser singleLineParser)
			{
				this.singleLineParser = singleLineParser;
			}

			public string Execute(string currentDirectory)
			{
				return currentDirectory;
			}

			public IEnumerable<ElfFile> ParseOutput(string currentDirectory, IEnumerable<string> output)
			{
				return output.Select(l => singleLineParser.Parse(currentDirectory, l))
								.Where(f => f is not null)
								.ToList()!;
			}
		}

		public interface ICommandFactory
		{
			IConsoleCommand CreateChangeDirectoryCommand(string param);
			IConsoleCommand CreateListContentCommand();
		}

		public class CommandFactory : ICommandFactory
		{
			public IConsoleCommand CreateChangeDirectoryCommand(string param)
			{
				return new ChangeDirectoryCommand(param);
			}
			public IConsoleCommand CreateListContentCommand()
			{
				return new ListContentCommand(new ListContentOutputLineParser());
			}
		}

		public class ConsoleOutputParser
		{
			private static readonly Regex cmdRegex = new Regex(@"\$\s+(cd|ls)(\s+([^\s]+))?");

			private readonly ICommandFactory commandFactory;

			public ConsoleOutputParser(ICommandFactory commandFactory)
			{
				this.commandFactory = commandFactory;
			}

			public IEnumerable<ElfFile> Parse(IEnumerable<string> output)
			{
				string currentDirectory = "/";
				var allFiles = new List<ElfFile>();

				IConsoleCommand? cmd = null;
				var cmdOutput = new List<string>();

				foreach (var line in output)
				{
					var match = cmdRegex.Match(line);
					if (match.Success)
					{
						if (cmd is not null)
						{
							ExceuteCommand();
						}

						cmd = match.Groups[1].Value switch
						{
							"cd" => commandFactory.CreateChangeDirectoryCommand(match.Groups[3].Value),
							"ls" => commandFactory.CreateListContentCommand(),
							_ => throw new InvalidProgramException()
						};
					}

					cmdOutput.Add(line);
				}

				ExceuteCommand();

				return allFiles;

				void ExceuteCommand()
				{
					currentDirectory = cmd!.Execute(currentDirectory);

					var files = cmd.ParseOutput(currentDirectory, cmdOutput);
					allFiles.AddRange(files);

					cmdOutput.Clear();
				}
			}
		}

		public static IEnumerable<string> GetDirectories(IEnumerable<ElfFile> files)
		{
			var dirs = new HashSet<string>
			{
				"/"
			};

			foreach (var file in files)
			{
				var dir = GetParentDir(file.FilePath);

				while (dir!.Length > 1)
				{
					dirs.Add(dir);
					dir = GetParentDir(dir);
				}
			}

			return dirs;

			static string GetParentDir(string dir)
			{
				return Path.GetDirectoryName(dir)?.Replace("\\", "/") ?? "/";
			}
		}

		public static int GetSizeOfDirectory(string path, IEnumerable<ElfFile> files)
		{
			if (!path.EndsWith("/"))
				path += "/";

			return files.Where(f => f.FilePath.StartsWith(path))
						.Sum(f => f.Size);
		}

		public static IEnumerable<ElfFile> GetSizesOfAllDirectories(IEnumerable<ElfFile> files)
		{
			var dirs = new List<ElfFile>();

			foreach (var dir in GetDirectories(files))
			{
				dirs.Add(new ElfFile(dir, GetSizeOfDirectory(dir, files)));
			}

			return dirs;
		}

		public static int GetSumOfDirectorySizes(IEnumerable<ElfFile> dirs, int maxSizeToSum)
		{
			return dirs
					.Where(d => d.Size <= maxSizeToSum)
					.Sum(d => d.Size);
		}

		public static ElfFile GetDirectoryToDelete(int diskSize, int updateSize, IEnumerable<ElfFile> dirs)
		{
			var freeSpace = diskSize - dirs.First(d => d.FilePath == "/").Size;
			var spaceToDelete = updateSize - freeSpace;

			return dirs.Where(d => d.Size >= spaceToDelete)
						.OrderBy(d => d.Size)
						.First();
		}
	}
}
