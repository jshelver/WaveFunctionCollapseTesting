using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [Range(1, 20)] public int mapLength = 10; // The length of the map (in cells)
    [Range(1, 20)] public int mapWidth = 10; // The width of the map (in cells)
    public const int cellSize = 10; // The size of each cell (based on the size of the cell prefab)
    [HideInInspector] public MapCell[,] cellMatrix;
    [HideInInspector] public GameObject mapParent;
    List<MapCell> cellsWithUnselectedType = new List<MapCell>();

    [Header("Cell Settings")]
    [SerializeField] List<CellData> cellDataList = new List<CellData>();

    void Start()
    {
        mapParent = new GameObject("Map");
        InitializeMap();
        FillCells();
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
                cellMatrix[i, j] = new MapCell(new Vector2Int(i, j), new List<CellData>(cellDataList), this);
            }
        }
    }

    private Vector2Int FindCellIndexWithLowestEntropy()
    {
        float lowestEntropy = float.MaxValue;
        Vector2Int lowestEntropyCellIndex = new Vector2Int(0, 0);

        foreach (MapCell cell in cellsWithUnselectedType)
        {
            float currentCellEntropy = cell.TotalEntropy;
            if (currentCellEntropy < lowestEntropy)
            {
                lowestEntropy = currentCellEntropy;
                lowestEntropyCellIndex = cell.MapIndex;
            }
        }

        return lowestEntropyCellIndex;
    }

    private void FillCells()
    {
        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                cellsWithUnselectedType.Add(cellMatrix[i, j]);
            }
        }

        StartCoroutine(FillCellsCoroutine(cellsWithUnselectedType));
    }

    private IEnumerator FillCellsCoroutine(List<MapCell> cellsWithUnselectedType)
    {
        while (cellsWithUnselectedType.Count > 0)
        {
            // Find the cell with the lowest entropy
            Vector2Int cellToUpdateIndex = FindCellIndexWithLowestEntropy();
            MapCell currentCell = cellMatrix[cellToUpdateIndex.x, cellToUpdateIndex.y];

            // Select the cell type and generate it
            currentCell.SelectCellType();
            GenerateCell(currentCell);

            // Remove the cell from the list
            Debug.Log("Cells left to select: " + cellsWithUnselectedType.Count);
            cellsWithUnselectedType.Remove(currentCell);
            yield return new WaitForSeconds(0.001f);
        }
    }

    private void GenerateCell(MapCell cell)
    {
        CellData cellData = cell.SelectedCellDataType;
        GameObject cellPrefab = cellData.cellPrefab;
        Vector3 cellPosition = new Vector3(cell.MapIndex.x * cellSize, 0, cell.MapIndex.y * cellSize);
        Instantiate(cellPrefab, cellPosition, Quaternion.identity, mapParent.transform);
    }
}
