using UnityEngine;
using UnityEngine.UI;

public class SCR_MainMenuController : MonoBehaviour
{
    public Button StartGameButton;

    private void OnEnable()
    {
        StartGameButton.onClick.AddListener(GameManager.Instance.StartGame);
    }

    private void OnDisable()
    {
        StartGameButton.onClick.RemoveAllListeners();
    }
}
