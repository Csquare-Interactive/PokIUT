using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokeIUTInstance
{
    public PokeIUTData baseData;
    public int health;
    public int speed;
    public int level;
    public bool canAttack = true;
    public List<CapaciteInstance> capacites;
    public PokeIUTState state;
    public Capacity savedCapacity;

    [SerializeField, Tooltip("Description of the current state")]
    private string stateDescription;

    public PokeIUTInstance(PokeIUTData data)
    {
        baseData = data;
        health = data.maxHealth;
        speed = data.maxSpeed;
        level = data.level;
        capacites = new List<CapaciteInstance>();
        foreach (var capData in data.capacites)
        {
            capacites.Add(new CapaciteInstance(capData));
        }
        state = new NormalState(this);
        UpdateStateDescription();
    }

    public void UpdateStateDescription()
    {
        stateDescription = state != null ? state.GetType().Name : "No State";
    }

    public void ApplyState()
    {
        this.state.ApplyEffect();
    }

    // StateInstances use this method to get their damage amount
    public int GetDamage(int power, int minVariation, int maxVariation) => Capacity.GetDamage(power, minVariation, maxVariation);

    public void Reset()
    {
        health = baseData.maxHealth;
        speed = baseData.maxSpeed;
        foreach (var capacite in capacites)
        {
            capacite.Reset();
        }
    }

    public void OnStartTurn()
    {
        state.OnStartTurn();
    }

    public void OnAction()
    {
        state.OnAction();
    }

    public void OnEndTurn()
    {
        state.OnEndTurn();
    }
}