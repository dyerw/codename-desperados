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
        if (photonView.IsMine)
        {
            renderController.ThirdPersonShadowsOnly();
            renderController.DisableFirstPersonShadows();
        } else
        {
            myCamera.enabled = false;
            myAudioListener.enabled = false;
            renderController.HideFirstPerson();
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
