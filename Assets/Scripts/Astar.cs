using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    LAVA,
    WDIRT,
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

    public string walkable = "dirt";

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask layerMask;

    private Vector3Int previousTile = new Vector3Int(-1,-1,-1);

    private Vector3 mouseWorldPos;
    private Vector3Int clickPos;
    private Vector3Int playerPos;

    private Node current;
    private Dictionary<Vector3Int, Node> allNodes;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Stack<Vector3Int> path;

    void Start() { AstarDebugger.MyInstance.DebugDisplay(); }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        playerPos = PlayerCellPos();
        clickPos = tilemap.WorldToCell(mouseWorldPos);

        if (Input.GetKeyDown(KeyCode.Escape))
            AstarDebugger.MyInstance.DebugDisplay();

        if (Input.GetMouseButtonDown(1))
            AstarAlgorythm();

    }

    private void AstarAlgorythm()
    {
        Reset();

        if (tilemap.GetTile(clickPos).name == walkable)
        {
            if (current == null) Initialize();

            while(openList.Count > 0 && path == null)
            {
                List<Node> neighbors = FindNeighbors(current.position);

                ExamineNeighbors(neighbors, current);

                UpdateCurrentTile(ref current);

                path = GeneratePath(current);
            }

            AstarDebugger.MyInstance.CreateTiles(openList, closedList, allNodes, playerPos, clickPos, path);
        }
    }

    private void ChangeTile(Vector3Int clickPos)
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
                    if (neighborPosition != playerPos && tilemap.GetTile(neighborPosition) && tilemap.GetTile(neighborPosition).name == walkable)
                        neighbors.Add(GetNode(neighborPosition));
                }
            }
        }

        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for(int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];

            if (!ConnectedDiagonally(current, neighbor))
                continue;

            int gScore = DetermineGScore(neighbors[i].position, current.position);

            if(openList.Contains(neighbor))
            {
                if(current.G + gScore < neighbor.G)
                {
                    CalcValues(current, neighbor, gScore);
                }
            }
            else if(!closedList.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore);

                openList.Add(neighbors[i]);
            }
        }
    }

    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.G = 0;
        neighbor.H = 0;
        neighbor.F = 0;

        neighbor.parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = ((Mathf.Abs(neighbor.position.x - clickPos.x) + Mathf.Abs(neighbor.position.y - clickPos.y))) * 10;
        neighbor.F = neighbor.G + neighbor.H;
    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Mathf.Abs(x - y) % 2 == 1) gScore = 10;
        else gScore = 14;

        return gScore;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if(openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private Stack<Vector3Int> GeneratePath(Node current)
    {
        if(current.position == clickPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while(current.position != playerPos)
            {
                finalPath.Push(current.position);

                current = current.parent;
            }

            return finalPath;
        }

        return null;
    }

    private bool ConnectedDiagonally(Node current, Node neighbor)
    {
        Vector3Int direct = current.position - neighbor.position;

        Vector3Int first = new Vector3Int(current.position.x + (direct.x * -1), current.position.y, current.position.z);
        Vector3Int second = new Vector3Int(current.position.x, current.position.y + (direct.x * -1), current.position.z);

        if (tilemap.GetTile(first).name != walkable || tilemap.GetTile(second).name != walkable)
            return false;

        return true;
    }

    public Stack<Vector3Int> GetPath() { return path; }

    public void Reset()
    {
        if(allNodes != null)
        {
            AstarDebugger.MyInstance.Reset();

            allNodes.Clear();
            path = null;
            current = null;
        }
    }
}