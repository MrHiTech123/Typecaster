using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
	
    private CombatStatistics stats;
    private Spell lastSpell;
    [SerializeField] private GameObject playerMainMenu;
    [SerializeField] private GameObject spellMenu;
    [SerializeField] private GameObject itemMenu;
	[SerializeField] private SceneAsset GameOverScene;
	[SerializeField] private SceneAsset NextScene;

    [SerializeField] private TextMeshProUGUI potionText;
    [SerializeField] private GameObject potionButton;
    [SerializeField] private int potionCount;
    [SerializeField] private Image enemyActionImage;
    [SerializeField] private Sprite[] actionSprites;
    [SerializeField] private int actionIDNextTurn;
    [SerializeField] private GameObject defenseIcon;
    private bool isDefending;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<CombatStatistics>();
        playerMainMenu.SetActive(true);
        spellMenu.SetActive(false);
        itemMenu.SetActive(false);
        lastSpell = null;
        enemyActionImage.sprite = actionSprites[actionIDNextTurn];
        defenseIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack() {
        Debug.Log("[Player Actions] Player attack enemy!!!");
        stats.EnemyTakeDamage(10, waitTime:1.0f);
        playerMainMenu.SetActive(false);
        EnemyAction();
    }

    public void OpenSpellMenu() {
        if(lastSpell == null)
        {
            Debug.Log("[Player Actions] Open spell selection menu!!!");
            playerMainMenu.SetActive(false);
            spellMenu.SetActive(true);
        }
        else
        {
            Debug.Log("[Player Actions] Player continues the last spell!!!");
            lastSpell.Cast(stats, this);
        }
    }

    public void CloseSpellMenu()
    {
        playerMainMenu.SetActive(true);
        spellMenu.SetActive(false);
    }

    public void Defense() {
        Debug.Log("[Player Actions] Player defenses this turn!!!");
        StartCoroutine(DefenseTimed());
    }

    private IEnumerator DefenseTimed()
    {
        isDefending = true;
        defenseIcon.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        EnemyAction();
    }

    public void OpenItemMenu() {
        Debug.Log("[Player Actions] Open item selection menu!!!");
        playerMainMenu.SetActive(false);
        itemMenu.SetActive(true);
        if(potionCount <= 0)
        {
            potionButton.SetActive(false);
        }
        else
        {
            potionButton.SetActive(true);
            potionText.SetText("HP Potion: " + potionCount);
        }
    }

    public void CloseItemMenu()
    {
        playerMainMenu.SetActive(true);
        itemMenu.SetActive(false);
    }

    public void EnemyAction()
    {
		if (stats.enemyHP <= 0)
		{
			SceneManager.LoadScene(NextScene.name, LoadSceneMode.Single);
		}
        else
        {
            Debug.Log("[Enemy Actions] Enemy deals 20 damage to player!!!");
            stats.PlayerTakeDamage(20, waitTime:1.0f);
            playerMainMenu.SetActive(true);
            // draw action for the next turn
            actionIDNextTurn = Random.Range(0, actionSprites.Length);
            enemyActionImage.sprite = actionSprites[actionIDNextTurn];
            if (stats.playerHP <= 0)
            {
                SceneManager.LoadScene(GameOverScene.name, LoadSceneMode.Single);
            }
        }
    }

    public float CalculateDefense()
    {
        if(isDefending)
        {
            Debug.Log("Damage to player is defended!!!");
            isDefending = false;
            defenseIcon.SetActive(false);
            return 1 - 0.8f;
        }
        else
        {
            Debug.Log("Damage is not defended!!!");
            return 1.0f;
        }
    }

    public void UsePotion()
    {
        StartCoroutine(UsePotionTimed());
    }

    private IEnumerator UsePotionTimed()
    {
        Debug.Log("[Player Actions] Player used a health potion!!!");
        itemMenu.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        stats.PlayerTakeDamage(-100);
        potionCount--;
        EnemyAction();
    }

    public void CastSpell()
    {
        Debug.Log("[Player Actions] Player casts a spell!!!");
        spellMenu.SetActive(false);
		lastSpell = Spell.FIRE_BALL;
        lastSpell.Cast(stats, this);
    }

    public void CastSpell(int spellid)
    {
        Debug.Log("[Player Actions] Player cast a spell!!!");
        switch(spellid)
        {
            case 0: lastSpell = Spell.FIRE_BALL;break;
            case 1: lastSpell = Spell.FIRE_STORM;break;
            case 2: lastSpell = Spell.CONSUME;break;
            case 3: lastSpell = Spell.METEOR_STRIKE;break;
        }
        spellMenu.SetActive(false);
        lastSpell.Cast(stats, this);
    }

    public void SpellCastOver()
    {
        lastSpell = null;
    }
}
