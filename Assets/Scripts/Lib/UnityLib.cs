using System;
using UnityEngine;

public class UnityLib
{
	public static TimeSpan DeltaTimeSpan {
		get {return TimeSpan.FromSeconds(Time.deltaTime);}
	}
	
	public static bool lessThan(TimeSpan a, TimeSpan b)
	{
		return a.TotalSeconds < b.TotalSeconds;
	}
	
	public static bool GreaterThan(TimeSpan a, TimeSpan b)
	{
		return a.TotalSeconds > b.TotalSeconds;
	}
	
	public static readonly TimeSpan TIMESPAN_MAX = TimeSpan.FromDays(1);
}