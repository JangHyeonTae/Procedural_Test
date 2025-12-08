using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomMap : AbstractDungeonGenerator
{
    [SerializeField] private SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomwalk(randomWalkParameters);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }


    private HashSet<Vector2Int> RunRandomwalk(SimpleRandomWalkSO parameters)
    {
        var currentPos = startPos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for(int i =0; i < parameters.iterations; i++)
        {
            var path = ProceduralAlgorithm.SimpleRandomWalk(currentPos, parameters.walkLength);
            floorPos.UnionWith(path);

            if(parameters.startRandomlyEachIteration)
            {
                currentPos = floorPos.ElementAt(UnityEngine.Random.Range(0, floorPos.Count));
            }
        }

        return floorPos;
    }

}
