using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{

    public PlayerData Player { get; private set; }
    public PokeIUTData Enemy { get; private set; }
    public PokeIUTData PlayerPokeIUT { get; set; }

    public event Action OnBattleStart;
    public event Action OnTurnEnd;
    public event Action OnBattleEnd;

    public CombatSystem(PlayerData player, PokeIUTData enemy)
    {
        Player = player;
        Enemy = enemy;
        PlayerPokeIUT = player.currentPokeIUT;
    }

    public void StartBattle()
    {
        OnBattleStart?.Invoke();
    }

    public void PlayerUseCapacite(int index)
    {
        var capacite = PlayerPokeIUT.capacites[index];
        if (capacite.powerPoints <= 0) return;

        capacite.powerPoints--;
        Enemy.health -= capacite.damage;
        
        if (Enemy.health <= 0)
        {
            Enemy.health = 0;
            OnBattleEnd?.Invoke();
            return;
        }

        OnTurnEnd?.Invoke();
    }

    public void PlayerSwitchPokeIUT(int index)
    {
        PlayerPokeIUT = Player.pokIUTTeam[index];
        OnTurnEnd?.Invoke();
    }

    public void EnemyTurn()
    {
        var validCapacites = Enemy.capacites.FindAll(c => c.powerPoints > 0);
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