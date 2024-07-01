using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public LineRenderer line { get; private set; }
    private Vector3 previousPosition;

    [SerializeField] private float minDistance = 0.1f;
    [SerializeField] private float width;
    public bool canDraw { get; private set; } = false;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
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

            line.enabled = true;


            if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
            {
                if (previousPosition == transform.position)
                {
                    line.SetPosition(0, currentPosition);
                }
                else
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, currentPosition);
                }
                previousPosition = currentPosition;
            }
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
        switch (returnValue)
        {
            case true: canDraw=true; break;
            case false: canDraw=false;  break;
        }
    }

    public void resetLine()
    {
        line.positionCount = 1;
        line.SetPosition(0, transform.position);
        previousPosition = transform.position;
        line.enabled = false;
    }

    bool IsLineOnePoint()
    {
        Vector3 position0 = line.GetPosition(0);
        Vector3 position1 = line.GetPosition(1);
        return position0 == position1;
    }
}