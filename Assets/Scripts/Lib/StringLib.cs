using System;

public class StringLib
{
	public static String CutOffLastCharacter(String text) {
		return text.Substring(0, text.Length - 1);
	}
	
	public static String TurnRed(String text)
	{
		return "<color=\"red\">" + text + "</color>";
	}
}