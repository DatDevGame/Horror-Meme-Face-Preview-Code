using _SDK.UI;
using UnityEngine.UI;
using UnityEngine;
using Assets._SDK.Game;

public class EnemyListPanel : AbstractPanel
{
    [SerializeField] private Button lobbyButton;
    void Start()
    {
        lobbyButton?.onClick.AddListener(BackToLobby);
    }

    private void BackToLobby()
    {
        GameManager.Instance.Fire(GameTrigger.BackToLobbyHome);
    }
}