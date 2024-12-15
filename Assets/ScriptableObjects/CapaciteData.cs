using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCapacite", menuName = "PokeIUT/Capacite")]
public class CapaciteData : ScriptableObject
{
    public string capaciteName;
    public string description;
    public int maxDamage;
    public int damage;
    public int maxPowerPoints;
    public int powerPoints;
    public PokeIUTData.Type type;
}