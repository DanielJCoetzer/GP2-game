using UnityEngine;

public class SCR_ProjectileBehavior : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
