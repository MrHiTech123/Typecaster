using UnityEngine;
using TMPro;
using System;
using Unity.Mathematics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class Script : MonoBehaviour
{
	private static readonly float CHARACTERS_PER_SECOND = 20;
	[SerializeField] private TMP_Text TextBox;
	[SerializeField] private DialogueLine[] dialogue;
	[SerializeField] private String NextScene;
	[SerializeField] private Dictionary<GameCharacter, Sprite> CHARACTER_SPRITES;
	[SerializeField] private Image CharacterPortrait;
	
	private static readonly float PORTRAIT_WIDTH = 3;
	
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
				FinishDialogue();
				return;
			}
			
			SetDialogueLine(dialogue[DialogueIndex]);
			
		}
	}
	
	private void FinishDialogue()
	{
		Destroy(gameObject);
		if (NextScene != null) {
			SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
		}
	}
	
	private TimeSpan totalTimeSinceMessageChanged = TimeSpan.Zero;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueIndex = 0;
    }
	
	void SetPortraitSprite(Sprite sprite)
	{
		CharacterPortrait.sprite = sprite;
		float spriteWidthHeightRatio = (float)sprite.texture.width / sprite.texture.height;
		CharacterPortrait.transform.localScale = new Vector3(PORTRAIT_WIDTH, PORTRAIT_WIDTH / spriteWidthHeightRatio, 1);
	}
	void SetDialogueLine(DialogueLine dialogueLine)
	{
		Message = dialogueLine.Text;
		SetPortraitSprite(CHARACTER_SPRITES[dialogueLine.Speaker]);
	}
	
	void UpdateTimeSinceMessageChanged()
	{
		totalTimeSinceMessageChanged += UnityLib.DeltaTimeSpan;
	}
	void UpdateText()
	{
		double totalSeconds = totalTimeSinceMessageChanged.TotalSeconds;
		int charsToShow = (int)(CHARACTERS_PER_SECOND * totalSeconds);
		int messageLengthWithoutTags = StringLib.LengthWithoutTags(Message);
		
		charsToShow = Math.Min(charsToShow, messageLengthWithoutTags);
		
		TextBox.text = StringLib.SubstringWithoutTags(Message, 0, charsToShow);
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (charsToShow == messageLengthWithoutTags)
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
