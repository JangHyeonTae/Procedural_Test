using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorsPos, TileMapVisualizer tilemapVisualizer)
    {
        var basicWallPos = FindWallsInDirections(floorsPos, Direction2D.cardinalDirectionsList);
    
        foreach (var pos in basicWallPos)
        {
            tilemapVisualizer.PaintSingleBasicWall(pos);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorsPos, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallspos = new HashSet<Vector2Int>();

        foreach(var pos in floorsPos)
        {
            foreach(var direction in directionList)
            {
                var neighbourPos = pos + direction;
                if (!floorsPos.Contains(neighbourPos))
                {
                    wallspos.Add(neighbourPos);
                }
            }    
        }

        return wallspos;
    }
}
