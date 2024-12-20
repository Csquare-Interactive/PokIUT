using UnityEngine;

[System.Serializable]
public class CompileAndCrash : Capacity
{
    public CompileAndCrash() : base() {}
    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }
    public override void Use(EntityData self, EntityData target)
    {
        Debug.Log("PokeIUT Capacity(Compile And Crash) | Powerful Attack but have to wait 1 turn");
    }
}