using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviour
{
	[SerializeField] private Scene GameOverScene;
	[SerializeField] private Scene NextScene;
    private CombatStatistics stats;
    [SerializeField] private GameObject playerMainMenu;
    [SerializeField] private GameObject spellMenu;
    [SerializeField] private GameObject itemMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<CombatStatistics>();
        playerMainMenu.SetActive(true);
        spellMenu.SetActive(false);
        itemMenu.SetActive(false);
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
        Debug.Log("[Player Actions] Open spell selection menu!!!");
        playerMainMenu.SetActive(false);
        spellMenu.SetActive(true);
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
        Debug.Log("[Player Actions] Player casts a fire ball!!!");
        spellMenu.SetActive(false);
		Spell.FIRE_BALL.Cast(stats, this);
		
    }
}
