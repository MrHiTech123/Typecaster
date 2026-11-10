using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class Spell
{
	public List<Spell> ALL_OF_THEM = new List<Spell>(){MANA_BLAST, FIRE_BALL, FIRE_STORM, CONSUME, METEOR_STRIKE};
	
	public static readonly Spell MANA_BLAST = new Spell(10, TimeSpan.FromSeconds(10), (statistics, testResult) => {});
	public static readonly Spell FIRE_BALL = new Spell(10, TimeSpan.FromSeconds(3.5), (statistics, testResult) => {Debug.Log(testResult.ToString);statistics.EnemyTakeDamage((int)(10 * (10 - testResult.Mistakes)));});
	public static readonly Spell FIRE_STORM = new Spell(10, TimeSpan.FromSeconds(10), (statistics, testResult) => {});
	public static readonly Spell CONSUME = new Spell(10, TimeSpan.FromSeconds(10), (statistics, testResult) => {});
	public static readonly Spell METEOR_STRIKE = new Spell(10, TimeSpan.FromSeconds(10), (statistics, testResult) => {});	
	
	public int manaCost;
	public TimeSpan timeToCast;
	
	public readonly Action<CombatStatistics, TypingTestResult> onCast;
	
	public void Cast(CombatStatistics combatStatistics, PlayerActions actions)
	{
		TypingTestBox.Create(Data.Poetry.GetPoem(this), timeToCast, () => actions.EnemyAction(), result => onCast.Invoke(combatStatistics, result));
	}	
	public Spell(int manaCost, TimeSpan timeToCast, Action<CombatStatistics, TypingTestResult> onCast)
	{
		this.manaCost = manaCost;
		this.timeToCast = timeToCast;
		this.onCast = onCast;
	}
	
	// public static void thing()
	// {
	// 	Spell.MANA_BLAST.Cast(statistics);
	// }
	
}
