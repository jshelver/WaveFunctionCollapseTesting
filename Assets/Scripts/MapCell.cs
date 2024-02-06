using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell
{
    public List<CellData> PotentialCellDataTypes { get; private set; } = new List<CellData>();
    public Vector2Int MapIndex { get; private set; }
    public CellData SelectedCellDataType { get; private set; }
    public List<Vector2Int> AdjacentCellIndexes { get; private set; }
    public float TotalWeight { get; private set; }
    public float TotalEntropy { get; private set; }
    
    private MapGenerator mapGenerator;

    public MapCell(Vector2Int mapIndex, List<CellData> cellDataTypes, MapGenerator mapGenerator)
    {
        PotentialCellDataTypes = cellDataTypes;
        MapIndex = mapIndex;
        SelectedCellDataType = null;
        this.mapGenerator = mapGenerator;
        AdjacentCellIndexes = GetAdjacentCellIndexes();
        TotalWeight = CalculateTotalWeight();
        TotalEntropy = CalculateEntropyOfCell();
    }

    private List<Vector2Int> GetAdjacentCellIndexes()
    {
        List<Vector2Int> cellIndexes = new List<Vector2Int>();

        // Left cell
        if (MapIndex.x - 1 >= 0) cellIndexes.Add(new Vector2Int(MapIndex.x - 1, MapIndex.y));
        // Bottom cell
        if (MapIndex.y - 1 >= 0) cellIndexes.Add(new Vector2Int(MapIndex.x, MapIndex.y - 1));
        // Right cell
        if (MapIndex.x + 1 < mapGenerator.mapLength) cellIndexes.Add(new Vector2Int(MapIndex.x + 1, MapIndex.y));
        // Top cell
        if (MapIndex.y + 1 < mapGenerator.mapWidth) cellIndexes.Add(new Vector2Int(MapIndex.x, MapIndex.y + 1));

        return cellIndexes;
    }

    public void SelectCellType()
    {
        float randomValue = Random.Range(0, TotalWeight);
        foreach (CellData cellData in PotentialCellDataTypes)
        {
            randomValue -= cellData.weight;
            if (randomValue <= 0)
            {
                // Set the selected cell data type
                SelectedCellDataType = cellData;
                break;
            }
        }

        UpdateAdjacentCells(this);
    }

    private void UpdateAdjacentCells(MapCell cellWithSelectedType)
    {
        foreach (var cellIndex in AdjacentCellIndexes)
        {
            // If the cell has not been selected, update it
            if (mapGenerator.cellMatrix[cellIndex.x, cellIndex.y].SelectedCellDataType == null)
                mapGenerator.cellMatrix[cellIndex.x, cellIndex.y].UpdateCell(cellWithSelectedType);
        }
    }

    private void UpdateCell(MapCell cellWithSelectedType)
    {
        // Create a list to store the cell data types to remove
        List<CellData> cellDataTypesToRemove = new List<CellData>();
        // Calculate the difference in map index
        var differenceInMapIndex = cellWithSelectedType.MapIndex - MapIndex;

        if (differenceInMapIndex.x == 1)
        {
            // Check the positive x edge
            foreach (CellData cellData in PotentialCellDataTypes)
            {
                if (cellData.postiveXEdge != cellWithSelectedType.SelectedCellDataType.negativeXEdge)
                {
                    cellDataTypesToRemove.Add(cellData);
                }
            }
        }
        if (differenceInMapIndex.x == -1)
        {
            // Check the negative x edge
            foreach (CellData cellData in PotentialCellDataTypes)
            {
                if (cellData.negativeXEdge != cellWithSelectedType.SelectedCellDataType.postiveXEdge)
                {
                    cellDataTypesToRemove.Add(cellData);
                }
            }
        }
        if (differenceInMapIndex.y == 1)
        {
            // Check the positive z edge
            foreach (CellData cellData in PotentialCellDataTypes)
            {
                if (cellData.postiveZEdge != cellWithSelectedType.SelectedCellDataType.negativeZEdge)
                {
                    cellDataTypesToRemove.Add(cellData);
                }
            }
        }
        if (differenceInMapIndex.y == -1)
        {
            // Check the negative z edge
            foreach (CellData cellData in PotentialCellDataTypes)
            {
                if (cellData.negativeZEdge != cellWithSelectedType.SelectedCellDataType.postiveZEdge)
                {
                    cellDataTypesToRemove.Add(cellData);
                }
            }
        }
        // Remove the cell data types that are not compatible with the selected cell data type
        int CellDataTypesRemoved = PotentialCellDataTypes.RemoveAll(cellData => cellDataTypesToRemove.Contains(cellData));

        // Update the total weight
        TotalWeight = CalculateTotalWeight();

        // Update the total entropy
        TotalEntropy = CalculateEntropyOfCell();
    }

    private float CalculateTotalWeight()
    {
        float totalWeight = 0;
        foreach (CellData cellData in PotentialCellDataTypes)
        {
            totalWeight += cellData.weight;
        }

        return totalWeight;
    }

    // Uses Shannon entropy formula
    private float CalculateEntropyOfCell()
    {
        float totalEntropy = 0;
        foreach (CellData cellData in PotentialCellDataTypes)
        {
            // Normalize the weight
            float normalizedWeight = cellData.weight / TotalWeight;

            // Calculate the entropy
            if (normalizedWeight > 0)
            {
                totalEntropy += normalizedWeight * Mathf.Log(normalizedWeight, 2);
            }
        }

        return -totalEntropy;
    }
}