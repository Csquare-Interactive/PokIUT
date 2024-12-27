using UnityEngine;

[System.Serializable]
public abstract class PokeIUTState
{
    public PokeIUTInstance pokeIUT;

    public PokeIUTState(PokeIUTInstance pokeIUT)
    {
        this.pokeIUT = pokeIUT;
    }

    public abstract void ApplyEffect();
    public abstract void OnStartTurn();
    public abstract void OnAction();
    public abstract void OnEndTurn();
}

[System.Serializable]
public class NormalState : PokeIUTState
{
    public NormalState(PokeIUTInstance pokeIUT) : base(pokeIUT)
    {}
    public override void ApplyEffect()
    {}

    public override void OnStartTurn()
    {}

    public override void OnAction()
    {}

    public override void OnEndTurn()
    {}
}

[System.Serializable]
public class ConfusedState : PokeIUTState
{
    public ConfusedState(PokeIUTInstance pokeIUT) : base(pokeIUT)
    {}
    public override void ApplyEffect()
    {
        Debug.Log("PokeIUT State(Confused): Confused");
        int damage = pokeIUT.GetDamage(15, -5, 5);
        pokeIUT.health -= damage;
    }

    public override void OnStartTurn()
    {}

    public override void OnAction()
    {
        if (Random.Range(0, 100) < 50)
        {
            pokeIUT.canAttack = true;
            pokeIUT.state = new NormalState(pokeIUT);
        }
        else
        {
            ApplyEffect();
            pokeIUT.canAttack = false;
        }
    }

    public override void OnEndTurn()
    {}
}

[System.Serializable]
public class ParalyzedState : PokeIUTState
{
    public ParalyzedState(PokeIUTInstance pokeIUT) : base(pokeIUT)
    {}
    public override void ApplyEffect()
    {

        Debug.Log("PokeIUT State(Paralyzed): Paralyzed");
        pokeIUT.canAttack = Random.Range(0, 100) < 25;
    }

    public override void OnStartTurn()
    {}

    public override void OnAction()
    {
        if (Random.Range(0, 100) < 50)
        {
            pokeIUT.canAttack = true;
            pokeIUT.state = new NormalState(pokeIUT);
        }
        else
        {
            ApplyEffect();
        }
    }

    public override void OnEndTurn()
    {}
}

[System.Serializable]
public class IndentState : PokeIUTState
{
    public IndentState(PokeIUTInstance pokeIUT) : base(pokeIUT)
    {}
    public override void ApplyEffect()
    {
        Debug.Log("PokeIUT State(Indent): Indented");
    }

    public override void OnStartTurn()
    {}

    public override void OnAction()
    {}

    public override void OnEndTurn()
    {
        if (Random.Range(0, 100) < 25)
        {
            pokeIUT.state = new NormalState(pokeIUT);
        }
        else
        {
            ApplyEffect();
        }
    }
}