using UnityEngine;

public class ClickToMove : MonoBehaviour, IClickable
{

    public GameObject player;
    private Navigator playerNavigator;
	void Start ()
	{
	    playerNavigator = player.GetComponent<Navigator>();
	}

    public void OnClick(RaycastHit hit)
    {
        playerNavigator.NavigateTo(hit.point);
		Network.Move(player.transform.position, hit.point);
    }
}
