public class IdleState : State
{
    bool hasTarget;

    PursueTargetState pursueTargetState;

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick()
    {
        if (hasTarget)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
