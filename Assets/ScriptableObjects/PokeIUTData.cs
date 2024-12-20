using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPokeIUTData", menuName = "PokeIUT/PokeIUTData")]
public class PokeIUTData : ScriptableObject
{
    public string pokeiutName;
    public int level;
    public int maxHealth;
    public int maxSpeed;
    public Sprite icon;
    public Type pokeiutType;
    public List<CapaciteData> capacites = new List<CapaciteData>();

    [Serializable]
    public enum Type
    {
        Interprete,
        Compile,
        BDD,
        Asynchrone,
        Web,
        TrashTier
    }
}
