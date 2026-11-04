using UnityEngine;

public class CombatStatistics : MonoBehaviour
{
    public int playerHP;
    public int playerMana;
    public int enemyHP;
    [SerializeField] private RectTransform playerHealthBar;
    [SerializeField] private RectTransform enemyHealthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyTakeDamage(int dmg)
    {
        enemyHP -= dmg;
        if(enemyHP < 0)
        {
            enemyHP = 0;
        }
        if(enemyHP > 100)
        {
            enemyHP = 100;
        }
        enemyHealthBar.sizeDelta = new Vector2(enemyHP / 100.0f * 265, 30);
    }

    public void PlayerTakeDamage(int dmg)
    {
        playerHP -= dmg;
        if(playerHP < 0)
        {
            playerHP = 0;
        }
        if(playerHP > 100)
        {
            playerHP = 100;
        }
        playerHealthBar.sizeDelta = new Vector2(playerHP / 100.0f * 650, 30);
    }
}
