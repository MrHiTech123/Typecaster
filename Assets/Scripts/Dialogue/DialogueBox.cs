using UnityEngine;
using TMPro;
using System;
using Unity.Mathematics;
using UnityEngine.UI;
public class Script : MonoBehaviour
{
	private static readonly float CHARACTERS_PER_SECOND = 5;
	[SerializeField] private TMP_Text TextBox;
	[SerializeField] private DialogueLine[] dialogue;
	private String __message;
	private String Message
	{
		get
		{
			return __message;
		}
		set
		{
			totalTimeSinceMessageChanged = TimeSpan.Zero;
			__message = value;
		}
	}
	
	private int __dialogueIndex;
	private int DialogueIndex
	{
		get
		{
			return __dialogueIndex;
		}
		set
		{
			__dialogueIndex = value;
			
			if (DialogueIndex >= dialogue.Length)
			{
				Destroy(gameObject);
				return;
			}
			
			SetDialogueLine(dialogue[DialogueIndex]);
			
		}
	}
	
	private TimeSpan totalTimeSinceMessageChanged = TimeSpan.Zero;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueIndex = 0;
    }
	
	void SetDialogueLine(DialogueLine dialogueLine)
	{
		Message = dialogueLine.Text;
	}
	
	void UpdateTimeSinceMessageChanged()
	{
		totalTimeSinceMessageChanged += UnityLib.DeltaTimeSpan;
	}
	void UpdateText()
	{
		double totalSeconds = totalTimeSinceMessageChanged.TotalSeconds;
		int charsToShow = (int)(CHARACTERS_PER_SECOND * totalSeconds);
		charsToShow = Math.Min(Message.Length, charsToShow);
		TextBox.text = Message.Substring(0, charsToShow);
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (charsToShow == Message.Length)
			{
				++DialogueIndex;
			}
			else
			{
				totalTimeSinceMessageChanged = UnityLib.TIMESPAN_MAX;
			}
		}
	}
    // Update is called once per frame
    void Update()
    {
		UpdateTimeSinceMessageChanged();
		UpdateText();
    }
}
