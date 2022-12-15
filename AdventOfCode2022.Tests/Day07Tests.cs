using static AdventOfCode2022.Day07;

namespace AdventOfCode2022.Tests
{
	using System.IO;
	using FakeItEasy;

	[TestFixture]
	public class Day07Tests
	{
		private static readonly string SampleInput =
			"""
			$ cd /
			$ ls
			dir a
			14848514 b.txt
			8504156 c.dat
			dir d
			$ cd a
			$ ls
			dir e
			29116 f
			2557 g
			62596 h.lst
			$ cd e
			$ ls
			584 i
			$ cd ..
			$ cd ..
			$ cd d
			$ ls
			4060174 j
			8033020 d.log
			5626152 d.ext
			7214296 k
			""";

		[Test]
		[TestCase("14848514 b.txt", 14848514, "b.txt")]
		[TestCase("8504156 c.dat", 8504156, "c.dat")]
		public void Day07_ListLineParser_Matches_SizeAndFilename(string line, int size, string filename)
		{
			var lineParser = new ListContentOutputLineParser();

			var file = lineParser.Parse("/", line)!;

			Assert.That(file.FilePath, Does.EndWith(filename));
			Assert.That(file.Size, Is.EqualTo(size));
		}

		[Test]
		public void Day07_ListLineParser_WhenDirectory_ReturnsNull()
		{
			var lineParser = new ListContentOutputLineParser();

			var file = lineParser.Parse("/", "dir a");

			Assert.That(file, Is.Null);
		}

		[Test]
		[TestCase("/")]
		[TestCase("/a/b")]
		[TestCase("/foobar")]
		public void Day07_ListContentCommand_Execute_DoesNotChange_Directory(string currentDir)
		{
			var cmd = new ListContentCommand(A.Fake<IListContentOutputLineParser>(x => x.Strict()));

			Assert.That(cmd.Execute(currentDir), Is.EqualTo(currentDir));
		}

		[Test]
		public void Day07_ListContentCommand_ParseOutput_ReturnsFiles()
		{
			var lineParserLocal = A.Fake<IListContentOutputLineParser>();
			A.CallTo(() => lineParserLocal.Parse(A<string>._, A<string>._)).ReturnsNextFromSequence(null, new ElfFile("/b.txt", 1), new ElfFile("/c.dat", 2), null);
			var cmd = new ListContentCommand(lineParserLocal);

			var files = cmd.ParseOutput("/", Enumerable.Repeat("", 4)).ToList();

			Assert.That(files, Has.Count.EqualTo(2));
		}

		[Test]
		[TestCase("b.txt", 1)]
		[TestCase("c.dat", 2)]
		public void Day07_ListContentCommand_ParseOutput_ContainsFile(string filename, int size)
		{
			var lineParser = A.Fake<IListContentOutputLineParser>();
			A.CallTo(() => lineParser.Parse(A<string>._, A<string>._)).ReturnsNextFromSequence(null, new ElfFile("/b.txt", 1), new ElfFile("/c.dat", 2), null);
			var cmd = new ListContentCommand(lineParser);

			var files = cmd.ParseOutput("/", Enumerable.Repeat("", 4)).ToList();
			var file = files.FirstOrDefault(f => f.FilePath.EndsWith(filename));

			Assert.That(file, Is.Not.Null);
			Assert.That(file.Size, Is.EqualTo(size));
		}

		[Test]
		[TestCase("/", "/")]
		[TestCase("foo", "/fiz/baz/foo")]
		[TestCase("/abc", "/abc")]
		[TestCase("foo/bar", "/fiz/baz/foo/bar")]
		public void Day07_ChangeDirCommand_Execute_ChangeDirectory(string param, string expected)
		{
			var cmd = new ChangeDirectoryCommand(param);

			var curDir = cmd.Execute("/fiz/baz");

			Assert.That(curDir, Is.EqualTo(expected));
		}

		[Test]
		[TestCase("/fiz/baz", "/fiz")]
		[TestCase("/fiz", "/")]
		public void Day07_ChangeDirCommand_Execute_CanGoToParent(string currentDirectory, string expected)
		{
			var cmd = new ChangeDirectoryCommand("..");

			var curDir = cmd.Execute(currentDirectory);

			Assert.That(curDir, Is.EqualTo(expected));
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(500)]
		public void Day07_ChangeDirCommand_ParseOutput_Empty(int numInputLines)
		{
			var cmd = new ChangeDirectoryCommand("foo");

			var files = cmd.ParseOutput("/", Enumerable.Repeat("1 a.txt", numInputLines)).ToList();

			Assert.That(files, Is.Empty);
		}

