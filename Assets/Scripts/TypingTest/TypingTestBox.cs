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
	
	public void Initialize(String whatToType)
	{
		this.whatToType = whatToType;
		BoxForWhatToType.text = whatToType;
	}

	void Start()
	{
		Initialize(whatToType);
	}

	void TypeKey(String key)
	{
		Debug.Log("Typing key " + key + " typed this frame: " + typedThisFrame.Count);
		
		String toType = "";
		
		if (key == "backspace")
		{
			if (typedSoFar.Length >= 1) {
				typedSoFar = typedSoFar.Substring(0, typedSoFar.Length - 1);
			}
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