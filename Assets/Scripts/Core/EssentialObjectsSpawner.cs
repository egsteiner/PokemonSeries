using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EssentialObjectsSpawner : MonoBehaviour
{
    [SerializeField] GameObject essentialObjectsPrefab;

    private void Awake()
    {
        var existingObjects = FindObjectsByType<EssentialObjects>(FindObjectsSortMode.None);
        if (existingObjects.Length == 0)
        {
            //If there is a grid, spawn at it's center
            var spawnPos = new Vector3(0, 0, 0);

            var grid = FindFirstObjectByType<Grid>();
            if (grid != null)
                spawnPos = grid.transform.position;


            Instantiate(essentialObjectsPrefab, spawnPos, Quaternion.identity);
        }
        
    }
}
