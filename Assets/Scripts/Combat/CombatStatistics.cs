using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CombatStatistics : MonoBehaviour
{
    public int playerHP;
    public int playerMana;
    public int enemyHP;
    [SerializeField] private RectTransform playerHealthBar;
    [SerializeField] private RectTransform enemyHealthBar;
    [SerializeField] private TextMeshProUGUI playerDamageText;
    [SerializeField] private TextMeshProUGUI enemyDamageText;
    private PlayerActions playerActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerActions = GetComponent<PlayerActions>();
        enemyDamageText.gameObject.SetActive(false);
        playerDamageText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void TakeDamage(int dmg, ref int hpAmount, RectTransform healthBar, int maxBarLen, int maxHP) {
		hpAmount -= dmg;
		hpAmount = Mathf.Clamp(hpAmount, 0, maxHP);
		healthBar.sizeDelta = new Vector2(hpAmount / 100.0f * maxBarLen, 30);
	}

    public void EnemyTakeDamage(int dmg, Boolean isArea = false, Boolean isSkill = false, float waitTime = 0.0f)
    {
        StartCoroutine(EnemyTakeDamageTimed(dmg, isArea, isSkill, waitTime));
    }

    public IEnumerator EnemyTakeDamageTimed(int dmg, Boolean isArea = false, Boolean isSkill = false, float waitTime = 0.0f)
    {
        yield return new WaitForSeconds(waitTime);
        enemyDamageText.gameObject.SetActive(true);
        if(dmg < 0)
        {
            enemyDamageText.SetText((-dmg).ToString());
            enemyDamageText.color = Color.green;
        }
        else
        {
            enemyDamageText.SetText(dmg.ToString());
            enemyDamageText.color = Color.red;
        }
        TakeDamage(dmg, ref enemyHP, enemyHealthBar, 265, 100);
        yield return new WaitForSeconds(0.5f);
        enemyDamageText.gameObject.SetActive(false);
        if(isSkill)
        {
            playerActions.SpellCastOver();
        }
        
    }

    public void PlayerTakeDamage(int dmg, float waitTime = 0.0f)
    {
        StartCoroutine(PlayerTakeDamageTimed(dmg, waitTime));
    }
	
    public IEnumerator PlayerTakeDamageTimed(int dmg, float waitTime = 0.0f)
    {
        yield return new WaitForSeconds(waitTime);
        playerDamageText.gameObject.SetActive(true);
        if(dmg < 0)
        {
            playerDamageText.SetText((-dmg).ToString());
            playerDamageText.color = Color.green;
        }
        else
        {
            playerDamageText.SetText(dmg.ToString());
            playerDamageText.color = Color.red;
        }
        TakeDamage(dmg, ref playerHP, playerHealthBar, 650, 100);
        yield return new WaitForSeconds(0.5f);
        playerDamageText.gameObject.SetActive(false);
    }
}
