using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Informations Générales")]
    public string enemyName;
    public EnemyType enemyType;

    [Header("Déplacements")]
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

        pokIUTTeam = new PokeIUTInstance[pokIUTTeamData.Length];

        for (int i = 0; i < pokIUTTeamData.Length; i++)
        {
            // Check if the PokeIUT is already in the team
            if (pokIUTTeam[i] == null || pokIUTTeam[i].baseData != pokIUTTeamData[i])
            {
                pokIUTTeam[i] = new PokeIUTInstance(pokIUTTeamData[i]);
                Debug.Log($"ENEMY DATA | Instancié {pokIUTTeamData[i].pokeiutName} dans PokeIUTTeam.");
            }
            else
            {
                Debug.Log($"ENEMY DATA | {pokIUTTeamData[i].pokeiutName} est déjà présent dans PokeIUTTeam.");
            }
        }

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
            Debug.Log("EnemyData | Items a été réinitialisé.");
        }

        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == null)
            {
                Debug.LogError($"EnemyData | itemsData[{i}] est null. Veuillez assigner un ItemData valide.");
                continue;
            }

            if (items[i] == null || items[i].baseData != itemsData[i])
            {
                items[i] = new ItemInstance(itemsData[i]);
                if (items[i].baseData != null)
                {
                    Debug.Log($"EnemyData | Instancié {items[i].baseData.itemName} dans Items à l'indice {i}.");
                }
                else
                {
                    Debug.Log($"EnemyData | Instancié {items[i]} avec baseData à null.");
                }
            }
            else
            {
                Debug.Log($"EnemyData | {itemsData[i].itemName} est déjà présent dans Items à l'indice {i}.");
            }
        }
    }
}

public enum EnemyType
{
    Trainer,
    Wild
}
