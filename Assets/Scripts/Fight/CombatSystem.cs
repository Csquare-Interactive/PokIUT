using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{

    public PlayerData Player { get; private set; }
    public EnemyData Enemy { get; private set; }
    public PokeIUTInstance PlayerPokeIUT { get; set; }
    public PokeIUTInstance EnemyPokeIUT { get; set; }

    public event Action OnBattleStart;
    public event Action OnTurnEnd;
    public event Action OnBattleEnd;

    public CombatSystem(PlayerData player, EnemyData enemy)
    {
        Player = player;
        Enemy = enemy;
        PlayerPokeIUT = player.currentPokeIUT;
        EnemyPokeIUT = enemy.currentPokeIUT;
    }

    public void StartBattle()
    {
        OnBattleStart?.Invoke();
    }

    public bool IsPlayerFirst() => PlayerPokeIUT.speed >= EnemyPokeIUT.speed;

    public int PlayerUseCapacite(int index)
    {
        var capacite = PlayerPokeIUT.capacites[index];
        if (capacite.powerPoints <= 0) return 1;

        capacite.powerPoints--;
        EnemyPokeIUT.health -= capacite.damage;
        
        if (EnemyPokeIUT.health <= 0)
        {
            EnemyPokeIUT.health = 0;
            OnBattleEnd?.Invoke();
            return 0;
        }

        OnTurnEnd?.Invoke();
        return 0;
    }

    public void PlayerSwitchPokeIUT(int index)
    {
        PlayerPokeIUT = Player.pokIUTTeam[index];
        OnTurnEnd?.Invoke();
    }

    public void PlayerUseItem(ItemInstance item, PokeIUTInstance target)
    {
        switch (item.baseData.itemName)
        {
            case "Potion":
                if (target.health + 20 >= target.baseData.maxHealth) return;
                target.health += 20;
                break;
            case "Super Potion":
                if (target.health + 50 >= target.baseData.maxHealth) return;
                target.health += 50;
                break;
        }
        OnTurnEnd?.Invoke();
    }

    public void EnemyTurn()
    {
        var validCapacites = EnemyPokeIUT.capacites.FindAll(c => c.powerPoints > 0);
        if (validCapacites.Count == 0)
        {
            OnTurnEnd?.Invoke();
            return;
        }

        var chosenCapacite = validCapacites[UnityEngine.Random.Range(0, validCapacites.Count)];
        chosenCapacite.powerPoints--;
        PlayerPokeIUT.health -= chosenCapacite.damage;

        if (PlayerPokeIUT.health <= 0)
        {
            PlayerPokeIUT.health = 0;
            OnBattleEnd?.Invoke();
            return;
        }

        OnTurnEnd?.Invoke();
    }

    public void EndBattle()
    {
        OnBattleEnd?.Invoke();
    }
}