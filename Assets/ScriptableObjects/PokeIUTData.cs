using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPokeIUTData", menuName = "PokeIUT/PokeIUTData")]
public class PokeIUTData : ScriptableObject
{
    public string pokeiutName;
    public int level;
    public int health;
    public string[] capacites;
    public Sprite icon;
    public Type pokeiutType;

    [Serializable]
    public enum Type
    {
        Fonctionnel,
        Procedural,
        OrienteeObjet,
    }
}