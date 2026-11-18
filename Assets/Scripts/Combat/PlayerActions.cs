using System;
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
    private AudioManagement audioManager;
    [SerializeField] private GameObject playerMainMenu;
    [SerializeField] private GameObject spellMenu;
    [SerializeField] private GameObject itemMenu;
	[SerializeField] private String GameOverScene = "Game Over";
	[SerializeField] private String NextScene;
    [SerializeField] private GameObject spellButton;
    [SerializeField] private GameObject contSpellButton;
    [SerializeField] private GameObject startNewSpellButton;

    [SerializeField] private TextMeshProUGUI potionText;
    [SerializeField] private GameObject potionButton;
    [SerializeField] private int potionCount;
    [SerializeField] private Image enemyActionImage;
    [SerializeField] private Sprite[] actionSprites;
    [SerializeField] private int actionIDNextTurn;
    [SerializeField] private GameObject defenseIcon;
    [SerializeField] private int[] EnemySkillIDs;
    [SerializeField] private int[] poolForActionIDs;
    private bool isDefending;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TypingTestBox.ResetForNewSpell();
        stats = GetComponent<CombatStatistics>();
        audioManager = FindAnyObjectByType<AudioManagement>();
        playerMainMenu.SetActive(true);
        spellMenu.SetActive(false);
        itemMenu.SetActive(false);
        spellButton.SetActive(true);
        contSpellButton.SetActive(false);
        startNewSpellButton.SetActive(false);
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
        
        // Audio Clip 0: Button Click Sound
        audioManager.PlayAudio(0);
        StartCoroutine(CastManaBlast());
    }

    private IEnumerator CastManaBlast()
    {
        playerMainMenu.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        // Audio Clip 1: Mana Blast
        audioManager.PlayAudio(1);
        yield return new WaitForSeconds(2.0f);
        stats.EnemyTakeDamage(10);
        EnemyAction();
    }

    public void OpenSpellMenu() {
        // Audio Clip 0: Button Click Sound
        audioManager.PlayAudio(0);
        if(lastSpell == null)
        {
            Debug.Log("[Player Actions] Open spell selection menu!!!");
            playerMainMenu.SetActive(false);
            spellMenu.SetActive(true);
        }
        else
        {
            spellButton.SetActive(false);
            contSpellButton.SetActive(true);
            startNewSpellButton.SetActive(true);
        }
    }

    public void ContinueLastSpell()
    {
        // Audio Clip 0: Button Click Sound
        audioManager.PlayAudio(0);
        Debug.Log("[Player Actions] Player continues the last spell!!!");
        playerMainMenu.SetActive(false);
        lastSpell.Cast(stats, this);
        
    }

    public void StartANewSpell()
    {
        TypingTestBox.ResetForNewSpell();
        lastSpell = null;
        OpenSpellMenu();
    }

    public void CloseSpellMenu()
    {
        // Audio Clip 0: Button Click Sound
        audioManager.PlayAudio(0);
        playerMainMenu.SetActive(true);
        spellMenu.SetActive(false);
    }

    public void Defense() {
        // Audio Clip 0: Button Click Sound
        audioManager.PlayAudio(0);
        Debug.Log("[Player Actions] Player defenses this turn!!!");
        StartCoroutine(DefenseTimed());
    }

    private IEnumerator DefenseTimed()
    {
        playerMainMenu.SetActive(false);
        
        yield return new WaitForSeconds(0.3f);
        audioManager.PlayAudio(7);
        yield return new WaitForSeconds(0.3f);
        isDefending = true;
        defenseIcon.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        EnemyAction();
    }

    public void OpenItemMenu() {
        Debug.Log("[Player Actions] Open item selection menu!!!");
        // Audio Clip 0: Button Click Sound
        audioManager.PlayAudio(0);
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
        // Audio Clip 0: Button Click Sound
        audioManager.PlayAudio(0);
        playerMainMenu.SetActive(true);
        itemMenu.SetActive(false);
    }

    public void EnemyAction()
    {
        // Check if enemy dies
		if (stats.enemyHP <= 0)
		{
			SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
		}
        else
        {
            StartCoroutine(EnemySkills(actionIDNextTurn));
        }
    }

    private IEnumerator EnemySkills(int skillID)
    {
        enemyActionImage.gameObject.SetActive(false);
        switch(EnemySkillIDs[skillID])
        {
            // Do nothing
            case 0: 
                Debug.Log("[Enemy Actions] Enemy does nothing this turn");
                yield return new WaitForSeconds(1.0f);
                break;
            // Do nothing -- Recover to max HP
            case 1: 
                Debug.Log("[Enemy Actions] Enemy does nothing this turn but heals itself");
                audioManager.PlayAudio(6);
                yield return new WaitForSeconds(1.3f);
                stats.EnemyTakeDamage(-9999);
                yield return new WaitForSeconds(0.8f);
                break;
            // Normal Attack -- Thief
            case 2: 
                Debug.Log("[Enemy Actions] Thief normal attack");
                if(isDefending)
                {
                    audioManager.PlayAudio(11);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    audioManager.PlayAudio(9);
                    yield return new WaitForSeconds(0.8f);
                }
                stats.PlayerTakeDamage((int)UnityEngine.Random.Range(20 * 0.8f, 20 * 1.2f));
                yield return new WaitForSeconds(0.8f);
                break;
            // Heavy Attack -- Thief
            case 3:
                Debug.Log("[Enemy Actions] Thief heavy attack");
                if(isDefending)
                {
                    audioManager.PlayAudio(11);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    audioManager.PlayAudio(10);
                    yield return new WaitForSeconds(1.0f);
                }
                stats.PlayerTakeDamage((int)UnityEngine.Random.Range(70 * 0.8f, 70 * 1.1f));
                yield return new WaitForSeconds(0.8f);
                break;
            // Normal Attack -- Wolf
            case 4:
                Debug.Log("[Enemy Actions] Wolf normal attack");
                for(int i = 0; i < stats.enemyCount; i++)
                {
                    if(isDefending)
                    {
                        audioManager.PlayAudio(11);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        audioManager.PlayAudio(10);
                        yield return new WaitForSeconds(0.3f);
                    }
                    stats.PlayerTakeDamage(UnityEngine.Random.Range(3, 8));
                    yield return new WaitForSeconds(0.8f);
                    
                }
                break;
            // Normal Attack -- Rival Wizard
            case 5:
                Debug.Log("[Enemy Actions] Wizard normal attack");
                for(int i = 0; i < stats.enemyCount; i++)
                {
                    if(isDefending)
                    {
                        audioManager.PlayAudio(11);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        audioManager.PlayAudio(2);
                        yield return new WaitForSeconds(0.5f);
                    }
                    stats.PlayerTakeDamage(UnityEngine.Random.Range(5, 10));
                    yield return new WaitForSeconds(0.8f);
                    
                }
                break;
            // Heavy Attack -- Rival Wizard
            case 6:
                Debug.Log("[Enemy Actions] Wizard heavy attack");
                for(int i = 0; i < stats.enemyCount; i++)
                {
                    
                    if(isDefending)
                    {
                        audioManager.PlayAudio(11);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        audioManager.PlayAudio(5);
                        yield return new WaitForSeconds(1.0f);
                    }
                    stats.PlayerTakeDamage(UnityEngine.Random.Range(18, 26));
                    yield return new WaitForSeconds(0.8f);
                    
                }
                if(stats.enemyCount > 1)
                {
                    stats.enemyCount--;
                    FindAnyObjectByType<EnemyCopiesManager>().UpdateCopies();
                }
                break;
            // Summon copies / recover health -- Rival Wizard
            case 7:
                Debug.Log("[Enemy Actions] Wizard special skill");
                if(stats.enemyCount < 4)
                {
                    audioManager.PlayAudio(8);
                    yield return new WaitForSeconds(2.5f);
                    stats.enemyCount++;
                    FindAnyObjectByType<EnemyCopiesManager>().UpdateCopies();
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    audioManager.PlayAudio(6);
                    yield return new WaitForSeconds(1.3f);
                    stats.EnemyTakeDamage(UnityEngine.Random.Range(-33, -14));
                    yield return new WaitForSeconds(0.8f);
                }
                break;
            // Normal attack -- Typo
            case 8:
                Debug.Log("[Enemy Actions] Typo normal attack");
                if(isDefending)
                {
                    audioManager.PlayAudio(11);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    audioManager.PlayAudio(12);
                    yield return new WaitForSeconds(0.3f);
                }
                stats.PlayerTakeDamage((int)UnityEngine.Random.Range(20 * 0.8f, 20 * 1.2f));
                yield return new WaitForSeconds(0.8f);
                break;
            // Full healing -- Typo
            case 9:
                Debug.Log("[Enemy Actions] Typo full heal");
                audioManager.PlayAudio(6);
                yield return new WaitForSeconds(1.3f);
                stats.EnemyTakeDamage(-9999);
                yield return new WaitForSeconds(0.8f);
                break;
            default:break;
        }

        // Check if player dies
        if (stats.playerHP <= 0)
        {
            SceneManager.LoadScene(GameOverScene, LoadSceneMode.Single);
        }
        // 
        else
        {
            playerMainMenu.SetActive(true);
            // draw action for the next turn
            actionIDNextTurn = poolForActionIDs[UnityEngine.Random.Range(0, poolForActionIDs.Length)];
            enemyActionImage.gameObject.SetActive(true);
            enemyActionImage.sprite = actionSprites[actionIDNextTurn];
            
            isDefending = false;
            defenseIcon.SetActive(false);
        }
    }

    public float CalculateDefense()
    {
        if(isDefending)
        {
            Debug.Log("Damage to player is defended!!!");
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
        
        audioManager.PlayAudio(6);
        itemMenu.SetActive(false);
        yield return new WaitForSeconds(1.3f);
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
        Debug.Log("[Player Actions] Player spell cast done. ");
        lastSpell = null;
        spellButton.SetActive(true);
        contSpellButton.SetActive(false);
        startNewSpellButton.SetActive(false);
    }
}
