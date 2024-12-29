using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : EntityData
{
    [Header("Informations Générales")]
    public int money;

    [Header("Déplacements")]
    public float walkSpeed;
    public float runSpeed;
    public float smoothFactor;
    public bool canRun;
    public Vector3 lastPosition;

    public void InitializePokeIUTTeam()
    {
        if (pokeIUTTeamData == null || pokeIUTTeamData.Length == 0)
        {
            Debug.LogWarning("PokeIUTTeamData est vide.");
            return;
        }

        if (pokeIUTTeam == null || pokeIUTTeam.Length != pokeIUTTeamData.Length)
        {
            pokeIUTTeam = new PokeIUTInstance[pokeIUTTeamData.Length];
        }

        for (int i = 0; i < pokeIUTTeamData.Length; i++)
        {
            if (pokeIUTTeamData[i] == null)
            {
                Debug.LogError($"PlayerData | pokeIUTTeamData[{i}] est null. Veuillez assigner un PokeIUTData valide.");
                continue;
            }

            if (pokeIUTTeam[i] == null || pokeIUTTeam[i].baseData != pokeIUTTeamData[i])
            {
                pokeIUTTeam[i] = new PokeIUTInstance(pokeIUTTeamData[i]);
                pokeIUTTeam[i].state = new NormalState(pokeIUTTeam[i]);
            }

        }

        // Instanciate currentPokeIUT if it's not null or if it's not in the team
        if (currentPokeIUT != null && currentPokeIUT.baseData != null)
        {
            bool found = false;
            foreach (PokeIUTInstance pokeIUT in pokeIUTTeam)
            {
                if (pokeIUT.baseData == currentPokeIUT.baseData)
                {
                    currentPokeIUT = pokeIUT;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                currentPokeIUT = pokeIUTTeam[0];
            }
        }
        else
        {
            currentPokeIUT = pokeIUTTeam[0];
        }
    }

    public void InitializeItems()
    {
        if (itemsData == null || itemsData.Length == 0)
        {
            Debug.LogWarning("ItemData est vide.");
            return;
        }

        if (items == null)
        {
            items = new List<ItemInstance>(itemsData.Length);
        }

        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == null)
            {
                Debug.LogError($"PlayerData | itemsData[{i}] est null. Veuillez assigner un ItemData valide.");
                continue;
            }

            if (i >= items.Count || items[i] == null || items[i].baseData != itemsData[i])
            {
                if (i < items.Count)
                {
                    items[i] = new ItemInstance(itemsData[i]);
                }
                else
                {
                    items.Add(new ItemInstance(itemsData[i]));
                }
            }
        }
    }

    public void ResetPokeIUT()
    {
        for (int i = 0; i < pokeIUTTeam.Length; i++)
        {
            if (pokeIUTTeam[i] != null)
            {
                pokeIUTTeam[i].Reset();
            }
        }
    }

    public void AddItem(ItemData item)
    {
        Debug.Log("AddItem called with item: " + item.itemName);

        foreach (var playerItem in items)
        {
            Debug.Log("Checking item: " + playerItem.baseData.itemName);

            if (playerItem.baseData == item)
            {
                playerItem.quantity++;
                Debug.Log("Item found. New quantity: " + playerItem.quantity);
                return;
            }
        }

        var newItem = new ItemInstance(item);
        newItem.quantity = 1;
        items.Add(newItem);
        Debug.Log("New item added: " + item.itemName + " with quantity: " + newItem.quantity);
    }

    public int GetItemQuantity(ItemData item)
    {
        foreach (var playerItem in items)
        {
            if (playerItem.baseData == item)
            {
                return playerItem.quantity;
            }
        }
        return 0;
    }
}
