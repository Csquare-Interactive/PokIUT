using UnityEngine;

[System.Serializable]
public class FatalIndent : Capacity
{
    public FatalIndent() : base() {}

    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }

    public override void Use(EntityData self, EntityData target)
    {
        if (target.currentPokeIUT.state is not IndentState)
        {
            target.currentPokeIUT.health -= Capacity.GetDamage(data.power, -5, 5);
            if (Random.Range(0, 100) <= 75) target.currentPokeIUT.state = new IndentState(target.currentPokeIUT);
        }
        else
            target.currentPokeIUT.health -= Capacity.GetDamage(data.power/2-10, -5, 5);
    }
}

[System.Serializable]
public class PipPipHourra : Capacity
{
    public PipPipHourra() : base() {}

    public override void Initialize(CapaciteData data)
    {
        base.Initialize(data);
    }

    public override void Use(EntityData self, EntityData target)
    {
        if (!data.isInUse)
        {
            self.waitingTurns = 1;
            self.currentPokeIUT.savedCapacity = this;
            data.isInUse = true;
        }
        if (self.waitingTurns <= 0)
        {
            self.currentPokeIUT.health += self.currentPokeIUT.baseData.maxHealth / 3;
            if (self.currentPokeIUT.health > self.currentPokeIUT.baseData.maxHealth) self.currentPokeIUT.health = self.currentPokeIUT.baseData.maxHealth;
            self.waitingTurns = 0;
            data.isInUse = false;
        }
    }
}