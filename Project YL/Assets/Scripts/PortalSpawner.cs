using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
      
    public Transform[] spawnPoints;
    public GameObject objectToSpawn;
    public Transform generatedObjectsParent;

    void Start()
    {
        if (spawnPoints.Length == 0 || objectToSpawn == null)
        {
            Debug.LogWarning("Spawn point veya prefab atanmamýþ!");
            return;
        }

        if (generatedObjectsParent == null)
        {
            Debug.LogWarning("Generated Objects Parent atanmamýþ! " +
                             "Obje Hiyerarþi'nin ana dizinine doðacak.");
        }
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        Transform chosenSpawnPoint = spawnPoints[spawnPointIndex];

        Instantiate(objectToSpawn, chosenSpawnPoint.position, chosenSpawnPoint.rotation, generatedObjectsParent);
    }
  
}