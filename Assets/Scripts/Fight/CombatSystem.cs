using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
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
    public event Action OnPokeIUTDead;
    public event Action<string> OnEnemyAction;

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
        if (Player.waitingTurns > 0)
        {
            Player.waitingTurns--;
            if (Player.waitingTurns == 0) // Use Waiting Capacity
            {
                PlayerPokeIUT.savedCapacity.Use(Player,Enemy);
                OnTurnEnd?.Invoke();
                return -3;
            }
            OnTurnEnd?.Invoke();
            return -2;
        }
        if (!PlayerPokeIUT.canAttack)
        {
            OnTurnEnd?.Invoke();
            return 0;
        }

        var capacite = PlayerPokeIUT.capacites[index];
        if (capacite.powerPoints <= 0) return 1;
        if (Random.Range(0f, 1f) >= capacite.GetAccuracy())
        {
            OnTurnEnd?.Invoke();
            return -1;
        }
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

                PokeIUTInstance pokeIUT = EnemyIA.ChoosePokeIUT(Enemy.pokeIUTTeam);
                EnemyPokeIUT = pokeIUT;
                Enemy.currentPokeIUT = pokeIUT;
                OnEnemyAction?.Invoke($"{Enemy.name} a changé de PokeIUT pour {pokeIUT.baseData.name} !");
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

                OnEnemyAction?.Invoke($"{Enemy.name} a utilisé {healingItem.baseData.itemName} sur {EnemyPokeIUT} !");

                break;

            case "Attack": //////////////////////////////////////////////////////////

                EnemyPokeIUT.OnAction(); // Apply the state effect of the enemy's PokeIUT when attacking
                if (!EnemyPokeIUT.canAttack)
                {
                    OnTurnEnd?.Invoke();
                    return;
                }

                if (Enemy.waitingTurns > 0)
                {
                    Enemy.waitingTurns--;
                    if (Enemy.waitingTurns == 0) // Use Waiting Capacity
                    {
                        EnemyPokeIUT.savedCapacity.Use(Player,Enemy);
                        OnEnemyAction?.Invoke($"{EnemyPokeIUT.baseData.name} a utilisé {EnemyPokeIUT.savedCapacity.data.capaciteName} !");
                        EnemyPokeIUT.savedCapacity = null;
                        OnTurnEnd?.Invoke();
                    }
                    OnTurnEnd?.Invoke();
                    OnEnemyAction?.Invoke($"{EnemyPokeIUT.baseData.name} attend pour lancer {EnemyPokeIUT.savedCapacity.data.capaciteName}...");
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
                OnEnemyAction?.Invoke($"{EnemyPokeIUT.baseData.name} a utilisé {capacite.baseData.capaciteName} !");

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