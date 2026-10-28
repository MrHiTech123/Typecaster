using System;
using UnityEngine;

public class UnityLib
{
	public static TimeSpan DeltaTimeSpan {
		get {return TimeSpan.FromSeconds(Time.deltaTime);}
	}
	
	public static readonly TimeSpan TIMESPAN_MAX = TimeSpan.FromDays(1);
}