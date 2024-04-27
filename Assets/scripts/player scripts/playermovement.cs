using UnityEngine;
using System.Collections;

public class playermovement : MonoBehaviour
{
    private Vector3 playerKeyboardInput;
    private Vector3 playerMouseInput;
    private float xRotation;
    private bool canJump = true;
    public static bool canMovePlayer = true;
    public static bool canMoveCamera = true;

    [SerializeField] private Rigidbody player;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJump;
    [Space]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float playerCameraSensitivity;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            canMoveCamera = false;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            canMoveCamera = true;
        }

        if (canMovePlayer)
        {
            playerKeyboardInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            MovePlayer();
        }
        

        if (canMoveCamera)
        {
            playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            MovePlayerCamera();
        }
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(playerKeyboardInput) * playerSpeed;
        player.velocity = new Vector3(MoveVector.x, player.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            player.AddForce(Vector3.up * playerJump, ForceMode.Impulse);
            canJump = false;
            StartCoroutine(EnableJumpAfterDelay(1.5f)); // Cooldown period
        }
    }

    IEnumerator EnableJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canJump = true;
    }
    private void MovePlayerCamera()
    {
        xRotation -= playerMouseInput.y * playerCameraSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0f, playerMouseInput.x * playerCameraSensitivity, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
