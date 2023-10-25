using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public IdleState startingState;
    public ThirdPersonController currentTarget;

     State currentState;

    private void Awake()
    {
        currentState = startingState;
    }
    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        State nextState;
        if (currentState != null)
        {
            nextState=currentState.Tick(this);
            if (nextState != null )
            {
                currentState = nextState;
            }
        }
    }
}
