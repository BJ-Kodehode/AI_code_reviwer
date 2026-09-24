# AI That Judges Your Code

A humorous C# console application that reviews pasted C# code and responds with randomized feedback from different reviewer personalities.

The application does not use a real AI model. It analyzes simple code patterns with regular expressions and generates playful, personality-based comments.

## Requirements

- .NET 10 SDK
- A terminal or the integrated VS Code terminal

## Run the application

From the project folder, run:

```bash
dotnet run
```

The application will ask you to choose a reviewer:

1. Senior Developer
2. Angry Tech Lead
3. Stack Overflow User
4. Intern

After selecting a reviewer, paste C# code into the terminal. Type `END` on its own line when you are finished.

The application then displays:

- Randomized review comments
- Code statistics
- A quality score
- A personality-specific final judgment

## Project structure

```text
AI_code_reviwer/
|-- Program.cs
|-- Models/
|   |-- CodeReport.cs
|   |-- Personality.cs
|-- Services/
|   |-- CodeAnalyzer.cs
|   |-- Reviewer.cs
|-- AI_code_reviwer.csproj
|-- README.md
|-- GUIDE.md
```

## How it works

1. `Program` displays the menu and reads the pasted code.
2. `CodeAnalyzer` counts code patterns and creates a `CodeReport`.
3. `Reviewer` selects comments based on the report and reviewer personality.
4. The comments are shuffled using one shared `Random` instance.
5. The review, statistics and final judgment are printed to the console.

## Build

To compile the project without running it:

```bash
dotnet build
```

## Important note

This tool is intended for entertainment and basic code inspection. It does not compile or execute the pasted code, and its metrics are estimates rather than a replacement for a real code review.

For a detailed explanation of the code and extension points, see [GUIDE.md](GUIDE.md).
