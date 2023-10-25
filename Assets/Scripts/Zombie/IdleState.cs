using StarterAssets;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] float detectionRadius = 5;
    [SerializeField] LayerMask detectionLayer;

    [Header("Detection Angle Radius")]
    [SerializeField] float minimumDetectionAngle = -50;
    [SerializeField] float maximumDetectionAngle = 50;

    PursueTargetState pursueTargetState;

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            FindTargetViaLineOfSight(zombieManager);
            return this;
        }
    }

    private void FindTargetViaLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            ThirdPersonController player = colliders[i].transform.GetComponent<ThirdPersonController>();

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                {
                    RaycastHit hit;
                    float characterHeight = 1.8f;
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterHeight, player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(transform.position.x, characterHeight, transform.position.z);

                    if (Physics.Linecast(playerStartPoint, zombieStartPoint, out hit))
                    {
                        Debug.Log("blocked");
                    }
                    else
                    {
                        Debug.Log("see u");
                        zombieManager.currentTarget = player;
                    }
                }
            }
        }
    }
}
