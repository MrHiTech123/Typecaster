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
	
	public void TakeDamage(int dmg, ref int hpAmount, RectTransform healthBar) {
		hpAmount -= dmg;
		hpAmount = Mathf.Clamp(dmg, 0, 100);
		healthBar.sizeDelta = new Vector2(hpAmount / 100.0f * 265, 30);
	}

    public void EnemyTakeDamage(int dmg)
    {
        TakeDamage(dmg, ref enemyHP, enemyHealthBar);
    }

    public void PlayerTakeDamage(int dmg)
    {
        TakeDamage(dmg, ref playerHP, playerHealthBar);
    }
}
