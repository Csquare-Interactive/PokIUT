using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections.Generic;

public class BattleUIManager : MonoBehaviour
{
    [Header("PlayerInfos")]
    public Text playerNameText;
    public Text playerLevelText;
    public Text playerHealthText;
    public Image playerIcon;

    [Header("EnemyInfos")]
    public Text enemyNameText;
    public Text enemyLevelText;
    public Text enemyHealthText;
    public Image enemyIcon;

    [Header("PlayerActions")]
    public Button fightButton;
    public Button bagButton;
    public Button pokeiutButton;
    public Button runButton;
    public Button backButton;

    [Header("PlayerCapacites")]
    public Button[] capaciteButtons;

    public void SetupBattleUI()
    {
        if (playerNameText == null) Debug.LogError("playerNameText is not assigned");
        if (playerLevelText == null) Debug.LogError("playerLevelText is not assigned");
        if (playerHealthText == null) Debug.LogError("playerHealthText is not assigned");
        if (playerIcon == null) Debug.LogError("playerIcon is not assigned");
        if (enemyNameText == null) Debug.LogError("enemyNameText is not assigned");
        if (enemyLevelText == null) Debug.LogError("enemyLevelText is not assigned");
        if (enemyHealthText == null) Debug.LogError("enemyHealthText is not assigned");
        if (enemyIcon == null) Debug.LogError("enemyIcon is not assigned");
        if (fightButton == null) Debug.LogError("fightButton is not assigned");
        if (bagButton == null) Debug.LogError("bagButton is not assigned");
        if (pokeiutButton == null) Debug.LogError("pokeiutButton is not assigned");
        if (runButton == null) Debug.LogError("runButton is not assigned");
        if (backButton == null) Debug.LogError("backButton is not assigned");

        fightButton.onClick.AddListener(OnFightButtonClicked);
        bagButton.onClick.AddListener(OnBagButtonClicked);
        pokeiutButton.onClick.AddListener(OnPokeiutButtonClicked);
        runButton.onClick.AddListener(OnRunButtonClicked);

        fightButton.gameObject.SetActive(false);
        bagButton.gameObject.SetActive(false);
        pokeiutButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void InitializeFightUI()
    {
        fightButton.gameObject.SetActive(true);
        bagButton.gameObject.SetActive(true);
        pokeiutButton.gameObject.SetActive(true);
        runButton.gameObject.SetActive(true);

        backButton.gameObject.SetActive(false);
    }

    public void UpdateUI(PokeIUTData player, PokeIUTData enemy)
    {
        playerNameText.text = player.pokIUTName;
        playerLevelText.text = $"Lvl {player.level}";
        playerHealthText.text = $"HP {player.health}";
        playerIcon.sprite = player.icon;

        enemyNameText.text = enemy.pokIUTName;
        enemyLevelText.text = $"Lvl {enemy.level}";
        enemyHealthText.text = $"HP {enemy.health}";
        enemyIcon.sprite = enemy.icon;

        UpdateCapaciteButtons(true);
    }

    public void RefreshUI(PokeIUTData player, PokeIUTData enemy)
    {
        playerHealthText.text = $"HP {player.health}";
        enemyHealthText.text = $"HP {enemy.health}";
        UpdateCapaciteButtons();
    }

    private void HandleBattleEnd(string result)
    {
        Debug.Log(result);
    }

    private void OnFightButtonClicked()
    {
        fightButton.gameObject.SetActive(false);
        bagButton.gameObject.SetActive(false);
        pokeiutButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);

        for (int i = 0; i < combatSystem.Player.capacites.Count; i++)
        {
            capaciteButtons[i].gameObject.SetActive(true);
        }
    }

    private void OnBagButtonClicked()
    {
        Debug.Log("Bag button clicked");
    }

    private void OnPokeiutButtonClicked()
    {
        Debug.Log("Pokeiut button clicked");
    }

    private void OnRunButtonClicked()
    {
        Debug.Log("Run button clicked");
    }

    private void UpdateCapaciteButtons(bool isVisible)
    {
        for (int i = 0; i < combatSystem.Player.capacites.Count; i++)
        {
            var capacite = combatSystem.Player.capacites[i];
            capaciteButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{capacite.capaciteName} ({capacite.powerPoints})";
            capaciteButtons[i].gameObject.SetActive(isVisible && i < combatSystem.Player.capacites.Count);
            int index = i;
            capaciteButtons[i].onClick.RemoveAllListeners();
            capaciteButtons[i].onClick.AddListener(() => combatSystem.PlayerUseCapacite(index));
        }
    }
}