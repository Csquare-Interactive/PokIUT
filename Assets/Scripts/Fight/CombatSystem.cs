using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{

    public PokeIUTData Player { get; private set; }
    public PokeIUTData Enemy { get; private set; }
    public bool IsPlayerTurn { get; private set; }

    public event Action OnBattleStart;
    public event Action OnTurnEnd;
    public event Action OnBattleEnd;

    public CombatSystem(PokeIUTData player, PokeIUTData enemy)
    {
        Player = player;
        Enemy = enemy;
        IsPlayerTurn = Player.speed >= Enemy.speed;
    }

    public void StartBattle()
    {
        OnBattleStart?.Invoke();
    }

    public void PlayerUseCapacite(int index)
    {
        var capacite = Player.capacites[index];
        if (capacite.powerPoints <= 0) return;

        capacite.powerPoints--;
        Enemy.health -= capacite.damage;
        
        if (Enemy.health <= 0)
        {
            Debug.Log("La");
            Enemy.health = 0;
            OnBattleEnd?.Invoke();
            return;
        }

        Debug.Log("ici");
        OnTurnEnd?.Invoke();
        IsPlayerTurn = false;
    }

    public void EnemyTurn()
    {
        var validCapacites = Enemy.capacites.FindAll(c => c.powerPoints > 0);
        if (validCapacites.Count == 0)
        {
            IsPlayerTurn = true;
            return;
        }

        var chosenCapacite = validCapacites[UnityEngine.Random.Range(0, validCapacites.Count)];
        chosenCapacite.powerPoints--;
        Player.health -= chosenCapacite.damage;

        if (Player.health <= 0)
        {
            Player.health = 0;
            OnBattleEnd?.Invoke();
            return;
        }

        IsPlayerTurn = true;
    }

    public void EndBattle()
    {
        OnBattleEnd?.Invoke();
    }
}