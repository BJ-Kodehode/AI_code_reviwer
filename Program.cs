using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
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

	enum Personality
	{
		SeniorDeveloper,
		AngryTechLead,
		StackOverflowUser,
		Intern
	}

	class CodeReport
	{
		public int Lines { get; set; }
		public int NonEmptyLines { get; set; }
		public int IfStatements { get; set; }
		public int ElseStatements { get; set; }
		public int ForLoops { get; set; }
		public int WhileLoops { get; set; }
		public int ForeachLoops { get; set; }
		public int TryCatchBlocks { get; set; }
		public int ConsoleWrites { get; set; }
		public int VarDeclarations { get; set; }
		public int AsyncMethods { get; set; }
		public int TodoComments { get; set; }
		public int Comments { get; set; }
		public int SwitchStatements { get; set; }
		public int GotoStatements { get; set; }
		public int DynamicUsages { get; set; }
		public int ThreadSleepUsages { get; set; }
		public int LongLines { get; set; }
		public int MaxNesting { get; set; }
		public int EstimatedMethods { get; set; }
		public int TotalBraces { get; set; }
		public double QualityScore { get; set; }
	}

	class CodeAnalyzer
	{
		public CodeReport Analyze(string code)
		{
			CodeReport report = new CodeReport();

			string[] lines = code.Split(
				new[] { "\r\n", "\n", "\r" },
				StringSplitOptions.None);

			report.Lines = lines.Length;
			report.NonEmptyLines = lines.Count(x => !string.IsNullOrWhiteSpace(x));

			report.IfStatements = CountMatches(code, @"\bif\s*\(");
			report.ElseStatements = CountMatches(code, @"\belse\b");
			report.ForLoops = CountMatches(code, @"\bfor\s*\(");
			report.WhileLoops = CountMatches(code, @"\bwhile\s*\(");
			report.ForeachLoops = CountMatches(code, @"\bforeach\s*\(");
			report.TryCatchBlocks = CountMatches(code, @"\btry\s*\{");
			report.ConsoleWrites = CountMatches(code, @"Console\.Write(Line)?\s*\(");
			report.VarDeclarations = CountMatches(code, @"\bvar\s+[A-Za-z_]");
			report.AsyncMethods = CountMatches(code, @"\basync\b");
			report.TodoComments = CountMatches(code, @"(?i)\bTODO\b");
			report.Comments = CountMatches(code, @"//");
			report.SwitchStatements = CountMatches(code, @"\bswitch\s*\(");
			report.GotoStatements = CountMatches(code, @"\bgoto\s+[A-Za-z_]");
			report.DynamicUsages = CountMatches(code, @"\bdynamic\b");
			report.ThreadSleepUsages = CountMatches(code, @"Thread\.Sleep\s*\(");
			report.LongLines = lines.Count(line => line.Length > 120);
			report.MaxNesting = CalculateMaxNesting(code);
			report.EstimatedMethods = EstimateMethods(code);
			report.TotalBraces = code.Count(c => c == '{');
			report.QualityScore = CalculateScore(report);

			return report;
		}

		private int CountMatches(string text, string pattern)
		{
			return Regex.Matches(text, pattern).Count;
		}

		private int CalculateMaxNesting(string code)
		{
			int current = 0;
			int maximum = 0;

			foreach (char character in code)
			{
				if (character == '{')
				{
					current++;
					maximum = Math.Max(maximum, current);
				}
				else if (character == '}')
				{
					current = Math.Max(0, current - 1);
				}
			}

			return maximum;
		}

		private int EstimateMethods(string code)
		{
			string pattern =
				@"(?:public|private|protected|internal|static|\s)+\s*" +
				@"(?:async\s+)?[\w<>\[\],\s]+\s+" +
				@"\w+\s*\([^;]*\)\s*\{";

			return Regex.Matches(code, pattern).Count;
		}

		private double CalculateScore(CodeReport report)
		{
			double score = 100;

			score -= report.IfStatements * 1.5;
			score -= report.ElseStatements;
			score -= report.LongLines * 2;
			score -= report.TodoComments * 5;
			score -= report.GotoStatements * 15;
			score -= report.DynamicUsages * 3;
			score -= report.ThreadSleepUsages * 5;

			if (report.MaxNesting > 4)
				score -= (report.MaxNesting - 4) * 4;

			if (report.Lines > 500)
				score -= 10;

			if (report.Lines > 1000)
				score -= 15;

			if (report.TryCatchBlocks > 10)
				score -= 5;

			return Math.Max(0, Math.Min(100, score));
		}
	}

	class Reviewer
	{
		private readonly Personality personality;
		private readonly Random random;

		public Reviewer(Personality personality)
		{
			this.personality = personality;
			random = new Random();
		}

		public void PrintReview(CodeReport report)
		{
			Console.WriteLine("==================================================");
			Console.WriteLine("                 CODE REVIEW");
			Console.WriteLine("==================================================");
			Console.WriteLine();

			PrintOpening(report);
			Console.WriteLine();

			List<string> comments = GenerateComments(report);

			if (comments.Count == 0)
			{
				comments.Add("I have carefully reviewed your code and found nothing obviously terrible.");
				comments.Add("This is deeply disappointing. I was promised chaos.");
			}

			foreach (string comment in comments)
			{
				Console.WriteLine("  > " + comment);
				Console.WriteLine();
			}

			PrintStatistics(report);
			Console.WriteLine();
			PrintFinalJudgment(report);
		}

		private void PrintOpening(CodeReport report)
		{
			switch (personality)
			{
				case Personality.SeniorDeveloper:
					Console.WriteLine("Senior Developer has entered the code review.");
					Console.WriteLine("They have seen this before. They wish they hadn't.");
					break;
				case Personality.AngryTechLead:
					Console.WriteLine("Tech Lead has opened your pull request.");
					Console.WriteLine("They have cancelled three meetings to deal with this.");
					break;
				case Personality.StackOverflowUser:
					Console.WriteLine("Stack Overflow User has reviewed your code.");
					Console.WriteLine("They are already typing \"duplicate question\".");
					break;
				case Personality.Intern:
					Console.WriteLine("Intern has been asked to review your code.");
					Console.WriteLine("They are terrified but trying their best.");
					break;
			}
		}

		private List<string> GenerateComments(CodeReport report)
		{
			List<string> comments = new List<string>();

			if (report.IfStatements >= 10)
				comments.Add($"Interesting choice using {report.IfStatements} if statements. Bold.");
			else if (report.IfStatements >= 5)
				comments.Add($"{report.IfStatements} if statements. You clearly believe in giving every possible reality a chance.");

			if (report.MaxNesting >= 10)
				comments.Add($"Maximum nesting depth: {report.MaxNesting}. At this point your code has developed geological layers.");
			else if (report.MaxNesting >= 6)
				comments.Add($"You managed {report.MaxNesting} levels of nesting. Have you considered simply leaving the function?");

			if (report.MaxNesting >= 4)
				comments.Add("I see nested logic. The indentation is beginning to look like a staircase to technical debt.");
			if (report.ElseStatements >= 5)
				comments.Add($"{report.ElseStatements} else statements. Because apparently every condition needs a sequel.");
			if (report.ForLoops >= 5)
				comments.Add($"{report.ForLoops} for loops. Very efficient if your goal is to keep the CPU emotionally invested.");
			if (report.WhileLoops >= 3)
				comments.Add($"{report.WhileLoops} while loops. The code has learned how to continue existing.");
			if (report.ForeachLoops >= 4)
				comments.Add($"{report.ForeachLoops} foreach loops. You really looked at those collections and thought: \"I should visit every single one.\"");
			if (report.ConsoleWrites >= 5)
				comments.Add($"{report.ConsoleWrites} Console.WriteLine calls. Congratulations on inventing logging.");
			if (report.VarDeclarations >= 10)
				comments.Add($"{report.VarDeclarations} var declarations. Type inference has successfully inferred that you don't want to type.");
			if (report.TryCatchBlocks >= 5)
				comments.Add($"{report.TryCatchBlocks} try/catch blocks. I admire your commitment to pretending exceptions are optional.");
			if (report.TodoComments > 0)
				comments.Add($"Found {report.TodoComments} TODO comment(s). Ah yes. The TODO system. Truly enterprise-grade project management.");
			if (report.GotoStatements > 0)
				comments.Add($"You used goto {report.GotoStatements} time(s). Somewhere, a programming language designer just felt a disturbance.");
			if (report.DynamicUsages > 0)
				comments.Add($"Found dynamic {report.DynamicUsages} time(s). Why let the compiler know what's happening when you can surprise it later?");
			if (report.ThreadSleepUsages > 0)
				comments.Add($"Thread.Sleep appears {report.ThreadSleepUsages} time(s). Nothing says asynchronous architecture like telling a thread to take a nap.");
			if (report.AsyncMethods > 0)
				comments.Add($"Found {report.AsyncMethods} async keyword(s). Excellent. The code is now waiting professionally.");
			if (report.SwitchStatements > 0)
				comments.Add($"There are {report.SwitchStatements} switch statement(s). A respectable choice. Suspiciously respectable.");
			if (report.LongLines >= 5)
				comments.Add($"{report.LongLines} lines are over 120 characters. Your horizontal scrollbar has seen things.");
			else if (report.LongLines > 0)
				comments.Add($"You have {report.LongLines} very long line(s). I hope your monitor is wide enough.");
			if (report.Lines > 1000)
				comments.Add($"{report.Lines} lines of code. This isn't a source file anymore. It's a lifestyle.");
			else if (report.Lines > 500)
				comments.Add($"{report.Lines} lines. At this point, splitting the file would be considered controversial.");
			if (report.Comments > 10)
				comments.Add($"{report.Comments} comments detected. Either this code is well documented or you're trying to convince yourself it makes sense.");

			AddPersonalitySpecificComments(report, comments);
			Shuffle(comments);

			return comments.Take(8).ToList();
		}

		private void AddPersonalitySpecificComments(CodeReport report, List<string> comments)
		{
			switch (personality)
			{
				case Personality.SeniorDeveloper:
					if (report.IfStatements > 3)
						comments.Add("Have you considered polymorphism? I'm not saying you should use it. I'm just saying I wanted you to feel bad.");
					comments.Add("I've worked on a codebase like this before. We don't talk about it anymore.");
					comments.Add("The code technically works, which is unfortunately not the same thing as good code.");
					break;
				case Personality.AngryTechLead:
					comments.Add("WHO APPROVED THIS?");
					if (report.Lines > 300)
						comments.Add("Why is this file longer than the sprint planning document?");
					if (report.IfStatements > 5)
						comments.Add("WE HAVE DESIGN PATTERNS FOR A REASON.");
					comments.Add("I'm adding this to the agenda for tomorrow's meeting.");
					break;
				case Personality.StackOverflowUser:
					comments.Add("Have you tried Google?");
					comments.Add("This question has already been answered in 2013.");
					comments.Add("Minimal reproducible example would be appreciated.");
					if (report.VarDeclarations > 0)
						comments.Add("Why are you using var? Please explain your reasoning.");
					comments.Add("I'm voting to close this code review as unclear.");
					break;
				case Personality.Intern:
					comments.Add("I don't really know C# yet, but this looks complicated.");
					comments.Add("Should I ask the senior developer about this?");
					if (report.ConsoleWrites > 0)
						comments.Add("I recognize Console.WriteLine. We learned that yesterday.");
					if (report.TodoComments > 0)
						comments.Add("Oh cool, TODO comments. I was told those are important.");
					comments.Add("I would approve this, but I'm not sure if I'm allowed.");
					break;
			}
		}

		private void PrintStatistics(CodeReport report)
		{
			Console.WriteLine();
			Console.WriteLine("==================================================");
			Console.WriteLine("                 VITAL STATISTICS");
			Console.WriteLine("==================================================");
			Console.WriteLine();
			Console.WriteLine($"Lines:              {report.Lines}");
			Console.WriteLine($"Non-empty lines:    {report.NonEmptyLines}");
			Console.WriteLine($"Estimated methods:  {report.EstimatedMethods}");
			Console.WriteLine($"if statements:      {report.IfStatements}");
			Console.WriteLine($"else statements:    {report.ElseStatements}");
			Console.WriteLine($"for loops:          {report.ForLoops}");
			Console.WriteLine($"while loops:        {report.WhileLoops}");
			Console.WriteLine($"foreach loops:      {report.ForeachLoops}");
			Console.WriteLine($"try blocks:         {report.TryCatchBlocks}");
			Console.WriteLine($"switch statements:  {report.SwitchStatements}");
			Console.WriteLine($"Console.WriteLine:  {report.ConsoleWrites}");
			Console.WriteLine($"var declarations:   {report.VarDeclarations}");
			Console.WriteLine($"async keywords:     {report.AsyncMethods}");
			Console.WriteLine($"TODOs:              {report.TodoComments}");
			Console.WriteLine($"goto statements:    {report.GotoStatements}");
			Console.WriteLine($"dynamic usages:     {report.DynamicUsages}");
			Console.WriteLine($"Thread.Sleep:       {report.ThreadSleepUsages}");
			Console.WriteLine($"long lines:         {report.LongLines}");
			Console.WriteLine($"max nesting:        {report.MaxNesting}");
		}

		private void PrintFinalJudgment(CodeReport report)
		{
			Console.WriteLine();
			Console.WriteLine("==================================================");
			Console.WriteLine("                 FINAL JUDGMENT");
			Console.WriteLine("==================================================");
			Console.WriteLine();

			string judgment = report.QualityScore switch
			{
				>= 90 => "Suspiciously clean. Are you sure you wrote this?",
				>= 75 => "Acceptable. I have seen significantly worse.",
				>= 55 => "Technically code. Spiritually questionable.",
				>= 30 => "This code has entered its villain arc.",
				_ => "I need to lie down after reading this."
			};

			Console.WriteLine(judgment);
			Console.WriteLine();

			string recommendation = personality switch
			{
				Personality.SeniorDeveloper => "Recommendation: Refactor it before someone else has to maintain it.",
				Personality.AngryTechLead => "Recommendation: Fix it before I schedule another meeting.",
				Personality.StackOverflowUser => "Recommendation: Search the documentation.",
				Personality.Intern => "Recommendation: Maybe ask someone senior.",
				_ => "Recommendation: Review the code again."
			};

			Console.WriteLine(recommendation);
			Console.WriteLine();
			Console.WriteLine($"Completely Unhelpful Score: {CalculateUnhelpfulScore(report)}/100");
		}

		private int CalculateUnhelpfulScore(CodeReport report)
		{
			int score = 25;
			score += report.IfStatements * 2;
			score += report.ElseStatements;
			score += report.MaxNesting * 3;
			score += report.TodoComments * 4;
			score += report.GotoStatements * 10;
			score += report.DynamicUsages * 3;
			score += report.LongLines;
			score += report.ThreadSleepUsages * 5;

			return Math.Min(100, score);
		}

		private void Shuffle(List<string> list)
		{
			for (int index = list.Count - 1; index > 0; index--)
			{
				int otherIndex = random.Next(index + 1);
				(list[index], list[otherIndex]) = (list[otherIndex], list[index]);
			}
		}
	}
}
