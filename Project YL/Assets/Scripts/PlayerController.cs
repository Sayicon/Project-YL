using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerModel;
    private Rigidbody rb;
    
    [Header("Player Movement Settings")]
    [Range(0, 20f)][SerializeField] private float moveSpeed = 5f;
    [SerializeField, Range(1f, 200f)] private float acceleration = 50f;
    [Range(0, 100f)][SerializeField] private float jumpForce = 5f;
    private Vector2 moveInput;
    private bool isGrounded = true;

    [Header("Ground Tags")]
    [SerializeField] private List<string> groundTags; // Editörden tag'leri seçmek için

    void Awake()
    {
        if (player != null)
        {
            rb = player.GetComponent<Rigidbody>();
            if (rb == null)
                Debug.LogError("Rigidbody component not found on the player GameObject.");
        }
        else
            Debug.Log("Player GameObject is not assigned in the inspector.");
    }
    void FixedUpdate()
    {
        if (rb == null) return;
        PlayerMovment();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Zıplama fonksiyonu
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void PlayerMovment()
    {
        // Kameraya göre hareket yönünü hesapla
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 inputDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
        Vector3 desiredVelocity = inputDir * moveSpeed;

        // Haraket
        Vector3 currentVel = rb.linearVelocity;
        Vector3 velChange = desiredVelocity - new Vector3(currentVel.x, 0f, currentVel.z);
        Vector3 force = velChange * acceleration;
        rb.AddForce(force, ForceMode.Acceleration);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (groundTags.Contains(collision.gameObject.tag))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (groundTags.Contains(collision.gameObject.tag))
        {
            isGrounded = false;
        }
    }

}
