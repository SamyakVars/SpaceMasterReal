using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RocketLineMovement : MonoBehaviour
{

    [SerializeField] private DrawWithMouse drawWithMouse;
    public BoxCollider2D boxCollider;
    [SerializeField] private LineRenderer line;

    public bool lineDrawn = false;

    [SerializeField] float speed = 5f;

    int pointsIndex = 0;

    void Start()
    {
        drawWithMouse.enableDrawing(false);
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

        // Rotation, maar maakt t helemaal buggy. Help ik weet niet meer wat er gebeurd.
        Vector2 targetRotate = line.GetPosition(0);
        Vector2 position2D = new Vector2(transform.position.x, transform.position.y);

        Vector2 rotateDirection = targetRotate - position2D;

        float rotateAngle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(0, 0, rotateAngle);


    }
}
