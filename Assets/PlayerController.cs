using UnityEngine;
using Photon.Pun;

using Desperados.Game;

public class PlayerController : MonoBehaviourPun
{
    public bool isDead = false;

    public void Die()
    {
        if (photonView.IsMine)
        {
            GameManagerSingleton.Instance.SyncDeath(PhotonNetwork.LocalPlayer.UserId, "");
        }
    }
    void OnEnable()
    {
        if (photonView.IsMine)
        {
            // Disable lobby camera and audio listeners
            LobbySingleton.Instance.DisableLobby();
        }
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            // Enabled lobby camera and listeners
            LobbySingleton.Instance.EnableLobby();
        }
    }

}
