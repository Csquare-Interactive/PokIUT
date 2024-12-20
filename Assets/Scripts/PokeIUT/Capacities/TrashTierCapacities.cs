using UnityEngine;

[System.Serializable]
public class UndifinedMadness : Capacity
{
    public UndifinedMadness() : base() {}

    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }

    public override void Use(EntityData self, EntityData target)
    {
        if(Random.Range(0, 5) == 0)
            target.currentPokeIUT.health -= Mathf.RoundToInt(base.GetDamage(data.power) * 1.5f);;
    }
}