using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : EntityData
{
    [Header("Informations Générales")]
    public int money;

    [Header("Déplacements")]
    public float walkSpeed;
    public float runSpeed;
    public float smoothFactor;
    public bool canRun;
    public Vector3 lastPosition;

    public void InitializePokeIUTTeam()
    {
        if (pokIUTTeamData == null || pokIUTTeamData.Length == 0)
        {
            Debug.LogWarning("PokeIUTTeamData est vide.");
            return;
        }

        if (pokIUTTeam == null || pokIUTTeam.Length != pokIUTTeamData.Length)
        {
            pokIUTTeam = new PokeIUTInstance[pokIUTTeamData.Length];
        }

        for (int i = 0; i < pokIUTTeamData.Length; i++)
        {
            if (pokIUTTeamData[i] == null)
            {
                Debug.LogError($"PlayerData | pokIUTTeamData[{i}] est null. Veuillez assigner un PokeIUTData valide.");
                continue;
            }

            if (pokIUTTeam[i] == null || pokIUTTeam[i].baseData != pokIUTTeamData[i])
            {
                pokIUTTeam[i] = new PokeIUTInstance(pokIUTTeamData[i]);
                pokIUTTeam[i].state = new NormalState(pokIUTTeam[i]);
            }

        }

        // Instanciate currentPokeIUT if it's not null or if it's not in the team
        if (currentPokeIUT != null && currentPokeIUT.baseData != null)
        {
            bool found = false;
            foreach (PokeIUTInstance pokeIUT in pokIUTTeam)
            {
                if (pokeIUT.baseData == currentPokeIUT.baseData)
                {
                    currentPokeIUT = pokeIUT;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                currentPokeIUT = pokIUTTeam[0];
            }
        }
        else
        {
            currentPokeIUT = pokIUTTeam[0];
        }
    }

    public void InitializeItems()
    {
        if (itemsData == null || itemsData.Length == 0)
        {
            Debug.LogWarning("ItemData est vide.");
            return;
        }

        if (items == null || items.Length != itemsData.Length)
        {
            items = new ItemInstance[itemsData.Length];
        }

        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == null)
            {
                Debug.LogError($"PlayerData | itemsData[{i}] est null. Veuillez assigner un ItemData valide.");
                continue;
            }

            if (items[i] == null || items[i].baseData != itemsData[i])
            {
                items[i] = new ItemInstance(itemsData[i]);
                items[i].quantity = itemsData[i].maxQuantity; //NOTE: Temporaire, le temps de permettre au joueur de récupérer des items //
            }
        }
    }

    public void ResetPokeIUT()
    {
        for (int i = 0; i < pokIUTTeam.Length; i++)
        {
            if (pokIUTTeam[i] != null)
            {
                pokIUTTeam[i].Reset();
            }
        }
    }
}
