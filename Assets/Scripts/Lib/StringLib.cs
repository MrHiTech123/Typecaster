using System;
using UnityEngine.Assemblies;
using UnityEngine.UIElements;

public class StringLib
{
	public static String CutOffLastCharacter(String text) {
		return text.Substring(0, text.Length - 1);
	}
	
	public static String TurnRed(String text)
	{
		return "<color=\"red\">" + text + "</color>";
	}
	
	public static int LengthWithoutTags(String text)
	{
		int toReturn = 0;
		
		for (int currentCharIndex = 0; currentCharIndex < text.Length; ++currentCharIndex)
		{
			if (text[currentCharIndex] == '<')
			{
				while (text[currentCharIndex] != '>')
				{
					++currentCharIndex;
				}
				++currentCharIndex;
			}
			++toReturn;
		}
		return toReturn;
	}
	public static String SubstringWithoutTags(String text, int start, int length)
	{
		String toReturn = "";
		int currentCharIndex = start;
		for (int i = 0; i < length && (currentCharIndex < text.Length); ++i)
		{
			if (text[currentCharIndex] == '<')
			{
				while (currentCharIndex < text.Length && text[currentCharIndex] != '>')
				{
					toReturn += text[currentCharIndex];
					++currentCharIndex;
				}
				toReturn += text[currentCharIndex];
				++currentCharIndex;
			}
			
			toReturn += text[currentCharIndex];
			
			
			++currentCharIndex;
		}
		return toReturn;
	}
}