using System;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{

    SocketIOComponent socket;
    public GameObject playerPrefab;
    public GameObject currentPlayer;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    // Use this for initialization
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawn);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
    }

    private void OnUpdatePosition(SocketIOEvent obj)
    {

        var position = new Vector3(GetFloatFromJson(obj.data, "x"), 0, GetFloatFromJson(obj.data, "y"));
        var player = players[obj.data["id"].ToString()];
        player.transform.position = position;
    }

    private void OnRequestPosition(SocketIOEvent obj)
    {
        socket.Emit("updatePosition", new JSONObject(VectorToJson(currentPlayer.transform.position)));
    }

    private void OnDisconnected(SocketIOEvent obj)
    {
        var disconnectedId = obj.data["id"].ToString();
        var player = players[disconnectedId];
        Destroy(player);
        players.Remove(disconnectedId);
    }

    private void OnMove(SocketIOEvent obj)
    {
        var position = new Vector3(GetFloatFromJson(obj.data, "x"), 0, GetFloatFromJson(obj.data, "y"));
        var player = players[obj.data["id"].ToString()];
        var navPos = player.GetComponent<NavigatePosition>();
        navPos.NavigateTo(position);
        
    }

    private void OnSpawn(SocketIOEvent obj)
    {

        Debug.Log("Spawn " + obj.data);
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (obj.data["x"])
        {
            var movePosition = new Vector3(GetFloatFromJson(obj.data, "x"), 0, GetFloatFromJson(obj.data, "y"));
            var navPos = player.GetComponent<NavigatePosition>();
            navPos.NavigateTo(movePosition);
        }
        players.Add(obj.data["id"].ToString(), player);
    }

    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }
    
    float GetFloatFromJson(JSONObject data, string key){
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    public static string VectorToJson(Vector3 vector)
    {
        return String.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
    }
}