		[Test]
		public void Day07_ConsoleParser_CallsCommands_InOrder()
		{
			var cdCmd = A.Fake<IConsoleCommand>();
			A.CallTo(() => cdCmd.Execute(A<string>._)).Returns("/");
			A.CallTo(() => cdCmd.ParseOutput(A<string>._, A<IEnumerable<string>>._)).Returns(Array.Empty<ElfFile>());

			var lsCmd = A.Fake<IConsoleCommand>();
			A.CallTo(() => lsCmd.Execute(A<string>._)).Returns("/");
			A.CallTo(() => lsCmd.ParseOutput(A<string>._, A<IEnumerable<string>>._)).Returns(Array.Empty<ElfFile>());

			var cmdFact = A.Fake<ICommandFactory>();
			A.CallTo(() => cmdFact.CreateChangeDirectoryCommand(A<string>._)).Returns(cdCmd);
			A.CallTo(() => cmdFact.CreateListContentCommand()).Returns(lsCmd);

			var consoleOutputParser = new ConsoleOutputParser(cmdFact);

			_ = consoleOutputParser.Parse(SampleInput.Split("\r\n"));

			A.CallTo(() => cdCmd.Execute(A<string>._)).MustHaveHappened()
				.Then(A.CallTo(() => lsCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => cdCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => lsCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => cdCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => lsCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => cdCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => cdCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => cdCmd.Execute(A<string>._)).MustHaveHappened())
				.Then(A.CallTo(() => lsCmd.Execute(A<string>._)).MustHaveHappened());

			A.CallTo(() => cdCmd.Execute(A<string>._)).MustHaveHappened(6, Times.Exactly);
			A.CallTo(() => lsCmd.Execute(A<string>._)).MustHaveHappened(4, Times.Exactly);
		}

		[Test]
		public void Day07_ConsoleParser_ReturnsAllFiles_FromListCommands()
		{
			var cdCmd = A.Fake<IConsoleCommand>();
			A.CallTo(() => cdCmd.Execute(A<string>._)).Returns("/");
			A.CallTo(() => cdCmd.ParseOutput(A<string>._, A<IEnumerable<string>>._)).Returns(Array.Empty<ElfFile>());

			var lsCmd = A.Fake<IConsoleCommand>();
			A.CallTo(() => lsCmd.Execute(A<string>._)).Returns("/");
			A.CallTo(() => lsCmd.ParseOutput(A<string>._, A<IEnumerable<string>>._))
				.Returns(new[] { File("/b.txt", 14_848_514), File("/c.dat", 85_04_156) }).Once()
				.Then.Returns(new[] { File("/a/f", 29_116), File("/a/g", 2_557), File("/a/h.lst", 65_596) }).Once()
				.Then.Returns(new[] { File("/a/e/i", 584) }).Once()
				.Then.Returns(new[] { File("/d/j", 4_060_174), File("/d/d.log", 9_033_020), File("/d/jd.ext", 5_626_152), File("/d/k", 7_214_296) });

			var cmdFact = A.Fake<ICommandFactory>();
			A.CallTo(() => cmdFact.CreateChangeDirectoryCommand(A<string>._)).Returns(cdCmd);
			A.CallTo(() => cmdFact.CreateListContentCommand()).Returns(lsCmd);

			var consoleOutputParser = new ConsoleOutputParser(cmdFact);

			var files = consoleOutputParser.Parse(SampleInput.Split("\r\n")).ToList();

			Assert.That(files, Has.Count.EqualTo(10));

			static ElfFile File(string path, int size)
			{
				return new ElfFile(path, size);
			}
		}

