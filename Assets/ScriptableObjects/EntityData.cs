using UnityEngine;
using System.Collections.Generic;

public abstract class EntityData : ScriptableObject
{
    public string name;

    [Header("Inventaire")]
    public int maxItems;
    public List<ItemInstance> items;
    public ItemData[] itemsData;

    [Header("pokeIUT")]
    public int maxpokeIUT;
    public PokeIUTData[] pokeIUTInventoryData;
    public PokeIUTData[] pokeIUTTeamData;
    public PokeIUTData currentPokeIUTData;

    public PokeIUTInstance[] pokeIUTInventory;
    public PokeIUTInstance[] pokeIUTTeam;
    public PokeIUTInstance currentPokeIUT;


}
