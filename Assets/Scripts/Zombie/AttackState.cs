using UnityEngine;

public class AttackState : State
{
    public override State Tick(ZombieManager zombieManager)
    {
        Debug.Log("attack");
        zombieManager.animator.SetFloat("Speed", 0, .2f, Time.deltaTime);
        return this;
    }
}
