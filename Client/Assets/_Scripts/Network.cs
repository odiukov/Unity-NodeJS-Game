using System;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{

    static SocketIOComponent socket;
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
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
		socket.On("attack", OnAttack);
        socket.On("disconnected", OnDisconnected);
    }
    
    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }

    private void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("registered id = " + obj.data);
        spawner.AddPlayer(obj.data["id"].str, currentPlayer);
		currentPlayer.GetComponent<NetworkEntity> ().id = obj.data ["id"].str;
    }
    
    private void OnSpawn(SocketIOEvent obj)
    {

        Debug.Log("Spawn " + obj.data);
        var player = spawner.SpawnPlayer(obj.data["id"].str);
        if (obj.data["x"])
        {
            var movePosition = GetVectorFromJson(obj);
            var navPos = player.GetComponent<Navigator>();
            navPos.NavigateTo(movePosition);
        }
    }

    private void OnMove(SocketIOEvent obj)
    {
        var position = GetVectorFromJson(obj);
        var player = spawner.GetPlayer(obj.data["id"].str);
        var navPos = player.GetComponent<Navigator>();
        navPos.NavigateTo(position);
    }

    private void OnFollow(SocketIOEvent obj)
    {
        Debug.Log("follow request " + obj.data);
        var player = spawner.GetPlayer(obj.data["id"].str);
		var targetTransform = spawner.GetPlayer(obj.data["targetId"].str).transform;
		var target = player.GetComponent<Targeter>();
		target.target = targetTransform;
    }

    private void OnUpdatePosition(SocketIOEvent obj)
    {
        var position = GetVectorFromJson(obj);
        var player = spawner.GetPlayer(obj.data["id"].str);
        player.transform.position = position;
    }

    private void OnRequestPosition(SocketIOEvent obj)
    {
        socket.Emit("updatePosition", VectorToJson(currentPlayer.transform.position));
    }

	void OnAttack (SocketIOEvent obj)
	{
		Debug.Log("received attack " + obj.data);
		var targetPlayer = spawner.GetPlayer(obj.data["targetId"].str);
		targetPlayer.GetComponent<Hittable> ().GetHit(20f);

		var attackingPlayer = spawner.GetPlayer(obj.data["id"].str);
		attackingPlayer.GetComponent<Animator> ().SetTrigger ("Attack");

	}

    private void OnDisconnected(SocketIOEvent obj)
    {
        var disconnectedId = obj.data["id"].str;
        spawner.Remove(disconnectedId);
    }
    
    private static Vector3 GetVectorFromJson(SocketIOEvent obj)
    {
        return new Vector3(obj.data["x"].n, 0, obj.data["y"].n);
    }

    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("x", vector.x);
        jsonObject.AddField("y", vector.z);
        return jsonObject;
    }

    public static JSONObject PlayerIdToJson(string id)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("targetId", id);
        return jsonObject;
    }

    public static void Move(Vector3 current, Vector3 destionation)
    {
		Debug.Log("send moving to node " + Network.VectorToJson(destionation));

		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
		jsonObject.AddField("c", Network.VectorToJson(current));
		jsonObject.AddField("d", Network.VectorToJson(destionation));
		socket.Emit("move", jsonObject);
    }

    public static void Follow(string id)
    {
        Debug.Log("send follow player id " + Network.PlayerIdToJson(id));
        socket.Emit("follow", Network.PlayerIdToJson(id));
    }

	public static void Attack(string targetId)
	{
		Debug.Log("attacking player id " + Network.PlayerIdToJson(targetId));
		socket.Emit("attack", Network.PlayerIdToJson(targetId));
	}
}
