using System;

public struct TypingTestResult
{
	
	
	// 1.0 = Did it in 0 seconds. 0.0 = Did it in the time allowed.
	public float TimeScore {get; private set;}
	
	public float MistakeScore {get; private set;}
	
	public String ToString()
	{
		return "Time score: " + TimeScore + "; Mistakes: " + MistakeScore;
	}
	
	public TypingTestResult(float TimeScore, float MistakeScore)
	{
		this.TimeScore = TimeScore;
		this.MistakeScore = MistakeScore;
	}
	
}