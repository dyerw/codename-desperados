using UnityEngine;

using Photon.Pun;

using Desperados.Game;

/**
 * Handles disabling/enabling things on spawn
 */
public class PlayerSetup : MonoBehaviourPun 
{
    [SerializeField] Camera myCamera;
    [SerializeField] AudioListener myAudioListener;
    [SerializeField] RenderController renderController;
    void Start()
    {
        SpawnSetup();
    }
    public void SpawnSetup()
    {
        if (photonView.IsMine)
        {
            LobbySingleton.Instance.DisableLobby();
            myCamera.enabled = true;
            myAudioListener.enabled = true;
            renderController.ThirdPersonShadowsOnly();
            renderController.DisableFirstPersonShadows();
            renderController.ShowFirstPerson();
            renderController.ThirdPersonNotShootable();
        } else
        {
            myCamera.enabled = false;
            myAudioListener.enabled = false;
            renderController.HideFirstPerson();
            renderController.ShowThirdPerson();
        }
    }

    void OnEnable()
    {
        string _netID = photonView.ViewID.ToString();
        // Register the player with the game state
        GameManagerSingleton.Instance.RegisterPlayer(_netID, gameObject);

        // Disable lobby camera and audio listeners
        LobbySingleton.Instance.DisableLobby();
    }

    private void OnDisable()
    {
        // Enabled lobby camera and listeners
        LobbySingleton.Instance.EnableLobby();

        // Remove player from game state
        GameManagerSingleton.Instance.UnRegisterPlayer(transform.name);
    }
}
