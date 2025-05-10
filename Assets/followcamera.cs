using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // usually the CameraPivot on the Player
    public float smoothSpeed = 5f;    // how quickly camera follows
    public Vector3 offset = new Vector3(0, 1.5f, -4f);  // default camera offset

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target); // optional: makes camera always face player
    }
}
