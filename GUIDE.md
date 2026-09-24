# Developer Guide

This guide explains how the project is organized and how the current implementation works.

## Getting started

Open a terminal in the project directory and run:

```bash
dotnet restore
dotnet build
dotnet run
```

The project targets `net10.0`. No third-party NuGet packages are required.

## Using the application

When the application starts:

1. Choose a reviewer by entering `1`, `2`, `3` or `4`.
2. Paste the C# source code you want to inspect.
3. Enter `END` on a separate line.
4. Read the generated review and statistics.

If the input is empty, the application prints a short message and exits. When input is redirected, the application skips the interactive `ReadKey` pause so it can finish correctly in scripts and tests.

## Current architecture

### `Program.cs`

`Program` owns the application flow:

- Displays the header and reviewer menu
- Reads the selected personality
- Reads pasted source code until `END`
- Runs the analyzer
- Creates the reviewer
- Prints the review

`Program` also owns the single shared `Random` instance. It passes that instance to `Reviewer`, making the dependency explicit and ensuring that all random choices use the same generator.

### `Models/Personality.cs`

Contains the available reviewer personalities:

- `SeniorDeveloper`
- `AngryTechLead`
- `StackOverflowUser`
- `Intern`

### `Models/CodeReport.cs`

Contains the values produced by the analyzer, including:

- Line counts
- Conditional and loop counts
- Exception, async and `dynamic` usage
- TODO and comment counts
- Long lines and maximum nesting
- Estimated methods
- Quality score

### `Services/CodeAnalyzer.cs`

Analyzes the pasted text using regular expressions and simple character scanning. It calculates the report and applies the quality-score rules.

The analyzer does not parse C# syntax completely. It searches for recognizable text patterns, so strings and comments may sometimes affect the counts.

### `Services/Reviewer.cs`

Generates and prints the review. It:

- Chooses an opening message for the selected personality
- Generates comments based on detected code patterns
- Adds personality-specific comments
- Shuffles the available comments
- Prints up to eight comments
- Prints statistics and a final judgment

The reviewer receives `Random` through its constructor:

```csharp
Reviewer reviewer = new Reviewer(personality, Random);
```

This keeps random-number creation in the application entry point instead of hiding it inside the reviewer.

## Adding new reviewer comments

The simplest way to add comments is to update `Services/Reviewer.cs`.

For a comment based on an existing metric, add a condition in `GenerateComments`, for example:

```csharp
if (report.LongLines > 10)
{
    comments.Add("The horizontal scrollbar has requested overtime.");
}
```

For comments that should only belong to one personality, add them inside `AddPersonalitySpecificComments`.

Keep comments short enough to remain readable in a terminal window.

## Adding a new personality

To add another reviewer personality:

1. Add a value to `Models/Personality.cs`.
2. Add a menu option in `Program.cs`.
3. Add its display name in `GetPersonalityName`.
4. Add an opening message in `Reviewer.PrintOpening`.
5. Add personality-specific comments in `Reviewer.AddPersonalitySpecificComments`.
6. Add a final judgment and recommendation in `Reviewer.PrintFinalJudgment`.

Search for `Personality.` to find the existing personality decision points.

## Changing the analyzer

To add a new metric:

1. Add a property to `CodeReport`.
2. Calculate it in `CodeAnalyzer.Analyze`.
3. Include it in `CalculateScore` if it should affect the quality score.
4. Display it in `Reviewer.PrintStatistics`.
5. Add relevant comments in `Reviewer`.

For more accurate C# analysis, replace regular expressions with a C# syntax parser such as Roslyn. That would provide syntax-aware results but would also add complexity and a package dependency.

## Validation

Build the project after changes:

```bash
dotnet build --no-restore
```

You can also test the input flow manually with:

```text
1
if (true) { Console.WriteLine("test"); }
END
```

The program should print a review, statistics and a final judgment.

## Design limitations

- The analysis is text-based and approximate.
- The pasted code is not compiled or executed.
- Randomized comments mean that output changes between runs.
- The quality score is intentionally simple and humorous.
- The tool currently reviews one code sample per run.
