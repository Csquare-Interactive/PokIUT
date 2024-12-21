[System.Serializable]
public class CapaciteInstance
{
    public CapaciteData baseData;
    public int power;
    public int powerPoints;

    public CapaciteInstance(CapaciteData data)
    {
        baseData = data;
        power = data.power;
        powerPoints = data.maxPowerPoints;
    }

    public void Reset()
    {
        power = baseData.power;
        powerPoints = baseData.maxPowerPoints;
    }
}