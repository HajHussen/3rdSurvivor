using UnityEngine;

public class AttackState : State
{
    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Speed", 0, .2f, Time.deltaTime);
            return this;
        }

        Debug.Log("attack");
        zombieManager.animator.SetFloat("Speed", 0, .2f, Time.deltaTime);
        return this;
    }
}
