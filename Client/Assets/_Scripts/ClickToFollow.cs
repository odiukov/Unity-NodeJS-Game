using UnityEngine;

public class ClickToFollow : MonoBehaviour, IClickable
{
    public Follower currentPlayerFollower;
    private NetworkEntity networkEntity;
    void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
    }
    public void OnClick(RaycastHit hit)
    {
        Debug.Log("follow " + hit.collider.gameObject.name);
        Network.Follow(networkEntity.id);
        currentPlayerFollower.target = transform;
    }

}
