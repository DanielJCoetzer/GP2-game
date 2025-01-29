using UnityEngine;

public class SCR_ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private int bulletDamage;

    public void SetDamage(int damage) {
        bulletDamage = damage;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
    /* private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    } */
}
