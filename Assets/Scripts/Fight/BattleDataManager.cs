using UnityEngine;

public class BattleDataManager : MonoBehaviour
{
    public static BattleDataManager Instance { get; private set; }

    private EnemyData enemyData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetEnemyData(EnemyData data)
    {
        enemyData = data;
    }

    public EnemyData GetEnemyData()
    {
        return enemyData;
    }
}
