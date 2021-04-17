using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraMover : MonoBehaviour
{
    public float Speed = 0.25f;
    public Vector2 nowPos, prePos;
    public Vector3 movePos;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Speed;
                camera.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPositon;
            }
            else if(touch.phase == TouchPhase.Ended)
            {

            }
        }
    }
}