		[Test]
		[TestCase("/b.txt", 14_848_514)]
		[TestCase("/c.dat", 85_04_156)]
		[TestCase("/a/f", 29_116)]
		[TestCase("/a/g", 2_557)]
		[TestCase("/a/h.lst", 65_596)]
		[TestCase("/a/e/i", 584)]
		[TestCase("/d/j", 4_060_174)]
		[TestCase("/d/d.log", 9_033_020)]
		[TestCase("/d/jd.ext", 5_626_152)]
		[TestCase("/d/k", 7_214_296)]
		public void Day07_ConsoleParser_ReturnsFile(string path, int size)
		{
			var cdCmd = A.Fake<IConsoleCommand>();
			A.CallTo(() => cdCmd.Execute(A<string>._)).Returns("/");
			A.CallTo(() => cdCmd.ParseOutput(A<string>._, A<IEnumerable<string>>._)).Returns(Array.Empty<ElfFile>());

			var lsCmd = A.Fake<IConsoleCommand>();
			A.CallTo(() => lsCmd.Execute(A<string>._)).Returns("/");
			A.CallTo(() => lsCmd.ParseOutput(A<string>._, A<IEnumerable<string>>._))
				.Returns(new[] { File("/b.txt", 14_848_514), File("/c.dat", 85_04_156) }).Once()
				.Then.Returns(new[] { File("/a/f", 29_116), File("/a/g", 2_557), File("/a/h.lst", 65_596) }).Once()
				.Then.Returns(new[] { File("/a/e/i", 584) }).Once()
				.Then.Returns(new[] { File("/d/j", 4_060_174), File("/d/d.log", 9_033_020), File("/d/jd.ext", 5_626_152), File("/d/k", 7_214_296) });

			var cmdFact = A.Fake<ICommandFactory>();
			A.CallTo(() => cmdFact.CreateChangeDirectoryCommand(A<string>._)).Returns(cdCmd);
			A.CallTo(() => cmdFact.CreateListContentCommand()).Returns(lsCmd);

			var consoleOutputParser = new ConsoleOutputParser(cmdFact);

			var files = consoleOutputParser.Parse(SampleInput.Split("\r\n")).ToDictionary(f => f.FilePath, f => f);

			Assert.That(files, Contains.Key(path));
			Assert.That(files[path].Size, Is.EqualTo(size));

			static ElfFile File(string path, int size)
			{
				return new ElfFile(path, size);
			}
		}

		[Test]
		public void Day07_GetDirectories_ReturnsDistinctPaths()
		{
			var dirs = GetDirectories(new[]
			{
				File("/"),
				File("/foo"),
				File("/bar"),
				File("/foo"),
				File("/foo"),
				File("/fiz/baz"),
				File("/fiz/baz"),
			});

			Assert.That(dirs, Is.Unique);

			static ElfFile File(string dir)
			{
				return new ElfFile(Path.Combine(dir, "a.txt").Replace("\\", "/"), 1);
			}
		}

		[Test]
		[TestCase("/")]
		[TestCase("/foo")]
		[TestCase("/bar")]
		[TestCase("/fiz/baz")]
		public void Day07_GetDirectories_HasPath(string dir)
		{
			var dirs = GetDirectories(new[]
			{
				File("/"),
				File("/foo"),
				File("/bar"),
				File("/foo"),
				File("/foo"),
				File("/fiz/baz"),
				File("/fiz/baz"),
			}).ToList();

			Assert.That(dirs, Contains.Item(dir));

			static ElfFile File(string dir)
			{
				return new ElfFile(Path.Combine(dir, "a.txt").Replace("\\", "/"), 1);
			}
		}

		[Test]
		public void Day07_GetDirectories_ContainsDirs_WithDirectoriesOnly()
		{
			var dirs = GetDirectories(new[]
			{
				File("/"),
				File("/foo"),
				File("/bar"),
				File("/foo"),
				File("/foo"),
				File("/fiz/baz"),
				File("/fiz/bar"),
			}).ToList();

			Assert.That(dirs, Contains.Item("/fiz"));

			static ElfFile File(string dir)
			{
				return new ElfFile(Path.Combine(dir, "a.txt").Replace("\\", "/"), 1);
			}
		}

		[Test]
		public void Day07_GetSizeOfDirectory_IncludesAllSubDirs()
		{
			var sizeOfDir = GetSizeOfDirectory("/foo", new[]
			{
				File("/", 1),
				File("/foo", 2),
				File("/bar", 3),
				File("/foo", 4),
				File("/foo", 5),
				File("/foo/baz", 6),
				File("/foo/bar", 7),
				File("/foo/bar/xyz", 8),
			});

			Assert.That(sizeOfDir, Is.EqualTo(32));

			static ElfFile File(string dir, int size)
			{
				return new ElfFile(Path.Combine(dir, "a.txt").Replace("\\", "/"), size);
			}
		}

