using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPokeIUTData", menuName = "PokeIUT/PokeIUTData")]
public class PokeIUTData : ScriptableObject
{
    public string pokeiutName;
    public int level;
    public int maxHealth;
    public int health;
    public int maxSpeed;
    public int speed;
    public Sprite icon;
    public Type pokeiutType;
    public List<CapaciteData> capacites = new List<CapaciteData>(4);

    [Serializable]
    public enum Type
    {
        Fonctionnel,
        Procedural,
        OrienteeObjet,
    }

    public void ResetPokeIUT()
    {
        health = maxHealth;
        speed = maxSpeed;
        foreach (var capacite in capacites)
        {
            capacite.powerPoints = capacite.maxPowerPoints;
        }
    }
}
