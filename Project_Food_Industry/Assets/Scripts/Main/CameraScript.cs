using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : UIScript {

    const float min_size = 1.0f;
    const float max_size = 20.0f;
    [SerializeField]
    float zoom_up_down_speed;
    [SerializeField]
    bool keyboard_scroll;

    [SerializeField]
    float drag_speed;
    Vector3 drag_origin;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        ZoomUpDown();
        Scroll();
    }

    void ZoomUpDown()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Camera.main.orthographicSize -= zoom_up_down_speed * Input.GetAxis("Mouse ScrollWheel");
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, min_size, max_size);
    }

    void Scroll()
    {
        float move = 2.0f * Camera.main.orthographicSize * (Time.deltaTime <= 1.0f / 30 ? Time.deltaTime : 1.0f / 30);

        //if(keyboard_scroll)
        //{
        //    if (Input.GetKey(KeyCode.W))
        //    {
        //        transform.position += new Vector3(move, 0, move);
        //    }
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        transform.position += new Vector3(-move, 0, -move);
        //    }
        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        transform.position += new Vector3(move, 0, -move);
        //    }
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        transform.position += new Vector3(-move, 0, move);
        //    }
        //}

        if (Input.GetMouseButtonDown(2))
        {
            drag_origin = Input.mousePosition;
            return;
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - drag_origin);
            Vector3 next = new Vector3(-move, 0, move) * pos.x * drag_speed + new Vector3(-move, 0, -move) * pos.y * drag_speed;

            transform.Translate(next, Space.World);
            drag_origin = Input.mousePosition;
        }

    }
}
