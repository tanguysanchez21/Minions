using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { UNSELECTED, SELECTED, WATER, GRASS, DIRT, ASTARDEBUGTILE }

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

    private bool hasJustSpawned = true;

    // Update is called once per frame
    void Update()
    {
        mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        clickPos = tilemap.WorldToCell(mouseWorldPos);
        playerPos = PlayerCellPos();

        RaycastHit2D hitSelect = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, selectionLayer);

        if (hitSelect.collider != null)
        {
            Vector3Int clickPosSelect = selectionTilemap.WorldToCell(mouseWorldPos);

            ChangeSelectedTile(clickPosSelect);
        }

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
        AstarDebugger.MyInstance.CreateTiles(playerPos, clickPos);
    }

    private void ChangeTileGround(Vector3Int clickPos)
    {
        tilemap.SetTile(clickPos, tiles[(int)tileType]);
    }

    private void ChangeSelectedTile(Vector3Int clickPos)
    {
        if(camera.GetComponent<CameraController>().isMoving)
        {
            selectionTilemap.SetTile(previousTile, tiles[0]);
            selectionTilemap.SetTile(clickPos, tiles[0]);
        }
        else
        {
            selectionTilemap.SetTile(previousTile, tiles[0]);
            selectionTilemap.SetTile(clickPos, tiles[1]);
            previousTile = clickPos;
        }
    }

    private Vector3Int PlayerCellPos()
    {
        Vector3Int pos = tilemap.WorldToCell(player.transform.position);
        pos.y -= 1;
        return pos;
    }
}