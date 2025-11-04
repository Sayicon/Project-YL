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

    [Header("Camera Collision")]
    [SerializeField] private LayerMask collisionLayers; 
    [SerializeField][Range(0.01f, 1f)] private float collisionOffset = 0.2f; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<Vector2>().y;
    }

    void LateUpdate()
    {
        float mouseX = lookInput.x * sensitivityX * 100f * Time.deltaTime;
        float mouseY = lookInput.y * sensitivityY * 100f * Time.deltaTime;
        float mouseWhell = zoomInput * mouseWhellSens * 100f * Time.deltaTime;
    
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, verticalClamp.x, verticalClamp.y); 
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        distanceFromPlayer -= mouseWhell;
        distanceFromPlayer = Mathf.Clamp(distanceFromPlayer, 2f, 15f); 
        
        Vector3 lookAtPoint = player.position + Vector3.up * 1.5f;

        Vector3 desiredPosition = lookAtPoint - (transform.forward * distanceFromPlayer);
        
        RaycastHit hit;
        
        if (Physics.Linecast(lookAtPoint, desiredPosition, out hit, collisionLayers))
        {
            transform.position = hit.point + (hit.normal * collisionOffset);
        }
        else
        {
            transform.position = desiredPosition; 
        }
    
        transform.LookAt(lookAtPoint); 
    }
}