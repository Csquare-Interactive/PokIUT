using UnityEngine;
using System.Collections.Generic;

public class EnemyIA
{
    CombatSystem CombatSystem { get; set; }

    private ItemInstance[] items;

    public EnemyIA(CombatSystem combatSystem)
    {
        CombatSystem = combatSystem;
        items = CombatSystem.Enemy.items;
    }

    public string WhichActions()
    {
        // If the enemy's health is less than 33%
        Debug.Log("Have healing item: " + HaveHealingItem());
        Debug.Log("Switch ? " + (GetEnemyPokeIUTHealth() <= GetEnemyPokeIUTMaxHealth() / 3));
        Debug.Log("Heal ? " + (GetEnemyPokeIUTHealth() <= GetEnemyPokeIUTMaxHealth() / 1.5f && HaveHealingItem() && GetPlayerPokeIUTHealth() >= GetPlayerPokeIUTMaxHealth() / 3));
        if (GetEnemyPokeIUTHealth() <= GetEnemyPokeIUTMaxHealth() / 3)
        {
            int random = Random.Range(0, 3);
            if (random == 0) return "Switch";
        }

        // If the enemy's health is less than 75% and the enemy has a healing item and the player's health is greater than 33%
        if (GetEnemyPokeIUTHealth() <= GetEnemyPokeIUTMaxHealth() / 1.5f && HaveHealingItem() && GetPlayerPokeIUTHealth() >= GetPlayerPokeIUTMaxHealth() / 3)
        {
            int random = Random.Range(0, 7);
            if (random == 0) return "Heal";
        }

        return "Attack";
    }

    public CapaciteInstance ChooseCapacity(List<CapaciteInstance> capacites)
    {
        int playerHealth = GetPlayerPokeIUTHealth();
        int enemyHealth = GetEnemyPokeIUTHealth();
        foreach(CapaciteInstance capacite in capacites)
        {
            if (capacite.GetDamage() >= playerHealth) return capacite;
        }
        return capacites[Random.Range(0, capacites.Count)];
    }

    public PokeIUTInstance ChoosePokeIUT(PokeIUTInstance[] pokeIUTs)
    {
        PokeIUTInstance pokeIUT = pokeIUTs[0];
        foreach (PokeIUTInstance pokeIUTInstance in pokeIUTs)
        {
            if (pokeIUTInstance.health > pokeIUT.health) pokeIUT = pokeIUTInstance;
        }
        return pokeIUT;
    }

    private bool HaveHealingItem()
    {
        foreach (var item in items)
        {
            if (item.baseData.itemType == ItemType.Healing && item.quantity > 0) return true;
        }
        return false;
    }

    private int GetEnemyPokeIUTHealth() => CombatSystem.EnemyPokeIUT.health;
    private int GetEnemyPokeIUTMaxHealth() => CombatSystem.EnemyPokeIUT.baseData.maxHealth;
    private int GetPlayerPokeIUTHealth() => CombatSystem.PlayerPokeIUT.health;
    private int GetPlayerPokeIUTMaxHealth() => CombatSystem.PlayerPokeIUT.baseData.maxHealth;    
}