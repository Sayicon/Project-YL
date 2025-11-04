using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject[] objects;
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        Instantiate(objects[rand], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
