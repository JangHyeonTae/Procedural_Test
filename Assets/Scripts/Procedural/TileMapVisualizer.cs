using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;

    [SerializeField] private Tilemap wallTilemap;

    [SerializeField] private TileBase floorTile;

    [SerializeField] private TileBase wallTop;


    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        PaintTiles(floorPos, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(var pos in positions)
        {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);
    }

    public void PaintSingleBasicWall(Vector2Int pos)
    {
        PaintSingleTile(wallTilemap, wallTop, pos);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
