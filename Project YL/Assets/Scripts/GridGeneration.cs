using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    public GameObject blockGameObject;

    //x ve z ekseninde kac tane block ekleyeceðimizi seciyoruz. 
    private int worldSizeX = 50;

    private int worldSizeZ = 50;

    private float noiseHeight = 8f;

    private float gridOffset = 2f;

    void Start() //baþlangýçta oluþturmasý icin startýn icine yazýyoruz.
    {
        //bu iç içe 2 for kullanarak x*ylik bir alan olusturmus oluyoruz.
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                Vector3 pos = new Vector3(
                    x * gridOffset,
                    generateNoise(x, z, 8f) * noiseHeight,
                    z * gridOffset);
                GameObject block = Instantiate(blockGameObject,
                    pos,
                    Quaternion.identity) as GameObject;

                block.transform.SetParent(this.transform);
            }
        }
    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;
        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}

