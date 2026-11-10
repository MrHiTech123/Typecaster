using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TypingTestBox : MonoBehaviour
{
	
	/*
		**********************
		START STATIC METHODS
		**********************
	*/
	
	// @param whatToType = the correct string that should be typed into the text box
	// @param timeAlloted = the amount of time that the player can spend typing the text and get a score > 0.0
	// @param onCompleted = a delegate function of the form (TypingTestResult result) => {doThing(result);}.
	// 		This function will be called when the player finishes typing the text, and the result will be passed as an argument.
	public static void Create(String whatToType, TimeSpan timeAlloted,  Action onTimeRanOut, Action<TypingTestResult> onSpellCompleted)
	{
		TypingTestBox textBox = Instantiate(GameManager.INSTANCE.TYPING_TEXT_BOX_PREFAB).GetComponent<TypingTestBox>();
		textBox.Initialize(whatToType, timeAlloted, onTimeRanOut, onSpellCompleted);
	}
	
	
	/*
		**********************
		END STATIC METHODS
		**********************
	*/
	
	
	
	
	
	
	HashSet<String> typedThisFrame = new HashSet<String>();
	
	public TMP_Text BoxForWhatToType;
	public TMP_Text BoxForWhatPlayerTyped;
	
	private static String whatToType;
	private static String typedSoFar = "";
	private String mistakeText;
	private TimeSpan timeAlloted;
	private Action onSessionEnded;
	private Action<TypingTestResult> onCompleted;
	
	private static bool Exists;
	private bool initialized = false;
	
	[SerializeField] private bool WasManuallyPutInLevel;
	
	private bool startedTyping = false;
	private DateTime startOfTyping = DateTime.MinValue;
	
	private static int MistakesMadeSoFar = 0;
	
	
	public static void ResetForNewSpell()
	{
		typedSoFar = "";
		MistakesMadeSoFar = 0;
	}
	public void Initialize(String whatToType, TimeSpan timeAlloted,  Action onSessionEnded, Action<TypingTestResult> onCompleted)
	{
		initialized = true;
		Exists = true;
		
		if (TypingTestBox.whatToType != whatToType)
		{
			TypingTestBox.whatToType = whatToType;
			ResetForNewSpell();
		}
		
		this.timeAlloted = timeAlloted;
		this.onSessionEnded = onSessionEnded;
		this.onCompleted = onCompleted;
		
		
		BoxForWhatToType.text = TypingTestBox.whatToType;
		BoxForWhatPlayerTyped.text = TypingTestBox.typedSoFar;
		
	}

	
	void Start()
	{
		if (WasManuallyPutInLevel) {
			Initialize(whatToType, TimeSpan.FromSeconds(10),  () => Debug.Log("Out of time!"), (TypingTestResult result) => Debug.Log( "Time Score: " + result.TimeScore + ", Mistakes: " + result.Mistakes));
		}
	}
	
	void Backspace()
	{
		Debug.Log("Mistake Text: "+ mistakeText + ";;;; Typed so far: " + typedSoFar);
		if (mistakeText.Length >= 1) {
			mistakeText = StringLib.CutOffLastCharacter(mistakeText);
		}
		else if (typedSoFar.Length >= 1) {
			typedSoFar = StringLib.CutOffLastCharacter(typedSoFar);
		}
	}
	
	bool Done()
	{
		return typedSoFar == whatToType;
	}
	
	double speedScore(TimeSpan timeTakenToType)
	{
		Debug.Log(timeTakenToType + "  " + startOfTyping);
		if (timeTakenToType > timeAlloted)
		{
			return 0;
		}
		Debug.Log(timeTakenToType.Divide(timeAlloted));
		
		return 1.0 - timeTakenToType.Divide(timeAlloted);
	}
	
	TimeSpan TimeSinceStartedTyping()
	{
		return DateTime.Now.Subtract(startOfTyping);
	}
	void Finish()
	{
		TimeSpan timeTakenToType = TimeSinceStartedTyping();
		float speed = (float)speedScore(timeTakenToType);
		
		TypingTestResult result = new TypingTestResult(1.0f, MistakesMadeSoFar);
		onCompleted.Invoke(result);
		
		ResetForNewSpell();
		EndTypingSession();
	}
	
	void UpdateText()
	{
		BoxForWhatPlayerTyped.text = typedSoFar + "<color=\"red\">" + mistakeText + "</color>";
	}
	
	void TypeKey(String key)
	{
		// Debug.Log("Typing key " + key + " typed this frame: " + typedThisFrame.Count);
		
		String toType = "";
		
		
		switch (key)
		{
			case "backspace": Backspace(); break;
			case "space": toType = " "; break;
			case "period": toType = "."; break;
			case "comma": toType = ","; break;
			default:
				if (key.Length == 1)
				{
					if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
					{
						key = key.ToUpper();
					}
					toType = key;
				}
				break;
		}
		
		
		
		if (typedSoFar.Length >= whatToType.Length)
		{
			return;
		}
		
		if (toType.Length == 0)
		{
			UpdateText();
			return;
		}
		
		char shouldveBeenTyped = whatToType[typedSoFar.Length];
		
		bool isCorrect = toType == new String(shouldveBeenTyped, 1);
		
		if (!startedTyping)
		{
			startedTyping = true;
			startOfTyping = DateTime.Now;
		}
		
		if (isCorrect)
		{
			typedSoFar += toType;
			mistakeText = "";
		}
		else if (toType.Length >= 1)
		{
			Debug.Log("MISTAKE: " + toType + " should've been " + shouldveBeenTyped + " mistakes: " + MistakesMadeSoFar + " typed so far: " + typedSoFar);
			++MistakesMadeSoFar;
			mistakeText += toType;
		}
		
		UpdateText();
		
		if (Done())
		{
			Finish();
		}
		
	}
	
	
	
	void UpdateTyping()
	{
		InputSystem.onAnyButtonPress.Call(key => {
			if (!Exists)
			{
				return;
			}
			if (typedThisFrame.Contains(key.name))
			{
				return;
			}
			
			typedThisFrame.Add(key.name);
			
			TypeKey(key.name);
		});
		typedThisFrame.Clear();
	}
	
	void EndTypingSession()
	{
		onSessionEnded();
		Exists = false;
		Destroy(gameObject);
	}
	void UpdateTimer()
	{
		if (startedTyping && UnityLib.GreaterThan(TimeSinceStartedTyping(), timeAlloted))
		{
			EndTypingSession();
		}
	}
	
	void Update()
	{
		if (initialized) {
			UpdateTimer();
			UpdateTyping();
		}
	}

}