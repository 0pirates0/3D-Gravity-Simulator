using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 5f;

    [Header("Hologram")]
    public GameObject hologramPrefab;
    private GameObject activeHologram;

    public float gravityStrength = 9.81f;

    private Vector3 gravityDir = Vector3.down;
    private Vector3 pendingGravityDir;
    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pendingGravityDir = gravityDir;
    }

    void Update()
    {
        HandleGravityInput();
        ApplyMovement();
        ApplyGravity();
    }

    void HandleGravityInput()
{
    Vector3 forward = transform.forward;
    Vector3 right = transform.right;

    bool arrowPressed = false;

    if (Input.GetKeyDown(KeyCode.UpArrow)) { pendingGravityDir = forward; arrowPressed = true;}
    if (Input.GetKeyDown(KeyCode.DownArrow)) { pendingGravityDir = -forward; arrowPressed = true;}
    if (Input.GetKeyDown(KeyCode.LeftArrow)) { pendingGravityDir = -right; arrowPressed = true;}
    if (Input.GetKeyDown(KeyCode.RightArrow)) { pendingGravityDir = right; arrowPressed = true;}

    if (arrowPressed)
    {
        ShowHologramPreview(pendingGravityDir);
    }

    if (Input.GetKeyDown(KeyCode.Return))
    {
        gravityDir = pendingGravityDir;
        StartCoroutine(SmoothAlign(-gravityDir));
        velocity = Vector3.zero;

        // Remove hologram
        if (activeHologram != null) Destroy(activeHologram);
    }
}

void ShowHologramPreview(Vector3 gravityDir)
{
    if (activeHologram != null)
    {
        Destroy(activeHologram);
        activeHologram = null;
    }

    activeHologram = Instantiate(hologramPrefab, transform);

    // Base head height + gravity preview direction
    Vector3 headPos = transform.position + transform.up * 2f;
    Vector3 basePos = headPos + gravityDir.normalized * 2f;

    // Offset direction based on key
    Vector3 offset = Vector3.zero;

    if (Input.GetKeyDown(KeyCode.UpArrow))    offset += transform.forward * 1.5f;
    if (Input.GetKeyDown(KeyCode.DownArrow))  offset += -transform.forward * 1.5f;
    if (Input.GetKeyDown(KeyCode.RightArrow)) offset += transform.right * 1.5f;
    if (Input.GetKeyDown(KeyCode.LeftArrow))  offset += -transform.right * 1.5f;

    activeHologram.transform.position = basePos + offset;

    // Attach and rotate
    var tracker = activeHologram.GetComponent<FollowAndFacePlayer>();
    if (tracker != null)
    {
        tracker.player = this.transform;
        tracker.gravityDir = gravityDir;
        tracker.isActive = true;
        tracker.ApplyRotation(); // Set rotation once
    }
}


    void ApplyMovement()
{
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    if (isGrounded && Vector3.Dot(velocity, gravityDir) > 0)
    {
        velocity = Vector3.zero;
    }

    // Manual WASD input (ignoring arrow keys)
    float x = 0f;
    float z = 0f;

    if (Input.GetKey(KeyCode.W)) z += 1;
    if (Input.GetKey(KeyCode.S)) z -= 1;
    if (Input.GetKey(KeyCode.A)) x -= 1;
    if (Input.GetKey(KeyCode.D)) x += 1;

    Vector3 inputDir = new Vector3(x, 0, z).normalized;

    if (inputDir.magnitude > 0.1f)
    {
        Vector3 move = transform.right * inputDir.x + transform.forward * inputDir.z;
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);
    }

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
        velocity += -gravityDir.normalized * jumpForce;
    }
}


    void ApplyGravity()
    {
        velocity += gravityDir.normalized * gravityStrength * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator SmoothAlign(Vector3 targetUp)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.FromToRotation(transform.up, targetUp) * transform.rotation;

        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
    }
}
