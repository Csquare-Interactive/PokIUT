using UnityEngine;

[System.Serializable]
public class LazyLoading : Capacity
{
    public LazyLoading() : base() {}

    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }

    public override void Use(EntityData self, EntityData target)
    {
        Debug.Log("PokeIUT Capacity(Lazy Loading) | Target Can't Attack Next Turn");
    }
}
