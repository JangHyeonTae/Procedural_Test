using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPos, TileMapVisualizer tilemapVisualizer)
    {
        var basicWallPos = FindWallsInDirections(floorPos, Direction2D.cardinalDirectionsList);
    
        foreach (var pos in basicWallPos)
        {
            tilemapVisualizer.PaintSingleBasicWall(pos);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPos, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallpos = new HashSet<Vector2Int>();

        foreach(var pos in floorPos)
        {
            foreach(var direction in directionList)
            {
                var neighbourPos = pos + direction;
                if (!floorPos.Contains(neighbourPos))
                {
                    wallpos.Add(neighbourPos);
                }
            }    
        }

        return wallpos;
    }
}
