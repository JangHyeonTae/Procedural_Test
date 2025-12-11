using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class ProceduralAlgorithm
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> paths = new();

        paths.Add(startPos);
        var prevPos = startPos;

        for(int i =0; i < walkLength; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomCardinalDirection();
            paths.Add(newPos);
            prevPos = newPos;
        }

        return paths;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridors = new();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPos = startPos;
        corridors.Add(currentPos);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPos += direction;
            corridors.Add(currentPos);
        }

        return corridors;
    }

    // bsp  Partitioning Setting(현재 Dequeue한 객체에서 가로,세로자른 위치를 판별하기)
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new();

        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if(Random.value < 0.5f)
                {
                    if(room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally( minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if(room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minHeight, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    // 자른 객체에 따른 최소값을 통해 나눠주기(queue에 Enqueue를 여기서 진행)
    // 추후에 BoundsInt를 3D로 사용할경우 그대로 사용해도 되는지
    // 안된다면 BoundsInt대신 struct형태의 Vector를 새로 만들자
    // 최대값 최소값을 가진 Vector3
    private static void SplitVertically(int minWidth,  Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        //Random값 = 1 ~ room.size.x - 1까지
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    //방향
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), // Up
        new Vector2Int(1,0), // Right
        new Vector2Int(0,-1), // Down
        new Vector2Int(-1,0) // Left
    };

    //코너
    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1), // Up - Right
        new Vector2Int(1,-1), // Right - Down
        new Vector2Int(-1,-1), // Down - Left
        new Vector2Int(-1,1) // Left - Up   
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>()
    {
        new Vector2Int(0,1), // Up
        new Vector2Int(1,1), // Up - Right
        new Vector2Int(1,0), // Right
        new Vector2Int(1,-1), // Right - Down
        new Vector2Int(0,-1), // Down
        new Vector2Int(-1,-1), // Down - Left
        new Vector2Int(-1,0), // Left
        new Vector2Int(-1,1) // Left - Up
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }

}