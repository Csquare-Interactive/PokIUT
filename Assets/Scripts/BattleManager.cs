using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;

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

    public Button fightButton;
    public Button bagButton;
    public Button pokeiutButton;
    public Button runButton;

    public Button[] capaciteButtons;

    private int initialEnemyHealth;
    private int[] initialPlayerPowerPoints;


    void Start()
    {
        SetupBattle();
    }

    void SetupBattle()
    {
        if (playerPokeIUT == null) Debug.LogError("playerPokeIUT is not assigned");
        if (enemyPokeIUT == null) Debug.LogError("enemyPokeIUT is not assigned");
        if (playerNameText == null) Debug.LogError("playerNameText is not assigned");
        if (playerLevelText == null) Debug.LogError("playerLevelText is not assigned");
        if (playerHealthText == null) Debug.LogError("playerHealthText is not assigned");
        if (playerIcon == null) Debug.LogError("playerIcon is not assigned");
        if (enemyNameText == null) Debug.LogError("enemyNameText is not assigned");
        if (enemyLevelText == null) Debug.LogError("enemyLevelText is not assigned");
        if (enemyHealthText == null) Debug.LogError("enemyHealthText is not assigned");
        if (enemyIcon == null) Debug.LogError("enemyIcon is not assigned");

        if (playerPokeIUT == null || enemyPokeIUT == null || playerNameText == null || playerLevelText == null || playerHealthText == null || playerIcon == null || enemyNameText == null || enemyLevelText == null || enemyHealthText == null || enemyIcon == null)
        {
            return;
        }

        // Store initial health and power points
        initialEnemyHealth = enemyPokeIUT.health;
        initialPlayerPowerPoints = new int[playerPokeIUT.capacites.Count];
        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            initialPlayerPowerPoints[i] = playerPokeIUT.capacites[i].damage;
        }

        playerNameText.text = playerPokeIUT.pokeiutName;
        playerLevelText.text = "Lvl " + playerPokeIUT.level;
        playerHealthText.text = "HP " + playerPokeIUT.health;
        playerIcon.sprite = playerPokeIUT.icon;

        enemyNameText.text = enemyPokeIUT.pokeiutName;
        enemyLevelText.text = "Lvl " + enemyPokeIUT.level;
        enemyHealthText.text = "HP " + enemyPokeIUT.health;
        enemyIcon.sprite = enemyPokeIUT.icon;

        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            capaciteButtons[i].GetComponentInChildren<Text>().text = playerPokeIUT.capacites[i].capaciteName;
            capaciteButtons[i].gameObject.SetActive(false);
            int index = i; // Capture the current value of i
            capaciteButtons[i].onClick.AddListener(() => UseCapacite(index));
        }
    }

    public void OnFightButton()
    {
        fightButton.gameObject.SetActive(false);
        bagButton.gameObject.SetActive(false);
        pokeiutButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);

        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            capaciteButtons[i].gameObject.SetActive(true);
        }
    }

    public void UseCapacite(int index)
    {
        CapaciteData capacite = playerPokeIUT.capacites[index];
        enemyPokeIUT.health -= capacite.damage;
        enemyHealthText.text = "HP " + enemyPokeIUT.health;

        // Hide capacity buttons after use
        for (int i = 0; i < capaciteButtons.Length; i++)
        {
            capaciteButtons[i].gameObject.SetActive(false);
        }

        // Show action buttons again
        fightButton.gameObject.SetActive(true);
        bagButton.gameObject.SetActive(true);
        pokeiutButton.gameObject.SetActive(true);
        runButton.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        if (enemyPokeIUT == null) Debug.LogError("enemyPokeIUT is not assigned");
        if (playerPokeIUT == null) Debug.LogError("playerPokeIUT is not assigned");

        // Restore initial health and power points
        if (enemyPokeIUT != null)
        {
            enemyPokeIUT.health = initialEnemyHealth;
        }

        if (playerPokeIUT != null)
        {
            for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
            {
                playerPokeIUT.capacites[i].damage = initialPlayerPowerPoints[i];
            }
        }
    }


}