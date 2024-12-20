using UnityEngine;

[System.Serializable]
public class PrimaryKeyPunch : Capacity
{
    public PrimaryKeyPunch() : base() {}
    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }
    public override void Use(EntityData self, EntityData target)
    {
        Debug.Log("PokeIUT Capacity(Primary Key Punch) | Critical Hit if Target have low Defense");
    }
}