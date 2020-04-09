using UnityEngine;
using System.Collections.Generic;

public class GameManagerSingleton : MonoBehaviour
{
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private const string PLAYER_PREFIX = "Player ";

    public static GameManagerSingleton Instance { get; private set; }

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

}
