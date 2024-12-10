using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class BattleManager : MonoBehaviour
{
    public PokeIUTData playerPokeIUT;
    public PokeIUTData enemyPokeIUT;

    public Text playerNameText;
    public Text playerLevelText;
    public Text playerHealthText;
    public Image playerIcon;

    public Text enemyNameText;
    public Text enemyLevelText;
    public Text enemyHealthText;
    public Image enemyIcon;

    void Start()
    {
        SetupBattle();
    }

    void SetupBattle()
    {
        playerNameText.text = playerPokeIUT.pokeiutName;
        playerLevelText.text = "Lvl " + playerPokeIUT.level;
        playerHealthText.text = "HP " + playerPokeIUT.health;
        playerIcon.sprite = playerPokeIUT.icon;

        enemyNameText.text = enemyPokeIUT.pokeiutName;
        enemyLevelText.text = "Lvl " + enemyPokeIUT.level;
        enemyHealthText.text = "HP " + enemyPokeIUT.health;
        enemyIcon.sprite = enemyPokeIUT.icon;
    }

}