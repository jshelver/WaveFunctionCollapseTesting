using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] [Range(1, 20)] int mapLength = 10; // The length of the map (in cells)
    [SerializeField] [Range(1, 20)] int mapWidth = 10; // The width of the map (in cells)
    const float cellSize = 10f; // The size of each cell (based on the size of the cell prefab)

    [Header("Cell Settings")]
    [SerializeField] CellData[] cellPrefabList; // The list of cell prefabs to use for the map

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void InitializeMap()
    {

    }
}
