using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    public string name;

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


}
