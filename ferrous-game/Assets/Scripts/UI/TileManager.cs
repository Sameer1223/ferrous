using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 715;
    public int numberOfTiles = 7;

    public Vector3 Change;
    public Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    void Start()
    {
        for(int i = 0; i < numberOfTiles; i++) 
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, tilePrefabs.Length));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z>zSpawn-(numberOfTiles*tileLength)-30)
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            Invoke("DeleteTile", 12);
        }
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, (transform.rotation));
        activeTiles.Add(go);
        zSpawn += tileLength;

    }
}
