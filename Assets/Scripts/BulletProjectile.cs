using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10;
    [SerializeField] float bulletDamage = 10;

    [SerializeField] Transform vfxHitTarget;
    [SerializeField] Transform vfxHitNonTarget;

    Rigidbody bulletRigidBody;
    Health health;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        health = GetComponent<Health>();
        bulletRigidBody.velocity = transform.forward * bulletSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>()!=null)
        {
            Instantiate(vfxHitTarget,transform.position, Quaternion.identity);
            other.GetComponent<Health>().TakeDamage(bulletDamage);
        }
        else
        {
            Instantiate(vfxHitNonTarget,transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
