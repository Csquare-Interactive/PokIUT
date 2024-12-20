using UnityEngine;
using System.Collections.Generic;

public class TallGrassManager : MonoBehaviour
{
    public List<EnemyData> possibleEnemies;
    public EnemyData GetRandomEnemy() => possibleEnemies[Random.Range(0, possibleEnemies.Count)];

    private void Start()
    {
        if (possibleEnemies.Count == 0)
            Debug.LogError("No enemies in the grass!");
    }
}