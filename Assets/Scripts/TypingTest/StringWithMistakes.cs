using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;

public class StringWithMistakes
{
	private String content;
	private HashSet<int> mistakes = new HashSet<int>();
	
	public int Length
	{
		get
		{
			return content.Length;
		}
	}
	
	public StringWithMistakes(String content, IEnumerable<int> mistakes)
	{
		this.content = content;
		
		foreach (int mistake in mistakes)
		{
			this.mistakes.Add(mistake);
		};
		
		
	}
	
	public StringWithMistakes(String content) : this(content, new HashSet<int>()) {}
	
	public StringWithMistakes AllMistakes(String content)
	{
		HashSet<int> mistakes = new HashSet<int>();
		for (int i = 0; i < content.Length; ++i)
		{
			mistakes.Add(i);
		}
		
		return new StringWithMistakes(content, mistakes);
	}
	
	public StringWithMistakes() : this("") {}
	
	public static StringWithMistakes operator +(StringWithMistakes first, StringWithMistakes second)
	{
		String toReturnContent = first.content + second.content;
		HashSet<int> toReturnMistakes = new HashSet<int>();
		
		foreach (int mistake in first.mistakes)
		{
			toReturnMistakes.Add(mistake);
		}
		
		foreach (int mistake in second.mistakes)
		{
			toReturnMistakes.Add(mistake + first.content.Length);
		}
		
		return new StringWithMistakes(toReturnContent, toReturnMistakes);
	}
	
	public String AsFormattedString
	{
		get {
			String toReturn = "";
			for (int i = 0; i < content.Length; ++i)
			{
				if (mistakes.Contains(i))
				{
					toReturn += "<color=\"red\">" + content[i] + "</color>";
				}
				else
				{
					toReturn += content[i];
				}
			}
			return toReturn;
		}
	}
	
	
	public void Backspace()
	{
		content = StringLib.CutOffLastCharacter(content);
		if (mistakes.Contains(content.Length))
		{
			mistakes.Remove(content.Length);
		}
	}
}
