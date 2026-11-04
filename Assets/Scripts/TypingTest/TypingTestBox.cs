using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TypingTestBox : MonoBehaviour
{
	
	HashSet<String> typedThisFrame = new HashSet<String>();
	
	public TMP_Text BoxForWhatToType;
	public TMP_Text BoxForWhatPlayerTyped;
	
	[SerializeField] private String whatToType;
	private String typedSoFar = "";
	private String mistakeText;
	private TimeSpan timeAlloted;
	private Action<TypingTestResult> onCompleted;
	
	[SerializeField] private bool WasManuallyPutInLevel;
	
	public void Initialize(String whatToType, TimeSpan timeAlloted, Action<TypingTestResult> onCompleted)
	{
		this.whatToType = whatToType;
		this.timeAlloted = timeAlloted;
		this.onCompleted = onCompleted;
		
		
		BoxForWhatToType.text = this.whatToType;
	}

	
	void Start()
	{
		if (WasManuallyPutInLevel) {
			Initialize(whatToType, TimeSpan.FromSeconds(100), (TypingTestResult result) => Debug.Log("Accuracy Score: " + result.AccuracyScore + ", Time Score: " + result.TimeScore));
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
	
	void Finish()
	{
		TypingTestResult result = new TypingTestResult(1.0f, 0.0f);
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
		
		bool isCorrect = toType == new String(whatToType[typedSoFar.Length], 1);
		
		if (isCorrect)
		{
			typedSoFar += toType;
			mistakeText = "";
		}
		else
		{
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