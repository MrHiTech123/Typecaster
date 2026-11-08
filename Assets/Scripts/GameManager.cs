using UnityEngine;


public class GameManager : MonoBehaviour
{
	public static GameManager INSTANCE;
	
	[SerializeField] public GameObject TYPING_TEXT_BOX_PREFAB;
	
	void Awake()
	{
		INSTANCE = this;
		TypingTestBox.ResetForNewSpell();
	}

	
}
