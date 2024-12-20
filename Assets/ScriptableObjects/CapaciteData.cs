using System;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "NewCapacite", menuName = "PokeIUT/Capacite")]
public class CapaciteData : ScriptableObject
{
    [Header("Informations Générales")]
    public string capaciteName;
    public string description;
    [Header("Stats de la capacité")]
    public int power;
    public int maxPowerPoints;
    public int powerPoints;
    [Header("Infos de la capacité")]
    public PokeIUTData.Type type;

    [Header("Effets spéciaux")]
    public int powerDebuff;
    [Header("Capacité Concrète")]
    [SerializeReference] 
    public Capacity capacity;
}

[System.Serializable]
public abstract class Capacity
{
    public CapaciteData data;

    protected Capacity() {}

    public virtual void Initialize(CapaciteData data)
    {
        this.data = data;
    }
    public abstract void Use(EntityData self, EntityData target);

    public int GetDamage(int power)
    {
        Random random = new Random();
        int variation = random.Next(-10, 11); // Generates a number between -10 and 10
        return power + (power * variation / 100);
    }
}