		[Test]
		[TestCase("/", 36)]
		[TestCase("/foo", 32)]
		[TestCase("/bar", 3)]
		[TestCase("/foo/baz", 6)]
		[TestCase("/foo/bar", 15)]
		[TestCase("/foo/bar/xyz", 8)]
		public void Day07_GetSizesOfAllDirectories_Ok(string path, int expectedSize)
		{
			var dirs = GetSizesOfAllDirectories(new[]
			{
				File("/", 1),
				File("/foo", 2),
				File("/bar", 3),
				File("/foo", 4),
				File("/foo", 5),
				File("/foo/baz", 6),
				File("/foo/bar", 7),
				File("/foo/bar/xyz", 8),
			}).ToDictionary(d => d.FilePath, d => d.Size);

			Assert.That(dirs, Contains.Key(path));
			Assert.That(dirs[path], Is.EqualTo(expectedSize));

			static ElfFile File(string dir, int size)
			{
				return new ElfFile(Path.Combine(dir, "a.txt").Replace("\\", "/"), size);
			}
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 1)]
		[TestCase(2, 3)]
		[TestCase(3, 6)]
		[TestCase(4, 6)]
		[TestCase(5, 6)]
		[TestCase(6, 12)]
		[TestCase(7, 19)]
		[TestCase(8, 27)]
		[TestCase(9, 27)]
		public void Day07_GetSumOfDirectorySizes_SumOnlyDirsSmallerThanGiven(int minSizeToSum, int expectedSum)
		{
			var sum = GetSumOfDirectorySizes(new[]
			{
				File("/", 1),
				File("/foo", 2),
				File("/bar", 3),
				File("/foo/baz", 6),
				File("/foo/bar", 7),
				File("/foo/bar/xyz", 8),
			}, minSizeToSum);

			Assert.That(sum, Is.EqualTo(expectedSum));

			static ElfFile File(string dir, int size)
			{
				return new ElfFile(Path.Combine(dir, "a.txt").Replace("\\", "/"), size);
			}
		}

		// --------------- Sample ----------------

		[Test]
		public void Day07_Sample_Has_Ten_Files()
		{
			var conParser = new ConsoleOutputParser(new CommandFactory());

			Assert.That(conParser.Parse(SampleInput.Split("\r\n")).ToList(), Has.Count.EqualTo(10));
		}

		[Test]
		[TestCase("/b.txt", 14_848_514)]
		[TestCase("/c.dat", 85_04_156)]
		[TestCase("/a/f", 29_116)]
		[TestCase("/a/g", 2_557)]
		[TestCase("/a/h.lst", 62_596)]
		[TestCase("/a/e/i", 584)]
		[TestCase("/d/j", 4_060_174)]
		[TestCase("/d/d.log", 8_033_020)]
		[TestCase("/d/d.ext", 5_626_152)]
		[TestCase("/d/k", 7_214_296)]
		public void Day07_Sample_HasFile(string path, int size)
		{
			var conParser = new ConsoleOutputParser(new CommandFactory());

			var files = conParser.Parse(SampleInput.Split("\r\n")).ToDictionary(f => f.FilePath, f => f);

			Assert.That(files, Contains.Key(path));
			Assert.That(files[path].Size, Is.EqualTo(size));
		}

		[Test]
		[TestCase("/")]
		[TestCase("/a")]
		[TestCase("/a/e")]
		[TestCase("/d")]
		public void Day07_Sample_GetDirectories_HasPath(string dir)
		{
			var dirs = GetDirectories(new[]
			{
				File("/b.txt"),
				File("/c.dat"),
				File("/a/f"),
				File("/a/g"),
				File("/a/h.lst"),
				File("/a/e/i"),
				File("/d/j"),
				File("/d/d.log"),
				File("/d/jd.ext"),
				File("/d/k")
			});

			Assert.That(dirs, Contains.Item(dir));

			static ElfFile File(string path)
			{
				return new ElfFile(path, 1);
			}
		}

