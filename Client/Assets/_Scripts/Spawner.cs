using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;

public class Spawner : MonoBehaviour {

    public GameObject networkPlayer;
    public GameObject currentPlayer;
    public SocketIOComponent socket;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    public GameObject SpawnPlayer(string id)
    {
        var player = Instantiate(networkPlayer, Vector3.zero, Quaternion.identity) as GameObject;
        player.GetComponent<ClickToFollow>().currentPlayer = currentPlayer;
        player.GetComponent<NetworkEntity>().id = id;
        AddPlayer(id, player);
        return player;
    }

    public GameObject GetPlayer(string id)
    {
        return players[id];
    }

    public void AddPlayer(string id, GameObject player)
    {
        players.Add(id, player);
    }
    public void Remove(string id)
    {
        var player = players[id];
        Destroy(player);
        players.Remove(id);
    }
}
