using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;


    // There is a mismatch between player position and camera position 
    // which causes camera position to be jittery if putting inside Update method
    void LateUpdate()
    {
        transform.position = target.position;
    }
}
