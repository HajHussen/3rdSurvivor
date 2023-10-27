using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEffectManager : MonoBehaviour
{
    ZombieManager zombieManager;

    private void Awake()
    {
        zombieManager = GetComponent<ZombieManager>();
    }

    public void DamageZombieHead()
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("Big Hit To Head", 0.2f);
    }

    public void DamageZombieTorso()
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("Hit To Body", 0.2f);
    }

    public void DamageZombieRightArm()
    {

    }

    public void DamageZombieLeftArm()
    {

    }

    public void DamageZombieRightLeg()
    {

    }

    public void DamageZombieLeftLeg()
    {

    }
}
