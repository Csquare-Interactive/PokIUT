using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Informations Générales")]
    public string enemyName;

    [Header("Déplacements")]
    public Vector3 lastPosition;

    [Header("Inventaire")]
    public int maxItems;
    public string[] items;

    [Header("PokIUT")]
    public int maxPokIUT;
    public PokeIUTData[] pokIUTInventoryData;
    public PokeIUTData[] pokIUTTeamData;
    public PokeIUTData currentPokeIUTData;

    public PokeIUTInstance[] pokIUTInventory;
    public PokeIUTInstance[] pokIUTTeam;
    public PokeIUTInstance currentPokeIUT;

    public void InitializePokeIUTTeam()
    {
        if (pokIUTTeamData == null || pokIUTTeamData.Length == 0)
        {
            Debug.LogWarning("PokeIUTTeamData est vide.");
            return;
        }

        pokIUTTeam = new PokeIUTInstance[pokIUTTeamData.Length];

        for (int i = 0; i < pokIUTTeamData.Length; i++)
        {
            // Check if the PokeIUT is already in the team
            if (pokIUTTeam[i] == null || pokIUTTeam[i].baseData != pokIUTTeamData[i])
            {
                pokIUTTeam[i] = new PokeIUTInstance(pokIUTTeamData[i]);
                Debug.Log($"ENEMY DATA | Instancié {pokIUTTeamData[i].pokeiutName} dans PokeIUTTeam.");
            }
            else
            {
                Debug.Log($"ENEMY DATA | {pokIUTTeamData[i].pokeiutName} est déjà présent dans PokeIUTTeam.");
            }
        }

        currentPokeIUT = pokIUTTeam[0];
    }
}
