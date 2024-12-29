using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : EntityData
{
    [Header("Informations Générales")]
    public EnemyType enemyType;

    [Header("Déplacements")]
    public Vector3 lastPosition;

    public void InitializePokeIUTTeam()
    {
        if (pokeIUTTeamData == null || pokeIUTTeamData.Length == 0)
        {
            Debug.LogWarning("PokeIUTTeamData est vide.");
            return;
        }

        pokeIUTTeam = new PokeIUTInstance[pokeIUTTeamData.Length];

        for (int i = 0; i < pokeIUTTeamData.Length; i++)
        {
            pokeIUTTeam[i] = new PokeIUTInstance(pokeIUTTeamData[i]);
        }

        currentPokeIUT = pokeIUTTeam[0];
    }

    public void InitializeItems()
    {
        if (itemsData == null || itemsData.Length == 0)
        {
            Debug.LogWarning("ItemData est vide.");
            return;
        }

        items = new List<ItemInstance>(itemsData.Length); // Utiliser une List<ItemInstance>

        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == null)
            {
                Debug.LogError($"EnemyData | itemsData[{i}] est null. Veuillez assigner un ItemData valide.");
                continue;
            }

            items.Add(new ItemInstance(itemsData[i]));
        }
    }
}

public enum EnemyType
{
    Trainer,
    Wild
}
