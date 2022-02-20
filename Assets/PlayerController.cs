using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float playerMoveSpeed = 6.0f;
    [SerializeField] private float gravity = -20.0f;
    [SerializeField] [Range(0.0f, 0.5f)] private float movementSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f;

    [SerializeField] private bool lockCursor = true;

    private float _cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private CharacterController _controller = null;
    private float jumpHeight = 3.0f;

    private Vector2 currentDirection = Vector2.zero;
    private Vector2 currentDirectionVelocity = Vector2.zero;
    
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    
// Start is called before the first frame update
// Locks cursor on start of the game
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    // Calls UpdateMouseLook() and UpdateMovement()
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();

        if (Input.GetKey("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    // code for mouse movement (rotation, pitch)
    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        
        _cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDirection.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity,
            movementSmoothTime);
        //checks if player is grounded and sets VelocityY to 0
        if (_controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        
        //checks if player is grounded and if Jump button is pressed
        if (_controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        //gravity
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * playerMoveSpeed + Vector3.up * velocityY;

        _controller.Move(velocity * Time.deltaTime);
    }
}