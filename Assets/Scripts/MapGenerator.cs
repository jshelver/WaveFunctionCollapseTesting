using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] [Range(1, 20)] int mapLength = 10; // The length of the map (in cells)
    [SerializeField] [Range(1, 20)] int mapWidth = 10; // The width of the map (in cells)
    const int cellSize = 10; // The size of each cell (based on the size of the cell prefab)
    MapCell[,] cellMatrix;
    GameObject mapParent;

    [Header("Cell Settings")]
    [SerializeField] CellData[] cellDataList;

    void Start()
    {
        mapParent = new GameObject("Map");
        InitializeMap();
    }

    void Update()
    {
        
    }

    private void InitializeMap()
    {
        cellMatrix = new MapCell[mapLength, mapWidth];
        
        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                cellMatrix[i, j] = new MapCell(new Vector3(i * cellSize, 0, j * cellSize), cellDataList, mapParent);
            }
        }
    }

    private Vector2Int FindCellIndexWithLowestEntropy()
    {
        float lowestEntropy = float.MaxValue;
        Vector2Int lowestEntropyCellIndex = new Vector2Int(0, 0);

        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                float cellEntropy = cellMatrix[i, j].CalculateEntropyOfCell();
                if (cellEntropy < lowestEntropy)
                {
                    lowestEntropy = cellEntropy;
                    lowestEntropyCellIndex = new Vector2Int(i, j);
                }
            }
        }

        return lowestEntropyCellIndex;
    }
}
