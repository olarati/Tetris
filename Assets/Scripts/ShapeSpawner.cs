using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public Shape[] shapePrefabs = new Shape[0];

    public Shape SpawnNextShape()
    {
        Shape randomPrefab = shapePrefabs[Random.Range(0, shapePrefabs.Length)];
        return Instantiate(randomPrefab);
    }
    
}
