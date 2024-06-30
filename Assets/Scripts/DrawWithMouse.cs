using System.Collections;
using System.Collections.Generic;
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
    }

}