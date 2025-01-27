using UnityEngine;

public class GameManager : StateMachine
{
    public static GameManager Instance;

    [Header("Toggle off if you want to run tests in your own scene")]
    public bool ShouldLoadMainMenu = true;

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

    private void Start()
    {
        if(ShouldLoadMainMenu)
            SwitchState<StMainMenu>();
    }

    public void StartGame()
    {
        SwitchState<StLoadGame>();
    }
}
