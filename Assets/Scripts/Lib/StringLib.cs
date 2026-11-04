using System;

public class StringLib
{
	public static String CutOffLastCharacter(String text) {
		return text.Substring(0, text.Length - 1);
	}
}