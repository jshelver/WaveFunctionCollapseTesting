using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell
{
    public CellData[] PotentialCellDataTypes { get; private set; }
    public Vector3 Position { get; private set; }
    public GameObject MapParent { get; private set; }

    public MapCell(Vector3 position, CellData[] cellDataTypes, GameObject mapParent)
    {
        Position = position;
        PotentialCellDataTypes = cellDataTypes;
        MapParent = mapParent;
    }

    public void GenerateCell()
    {
        CellData cellData = PotentialCellDataTypes[Random.Range(0, PotentialCellDataTypes.Length)];
        GameObject cell = GameObject.Instantiate(cellData.cellPrefab, Position, Quaternion.identity);
        cell.transform.parent = MapParent.transform;
    }

    // Uses Shannon entropy formula
    public float CalculateEntropyOfCell()
    {
        float totalWeight = 0;
        foreach (CellData cellData in PotentialCellDataTypes)
        {
            // Calculate total weight
            totalWeight += cellData.weight;
        }

        float totalEntropy = 0;
        foreach (CellData cellData in PotentialCellDataTypes)
        {
            // Normalize the weight
            float normalizedWeight = cellData.weight / totalWeight;

            // Calculate the entropy
            if (normalizedWeight > 0)
            {
                totalEntropy += normalizedWeight * Mathf.Log(normalizedWeight, 2);
            }
        }

        return -totalEntropy;
    }
}
