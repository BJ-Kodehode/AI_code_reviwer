using System;
using System.Text;

namespace AIThatJudgesYourCode
{
	internal class Program
	{
		static void Main()
		{
			Console.Title = "AI That Judges Your Code";

			PrintHeader();

			Console.WriteLine("Choose your reviewer:");
			Console.WriteLine();
			Console.WriteLine("  1. Senior Developer");
			Console.WriteLine("  2. Angry Tech Lead");
			Console.WriteLine("  3. Stack Overflow User");
			Console.WriteLine("  4. Intern");
			Console.WriteLine();

			Console.Write("Personality: ");
			string choice = Console.ReadLine()?.Trim() ?? "";

			Personality personality = choice switch
			{
				"1" => Personality.SeniorDeveloper,
				"2" => Personality.AngryTechLead,
				"3" => Personality.StackOverflowUser,
				"4" => Personality.Intern,
				_ => Personality.SeniorDeveloper
			};

			Console.Clear();
			PrintHeader();

			Console.WriteLine($"Reviewer: {GetPersonalityName(personality)}");
			Console.WriteLine();
			Console.WriteLine("Paste your C# code below.");
			Console.WriteLine("When you're finished, type END on a new line.");
			Console.WriteLine();
			Console.WriteLine("--------------------------------------------------");

			string code = ReadCode();

			if (string.IsNullOrWhiteSpace(code))
			{
				Console.WriteLine();
				Console.WriteLine("You gave me no code.");
				Console.WriteLine("Honestly, that's probably the cleanest code I've seen today.");
				return;
			}

			Console.WriteLine("--------------------------------------------------");
			Console.WriteLine();
			Console.WriteLine("Analyzing your masterpiece...");
			Console.WriteLine();

			System.Threading.Thread.Sleep(800);

			CodeAnalyzer analyzer = new CodeAnalyzer();
			CodeReport report = analyzer.Analyze(code);

			Reviewer reviewer = new Reviewer(personality);

			reviewer.PrintReview(report);

			Console.WriteLine();
			if (!Console.IsInputRedirected)
			{
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey(true);
			}
		}

		static void PrintHeader()
		{
			Console.WriteLine("==================================================");
			Console.WriteLine("             AI THAT JUDGES YOUR CODE");
			Console.WriteLine("==================================================");
			Console.WriteLine();
			Console.WriteLine("An advanced artificial intelligence");
			Console.WriteLine("designed specifically to make you regret");
			Console.WriteLine("opening Visual Studio this morning.");
			Console.WriteLine();
		}

		static string ReadCode()
		{
			StringBuilder builder = new StringBuilder();

			while (true)
			{
				string? line = Console.ReadLine();

				if (line == null)
					break;

				if (line.Trim().Equals("END", StringComparison.OrdinalIgnoreCase))
					break;

				builder.AppendLine(line);
			}

			return builder.ToString();
		}

		static string GetPersonalityName(Personality personality)
		{
			return personality switch
			{
				Personality.SeniorDeveloper => "Senior Developer",
				Personality.AngryTechLead => "Angry Tech Lead",
				Personality.StackOverflowUser => "Stack Overflow User",
				Personality.Intern => "Intern",
				_ => "Unknown"
			};
		}
	}
}
