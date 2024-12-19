using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Informations Générales")]
    public string playerName;
    public int money;

    [Header("Déplacements")]
    public float walkSpeed;
    public float runSpeed;
    public float smoothFactor;
    public bool canRun;
    public Vector3 lastPosition;

    [Header("Inventaire")]
    public int maxItems;
    public ItemData[] itemsData;
    public ItemInstance[] items;

    [Header("PokIUT")]
    public int maxPokIUT;
    public PokeIUTData[] pokIUTInventoryData;
    public PokeIUTData[] pokIUTTeamData;
    public PokeIUTData currentPokeIUTData;

    public PokeIUTInstance[] pokIUTInventory;
    public PokeIUTInstance[] pokIUTTeam;
    public PokeIUTInstance currentPokeIUT;

    public void InitializePokeIUTTeam()
    {
        if (pokIUTTeamData == null || pokIUTTeamData.Length == 0)
        {
            Debug.LogWarning("PokeIUTTeamData est vide.");
            return;
        }

        if (pokIUTTeam == null || pokIUTTeam.Length != pokIUTTeamData.Length)
        {
            pokIUTTeam = new PokeIUTInstance[pokIUTTeamData.Length];
            Debug.Log("PlayerData | PokeIUTTeam a été réinitialisé.");
        }

        for (int i = 0; i < pokIUTTeamData.Length; i++)
        {
            if (pokIUTTeamData[i] == null)
            {
                Debug.LogError($"PlayerData | pokIUTTeamData[{i}] est null. Veuillez assigner un PokeIUTData valide.");
                continue;
            }

            if (pokIUTTeam[i] == null || pokIUTTeam[i].baseData != pokIUTTeamData[i])
            {
                pokIUTTeam[i] = new PokeIUTInstance(pokIUTTeamData[i]);
                if (pokIUTTeam[i].baseData != null)
                {
                    Debug.Log($"PlayerData | Instancié {pokIUTTeam[i].baseData.pokeiutName} dans PokeIUTTeam à l'indice {i}.");
                }
                else
                {
                    Debug.Log($"PlayerData | Instancié {pokIUTTeam[i]} avec baseData à null.");
                }
            }
            else
            {
                Debug.Log($"PlayerData | {pokIUTTeamData[i].pokeiutName} est déjà présent dans PokeIUTTeam à l'indice {i}.");
            }
        }

        if (currentPokeIUT == null || currentPokeIUT.baseData == null)
            currentPokeIUT = pokIUTTeam[0];
    }

    public void InitializeItems()
    {
        if (itemsData == null || itemsData.Length == 0)
        {
            Debug.LogWarning("ItemData est vide.");
            return;
        }

        if (items == null || items.Length != itemsData.Length)
        {
            items = new ItemInstance[itemsData.Length];
            Debug.Log("PlayerData | Items a été réinitialisé.");
        }

        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == null)
            {
                Debug.LogError($"PlayerData | itemsData[{i}] est null. Veuillez assigner un ItemData valide.");
                continue;
            }

            if (items[i] == null || items[i].baseData != itemsData[i])
            {
                items[i] = new ItemInstance(itemsData[i]);
                items[i].quantity = itemsData[i].maxQuantity; //NOTE: Temporaire, le temps de permettre au joueur de récupérer des items //
                if (items[i].baseData != null)
                {
                    Debug.Log($"PlayerData | Instancié {items[i].baseData.itemName} dans Items à l'indice {i}.");
                }
                else
                {
                    Debug.Log($"PlayerData | Instancié {items[i]} avec baseData à null.");
                }
            }
            else
            {
                Debug.Log($"PlayerData | {itemsData[i].itemName} est déjà présent dans Items à l'indice {i}.");
            }
        }
    }

    public void ResetPokeIUT()
    {
        for (int i = 0; i < pokIUTTeam.Length; i++)
        {
            if (pokIUTTeam[i] != null)
            {
                pokIUTTeam[i].Reset();
            }
        }
    }
}
