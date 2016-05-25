using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {

    SocketIOComponent socket;
    public GameObject playerPrefab;
	// Use this for initialization
	void Start() {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawn);
	}

    private void OnSpawn(SocketIOEvent obj)
    {
        Debug.Log("Spawn");
        Instantiate(playerPrefab);
    }

    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
        socket.Emit("move");
    }
}
