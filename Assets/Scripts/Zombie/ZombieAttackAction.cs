using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Zombie Attack Action")]
public class ZombieAttackAction : ScriptableObject
{
    public string attackAnimation;
    public float attackCooldown = 5;
    public float maximumAttackAngle = 20;
    public float minimumAttackAngle = -20;
    public float maximumAttackDistance = 1;
    public float minimumAttackDistance = 3.5f;
}
