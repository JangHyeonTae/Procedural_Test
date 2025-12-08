using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomMap : MonoBehaviour
{

    [SerializeField] protected Vector2Int startPos = Vector2Int.zero;

    [SerializeField] private int iterations = 10;
    public int walkLength = 10;
    public bool startRandomlyEachIteration = true;

    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomwalk();

        foreach(var pos in floorPositions)
        {
            Debug.Log(pos);
        }
    }

    private HashSet<Vector2Int> RunRandomwalk()
    {
        var currentPos = startPos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for(int i =0; i < iterations; i++)
        {
            var path = ProceduralAlgorithm.SimpleRandomWalk(currentPos, walkLength);
            floorPos.UnionWith(path);

            if(startRandomlyEachIteration)
            {
                currentPos = floorPos.ElementAt(UnityEngine.Random.Range(0, floorPos.Count));
            }
        }

        return floorPos;
    }
}
