namespace AIThatJudgesYourCode
{
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
}