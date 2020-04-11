using System.Collections.Generic;

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

            int index = Random.Range(0, spawnPoints.Length);
            Transform spawnTransform = spawnPoints[index].transform;
            LobbySingleton.Instance.DisableLobby();
            PhotonNetwork.Instantiate(playerPrefab.name, spawnTransform.position, spawnTransform.rotation, 0);
        }

        #region Public Methods

        public GameObject GetPlayer(string _playerId)
        {
            return players[_playerId];
        }

        public void RegisterPlayer(string _netId, GameObject _player)
        {
            string playerId = PLAYER_PREFIX + _netId;
            players.Add(playerId, _player);
            _player.transform.name = playerId;
        }

        public void UnRegisterPlayer(string _playerId)
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
            SceneManager.LoadScene("Lobby");
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
