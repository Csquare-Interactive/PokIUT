[System.Serializable]
public class CapaciteInstance
{
    public CapaciteData baseData;
    public int damage;
    public int powerPoints;

    public CapaciteInstance(CapaciteData data)
    {
        baseData = data;
        damage = data.maxDamage;
        powerPoints = data.maxPowerPoints;
    }

    public void Reset()
    {
        damage = baseData.maxDamage;
        powerPoints = baseData.maxPowerPoints;
    }
}