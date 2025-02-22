using UnityEngine;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    
    [SerializeField] private float gridSize = 1f; 
    [SerializeField] private float moveDuration = 0.2f; 
    private bool isMoving = false;
    [SerializeField] private Rigidbody rb;
    
    [Header("Camera Settings")]

    [SerializeField] private Transform cameraTransform; 
    [SerializeField] private Vector3 initialCameraOffset = new Vector3(0, 5, -5); 
    [SerializeField] private Vector3 cameraRotationOffset = new Vector3(45, 0, 0); 
    private Vector3 currentCameraOffset;
    
    [Header("Player Jump Settings")]
    [SerializeField] private float jumpPower = 0.5f; 
    [SerializeField] private int jumpCount = 1;
    
    [SerializeField] private LayerMask obstacleLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing!");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false; // Ensure Rigidbody is controlled by physics
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        currentCameraOffset = initialCameraOffset; // Start with initial offset
        UpdateCameraPosition();
    }

    void Update()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        if (isMoving) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = Vector3.zero;

        if (horizontal != 0)
            direction = new Vector3(horizontal, 0, 0);
        else if (vertical != 0)
            direction = new Vector3(0, 0, vertical);

        if (direction != Vector3.zero)
            Move(direction);
    }

    void Move(Vector3 direction)
    {
        if (isMoving) return;

        Vector3 targetPosition = transform.position + (direction * gridSize);

        if (Physics.BoxCast(transform.position, Vector3.one * 0.25f, direction, Quaternion.identity, gridSize, obstacleLayer))
        {
            Debug.Log("Obstacle detected! Movement blocked. " + obstacleLayer);
            return;
        }

        isMoving = true;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // **Use Rigidbody to move instead of directly modifying transform**
        rb.DOJump(targetPosition, jumpPower, jumpCount, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isMoving = false;
                UpdateCameraPosition();
            });

        transform.DORotateQuaternion(targetRotation, moveDuration).SetEase(Ease.Linear);
    }

    void UpdateCameraPosition()
    {
        Vector3 targetCameraPosition = transform.position + currentCameraOffset;
        cameraTransform.DOMove(targetCameraPosition, moveDuration)
            .SetEase(Ease.Linear);

        Quaternion targetRotation = Quaternion.Euler(cameraRotationOffset);
        cameraTransform.DORotateQuaternion(targetRotation, moveDuration).SetEase(Ease.Linear);
    }
}
