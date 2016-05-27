using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour
{

    public GameObject player;
    private NavigatePosition playerNavigatePosition;
    private NetworkMove networkMove;
	void Start ()
	{
	    playerNavigatePosition = player.GetComponent<NavigatePosition>();
	    networkMove = player.GetComponent<NetworkMove>();

	}
	
	public void OnClick (Vector3 position) {
	    playerNavigatePosition.NavigateTo(position);
        networkMove.OnMove(position);
    }
}
