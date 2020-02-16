using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarDebugger : MonoBehaviour
{
    private static AstarDebugger instance;

    public static AstarDebugger MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AstarDebugger>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Tile tile;

    [SerializeField]
    private Color openColor, closeColor, pathColor, currentColor, startColor, goalColor;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject debugTextPrefab;

    private List<GameObject> debugObjects = new List<GameObject>();

    private Vector3Int previousStart, previousGoal;

    public void CreateTiles(Vector3Int start, Vector3Int goal)
    {
        if(goal != start)
        {
            tilemap.SetTile(previousStart, null);
            tilemap.SetTile(previousGoal, null);

            ColorTile(start, startColor);
            ColorTile(goal, goalColor);

            previousStart = start;
            previousGoal = goal;
        }
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
