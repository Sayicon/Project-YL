// Dosya Yolu: Assets/Scripts/CameraController.cs
using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraController : MonoBehaviour
{
    public Transform player; 
    [SerializeField][Range(0, 10f)] private float sensitivityX = 1f;
    [SerializeField][Range(0, 10f)] private float sensitivityY = 1f; 

    [Range(0, 10f)] public float lerpSens = 2f;
    [Range(0, 10f)] public float mouseWhellSens = 1f;
    [Range(0, 100f)] public float distanceFromPlayer = 5f; 
    public Vector2 verticalClamp = new Vector2(-40f, 80f); 

    private float pitch = 0f; 
    private float yaw = 0f;   

    private Vector2 lookInput;
    private float zoomInput;

    // YENİ: Kameranın çarpışacağı katmanları seçin
    [Header("Camera Collision")]
    [SerializeField] private LayerMask collisionLayers; // Çarpışma katmanları
    [SerializeField][Range(0.01f, 1f)] private float collisionOffset = 0.2f; // Kameranın duvardan ne kadar uzakta duracağı

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // --- INPUT METOTLARI (Aynı) ---
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<Vector2>().y;
    }

    // --- GÜNCELLENMİŞ LATEUPDATE ---
    void LateUpdate()
    {
        // 1. INPUT (Aynı)
        float mouseX = lookInput.x * sensitivityX * 100f * Time.deltaTime;
        float mouseY = lookInput.y * sensitivityY * 100f * Time.deltaTime;
        float mouseWhell = zoomInput * mouseWhellSens * 100f * Time.deltaTime;
    
        // 2. ROTATION (Aynı)
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, verticalClamp.x, verticalClamp.y); 
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // 3. ZOOM (Aynı)
        distanceFromPlayer -= mouseWhell;
        distanceFromPlayer = Mathf.Clamp(distanceFromPlayer, 2f, 15f); 
        
        // 4. POSITION & COLLISION (GÜNCELLENDİ)
        
        // Nereye bakacağımızı tanımla (oyuncunun biraz üzeri)
        Vector3 lookAtPoint = player.position + Vector3.up * 1.5f;

        // Kameranın olmasını istediğimiz ideal nokta (pivot noktasının arkası)
        Vector3 desiredPosition = lookAtPoint - (transform.forward * distanceFromPlayer);
        
        RaycastHit hit;
        
        // GÜNCELLENMİŞ KONTROL: Linecast
        // Bakış noktasından (A) ideal kamera noktasına (B) bir çizgi çek.
        // SADECE "collisionLayers" içindeki nesneleri kontrol et (Player'ı DEĞİL).
        if (Physics.Linecast(lookAtPoint, desiredPosition, out hit, collisionLayers))
        {
            // Bir duvara çarparsa, kamerayı çarpma noktasının biraz gerisine (normal yönünde) yerleştir.
            transform.position = hit.point + (hit.normal * collisionOffset);
        }
        else
        {
            // Arada bir şey yoksa, ideal pozisyona geç.
            transform.position = desiredPosition; 
        }
    
        // 5. LOOK AT (GÜNCELLENDİ)
        // Kameranın her zaman pivot noktasına bakmasını sağla
        transform.LookAt(lookAtPoint); 
    }
}