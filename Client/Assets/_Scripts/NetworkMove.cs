using System;
using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkMove : MonoBehaviour {

	// Use this for initialization
    public SocketIOComponent socket;

    public void OnMove(Vector3 position)
    {
        Debug.Log("send to node " + Network.VectorToJson(position));
        socket.Emit("move", new JSONObject(Network.VectorToJson(position)));
    }
}
