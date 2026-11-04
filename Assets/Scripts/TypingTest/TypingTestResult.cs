using System;

public struct TypingTestResult
{
	
	// 1.0 = full accuracy. 0.0 = one mistake per character
	public float AccuracyScore {get; private set;}
	
	// 1.0 = Did it in 0 seconds. 0.0 = Did it in the time allowed.
	public float TimeScore {get; private set;}
	
	
	public TypingTestResult(float AccuracyScore, float TimeScore)
	{
		this.AccuracyScore = AccuracyScore;
		this.TimeScore = TimeScore;
	}
	
}