using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;

public class UIScript : MonoBehaviour
{
    protected FIS fis;

    protected virtual void Start()
    {
        fis = FISLoader.GetFIS();
    }
}
