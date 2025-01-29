using UnityEngine;

public class SCR_Enemy : MonoBehaviour
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] public int enemyHealth = 30;
    [SerializeField] public int enemyDamage = 3;
    [SerializeField] private SCR_EnemyController enemyController;

    private Vector3 dir = Vector3.left;
    float targetPosRight, targetPosLeft;

    //Daniel did this, feel free to yeet, its only temp code for the build
    public bool shouldMove = true;
    public float speed = 1f;
    void Start()
    {
        targetPosRight = transform.position.x - 4;
        targetPosLeft = transform.position.x + 4;
        enemyController = SCR_EnemyController.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth < 1)
        {
            Defeat();
        }

        //Daniel did this, feel free to yeet, its only temp code for the build
        if(!shouldMove)
            return;
        transform.Translate(dir * speed * Time.deltaTime);

        if (transform.position.x <= targetPosRight)
        {
            dir = Vector3.right;
        }
        else if (transform.position.x >= targetPosLeft)
        {
            dir = Vector3.left;
        }
    }

    void Defeat()
    {
        isAlive = false;
        enemyController.DefeatEnemy(gameObject);
    }
}
