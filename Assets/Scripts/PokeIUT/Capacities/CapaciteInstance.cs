[System.Serializable]
public class CapaciteInstance
{
    public CapaciteData baseData;
    public int power;
    public int accuracyLevel;
    public int powerPoints;

    public CapaciteInstance(CapaciteData data)
    {
        baseData = data;
        power = data.power;
        accuracyLevel = data.accuracyLevel;
        powerPoints = data.maxPowerPoints;
    }

    public void Reset()
    {
        power = baseData.power;
        accuracyLevel = baseData.accuracyLevel;
        powerPoints = baseData.maxPowerPoints;
    }

    public float GetAccuracy() => 1.0f / (1 + accuracyLevel * 0.25f); // Max 100% | Min 40%
}