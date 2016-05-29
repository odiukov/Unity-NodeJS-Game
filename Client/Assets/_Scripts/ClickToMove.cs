using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour, IClickable
{

    public GameObject player;
    private Navigator playerNavigator;
    private NetworkMove networkMove;
	void Start ()
	{
	    playerNavigator = player.GetComponent<Navigator>();
	    networkMove = player.GetComponent<NetworkMove>();
	}

    public void OnClick(RaycastHit hit)
    {
        playerNavigator.NavigateTo(hit.point);
        networkMove.OnMove(hit.point);
    }
}
