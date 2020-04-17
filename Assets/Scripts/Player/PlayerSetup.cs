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
    [SerializeField] FirstPersonHUDController fpsHudController;
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
            fpsHudController.DisableHUD();
            myCamera.enabled = false;
            myAudioListener.enabled = false;
            renderController.HideFirstPerson();
            renderController.ShowThirdPerson();
        }
    }

}
