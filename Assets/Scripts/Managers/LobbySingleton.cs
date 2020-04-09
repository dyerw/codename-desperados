using UnityEngine;

public class LobbySingleton : MonoBehaviour
{
    [SerializeField]
    Camera lobbyCamera;

    [SerializeField]
    AudioListener lobbyAudioListener;

    public static LobbySingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
        }
    }

    public void EnableLobby()
    {
        lobbyCamera.enabled = true;
        lobbyAudioListener.enabled = true;
    }

    public void DisableLobby()
    {
        lobbyCamera.enabled = false;
        lobbyAudioListener.enabled = false;
    }
}
