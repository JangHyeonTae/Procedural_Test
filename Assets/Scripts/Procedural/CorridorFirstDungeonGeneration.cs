using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel;

public class CorridorFirstDungeonGenerator : SimpleRandomMap
{
    [SerializeField] private int corridorLength = 14;
    [SerializeField] private int corridorCount = 5;

    [SerializeField][Range(0.1f, 1f)] private float roomPercent;
    
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorsPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomsPos = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorsPos, potentialRoomsPos);

        HashSet<Vector2Int> roomsPos = CresteRooms(potentialRoomsPos);

        //초기방 설정?, 끝지점?
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorsPos); ;

        CreateRoomsAtDeadEnd(deadEnds, roomsPos);

        floorsPos.UnionWith(roomsPos);

        for (int i = 0; i < corridors.Count; i++)
        {
            //corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            corridors[i] = IncreaseCorridorBrush3vt3(corridors[i]);
            floorsPos.UnionWith(corridors[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floorsPos);
        WallGenerator.CreateWalls(floorsPos, tilemapVisualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var pos in deadEnds)
        {
            if(roomFloors.Contains(pos) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, pos);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorsPos)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var pos in floorsPos)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorsPos.Contains(pos + direction))
                {
                    neighboursCount++;
                }
            }
            if (neighboursCount == 1)
            {
                deadEnds.Add(pos);
            }
        }

        return deadEnds;
    }

    private HashSet<Vector2Int> CresteRooms(HashSet<Vector2Int> potentialRoomsPos)
    { 
        HashSet<Vector2Int> roomsPos = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomsPos.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomsPos.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPos in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPos);
            roomsPos.UnionWith(roomFloor);
        }

        return roomsPos;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorsPos, HashSet<Vector2Int> potentialRoomsPos)
    {
        var currentPos = startPos;
        potentialRoomsPos.Add(currentPos);
        List<List<Vector2Int>> corridors = new();

        for(int i =0; i < corridorCount; i++)
        {
            var corridor = ProceduralAlgorithm.RandomWalkCorridor(currentPos, corridorLength);
            corridors.Add(corridor);
            currentPos = corridor[corridor.Count - 1];
            potentialRoomsPos.Add(currentPos);
            floorsPos.UnionWith(corridor);
        }

        return corridors;   
    }

    public List<Vector2Int> IncreaseCorridorBrush3vt3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 0; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }

        return newCorridor;
    }

    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new();
        Vector2Int previousDirection = Vector2Int.zero;

        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector2Int.zero &&
                directionFromCell != previousDirection)
            {
                //Handel corner
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previousDirection = directionFromCell;
            }
            else
            {
                //Add a single cell in the direction + 90 degree
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }

        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            return Vector2Int.right;
        if (dir == Vector2Int.right)
            return Vector2Int.down;
        if(dir == Vector2Int.down)
            return Vector2Int.left;
        if(dir == Vector2Int.left)
            return Vector2Int.up;

        return Vector2Int.zero;
    }
}
