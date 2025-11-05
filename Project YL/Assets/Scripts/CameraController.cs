using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Player'ın referansı
    [SerializeField][Range(0, 10f)] private float sensitivityX = 1f;
    [SerializeField][Range(0, 10f)] private float sensitivityY = 1f; 

    [Range(0, 10f)] public float lerpSens = 2f;
    [Range(0, 10f)] public float mouseWhellSens = 1f;
    [Range(0, 100f)] public float distanceFromPlayer = 5f; // Kamera ile oyuncu arasındaki mesafe
    public Vector2 verticalClamp = new Vector2(-40f, 80f); // Dikey eksen sınırları

    private float pitch = 0f; // Dikey eksen (X) dönüşü
    private float yaw = 0f;   // Yatay eksen (Y) dönüşü

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
    }

    void LateUpdate()
    {
        // Fare hareketlerini al
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * 100f * Time.deltaTime;
        float mouseWhell = Input.GetAxis("Mouse ScrollWheel") * mouseWhellSens * 100f * Time.deltaTime;
    
        // Yatay ve dikey dönüşleri hesapla
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, verticalClamp.x, verticalClamp.y); // Dikey ekseni sınırla

        // Kameranın dönüşünü ayarla
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Kamera pozisyonunu ayarla
        distanceFromPlayer -= mouseWhell;
        distanceFromPlayer = Mathf.Clamp(distanceFromPlayer, 3f, 15f); // Mesafeyi sınırla
        Vector3 desiredPosition = Vector3.Lerp(transform.position, player.position - transform.forward * distanceFromPlayer, Time.deltaTime * 10f * lerpSens);
        RaycastHit hit;
    
        if (Physics.Raycast(player.position, desiredPosition - player.position, out hit, distanceFromPlayer))
        {
            if (!hit.collider.CompareTag("Enemy"))
            {
                Vector3 safePos = hit.point - (desiredPosition - player.position).normalized * 0.2f;
                transform.position = safePos;
            }
            else
            {
                transform.position = desiredPosition;
            }
        }
        else
        {
            transform.position = desiredPosition;
        }
        // Kamerayı oyuncuya çevir
        transform.LookAt(player.position + Vector3.up * 1.5f); // Oyuncunun biraz yukarısına bak
    }
}
