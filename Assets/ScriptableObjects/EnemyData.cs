using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Informations Générales")]
    public string enemyName;

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
            pokIUTTeam[i] = new PokeIUTInstance(pokIUTTeamData[i]);
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

        items = new ItemInstance[itemsData.Length];

        for (int i = 0; i < itemsData.Length; i++)
        {
            items[i] = new ItemInstance(itemsData[i]);
            items[i].quantity = itemsData[i].maxQuantity;
        }
    }
}
