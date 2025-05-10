using UnityEngine;

public class GravityController : MonoBehaviour
{
    public Vector3 currentGravity = Vector3.down; // Default gravity
    private Vector3 selectedDirection = Vector3.down;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Choose direction
        if (Input.GetKeyDown(KeyCode.UpArrow))
            selectedDirection = Vector3.forward;  // Forward (Z+)
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            selectedDirection = Vector3.back;     // Backward (Z-)
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            selectedDirection = Vector3.left;     // Left (X-)
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            selectedDirection = Vector3.right;    // Right (X+)

        // Optional: Show the direction visually or log it
        Debug.DrawRay(transform.position, selectedDirection * 2, Color.cyan);
        Debug.Log("Selected Gravity Direction: " + selectedDirection);

        // Apply direction
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplyGravity();
        }
    }

    void ApplyGravity()
    {
        currentGravity = selectedDirection;
        Debug.Log("Gravity Applied: " + currentGravity);
    }

    public Vector3 GetGravity()
    {
        return currentGravity;
    }
}
