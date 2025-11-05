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
    [Range(0, 20f)][SerializeField] private float rotationSpeed = 10f;
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
        RotateModel(inputDir);
    }

    public void RotateModel(Vector3? dir = null, bool cameraRelativeIfNull = true)
    {
        Vector3 inputDir;

        if (dir.HasValue)
        {
            inputDir = dir.Value;
        }
        else
        {
            Vector3 raw = new Vector3(moveInput.x, 0f, moveInput.y);
            if (raw.sqrMagnitude < 0.0001f) return;

            if (cameraRelativeIfNull && Camera.main != null)
            {
                Vector3 camF = Camera.main.transform.forward;
                camF.y = 0f;
                camF.Normalize();

                Vector3 camR = Camera.main.transform.right;
                camR.y = 0f;
                camR.Normalize();

                inputDir = camF * raw.z + camR * raw.x;
            }
            else
            {
                inputDir = raw;
            }
        }

        inputDir.y = 0f;
        if (inputDir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(inputDir.normalized, Vector3.up);
        playerModel.transform.rotation = Quaternion.Slerp(
            playerModel.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
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
