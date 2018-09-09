using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Localize : MonoBehaviour
{
    [SerializeField]
    string name;

    void Start()
    {
        GetComponent<Text>().text = Lang.Get(name);
    }
}
