using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class Spell
{
	public readonly List<Spell> ALL_OF_THEM = new List<Spell>(){MANA_BLAST, FIRE_BALL, FIRE_STORM, CONSUME, METEOR_STRIKE};
	
	public static readonly Spell MANA_BLAST = new Spell(10, TimeSpan.FromSeconds(10), 10, (statistics, testResult) => {});
	public static readonly Spell FIRE_BALL = new Spell(10, TimeSpan.FromSeconds(5), 10, (statistics, testResult) => {Debug.Log(testResult.ToString());Debug.Log("Fire Ball!!!");int dmg = (int)UnityEngine.Random.Range(100 * testResult.MistakeScore * 0.8f, 100 * testResult.MistakeScore * 1.2f);statistics.EnemyTakeDamage(dmg, isSkill:true, waitTime:1.0f, endTurn:true);});
	public static readonly Spell FIRE_STORM = new Spell(10, TimeSpan.FromSeconds(10), 10, (statistics, testResult) => {Debug.Log(testResult.ToString());Debug.Log("Fire Storm!!!");int dmg = (int)UnityEngine.Random.Range(75 * testResult.MistakeScore * 0.8f, 75 * testResult.MistakeScore * 1.2f);statistics.EnemyTakeDamage(dmg, isArea:true, isSkill:true, waitTime:1.0f, endTurn:true);});
	public static readonly Spell CONSUME = new Spell(10, TimeSpan.FromSeconds(10), 10, (statistics, testResult) => {Debug.Log(testResult.ToString());Debug.Log("Consume!!!");int dmg = (int)UnityEngine.Random.Range(50 * testResult.MistakeScore * 0.8f, 50 * testResult.MistakeScore * 1.2f);statistics.EnemyTakeDamage(dmg, isSkill:true, waitTime:1.0f);statistics.PlayerTakeDamage(-3 * dmg, waitTime:1.0f, endTurn:true);});
	public static readonly Spell METEOR_STRIKE = new Spell(10, TimeSpan.FromSeconds(10), 10, (statistics, testResult) => {Debug.Log(testResult.ToString());statistics.EnemyTakeDamage(testResult.MistakeScore > 0.5 ? 99999:0, isSkill:true, waitTime:1.0f, endTurn:true);});	
	
	public readonly int manaCost;
	
	public readonly int AllowedMistakes;
	public readonly TimeSpan timeToCast;
	
	public readonly Action<CombatStatistics, TypingTestResult> onCast;
	
	public void Cast(CombatStatistics combatStatistics, PlayerActions actions)
	{
		TypingTestBox.Create(Data.Poetry.GetPoem(this), timeToCast, AllowedMistakes, () => actions.EnemyAction(), result => onCast.Invoke(combatStatistics, result));
	}	
	public Spell(int manaCost, TimeSpan timeToCast, int AllowedMistakes, Action<CombatStatistics, TypingTestResult> onCast)
	{
		this.manaCost = manaCost;
		this.timeToCast = timeToCast;
		this.onCast = onCast;
		this.AllowedMistakes = AllowedMistakes;
	}
	
	// public static void thing()
	// {
	// 	Spell.MANA_BLAST.Cast(statistics);
	// }
	
}
