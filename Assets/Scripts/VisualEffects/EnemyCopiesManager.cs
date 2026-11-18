using UnityEngine;

public class EnemyCopiesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] copies;
    private CombatStatistics combatStatistics;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combatStatistics = FindAnyObjectByType<CombatStatistics>();
        UpdateCopies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCopies()
    {
        for(int i = 2; i <= combatStatistics.enemyCount; i++)
        {
            copies[i - 2].SetActive(true);
        }
        for(int i = combatStatistics.enemyCount - 1; i < copies.Length; i++)
        {
            copies[i].SetActive(false);
        }
        
    }
}
