using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class ProceduralAlgorithm
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new();

        path.Add(startPos);
        var prevPos = startPos;

        for(int i =0; i < walkLength; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomCardinalDirection();
            path.Add(newPos);
            prevPos = newPos;
        }

        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridor = new();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPos = startPos;
        corridor.Add(currentPos);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPos += direction;
            corridor.Add(currentPos);
        }

        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), // Up
        new Vector2Int(1,0), // Right
        new Vector2Int(0,-1), // Down
        new Vector2Int(-1,0) // Left
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }

}