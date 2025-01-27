using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SCR_Shoot_Projectile : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float projectileSpeed = 100f;
    [SerializeField] float projectileLifeTime = 3f;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            FireProjectile();
            AudioManager.Instance.Play("Pew pew");
        }
    }

    void FireProjectile() {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(firePoint.forward.normalized * projectileSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyProjectile(projectile, projectileLifeTime));
        
    }

    private IEnumerator DestroyProjectile(GameObject projectile, float lifetime) {
        yield return new WaitForSeconds(lifetime);
        Destroy(projectile);
    }
}
