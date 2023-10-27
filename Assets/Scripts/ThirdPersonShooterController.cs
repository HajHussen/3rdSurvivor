using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    [Range(.1f, 4)]
    [SerializeField] float aimSensitivity;
    [Range(1, 8)]
    [SerializeField] float normalSensitivity;

    [SerializeField] float weaponRange = 100f;
    [SerializeField] float bulletDamage = 10;

    [SerializeField] GameObject crosshair;

    [SerializeField] Rig aimRig;
    [SerializeField] CinemachineVirtualCamera aimCam;
    [SerializeField] Transform debugTransform;
    [SerializeField] Transform gunShotVFX;
    [SerializeField] Transform bulletEjectionVFX;
    //[SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform spawnBulletPosition;
    [SerializeField] Transform bulletEjectionPosition;
    [SerializeField] LayerMask shootableLayermask;
    [SerializeField] Transform vfxHitTarget;
    [SerializeField] Transform vfxHitNonTarget;

    ThirdPersonController thirdPersonController;
    StarterAssetsInputs starterAssets;

    Animator animator;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssets = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWordPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, weaponRange, shootableLayermask))
        {
            debugTransform.position = raycastHit.point;
            mouseWordPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        PlayerAim(mouseWordPosition);

        PlayerShoot(mouseWordPosition, hitTransform, raycastHit);
    }

    private void PlayerAim(Vector3 mouseWordPosition)
    {
        if (starterAssets.aim)
        {
            aimCam.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWordPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            thirdPersonController.SprintSpeed = 4f;

            crosshair.gameObject.SetActive(true);
            aimRig.weight = 1f;
        }

        else
        {
            aimCam.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

            thirdPersonController.SprintSpeed = 5.335f;

            crosshair.gameObject.SetActive(false);
            starterAssets.shoot = false;
            aimRig.weight = 0f;
        }
    }

    private void PlayerShoot(Vector3 mouseWordPosition, Transform hitTransform, RaycastHit raycastHit)
    {
        if (starterAssets.shoot)
        {
            if (hitTransform != null)
            {
                if (hitTransform.GetComponent<BulletTarget>() != null)
                {
                    Instantiate(vfxHitTarget, mouseWordPosition, Quaternion.identity);

                    Debug.Log(raycastHit.collider.gameObject.layer);
                    ZombieTakingShot(hitTransform, raycastHit);
                }
                else
                {
                    Instantiate(vfxHitNonTarget, mouseWordPosition, Quaternion.identity);
                }
            }
            //Vector3 aimDir = (mouseWordPosition - spawnBulletPosition.position).normalized;
            //Instantiate(bulletProjectilePrefab, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            Instantiate(gunShotVFX, spawnBulletPosition);
            Instantiate(bulletEjectionVFX, bulletEjectionPosition);
            starterAssets.shoot = false;
        }
    }

    private void ZombieTakingShot(Transform hitTransform, RaycastHit raycastHit)
    {
        ZombieEffectManager zombie = raycastHit.collider.gameObject.GetComponentInParent<ZombieEffectManager>();

        if (zombie != null && !zombie.GetComponent<Health>().isDead)
        {

            switch (raycastHit.collider.gameObject.layer)
            {
                case 8:
                    zombie.DamageZombieHead();

                    hitTransform.GetComponent<Health>().TakeDamage(bulletDamage*5);
                    break;
                case 9:
                    zombie.DamageZombieTorso();

                    hitTransform.GetComponent<Health>().TakeDamage(bulletDamage*2);
                    break;
                case 10:
                    zombie.DamageZombieRightArm();

                    hitTransform.GetComponent<Health>().TakeDamage(bulletDamage);
                    break;
                case 11:
                    zombie.DamageZombieLeftArm();

                    hitTransform.GetComponent<Health>().TakeDamage(bulletDamage);
                    break;
                case 12:
                    zombie.DamageZombieRightLeg();

                    hitTransform.GetComponent<Health>().TakeDamage(bulletDamage);
                    break;
                case 13:
                    zombie.DamageZombieLeftLeg();

                    hitTransform.GetComponent<Health>().TakeDamage(bulletDamage);
                    break;
                default:
                    // Handle the default case if needed.
                    break;
            }
        }
    }
}
