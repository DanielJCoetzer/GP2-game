using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SCR_Shotgun : MonoBehaviour
{
    [SerializeField] private float range = 100f;
    [SerializeField] private int damage = 5;
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float hitSpotLifetime = 2f;

    [Header("Shotgun Parameters")]
    [SerializeField] private float spreadArea = 5f; 
    [SerializeField] private int pelletsPerShot = 8; 

    [Header("Reload Values")]
    [SerializeField] private int clipSize = 6;
    [SerializeField] private int shotsFired = 0;
    [SerializeField] private float reloadSpeed = 2.5f;
    private bool _isReloading = false;

    [Header("HUD Reference")]
    [SerializeField] private SCR_HeadsUpDisplay hud;

    void Awake()
    {
        hud = SCR_HeadsUpDisplay.Instance;

    }
    void Start() {
        hud.pointReticle.SetActive(false);
        hud.shotgunReticle.SetActive(true);
        hud.UpdateAmmoCount(shotsFired, clipSize);

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (shotsFired < clipSize))
        {
            ShootShotgun();
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void OnEnable()
    {
        hud.UpdateAmmoCount(shotsFired, clipSize);
        hud.pointReticle.SetActive(false);
        hud.shotgunReticle.SetActive(true);
    }

    private void OnDisable()
    {

    }

    private void ShootShotgun() {
        shotsFired++;
        hud.UpdateAmmoCount(shotsFired, clipSize);

        for (int i = 0; i < pelletsPerShot; i++)
        {
            // Generate a random spread direction for each of the pellets
            Vector3 spreadDirection = fpsCamera.transform.forward +
                                       new Vector3(
                                           Random.Range(-spreadArea, spreadArea),
                                           Random.Range(-spreadArea, spreadArea),
                                           Random.Range(-spreadArea, spreadArea)
                                       ).normalized * 0.1f;

            if (Physics.Raycast(fpsCamera.transform.position, spreadDirection, out RaycastHit hit, range))
            {
                Debug.Log($"Hit: {hit.collider.name}");
                SCR_Enemy enemy = hit.collider.GetComponent<SCR_Enemy>();
                if (enemy != null)
                {
                    Debug.Log("Damaged Enemy");
                    enemy.enemyHealth -= damage;
                }

                GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                StartCoroutine(DestroyHitSpot(hitEffect, hitSpotLifetime));
            }
        }
    }

    private IEnumerator DestroyHitSpot(GameObject hitSpot, float lifetime) {
        yield return new WaitForSeconds(lifetime);
        Destroy(hitSpot);
    }

    private IEnumerator Reload() {
        _isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadSpeed);
        shotsFired = 0;
        _isReloading = false;
        hud.UpdateAmmoCount(shotsFired, clipSize);
        Debug.Log("Reload complete!");
    }
}
