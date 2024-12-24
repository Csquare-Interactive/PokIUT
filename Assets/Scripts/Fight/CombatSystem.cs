using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatSystem
{

    public PlayerData Player { get; private set; }
    public EnemyData Enemy { get; private set; }
    public PokeIUTInstance PlayerPokeIUT { get; set; }
    public PokeIUTInstance EnemyPokeIUT { get; set; }
    private EnemyIA EnemyIA { get; set; }
    private string enemyAction;

    public event Action OnBattleStart;
    public event Action OnTurnEnd;
    public event Action OnBattleEnd;

    public CombatSystem(PlayerData player, EnemyData enemy)
    {
        Player = player;
        Enemy = enemy;
        PlayerPokeIUT = player.currentPokeIUT;
        EnemyPokeIUT = enemy.currentPokeIUT;
        EnemyIA = new EnemyIA(this);
    }

    public void StartBattle()
    {
        OnBattleStart?.Invoke();
    }

    public bool IsPlayerFirst() => PlayerPokeIUT.speed >= EnemyPokeIUT.speed;

    public int PlayerUseCapacite(int index)
    {
        PlayerPokeIUT.OnAction(); // Apply the state effect of the player's PokeIUT when attacking
        if (!PlayerPokeIUT.canAttack)
        {
            OnTurnEnd?.Invoke();
            return 0;
        }

        var capacite = PlayerPokeIUT.capacites[index];
        if (capacite.powerPoints <= 0) return 1;

        capacite.baseData.capacity.Use(Player, Enemy);
        capacite.powerPoints--;
        
        foreach (var pokeIUT in Enemy.pokIUTTeam)
        {
            if (pokeIUT.health > 0)
            {
                break;
            }
            OnBattleEnd?.Invoke();
        }

        OnTurnEnd?.Invoke();
        return 0;
    }

    public int PlayerSwitchPokeIUT(int index)
    {
        if (Player.pokIUTTeam[index].health <= 0) return 1;
        if (Player.pokIUTTeam[index] == PlayerPokeIUT) return 1;
        PlayerPokeIUT = Player.pokIUTTeam[index];
        Player.currentPokeIUT = PlayerPokeIUT;
        OnTurnEnd?.Invoke();
        return 0;
    }

    public int PlayerUseItem(ItemInstance item, PokeIUTInstance target)
    {
        switch (item.baseData.itemName)
        {
            case "Potion":
                if (target.health + 20 > target.baseData.maxHealth) return 1;
                target.health += 20;
                break;
            case "Super Potion":
                if (target.health + 50 > target.baseData.maxHealth) return 1;
                target.health += 50;
                break;
        }
        OnTurnEnd?.Invoke();
        return 0;
    }

    public void EnemyTurn()
    {
        enemyAction = EnemyIA.WhichActions();
        switch (enemyAction)
        {
            case "Switch": //////////////////////////////////////////////////////////

                PokeIUTInstance pokeIUT = EnemyIA.ChoosePokeIUT(Enemy.pokIUTTeam);
                EnemyPokeIUT = pokeIUT;
                Enemy.currentPokeIUT = pokeIUT;
                break;

            case "Heal": //////////////////////////////////////////////////////////

                ItemInstance[] healingItems = Array.FindAll(Enemy.items, i => i.baseData.itemType == ItemType.Healing);
                if (healingItems.Length == 0)
                {
                    OnTurnEnd?.Invoke();
                    return;
                }

                ItemInstance healingItem = healingItems[UnityEngine.Random.Range(0, healingItems.Length)];
                healingItem.quantity--;

                switch (healingItem.baseData.itemName)
                {
                    case "Potion":
                        if (EnemyPokeIUT.health + 20 > EnemyPokeIUT.baseData.maxHealth) break;
                        EnemyPokeIUT.health += 20;
                        break;
                    case "Super Potion":
                        if (EnemyPokeIUT.health + 50 > EnemyPokeIUT.baseData.maxHealth) break;
                        EnemyPokeIUT.health += 50;
                        break;
                }

                break;

            case "Attack": //////////////////////////////////////////////////////////

                EnemyPokeIUT.OnAction(); // Apply the state effect of the enemy's PokeIUT when attacking
                if (!EnemyPokeIUT.canAttack)
                {
                    OnTurnEnd?.Invoke();
                    return;
                }

                List<CapaciteInstance> validCapacites = EnemyPokeIUT.capacites.FindAll(c => c.powerPoints > 0);
                if (validCapacites.Count == 0)
                {
                    OnTurnEnd?.Invoke();
                    return;
                }

                CapaciteInstance capacite = EnemyIA.ChooseCapacity(validCapacites);
                capacite.baseData.capacity.Use(Enemy, Player);
                capacite.powerPoints--;

                foreach (PokeIUTInstance playerPokeIUT in Player.pokIUTTeam)
                {
                    if (playerPokeIUT.health > 0)
                    {
                        break;
                    }
                    OnBattleEnd?.Invoke();
                }

                break;
        }    

        OnTurnEnd?.Invoke();
    }

    public void EndBattle()
    {
        OnBattleEnd?.Invoke();
    }
}