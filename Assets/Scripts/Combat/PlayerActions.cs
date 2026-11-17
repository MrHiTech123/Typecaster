using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviour
{
	
    private CombatStatistics stats;
    private Spell lastSpell;
    [SerializeField] private GameObject playerMainMenu;
    [SerializeField] private GameObject spellMenu;
    [SerializeField] private GameObject itemMenu;
	[SerializeField] private SceneAsset GameOverScene;
	[SerializeField] private SceneAsset NextScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<CombatStatistics>();
        playerMainMenu.SetActive(true);
        spellMenu.SetActive(false);
        itemMenu.SetActive(false);
        lastSpell = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack() {
        Debug.Log("[Player Actions] Player attack enemy!!!");
        stats.EnemyTakeDamage(10);
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
        Debug.Log("[Player Actions] Player skips this turn!!!");
        EnemyAction();
    }

    public void OpenItemMenu() {
        Debug.Log("[Player Actions] Open item selection menu!!!");
        playerMainMenu.SetActive(false);
        itemMenu.SetActive(true);
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
		
        Debug.Log("[Enemy Actions] Enemy deals 20 damage to player!!!");
        stats.PlayerTakeDamage(20);
        playerMainMenu.SetActive(true);
		if (stats.playerHP <= 0)
		{
			SceneManager.LoadScene(GameOverScene.name, LoadSceneMode.Single);
		}
    }

    public void UsePotion()
    {
        Debug.Log("[Player Actions] Player used a health potion!!!");
        itemMenu.SetActive(false);
        stats.PlayerTakeDamage(-100);
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
