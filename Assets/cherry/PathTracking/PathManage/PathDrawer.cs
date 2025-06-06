using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float pointSpacing = 0.2f;

    private List<Vector3> pathPoints = new();
    private Camera cam;
    private bool isDrawing = false;

    void Start()
    {
        cam = Camera.main;
        lineRenderer.positionCount = 0;
    }

    public void StartDrawing()
    {
        pathPoints.Clear();
        lineRenderer.positionCount = 0;
        isDrawing = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (!isDrawing) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 8.74f));
            if (pathPoints.Count == 0 || Vector3.Distance(pathPoints[^1], mouseWorld) > pointSpacing)
            {
                pathPoints.Add(mouseWorld);
                lineRenderer.positionCount = pathPoints.Count;
                lineRenderer.SetPosition(pathPoints.Count - 1, mouseWorld);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }

    public List<Vector3> GetPath() => pathPoints;
}