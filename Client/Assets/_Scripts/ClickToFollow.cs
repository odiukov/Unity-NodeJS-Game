using UnityEngine;

public class ClickToFollow : MonoBehaviour, IClickable
{
	public GameObject currentPlayer;
    private NetworkEntity networkEntity;
	private Targeter currentPlayerTargeter;
    void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
		currentPlayerTargeter = currentPlayer.GetComponent<Targeter> ();
    }
    public void OnClick(RaycastHit hit)
    {
        Debug.Log("follow " + hit.collider.gameObject.name);
        Network.Follow(networkEntity.id);
		currentPlayerTargeter.target = transform;
    }

}
