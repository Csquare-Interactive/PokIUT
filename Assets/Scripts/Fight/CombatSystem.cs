using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

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
    public event Action OnPokeIUTDead;

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

    public bool IsPokeIUTsAvailable(PokeIUTInstance[] pokeIUTs) => pokeIUTs.Any(p => p.health > 0);

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
        
        if (EnemyPokeIUT.health <= 0)
        {
            if (!IsPokeIUTsAvailable(Enemy.pokeIUTTeam))
            {
                OnBattleEnd?.Invoke();
                return 0;
            }
            EnemyPokeIUT = EnemyIA.ChoosePokeIUT(Enemy.pokeIUTTeam);
            Enemy.currentPokeIUT = EnemyPokeIUT;
        }

        OnTurnEnd?.Invoke();
        return 0;
    }

    public int PlayerSwitchPokeIUT(int index)
    {
        if (Player.pokeIUTTeam[index].health <= 0) return 1;
        if (Player.pokeIUTTeam[index] == PlayerPokeIUT) return 1;
        PlayerPokeIUT = Player.pokeIUTTeam[index];
        Player.currentPokeIUT = PlayerPokeIUT;
        OnTurnEnd?.Invoke();
        return 0;
    }

    public int PlayerUseItem(ItemInstance item, PokeIUTInstance target = null)
    {
        switch (item.baseData.itemName)
        {
            case "Potion":
                if (target == null || target.health + 20 > target.baseData.maxHealth) return 1;
                target.health += 20;
                break;
            case "Super Potion":
                if (target == null || target.health + 50 > target.baseData.maxHealth) return 1;
                target.health += 50;
                break;
            case "Pokiutball":
                Debug.Log("PokeIUT type: " + Enemy.enemyType);
                if (Enemy.enemyType != EnemyType.Wild || EnemyPokeIUT == null) return 1; // Only allow capture for wild enemies

                // Calculate capture probability
                float captureProbability = Mathf.Clamp01(1.0f - (float)EnemyPokeIUT.health / EnemyPokeIUT.baseData.maxHealth);
                if (Random.Range(0f, 1f) <= captureProbability)
                {
                    // Capture successful
                    Debug.Log("Capture successful!");
                    AddPokeIUTToTeam(EnemyPokeIUT);
                    Debug.Log("PokeIUT added to team: " + EnemyPokeIUT.baseData.name);
                    OnBattleEnd?.Invoke();
                }
                else
                {
                    // Capture failed
                    Debug.Log("Capture failed!");
                }
                return 0;

        }
        OnTurnEnd?.Invoke();
        return 0;
    }

public void AddPokeIUTToTeam(PokeIUTInstance pokeIUT)
{
    Debug.Log("Attempting to add PokeIUT to team");

    for (int i = 0; i < Player.pokeIUTTeam.Length; i++)
    {
        if (Player.pokeIUTTeam[i] == null)
        {
            Player.pokeIUTTeam[i] = pokeIUT;
            Debug.Log("PokeIUT added to team at position: " + i);
            return;
        }
    }
    Debug.LogWarning("No space available in the team to add a new PokeIUT.");
}

    public void EnemyTurn()
    {
        enemyAction = EnemyIA.WhichActions();
        switch (enemyAction)
        {
            case "Switch": //////////////////////////////////////////////////////////

                PokeIUTInstance pokeIUT = EnemyIA.ChoosePokeIUT(Enemy.pokeIUTTeam);
                EnemyPokeIUT = pokeIUT;
                Enemy.currentPokeIUT = pokeIUT;
                break;

            case "Heal": //////////////////////////////////////////////////////////

                ItemInstance[] healingItems = Enemy.items.Where(i => i.baseData.itemType == ItemType.Healing).ToArray();
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

                if (PlayerPokeIUT.health <= 0)
                {
                    if (!IsPokeIUTsAvailable(Player.pokeIUTTeam))
                    {
                        OnBattleEnd?.Invoke();
                        return;
                    }
                    OnPokeIUTDead?.Invoke();
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