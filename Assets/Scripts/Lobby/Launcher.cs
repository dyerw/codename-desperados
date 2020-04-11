using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Desperados
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        string gameVersion = "0.0.1";

        #region SerializeFields

        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [SerializeField]
        private GameObject controlPanel;

        [SerializeField]
        private GameObject progressLabel;

        #endregion

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        #region Public Methods

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

        #region Private Methods

        void LoadLevel()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Photon Network: Trying to Load a level but we are not the master Client");
            }
            PhotonNetwork.LoadLevel("Level");
        }

        #endregion 

        #region MonoBehaviour PunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("Desperados: OnConnectedToMaster was called by PUN");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Couldn't join a room, creating one instead");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("We joined a room.");
            PhotonNetwork.LoadLevel("Level");
        }



        #endregion
    }
}
