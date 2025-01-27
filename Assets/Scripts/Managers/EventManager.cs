using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //Event gets fired everytime a state has been swapped
    public event Action<State> OnStateSwapped;

    public void StateSwapped(State state)
    {
        OnStateSwapped?.Invoke(state);
    }


    public event Action<GameObject> OnRoomCompleted;
    public void RoomCompleted(GameObject lastEnemy)
    {
        OnRoomCompleted?.Invoke(lastEnemy);
    }

    public event Action OnPauseGame;
    public void PauseGame()
    {
        OnPauseGame?.Invoke();
    }


}
