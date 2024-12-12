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
    public float maxSlopeAngle;

    [Header("Inventaire")]
    public int maxItems;
    public string[] items;

    [Header("PokIUT")]
    public int maxPokIUT;
    public string[] pokIUTInventory;
    public string[] pokIUTTeam;

    [Header("TestInfos")]
    public string state;
}
