using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour
{

    public GameObject player;
    private NavigatePosition playerNavigatePosition;
	void Start ()
	{
	    playerNavigatePosition = player.GetComponent<NavigatePosition>();
	}
	
	public void OnClick (Vector3 position) {
	    playerNavigatePosition.NavigateTo(position);
	}
}
