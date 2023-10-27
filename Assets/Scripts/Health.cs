using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Health : MonoBehaviour
{
    [SerializeField] float startingHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    ZombieManager zombieManager;

    public bool isDead=false;

    Animator animator;

    private void Start()
    {
        zombieManager=GetComponent<ZombieManager>();
        animator = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    private void Update()
    {
        Die();
    }
    public float TakeDamage(float damage)
    {
        return currentHealth-=damage;
    }
    private void Die()
    {
        if (currentHealth <= 0 && isDead==false)
        {
            animator.SetTrigger("Death");
            gameObject.GetComponent<Collider>().enabled = false;
            zombieManager.zombieNavMesh.enabled = false;
            isDead = true;
        }
    }
}
