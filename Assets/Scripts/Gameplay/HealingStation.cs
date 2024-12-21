using UnityEngine;

public class HealingStation : MonoBehaviour
{
    

    public void HealAllPokeIUTs()
    {
        PlayerData playerData = GameManager.Instance.playerData;
        if (playerData != null)
        {
            foreach (var pokeIUT in playerData.pokIUTTeam)
            {
                if (pokeIUT != null)
                {
                    pokeIUT.health = pokeIUT.baseData.maxHealth;
                    foreach (var capacite in pokeIUT.capacites)
                    {
                    capacite.powerPoints = capacite.baseData.maxPowerPoints;
                    }
                    pokeIUT.canAttack = true;
                    pokeIUT.state = new NormalState(pokeIUT);
                    pokeIUT.UpdateStateDescription(); // Display correct state on Inspector
                }
            }
            Debug.Log("Tous les PokeIUTs ont été soignés !");
        }
    }
}