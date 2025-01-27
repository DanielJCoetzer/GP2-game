using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SCR_EnemyController : MonoBehaviour
{
    [SerializeField] public List<GameObject> activeEnemies;
    [SerializeField] public  List<GameObject> deadEnemies;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckEnemies(GameObject lastEnemy)
    {
        if (activeEnemies.Count == 0)
        {
            Debug.Log("All enemies dead");
            EventManager.Instance.RoomCompleted(lastEnemy);
        }
    }

    public void DefeatEnemy(GameObject enemyObject)
    {
        if (!deadEnemies.Contains(enemyObject))
        {
            activeEnemies.Remove(enemyObject);
            deadEnemies.Add(enemyObject);
            enemyObject.SetActive(false);
            CheckEnemies(enemyObject);

        }
    }

}
