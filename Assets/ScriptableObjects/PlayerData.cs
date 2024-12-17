using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Informations Générales")]
    public string playerName;
    public int money;

    [Header("Déplacements")]
    public float walkSpeed;
    public float runSpeed;
    public float smoothFactor;
    public bool canRun;
    public Vector3 lastPosition;

    [Header("Inventaire")]
    public int maxItems;
    public string[] items;

    [Header("PokIUT")]
    public int maxPokIUT;
    public PokeIUTData[] pokIUTInventory;
    public PokeIUTData[] pokIUTTeam;
    public PokeIUTData currentPokeIUT;

    [Header("TestInfos")]
    public string state;
}
