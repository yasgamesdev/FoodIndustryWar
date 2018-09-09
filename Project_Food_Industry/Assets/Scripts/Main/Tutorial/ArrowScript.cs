﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    float timer = 0.0f;
    bool up = true;

    Vector3 init_pos;

    void Start()
    {
        init_pos = transform.position;
    }

    void Update()
    {
        timer += up ? Time.deltaTime * 4.0f : -Time.deltaTime * 4.0f;

        if(up && timer >= 1.0f)
        {
            timer = 1.0f;
            up = false;
        }
        else if(!up && timer <= 0.0f)
        {
            timer = 0.0f;
            up = true;
        }

        transform.position = init_pos + new Vector3(0, timer,  0);
    }
}
