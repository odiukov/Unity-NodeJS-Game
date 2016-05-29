using System;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{

    SocketIOComponent socket;
    public GameObject currentPlayer;
    public Spawner spawner;
    // Use this for initialization
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("register", OnRegister);
        socket.On("spawn", OnSpawn);
        socket.On("move", OnMove);
        socket.On("follow", OnFollow);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
    }

    private void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("registered id = " + obj.data);
        spawner.AddPlayer(obj.data["id"].str, currentPlayer);
    }

    private void OnFollow(SocketIOEvent obj)
    {
        Debug.Log("follow request " + obj.data);
        var player = spawner.GetPlayer(obj.data["id"].str);
        var target = spawner.GetPlayer(obj.data["targetId"].str);
        var follower = player.GetComponent<Follower>();
        follower.target = target.transform;
    }

    private void OnUpdatePosition(SocketIOEvent obj)
    {
        var position = new Vector3(GetFloatFromJson(obj.data, "x"), 0, GetFloatFromJson(obj.data, "y"));
        var player = spawner.GetPlayer(obj.data["id"].str);
        player.transform.position = position;
    }

    private void OnRequestPosition(SocketIOEvent obj)
    {
        socket.Emit("updatePosition", new JSONObject(VectorToJson(currentPlayer.transform.position)));
    }

    private void OnDisconnected(SocketIOEvent obj)
    {
        var disconnectedId = obj.data["id"].str;
        spawner.Remove(disconnectedId);
    }

    private void OnMove(SocketIOEvent obj)
    {
        var position = new Vector3(GetFloatFromJson(obj.data, "x"), 0, GetFloatFromJson(obj.data, "y"));
        var player = spawner.GetPlayer(obj.data["id"].str);
        var navPos = player.GetComponent<Navigator>();
        navPos.NavigateTo(position);
        
    }

    private void OnSpawn(SocketIOEvent obj)
    {

        Debug.Log("Spawn " + obj.data);
        var player = spawner.SpawnPlayer(obj.data["id"].str);
        if (obj.data["x"])
        {
            var movePosition = new Vector3(GetFloatFromJson(obj.data, "x"), 0, GetFloatFromJson(obj.data, "y"));
            var navPos = player.GetComponent<Navigator>();
            navPos.NavigateTo(movePosition);
        }
    }

    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }
    
    float GetFloatFromJson(JSONObject data, string key){
        return float.Parse(data[key].str);
    }

    public static string VectorToJson(Vector3 vector)
    {
        return String.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
    }

    public static string PlayerIdToJson(string id)
    {
        return String.Format(@"{{""targetId"":""{0}""}}", id);
    }
}
