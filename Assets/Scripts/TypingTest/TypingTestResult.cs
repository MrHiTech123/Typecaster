using System;

public struct TypingTestResult
{
	
	
	// 1.0 = Did it in 0 seconds. 0.0 = Did it in the time allowed.
	public float TimeScore {get; private set;}
	
	public int Mistakes {get; private set;}
	
	
	public TypingTestResult(float TimeScore, int Mistakes)
	{
		this.TimeScore = TimeScore;
		this.Mistakes = Mistakes;
	}
	
}