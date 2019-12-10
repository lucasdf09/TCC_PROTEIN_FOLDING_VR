using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseDrag : MonoBehaviour
{
    /*
    private Vector3 mouseOffset;

    private float mouseZCoord;

    private void OnMouseDown()
    {
        mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mouseOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)    
        Vector3 mousePoint = Input.mousePosition;

        //z coordinate of game on screen
        mousePoint.z = mouseZCoord;

        //Convert it to world points
        return Camera.main.ScreenToViewportPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mouseOffset; 
    }
    
    //---------------------------------------------------------------------------------------------

    float distance = 0;

    private void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = objectPos;
    }

    */

    private void Start()
    {
        Vector3 objStartPosition = gameObject.transform.position;
    }

    void OnMouseDrag()
    {
        // camera should be tagged as mainCamera, otherwise you should reference it above.
        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
    }

}
