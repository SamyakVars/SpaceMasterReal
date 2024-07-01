using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketLineMovement : MonoBehaviour
{

    private DrawWithMouse drawWithMouse;
    private BoxCollider2D boxCollider;
    private LineRenderer line;

    public bool lineDrawn = false;

    [SerializeField] float speed = 5f;

    int pointsIndex = 0;

    void Start()
    {
        drawWithMouse = GetComponentInChildren<DrawWithMouse>();
        drawWithMouse.enableDrawing(false);

        boxCollider = GetComponent<BoxCollider2D>();

        line = drawWithMouse.gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (boxCollider.OverlapPoint(mousePosition))
            {
                drawWithMouse.resetLine();
                drawWithMouse.enableDrawing(true);

                lineDrawn = true;
            }
            else
            {
                drawWithMouse.enableDrawing(false);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            drawWithMouse.enableDrawing(false);
        }


        if (lineDrawn && !drawWithMouse.canDraw)
        {
            if (pointsIndex <= line.positionCount)
            {
                transform.position = Vector2.MoveTowards(transform.position, line.GetPosition(pointsIndex), speed * Time.deltaTime);

                if (transform.position == line.GetPosition(pointsIndex))
                {
                    var pointsArray = new Vector3[line.positionCount];
                    line.GetPositions(pointsArray);
                    var modifiedPointsArray = pointsArray.ToList<Vector3>();
                    modifiedPointsArray.RemoveAt(pointsIndex);
                    line.SetPositions(modifiedPointsArray.ToArray());
                }
            }
        }
    }
}
