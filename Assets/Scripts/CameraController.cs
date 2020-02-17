using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject cursor;

    private Camera self;
    private float targetZoom;
    private float scrollData;
    private float zoomFactor = 6;
    [SerializeField] private float zoomLerpSpeed = 10;

    private Vector3 position;
    private Vector3 mouseDelta;
    private Vector3 lastMouseCoordinate;
    private float xmag;
    private float ymag;
    public float moveSpeed = 25f;
    public bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        self = Camera.main;
        targetZoom = self.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Move();
    }

    void Move()
    {
        if (Input.GetMouseButton(2))
        {
            isMoving = true;
            Cursor.visible = false;
            cursor.GetComponent<Renderer>().enabled = false;

            mouseDelta = Input.mousePosition - lastMouseCoordinate;

            xmag = Mathf.Abs(mouseDelta.x);
            ymag = Mathf.Abs(mouseDelta.y);

            if (xmag > ymag)
            {
                if (mouseDelta.x > 0) MoveLeft();
                else MoveRight();
            }
            else if (ymag > xmag)
            {
                if (mouseDelta.y > 0) MoveDown();
                else MoveUp();
            }

            lastMouseCoordinate = Input.mousePosition;
        }
        else
        {
            isMoving = false;
            Cursor.visible = true;
            cursor.GetComponent<Renderer>().enabled = true;
        }
    }
    void Zoom()
    {
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 4.5f, 16f);
        self.orthographicSize = Mathf.Lerp(self.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }

    private void MoveRight()
    {
        position = self.transform.position;
        position.x += Time.deltaTime * moveSpeed;
        self.transform.position = position;
    }

    private void MoveLeft()
    {
        position = self.transform.position;
        position.x -= Time.deltaTime * moveSpeed;
        self.transform.position = position;
    }

    private void MoveDown()
    {
        position = self.transform.position;
        position.y -= Time.deltaTime * moveSpeed;
        self.transform.position = position;
    }

    private void MoveUp()
    {
        position = self.transform.position;
        position.y += Time.deltaTime * moveSpeed;
        self.transform.position = position;
    }
}
