using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class ThirdPersonShooterController : MonoBehaviour
{
    [Range(.1f, 4)]
    [SerializeField] float aimSensitivity;
    [Range(1, 8)]
    [SerializeField] float normalSensitivity;

    [SerializeField] GameObject crosshair;

    [SerializeField] Rig aimRig;
    [SerializeField] CinemachineVirtualCamera aimCam;
    [SerializeField] Transform debugTransform;
    [SerializeField] Transform gunShotVFX;
    [SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform spawnBulletPosition;
    [SerializeField] LayerMask aimColliderLayerMask = new LayerMask();

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
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWordPosition = raycastHit.point;
        }

        if (starterAssets.aim)
        {
            aimCam.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1),1f,Time.deltaTime*10f));

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

        if (starterAssets.shoot)
        {
            Vector3 aimDir= (mouseWordPosition-spawnBulletPosition.position).normalized;
            Instantiate(bulletProjectilePrefab, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            Instantiate(gunShotVFX, spawnBulletPosition);
            starterAssets.shoot = false;
        }
    }
}
