using System;
using System.Collections.Generic;

public class Data
{
	public static class Poetry
	{
		public static readonly Dictionary<Spell, List<String>> POEMS = new Dictionary<Spell, List<string>>
		{
			{
				Spell.MANA_BLAST, new List<string>
				{
					"This spell gives you more mana.",
					"Drop enemies to 0 HP to kill them.",
					"The faster you type, the more damage you deal.",
					"Don't forget to use your items!",
					
				}
			},
			{
				Spell.FIRE_BALL, new List<string>
				{
					// "This spell gives you more mana.",
					// "Drop enemies to 0 HP to kill them.",
					"The faster you type, the more damage you deal."
				}
			},
			{
				Spell.FIRE_STORM, new List<string>
				{
					"This spell gives you more mana.",
					"Drop enemies to 0 HP to kill them.",
					"The faster you type, the more damage you deal."
				}
			},
			{
				Spell.CONSUME, new List<string>
				{
					"This spell gives you more mana.",
					"Drop enemies to 0 HP to kill them.",
					"The faster you type, the more damage you deal."
				}
			},
			{
				Spell.METEOR_STRIKE, new List<string>
				{
					"A strike by a meteor that dwarfs the K-T Extinction event by comparison, this meteor strike instantaneously obliterates anything you attack with it. Also, the K-T extinction event happened in this world even though it’s not Earth. Don’t worry about it."
				}
			}
		};
		
		
		
		public static String GetPoem(Spell spell)
		{
			return DataStructureLib.Choice(POEMS[spell]);
		}
	}
	
	
}