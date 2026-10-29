using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    public static readonly int HashSpeed = Animator.StringToHash("Speed");

    public float moveSpeed = 5f;

    private Animator playerAnimator;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(moveX, 0f, moveZ);
        float inputMagnitude = inputDir.magnitude;

        playerAnimator.SetFloat(HashSpeed, inputMagnitude);
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        Vector3 moveDelta = moveDir * moveSpeed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDelta);
    }
}