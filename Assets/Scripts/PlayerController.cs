using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 6f;
    private Rigidbody rb;
    private bool isGrounded;
    public Transform cameraTransform; // reference for freelook camera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // handle movement (WASD)
        Vector2 inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;
        }

        handleMove(inputVector);

        // handle jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handleJump(jumpHeight);
        }
    }

    public void handleMove(Vector2 inputVector)
    {
        // get camera directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // flatten camera directions
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // calculate movement
        Vector3 movement = (cameraForward * inputVector.y + cameraRight * inputVector.x) * speed * Time.deltaTime;

        // use Space.World to ensure the player's movement works with the cinemachine freelook camera
        transform.Translate(movement, Space.World);
    }
    // allow jumping if player is on ground (if collision detected)
    public void handleJump(float jumpHeight)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    // use ground tag with plane object & box objects to detect if player can jump
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}