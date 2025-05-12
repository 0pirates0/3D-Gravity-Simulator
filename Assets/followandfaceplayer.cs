using UnityEngine;

public class FollowAndFacePlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 gravityDir;
    public float distance = 2f;
    public float height = 1.5f;
    public bool isActive = false;

    public Vector3 basePos = Vector3.zero;

    public void ApplyRotation()
    {
        if (!isActive || player == null) return;

        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 upDir = -gravityDir.normalized;
        Quaternion lookRot = Quaternion.LookRotation(toPlayer, upDir);
        Quaternion finalRot = lookRot * Quaternion.Euler(60f, 0f, 0f); // Optional tilt
        transform.rotation = finalRot;
    }
}
