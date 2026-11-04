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
	public static void Create(String whatToType, TimeSpan timeAlloted, Action<TypingTestResult> onCompleted)
	{
		TypingTestBox textBox = Instantiate(GameManager.INSTANCE.TYPING_TEXT_BOX_PREFAB).GetComponent<TypingTestBox>();
		textBox.Initialize(whatToType, timeAlloted, onCompleted);
	}
	
	
	/*
		**********************
		END STATIC METHODS
		**********************
	*/
	
	
	
	
	
	
	HashSet<String> typedThisFrame = new HashSet<String>();
	
	public TMP_Text BoxForWhatToType;
	public TMP_Text BoxForWhatPlayerTyped;
	
	[SerializeField] private String whatToType;
	private String typedSoFar = "";
	private String mistakeText;
	private TimeSpan timeAlloted;
	private Action<TypingTestResult> onCompleted;
	
	[SerializeField] private bool WasManuallyPutInLevel;
	
	private bool startedTyping = false;
	private DateTime startOfTyping = DateTime.MinValue;
	
	private int MistakesMadeSoFar;
	
	
	
	public void Initialize(String whatToType, TimeSpan timeAlloted, Action<TypingTestResult> onCompleted)
	{
		this.whatToType = whatToType;
		this.timeAlloted = timeAlloted;
		this.onCompleted = onCompleted;
		
		
		BoxForWhatToType.text = this.whatToType;
		BoxForWhatPlayerTyped.text = "";
	}

	
	void Start()
	{
		if (WasManuallyPutInLevel) {
			Initialize(whatToType, TimeSpan.FromSeconds(10), (TypingTestResult result) => Debug.Log( "Time Score: " + result.TimeScore + ", Mistakes: " + result.Mistakes));
		}
	}
	
	void Backspace()
	{
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
	void Finish()
	{
		DateTime endOfTyping = DateTime.Now;
		TimeSpan timeTakenToType = endOfTyping.Subtract(startOfTyping);		
		
		
		TypingTestResult result = new TypingTestResult((float)speedScore(timeTakenToType), MistakesMadeSoFar);
		onCompleted.Invoke(result);
		
		Destroy(gameObject);
	}
	
	void TypeKey(String key)
	{
		// Debug.Log("Typing key " + key + " typed this frame: " + typedThisFrame.Count);
		
		String toType = "";
		
		if (key == "backspace")
		{
			Backspace();
		}
		
		else if (key == "space")
		{
			toType = " ";
		}
		else if (key == "period")
		{
			toType = ".";
		}
		
		
		else if (key.Length == 1)
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				key = key.ToUpper();
			}
			toType = key;
		}
		
		
		if (typedSoFar.Length >= whatToType.Length)
		{
			return;
		}
		
		if (toType.Length == 0)
		{
			return;
		}
		
		bool isCorrect = toType == new String(whatToType[typedSoFar.Length], 1);
		
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
			++MistakesMadeSoFar;
			mistakeText += toType;
		}
		
		BoxForWhatPlayerTyped.text = typedSoFar + "<color=\"red\">" + mistakeText + "</color>";
		
		if (Done())
		{
			Finish();
		}
		
	}
	
	
	
	void UpdateTyping()
	{
		InputSystem.onAnyButtonPress.Call(key => {
			if (typedThisFrame.Contains(key.name))
			{
				return;
			}
			
			typedThisFrame.Add(key.name);
			
			TypeKey(key.name);
		});
		typedThisFrame.Clear();
	}
	void Update()
	{
		UpdateTyping();
	}

}