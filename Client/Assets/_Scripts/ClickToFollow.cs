using UnityEngine;
using System.Collections;

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
        NetworkFollow networkFollow = GetComponent<NetworkFollow>();
        Debug.Log(networkFollow);
        GetComponent<NetworkFollow>().OnFollow(networkEntity.id);
        currentPlayerFollower.target = transform;
    }

}
