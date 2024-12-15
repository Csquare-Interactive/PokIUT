using System;
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

    [Header("Description")]
    public Text descriptionText;

    public event Action OnFightClicked;
    public event Action<int> OnCapaciteClicked;
    public event Action OnBagClicked;
    public event Action OnPokeiutClicked;
    public event Action OnRunClicked;

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
        if (capaciteButtons == null) Debug.LogError("capaciteButtons is not assigned");
        if (capaciteButtons.Length < 4 || capaciteButtons.Length > 4) Debug.LogError("capaciteButtons must have 4 buttons");

        // Events listeners when buttons are clicked (To help BattleManager to know what to do)
        fightButton.onClick.AddListener(() => OnFightClicked?.Invoke());
        bagButton.onClick.AddListener(() => OnBagClicked?.Invoke());
        pokeiutButton.onClick.AddListener(() => OnPokeiutClicked?.Invoke());
        runButton.onClick.AddListener(() => OnRunClicked?.Invoke());
        foreach (var button in capaciteButtons)
        {
            int index = Array.IndexOf(capaciteButtons, button);
            button.onClick.AddListener(() => OnCapaciteClicked?.Invoke(index));
        }

        fightButton.gameObject.SetActive(false);
        bagButton.gameObject.SetActive(false);
        pokeiutButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        foreach (var button in capaciteButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void UpdateUI(PokeIUTData player, PokeIUTData enemy)
    {
        playerNameText.text = player.pokeiutName;
        playerLevelText.text = $"Lvl {player.level}";
        playerHealthText.text = $"HP {player.health}";
        playerIcon.sprite = player.icon;

        enemyNameText.text = enemy.pokeiutName;
        enemyLevelText.text = $"Lvl {enemy.level}";
        enemyHealthText.text = $"HP {enemy.health}";
        enemyIcon.sprite = enemy.icon;
    }

    public void RefreshUI(PokeIUTData player, PokeIUTData enemy)
    {
        playerHealthText.text = $"HP {player.health}";
        enemyHealthText.text = $"HP {enemy.health}";
    }

    public void ShowActionButtons(bool show)
    {
        fightButton.gameObject.SetActive(show);
        bagButton.gameObject.SetActive(show);
        pokeiutButton.gameObject.SetActive(show);
        runButton.gameObject.SetActive(show);
    }

    public void ShowCapaciteButtons(bool show, PokeIUTData player)
    {
        for (int i = 0; i < player.capacites.Count; i++)
        {
            capaciteButtons[i].gameObject.SetActive(show);
            capaciteButtons[i].GetComponentInChildren<Text>().text = player.capacites[i].name + $" ({player.capacites[i].powerPoints})";
        }

        backButton.gameObject.SetActive(show);
    }

    public void ShowDescription(string format, params object[] args)
    {
        descriptionText.text = string.Format(format, args);
    }
}