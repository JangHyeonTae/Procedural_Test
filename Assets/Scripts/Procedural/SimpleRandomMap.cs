using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomMap : AbstractDungeonGenerator
{
    [SerializeField] protected SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorsPos = RunRandomWalk(randomWalkParameters, startPos);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorsPos);
        WallGenerator.CreateWalls(floorsPos, tilemapVisualizer);
    }


    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int pos)
    {
        var currentPos = pos;
        HashSet<Vector2Int> floorsPos = new HashSet<Vector2Int>();

        for(int i =0; i < parameters.iterations; i++)
        {
            var path = ProceduralAlgorithm.SimpleRandomWalk(currentPos, parameters.walkLength);
            floorsPos.UnionWith(path);

            if(parameters.startRandomlyEachIteration)
            {
                currentPos = floorsPos.ElementAt(UnityEngine.Random.Range(0, floorsPos.Count));
            }
        }

        return floorsPos;
    }

}
