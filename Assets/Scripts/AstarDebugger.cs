﻿using System.Collections;
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

    public void CreateTiles(HashSet<Node> openList, HashSet<Node> closedList, Dictionary<Vector3Int, Node> allNodes, Vector3Int start, Vector3Int goal)
    {
        foreach(Node node in openList)
        {
            ColorTile(node.position, openColor);
        }

        foreach (Node node in closedList)
        {
            ColorTile(node.position, closeColor);
        }

        if (goal != start)
        {
            tilemap.SetTile(previousStart, null);
            tilemap.SetTile(previousGoal, null);

            ColorTile(start, startColor);
            ColorTile(goal, goalColor);

            previousStart = start;
            previousGoal = goal;

            foreach(KeyValuePair<Vector3Int, Node> node in allNodes)
            {
                if(node.Value.parent != null)
                {
                    GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                    go.transform.position = grid.CellToWorld(node.Key);
                    debugObjects.Add(go);
                    GenerateDebugText(node.Value, go.GetComponent<DebugText>());
                }
            }
        }
    }

    private void GenerateDebugText(Node node, DebugText debugText)
    {
        if (node.parent.position.x < node.position.x && node.parent.position.y == node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (node.parent.position.x < node.position.x && node.parent.position.y > node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
        else if (node.parent.position.x < node.position.x && node.parent.position.y < node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 225));
        }
        else if (node.parent.position.x > node.position.x && node.parent.position.y == node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (node.parent.position.x > node.position.x && node.parent.position.y > node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (node.parent.position.x > node.position.x && node.parent.position.y < node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else if (node.parent.position.x == node.position.x && node.parent.position.y > node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (node.parent.position.x == node.position.x && node.parent.position.y < node.position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
