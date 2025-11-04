using UnityEngine;
using UnityEngine.InputSystem; 
using _Controllers;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody rb;

    [SerializeField] private AnimationsControl animationsControl; 

    [Header("Shooting Settings")] 
    [SerializeField] private GameObject arrowPrefab; 
    [SerializeField] private Transform arrowSpawnPoint; 

    [Header("Player Movement Settings")]
    [Range(0, 20f)][SerializeField] private float walkSpeed = 5f;
    [Range(0, 40f)][SerializeField] private float runSpeed = 10f; 
    [SerializeField, Range(1f, 200f)] private float acceleration = 50f;
    [Range(0, 100f)][SerializeField] private float jumpForce = 5f;

    [Header("Player Rotation Settings")]
    [Range(0, 20f)][SerializeField] private float rotationSpeed = 10f; 

    private Vector2 moveInput;
    private bool isGrounded = true;
    private bool isShiftPressed = false; 
    private bool isAiming = false;
    private float animLockTimer = 0f;

    [Header("Ground Tags")]
    [SerializeField] private List<string> groundTags; 

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
            
        if (animationsControl == null)
            Debug.LogError("AnimationsControl is not assigned in the PlayerController inspector!");
    }

   void FixedUpdate()
    {
        if (rb == null) return;
        
        if (animLockTimer > 0)
        {
            animLockTimer -= Time.fixedDeltaTime;
        }
        
        PlayerMovment();
    }

    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isShiftPressed = context.ReadValueAsButton();
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        if (animLockTimer > 0 && !context.started) return; 

        isAiming = context.ReadValueAsButton();

        if (context.started) 
        {
            animationsControl.StartAimingAnim();

            animLockTimer = 0.3f; 
        }
        else if (context.canceled) 
        {
            isAiming = false; 
            animationsControl.ShootAnim();

            ShootArrow();

            animLockTimer = 0.5f; 
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isAiming)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            if (animationsControl != null)
            {
                if (isShiftPressed && moveInput.magnitude > 0.1f)
                {
                    animationsControl.RunningJumpAnim();
                }
                else
                {
                    animationsControl.NormalJumpAnim();
                }
            }
        }
    }

    private void ShootArrow()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogError("Ok Prefab'ı veya Ok Çıkış Noktası (Spawn Point) atanmamış!");
            return;
        }

        Vector3 lookDirection = Camera.main.transform.forward;
        lookDirection.y = 0; 
        
        Quaternion arrowRotation;
        
        if (lookDirection.sqrMagnitude > 0.01f)
        {
             arrowRotation = Quaternion.LookRotation(lookDirection.normalized);
        }
        else
        {
             arrowRotation = arrowSpawnPoint.rotation; 
        }

        Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowRotation);
    }

    void PlayerMovment()
    {
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;
        Vector3 inputDir = cameraForward * moveInput.y + cameraRight * moveInput.x;

        if (animationsControl != null && isGrounded && animLockTimer <= 0)
        {
            if (isAiming) 
            {
                if (moveInput.magnitude < 0.1f)
                {
                    animationsControl.AimIdleAnim(); 
                }
                else
                {
                    if (Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x))
                    {
                        if (moveInput.y > 0) 
                            animationsControl.AimForwardWalkAnim();
                        else 
                            animationsControl.AimBackwardWalkAnim();
                    }
                    else
                    {
                        if (moveInput.x < 0) 
                            animationsControl.AimLeftWalkAnim();
                        else 
                            animationsControl.AimRightWalkAnim();
                    }
                }
            }
            else 
            {
                if (moveInput.magnitude < 0.1f) 
                {
                    animationsControl.IdleAnim(); 
                }
                else
                {
                    if (Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x))
                    {
                        if (moveInput.y > 0) 
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Forward);
                        else 
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Backward);
                    }
                    else
                    {
                        if (moveInput.x < 0)
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Left);
                        else 
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Right);
                    }
                }
            }
        }

        
        float currentTargetSpeed;
        if (isAiming)
        {
            currentTargetSpeed = walkSpeed; 
        }
        else
        {
            currentTargetSpeed = isShiftPressed ? runSpeed : walkSpeed;
        }
        
        Vector3 desiredVelocity = inputDir * currentTargetSpeed;
        Vector3 currentVel = rb.linearVelocity;
        Vector3 velChange = desiredVelocity - new Vector3(currentVel.x, 0f, currentVel.z);
        Vector3 force = velChange * acceleration;
        rb.AddForce(force, ForceMode.Acceleration);
        
        if (isAiming || animLockTimer > 0)
        {
            Quaternion correctionOffset = Quaternion.Euler(0, 90, 0); 
            Vector3 lookDirection = Camera.main.transform.forward;
            lookDirection.y = 0; 

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection.normalized);
                Quaternion finalRotation = targetRotation * correctionOffset; 
                Quaternion newRotation = Quaternion.Slerp(rb.rotation, finalRotation, Time.fixedDeltaTime * rotationSpeed * 2.0f); 
                rb.MoveRotation(newRotation);
            }
        }
        else if (inputDir.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            rb.MoveRotation(newRotation);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (groundTags.Contains(collision.gameObject.tag))
        {
            isGrounded = true;
            
            if (animationsControl != null && moveInput.magnitude < 0.1f && !isAiming)
            {
                animationsControl.IdleAnim();
            }
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