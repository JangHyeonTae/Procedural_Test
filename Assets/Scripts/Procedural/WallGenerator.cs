using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorsPos, TileMapVisualizer tilemapVisualizer)
    {
        var basicWallPos = FindWallsInDirections(floorsPos, Direction2D.cardinalDirectionsList);
        var cornerWallPos = FindWallsInDirections(floorsPos, Direction2D.diagonalDirectionsList);

        CreateBasicWall(tilemapVisualizer, basicWallPos, floorsPos);
        CreateCornerWalls(tilemapVisualizer, cornerWallPos, floorsPos);
    }

    private static void CreateCornerWalls(TileMapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPos, HashSet<Vector2Int> floorsPos)
    {
        foreach (var pos in cornerWallPos)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPos = pos + direction;
                if (floorsPos.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(pos,neighboursBinaryType);
        }
    }

    private static void CreateBasicWall(TileMapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPos,HashSet<Vector2Int> floorsPos)
    {
        foreach (var pos in basicWallPos)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPos = pos + direction;
                if (floorsPos.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(pos, neighboursBinaryType);
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
