using System.Collections.Generic;

[System.Serializable]
public class PokeIUTInstance
{
    public PokeIUTData baseData;
    public int health;
    public int speed;
    public int level;
    public List<CapaciteInstance> capacites;

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
    }

    public void Reset()
    {
        health = baseData.maxHealth;
        speed = baseData.maxSpeed;
        foreach (var capacite in capacites)
        {
            capacite.Reset();
        }
    }
}