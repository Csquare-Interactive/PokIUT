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
    public event Action<string> OnBattleEnd;

    public CombatSystem(PokeIUTData player, PokeIUTData enemy)
    {
        Player = ClonePokeIUTData(player);
        Enemy = ClonePokeIUTData(enemy);
        IsPlayerTurn = Player.speed >= Enemy.speed;
    }

    private PokeIUTData ClonePokeIUTData(PokeIUTData original) => ScriptableObject.Instantiate(original);

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
        OnTurnEnd?.Invoke();

        if (Enemy.health <= 0)
        {
            Enemy.health = 0;
            OnBattleEnd?.Invoke("Player wins!");
            return;
        }

        IsPlayerTurn = false;
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        var validCapacites = Enemy.capacites.FindAll(c => c.powerPoints > 0);
        if (validCapacites.Count == 0)
        {
            OnBattleEnd?.Invoke("Enemy has no PP left! Player's turn.");
            IsPlayerTurn = true;
            return;
        }

        var chosenCapacite = validCapacites[UnityEngine.Random.Range(0, validCapacites.Count)];
        chosenCapacite.powerPoints--;
        Player.health -= chosenCapacite.damage;
        OnTurnEnd?.Invoke();

        if (Player.health <= 0)
        {
            Player.health = 0;
            OnBattleEnd?.Invoke("Enemy wins!");
            return;
        }

        IsPlayerTurn = true;
    }
}