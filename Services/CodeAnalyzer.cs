using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIThatJudgesYourCode
{
	class CodeAnalyzer
	{
		public CodeReport Analyze(string code)
		{
			CodeReport report = new CodeReport();
			string[] lines = code.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

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

		private int CountMatches(string text, string pattern) => Regex.Matches(text, pattern).Count;

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
					current = Math.Max(0, current - 1);
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
}