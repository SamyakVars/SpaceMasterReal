using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 previousPosition;

    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float width;
    public bool canDraw { get; private set; } = false;

    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        line.positionCount = 1;
        previousPosition = transform.position;
        line.SetWidth(width, width);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && canDraw)
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;

            if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
            {
                if (previousPosition == transform.position)
                {
                    line.SetPosition(0, currentPosition);
                }
                else
                {
                    points.Add(currentPosition);
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, currentPosition);
                }
                previousPosition = currentPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0) && canDraw)
        {
            // When the user releases the mouse button, smooth the line
            SmoothLine();
        }

        if (line.positionCount >= 2)
        {
            if (IsLineOnePoint())
            {
                line.enabled = false;
            }
            else
            {
                line.enabled = true;
            }
        }
    }

    public void enableDrawing(bool returnValue)
    {
        canDraw = returnValue;
    }

    public void resetLine()
    {
        line.positionCount = 1;
        line.SetPosition(0, transform.position);
        previousPosition = transform.position;
        line.enabled = false;
        points.Clear();
    }

    bool IsLineOnePoint()
    {
        Vector3 position0 = line.GetPosition(0);
        Vector3 position1 = line.GetPosition(1);
        return position0 == position1;
    }

    void SmoothLine()
    {
        if (points.Count < 2) return;

        List<Vector3> smoothPoints = new List<Vector3>();

        smoothPoints.Add(points[0]);

        for (int i = 0; i < points.Count - 1; i++)
        {
            smoothPoints.Add(points[i]);

            Vector3 p0 = i == 0 ? points[i] : points[i - 1];
            Vector3 p1 = points[i];
            Vector3 p2 = points[i + 1];
            Vector3 p3 = i == points.Count - 2 ? points[i + 1] : points[i + 2];

            for (int j = 1; j < 4; j++)
            {
                float t = j / 4.0f;
                Vector3 smoothedPoint = 0.5f * (
                    2f * p1 +
                    (-p0 + p2) * t +
                    (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                    (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
                );
                smoothPoints.Add(smoothedPoint);
            }
        }

        smoothPoints.Add(points[points.Count - 1]);

        line.positionCount = smoothPoints.Count;
        line.SetPositions(smoothPoints.ToArray());
    }
}
