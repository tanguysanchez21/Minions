using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorControler : MonoBehaviour
{
    public Camera camera;
    public Grid grid;

    private Vector3 mouseWorldPos;
    private Vector3Int mousePos;
    private Vector3 cellCoords;

    // Start is called before the first frame update
    void Start()
    {
        //...
    }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos = grid.WorldToCell(mouseWorldPos);
        cellCoords = grid.GetCellCenterWorld(mousePos);

        transform.position = cellCoords;
    }
}
