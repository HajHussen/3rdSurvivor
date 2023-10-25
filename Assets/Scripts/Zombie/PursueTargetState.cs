using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    public override State Tick(ZombieManager zombieManager)
    {
        return this;
    }
}
