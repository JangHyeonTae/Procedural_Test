using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomMap
{
    [SerializeField] private int corridorLength = 14;
    [SerializeField] private int corridorCount = 5;
    [SerializeField][Range(0.1f, 1f)] private float roomPercent;
    [SerializeField] public SimpleRandomWalkSO roomGenerationParameters;
    
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        CreateCorridors(floorPos);
        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tilemapVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPos)
    {
        var currentPos = startPos;

        for(int i =0; i < corridorCount; i++)
        {
            var corridor = ProceduralAlgorithm.RandomWalkCorridor(currentPos, corridorLength);
            currentPos = corridor[corridor.Count - 1];
            floorPos.UnionWith(corridor);
        }
    }
}