		[Test]
		public void Day07_Sample_GetSizeOfDirectory_IncludesAllSubDirs()
		{
			var sizeOfDir = GetSizeOfDirectory("/a", new[]
			{
				File("/b.txt", 14_848_514),
				File("/c.dat", 85_04_156),
				File("/a/f", 29_116),
				File("/a/g", 2_557),
				File("/a/h.lst", 62_596),
				File("/a/e/i", 584),
				File("/d/j", 4_060_174),
				File("/d/d.log", 8_033_020),
				File("/d/d.ext", 5_626_152),
				File("/d/k", 7_214_296)
			});

			Assert.That(sizeOfDir, Is.EqualTo(94_853));

			static ElfFile File(string path, int size)
			{
				return new ElfFile(path, size);
			}
		}

		[Test]
		[TestCase("/", 483_811_65)]
		[TestCase("/a", 94_853)]
		[TestCase("/a/e", 584)]
		[TestCase("/d", 24_933_642)]
		public void Day07_Sample_GetSizesOfAllDirectories_Ok(string path, int expectedSize)
		{
			var dirs = GetSizesOfAllDirectories(new[]
			{
				File("/b.txt", 14_848_514),
				File("/c.dat", 85_04_156),
				File("/a/f", 29_116),
				File("/a/g", 2_557),
				File("/a/h.lst", 62_596),
				File("/a/e/i", 584),
				File("/d/j", 4_060_174),
				File("/d/d.log", 8_033_020),
				File("/d/jd.ext", 5_626_152),
				File("/d/k", 7_214_296)
			}).ToDictionary(d => d.FilePath, d => d.Size);

			Assert.That(dirs, Contains.Key(path));
			Assert.That(dirs[path], Is.EqualTo(expectedSize));

			static ElfFile File(string path, int size)
			{
				return new ElfFile(path, size);
			}
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(50_000, 584)]
		[TestCase(100_000, 95_437)]
		[TestCase(1_000_000, 95_437)]
		[TestCase(25_000_000, 25_029_079)]
		[TestCase(48_381_165, 73_410_244)]
		public void Day07_Smaple_GetSumOfDirectorySizes_SumOnlyDirsSmallerOrEqualThanGiven(int minSizeToSum, int expectedSum)
		{
			var sum = GetSumOfDirectorySizes(new[]
			{
				File("/", 48_381_165),
				File("/a", 94_853),
				File("/a/e", 584),
				File("/d", 24_933_642)
			}, minSizeToSum);

			Assert.That(sum, Is.EqualTo(expectedSum));

			static ElfFile File(string dir, int size)
			{
				return new ElfFile(Path.Combine(dir, "a.txt").Replace("\\", "/"), size);
			}
		}

		[Test]
		public async Task Day07_Puzzle1_SumOfDirsWitzhSizesBelowOrEqual__100_000__Is__1_454_188()
		{
			var consoleOutputParser = new ConsoleOutputParser(new CommandFactory());
			var files = consoleOutputParser.Parse(await File.ReadAllLinesAsync("Day07.txt"));
			var dirs = GetSizesOfAllDirectories(files);
			var sum = GetSumOfDirectorySizes(dirs, 100_000);

			Assert.That(sum, Is.EqualTo(1_454_188));
		}

		[Test]
		public void Day07_Sample_GetDirectoryToDelete()
		{
			var dirToDelete = GetDirectoryToDelete(
				70_000_000,
				30_000_000,
				new[]
				{
					Dir("/", 48_381_165),
					Dir("/a", 94_853),
					Dir("/a/e", 584),
					Dir("/d", 24_933_642)
				});

			Assert.That(dirToDelete.FilePath, Is.EqualTo("/d"));

			static ElfFile Dir(string path, int size)
			{
				return new ElfFile(path, size);
			}
		}

		[Test]
		public async Task Day07_Puzzle1_GetDirectoryToDelete_Size_Is_4183246()
		{
			var consoleOutputParser = new ConsoleOutputParser(new CommandFactory());
			var files = consoleOutputParser.Parse(await File.ReadAllLinesAsync("Day07.txt"));
			var dirs = GetSizesOfAllDirectories(files);
			var dirToDelete = GetDirectoryToDelete(70_000_000, 30_000_000, dirs);

			Assert.That(dirToDelete.FilePath, Is.EqualTo("/cmvqf/gccnrw/wvq"));
			Assert.That(dirToDelete.Size, Is.EqualTo(4_183_246));
		}
	}
}
