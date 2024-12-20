using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainerManager : MonoBehaviour {
    
    public EnemyData enemyData;

    private void Start()
    {
        if (enemyData == null)
            Debug.LogError("No enemy data assigned to the trainer!");
    }

    public void StartBattle()
    {
        BattleDataManager.Instance.SetEnemyData(enemyData);
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
    }
}