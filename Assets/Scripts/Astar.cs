using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    LAVA,
    DIRT,
    LAVA1,
    LAVA2,
    LAVA3,
    LAVA4,
    LAVA5,
    LAVA6,
    LAVA7,
    LAVA8,
    LAVA9,
    LAVA10,
    LAVA11,
    LAVA12,
    ASTARDEBUGTILE
}

public class Astar : MonoBehaviour
{
    private TileType tileType = 0;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Tilemap selectionTilemap;

    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private LayerMask selectionLayer;

    private Vector3Int previousTile = new Vector3Int(-1,-1,-1);

    private Vector3 mouseWorldPos;
    private Vector3Int clickPos;
    private Vector3Int playerPos;

    private Node current;
    private Dictionary<Vector3Int, Node> allNodes;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;

    // Update is called once per frame
    void Update()
    {
        mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        clickPos = tilemap.WorldToCell(mouseWorldPos);
        playerPos = PlayerCellPos();

        //if (Input.GetMouseButtonDown(0))
        //{
        /* RaycastHit2D hitGround = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask); */

        /* if (hitGround.collider != null)
        {
            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickPos = tilemap.WorldToCell(mouseWorldPos);

            //ChangeTileGround(clickPosGround);
        } */
        //}

        if (Input.GetMouseButtonDown(1))
        {
            AstarAlgorythm();
        }
    }

    private void AstarAlgorythm()
    {
        if (current == null) Initialize();

        List<Node> neighbors = FindNeighbors(current.position);

        ExamineNeighbors(neighbors, current);

        UpdateCurrentTile(ref current);

        AstarDebugger.MyInstance.CreateTiles(openList, closedList, allNodes, playerPos, clickPos);
    }

    private void ChangeTileGround(Vector3Int clickPos)
    {
        tilemap.SetTile(clickPos, tiles[(int)tileType]);
    }

    private Vector3Int PlayerCellPos()
    {
        Vector3Int pos = tilemap.WorldToCell(player.transform.position);
        pos.y -= 1;
        return pos;
    }

    private void Initialize()
    {
        if (allNodes == null) allNodes = new Dictionary<Vector3Int, Node>();

        current = GetNode(playerPos);
        openList = new HashSet<Node>();
        openList.Add(current);

        closedList = new HashSet<Node>();
    }

    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position)) return allNodes[position];
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return allNodes[position];
        }
    }

    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPosition = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                if (y != 0 || x != 0)
                {
                    if(neighborPosition != playerPos && tilemap.GetTile(neighborPosition))
                    {
                        Node neighbor = GetNode(neighborPosition);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for(int i = 0; i < neighbors.Count; i++)
        {
            openList.Add(neighbors[i]);

            CalcValues(current, neighbors[i], 0);
        }
    }

    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.parent = parent;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);
    }
}