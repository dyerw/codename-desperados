using UnityEngine;
using UnityEngine.Networking;

/**
 * Handles disabling/enabling things on spawn
 */
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
    [SerializeField] Camera myCamera;
    [SerializeField] RenderController renderController;
    void Start()
    {
        // If this Player instance is NOT the local player disable all the linked stuff
        if (!isLocalPlayer)
        {
            renderController.HideFirstPerson();
            foreach (Behaviour b in componentsToDisable)
            {
                b.enabled = false;
            }
        } else
        {
            renderController.ThirdPersonShadowsOnly();
            renderController.DisableFirstPersonShadows();
            // Disable all cameras that aren't the first person camera for this player
            Camera[] allCameras = Camera.allCameras;
            foreach (Camera c in allCameras)
            {
                if (c != myCamera)
                {
                    c.enabled = false;
                }
            }


        }

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // Register the player with the game state
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
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
