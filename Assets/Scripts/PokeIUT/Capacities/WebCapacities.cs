using UnityEngine;

[System.Serializable]
public class ZIndexKick : Capacity
{
    public ZIndexKick() : base() {}

    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }

    public override void Use(EntityData self, EntityData target)
    {
        target.currentPokeIUT.health -= Capacity.GetDamage(data.power, -10, 10);
    }
}