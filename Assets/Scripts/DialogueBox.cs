using UnityEngine;
using TMPro;
using System;
using Unity.Mathematics;
using UnityEngine.UI;
public class Script : MonoBehaviour
{
	private static readonly float CHARACTERS_PER_SECOND = 5;
	[SerializeField] private TMP_Text TextBox;
	[SerializeField] private String __message;
	private String Message
	{
		get
		{
			return __message;
		}
		set
		{
			totalTimeSinceMessageChanged = 0;
			__message = value;
		}
	}
	
	private float totalTimeSinceMessageChanged = 0;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Message = __message;
    }
	
	
	void UpdateTimeSinceMessageChanged()
	{
		totalTimeSinceMessageChanged += Time.deltaTime;
	}
	void UpdateText()
	{
		float totalSeconds = totalTimeSinceMessageChanged;
		int charsToShow = (int)(CHARACTERS_PER_SECOND * totalSeconds);
		charsToShow = Math.Min(Message.Length, charsToShow);
		TextBox.text = Message.Substring(0, charsToShow);
	}
    // Update is called once per frame
    void Update()
    {
		UpdateTimeSinceMessageChanged();
		UpdateText();
    }
}
