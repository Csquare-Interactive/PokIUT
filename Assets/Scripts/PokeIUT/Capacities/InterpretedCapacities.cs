using UnityEngine;

[System.Serializable]
public class ClasseAbstraite : Capacity
{
    public ClasseAbstraite() : base() {}

    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }

    public override void Use(EntityData self, EntityData target)
    {
        Debug.Log("PokeIUT Capacity(Classe Abstraite) | Target Get Paralyzed");
    }
}