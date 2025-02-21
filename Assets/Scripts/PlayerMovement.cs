using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    
    [SerializeField] private float gridSize = 1f; 
    [SerializeField] private float moveDuration = 0.2f; 
    private bool isMoving = false;
    
    [Header("Camera Settings")]

    [SerializeField] private Transform cameraTransform; 
    [SerializeField] private Vector3 initialCameraOffset = new Vector3(0, 5, -5); 
    [SerializeField] private Vector3 cameraRotationOffset = new Vector3(45, 0, 0); 
    private Vector3 currentCameraOffset;
    
    [Header("Player Jump Settings")]
    [SerializeField] private float jumpPower = 0.5f; 
    [SerializeField] private int jumpCount = 1;

    void Start()
    {
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
        isMoving = true;

        Vector3 targetPosition = transform.position + (direction * gridSize);
        
        // Use DOJump for a small jump effect while moving
        transform.DOJump(targetPosition, jumpPower, jumpCount, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                isMoving = false;
                UpdateCameraPosition();
            });
    }

    void UpdateCameraPosition()
    {
        Vector3 targetCameraPosition = transform.position + currentCameraOffset;
        cameraTransform.DOMove(targetCameraPosition, moveDuration)
            .SetEase(Ease.OutQuad);

        Quaternion targetRotation = Quaternion.Euler(cameraRotationOffset);
        cameraTransform.DORotateQuaternion(targetRotation, moveDuration).SetEase(Ease.OutQuad);
    }
}
