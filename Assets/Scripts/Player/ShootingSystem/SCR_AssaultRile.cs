using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SCR_AssaultRile : MonoBehaviour
{
    [SerializeField] float range = 100f;
    [SerializeField] int damage = 2;
    [SerializeField] Camera fpsCamera;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] float hitSpotLifetime = 2f;

    [Header("Firing Values")]
    [SerializeField] private float fireRate = 0.1f; 
    private bool isFiring = false;

    [Header("Reload Values")]
    [SerializeField] private int clipSize = 30;
    [SerializeField] private int shotsFired;
    [SerializeField] private float reloadSpeed = 2.0f;
    bool _isReloading = false;

    [Header("HUD Reference")]
    [SerializeField] private SCR_HeadsUpDisplay hud;

    void Awake() {
        hud = SCR_HeadsUpDisplay.Instance;

    }

    void Start() {
        hud = SCR_HeadsUpDisplay.Instance;
        hud.UpdateAmmoCount(shotsFired, clipSize);
        hud.pointReticle.SetActive(true);
        hud.shotgunReticle.SetActive(false);

    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKey(KeyCode.Mouse0) && shotsFired < clipSize && !_isReloading)
        {
            if (!isFiring)
            {
                StartCoroutine(HandleContinuousFire());
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isFiring = false;
        }
    }

    void OnEnable() {
        if (hud == null)
        {
            return;
        }
        hud.UpdateAmmoCount(shotsFired, clipSize);
        hud.pointReticle.SetActive(true);
        hud.shotgunReticle.SetActive(false);
        _isReloading = false;
    }


    private void ShootRay() {
        AudioManager.Instance.Play("Pew pew");
        shotsFired++;
        hud.UpdateAmmoCount(shotsFired, clipSize);
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
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

    private IEnumerator HandleContinuousFire() {
        isFiring = true;
        while (isFiring && shotsFired < clipSize)
        {
            ShootRay();
            yield return new WaitForSeconds(fireRate);
        }
        isFiring = false;
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
