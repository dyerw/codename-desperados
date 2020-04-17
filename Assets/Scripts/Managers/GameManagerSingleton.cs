using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

namespace Desperados.Game
{
    public class GameManagerSingleton : MonoBehaviourPunCallbacks
    {
        private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
        private const string PLAYER_PREFIX = "Player ";

        public Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();

        [SerializeField]
        public GameObject[] spawnPoints;

        [SerializeField]
        public GameObject playerPrefab;

        [SerializeField]
        Weapon revolver;

        [SerializeField]
        int respawnTimeSeconds = 0;

        public static GameManagerSingleton Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            weapons.Add(revolver.name, revolver);
            StartCoroutine(SpawnPlayer(0));
        }

        IEnumerator SpawnPlayer(int delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            Debug.Log("DEBUG :: Spawning Player");
            Transform spawnTransform = GetRandomSpawnTransform();
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransform.position, spawnTransform.rotation);
            photonView.RPC("RpcRegisterPlayerObject", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.UserId, player.GetPhotonView().ViewID);
        }

        public void SyncDeath(string killedUserId, string killedByUserId)
        {
            Debug.Log("DEBUG :: Running SyncDeath");
            GameObject player = GetPlayer(killedUserId);
            PhotonNetwork.Destroy(player);
            StartCoroutine(SpawnPlayer(respawnTimeSeconds));
        }
        
        private Transform GetRandomSpawnTransform()
        {
            int index = Random.Range(0, spawnPoints.Length);
            return spawnPoints[index].transform;
        }

        #region Public Methods

        public GameObject GetPlayer(string _userId)
        {
            return players[_userId];
        }

        [PunRPC]
        private void RpcRegisterPlayerObject(string _userId, int viewID)
        {
            GameObject _player = PhotonNetwork.GetPhotonView(viewID).gameObject;
            string playerName = PLAYER_PREFIX + _userId;
            _player.transform.name = playerName;
            players[_userId] = _player;
        }

        private void UnRegisterPlayer(string _playerId)
        {
            players.Remove(_playerId);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Photon Callbacks

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Launcher");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
        }

        #endregion

    }
}
