using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell", menuName = "ScriptableObjects/Cell", order = 1)]
public class CellData : ScriptableObject
{
    public string cellName;
    public GameObject cellPrefab;
    public float weight;

    public EdgeType postiveXEdge;
    public EdgeType negativeXEdge;
    public EdgeType postiveZEdge;
    public EdgeType negativeZEdge;
}

public enum EdgeType: int
{
    None = -1,
    Grass = 0,
    Road = 1
}