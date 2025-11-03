// Dosya Yolu: Assets/Scripts/PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem; // BU SATIR ÇOK ÖNEMLİ
using _Controllers; // AnimationsControl script'ine erişim için
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody rb;

    [SerializeField] private AnimationsControl animationsControl; 

    [Header("Shooting Settings")] // YENİ
    [SerializeField] private GameObject arrowPrefab; // Fırlatılacak ok prefab'ı
    [SerializeField] private Transform arrowSpawnPoint; // Okun çıkacağı nokta (örn: el, yay)

    [Header("Player Movement Settings")]
    [Range(0, 20f)][SerializeField] private float walkSpeed = 5f;
    [Range(0, 40f)][SerializeField] private float runSpeed = 10f; 
    [SerializeField, Range(1f, 200f)] private float acceleration = 50f;
    [Range(0, 100f)][SerializeField] private float jumpForce = 5f;

    [Header("Player Rotation Settings")]
    [Range(0, 20f)][SerializeField] private float rotationSpeed = 10f; // karakterin dönmesi mouse ile

    private Vector2 moveInput;
    private bool isGrounded = true;
    private bool isShiftPressed = false; //  Shift basılı mı?
    private bool isAiming = false; //  Nişan alma durumu
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
        
        // YENİ: Zamanlayıcıyı güncelle
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
        // Kilit (zamanlayıcı) bitene kadar yeni bir komut alma
        if (animLockTimer > 0 && !context.started) return; 

        // Nişan alma durumunu anlık olarak alıyoruz
        isAiming = context.ReadValueAsButton();

        if (context.started) // Tuşa basıldığı an
        {
            animationsControl.StartAimingAnim();

            // Nişan Alma (StartAiming) animasyonu için kilitle
            animLockTimer = 0.3f; // (Animasyonunuzun uzunluğuna göre ayarlayın)
        }
        else if (context.canceled) // Tuş BIRAKILDIĞI an
        {
            isAiming = false; 
            animationsControl.ShootAnim(); // ATEŞ ET! (Animasyon)

            // YENİ: OKU FIRLAT! (Fizik)
            ShootArrow();

            // Ateş etme animasyonu için kilitle
            animLockTimer = 0.5f; // (Animasyonunuzun uzunluğuna göre ayarlayın)
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

    // YENİ: Oku Fırlatma Metodu
    private void ShootArrow()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogError("Ok Prefab'ı veya Ok Çıkış Noktası (Spawn Point) atanmamış!");
            return;
        }

        // 1. Kameranın ileri baktığı yönü al (düz yön, 90 derece düzeltmesi OLMADAN)
        Vector3 lookDirection = Camera.main.transform.forward;
        lookDirection.y = 0; // Okun havaya/yere gitmesini engelle (sadece yatay)
        
        Quaternion arrowRotation;
        
        // Güvenlik kontrolü (eğer kamera tam yukarı/aşağı bakmıyorsa)
        if (lookDirection.sqrMagnitude > 0.01f)
        {
             // 2. Okun rotasyonunu kameranın baktığı bu düz yöne ayarla
             arrowRotation = Quaternion.LookRotation(lookDirection.normalized);
        }
        else
        {
            // Eğer bir sorun olursa, spawn point'in kendi yönünü kullansın (güvenlik)
             arrowRotation = arrowSpawnPoint.rotation; 
        }

        // 3. Oku, 'arrowSpawnPoint'un POZİSYONUNDA ama kameranın (düzeltilmiş) ROTASYONUNDA oluştur
        Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowRotation);
    }

  // --- HAREKET VE ANİMASYON MANTIĞI ---
    void PlayerMovment()
    {
        // 1. KAMERA VE GİRDİ HESAPLAMASI
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;
        Vector3 inputDir = cameraForward * moveInput.y + cameraRight * moveInput.x;

        // 2. ANİMASYON YÖNÜNE KARAR VERME
        // (Bu kod bloğu, animasyon kilitli DEĞİLSE çalışır)
        if (animationsControl != null && isGrounded && animLockTimer <= 0)
        {
            if (isAiming) // Eğer nişan alıyorsak
            {
                if (moveInput.magnitude < 0.1f)
                {
                    animationsControl.AimIdleAnim(); 
                }
                else // Nişan alırken hareket ediliyorsa
                {
                    if (Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x))
                    {
                        if (moveInput.y > 0) // İleri
                            animationsControl.AimForwardWalkAnim();
                        else // Geri
                            animationsControl.AimBackwardWalkAnim();
                    }
                    else
                    {
                        if (moveInput.x < 0) // Sol
                            animationsControl.AimLeftWalkAnim();
                        else // Sağ
                            animationsControl.AimRightWalkAnim();
                    }
                }
            }
            else // Nişan almıyorsak (normal durum)
            {
                if (moveInput.magnitude < 0.1f) 
                {
                    animationsControl.IdleAnim(); // Duruyorsa
                }
                else
                {
                    // Normal yürüme/koşma mantığı...
                    if (Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x))
                    {
                        if (moveInput.y > 0) // İleri
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Forward);
                        else // Geri
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Backward);
                    }
                    else
                    {
                        if (moveInput.x < 0) // Sol
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Left);
                        else // Sağ
                            animationsControl.ChangeAnim(isShiftPressed, AnimType.Right);
                    }
                }
            }
        }

        // 3. FİZİKSEL HAREKET (İtme kuvveti) (DÜZELTİLDİ)
        
        float currentTargetSpeed;
        if (isAiming)
        {
            // Nişan alırken yavaş yürü (koşamazsın)
            // BU SATIR ARTIK HAREKETİ DURDURMUYOR, SADECE HIZI DÜŞÜRÜYOR
            currentTargetSpeed = walkSpeed; 
        }
        else
        {
            // Normal hız (yürüme veya koşma)
            currentTargetSpeed = isShiftPressed ? runSpeed : walkSpeed;
        }
        
        // Hareketi uygula (artık 'if (isAiming)' bloğu içinde değil)
        Vector3 desiredVelocity = inputDir * currentTargetSpeed;
        Vector3 currentVel = rb.linearVelocity;
        Vector3 velChange = desiredVelocity - new Vector3(currentVel.x, 0f, currentVel.z);
        Vector3 force = velChange * acceleration;
        rb.AddForce(force, ForceMode.Acceleration);
        
        
        // 4. KARAKTERİ DÖNDÜRME (DÜZELTİLDİ)
        // Rotasyon kilidini (animLockTimer) koruyoruz
        
        if (isAiming || animLockTimer > 0)
        {
            // --- NİŞAN ALMA / ATEŞ ETME KISMI (90 DERECE DÜZELTMESİ BURADA) ---
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
        else if (inputDir.sqrMagnitude > 0.01f) // NORMAL HAREKET KISMI (Düzeltmesiz)
        {
            // Normal hareket dönüşü (düzeltme yok)
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            rb.MoveRotation(newRotation);
        }
    }
    // --- YER KONTROLÜ ---
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