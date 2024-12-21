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
        if (target.currentPokeIUT.state.GetType() != typeof(ParalyzedState))
            target.currentPokeIUT.state = new ParalyzedState(target.currentPokeIUT);
        else
            Debug.Log("PokeIUT Capacity(Lazy Loading) | Target is already paralyzed");
    }
}
