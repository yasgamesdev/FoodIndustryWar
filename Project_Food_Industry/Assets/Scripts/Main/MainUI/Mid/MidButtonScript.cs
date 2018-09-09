using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class MidButtonScript : UIScript
{
    [SerializeField]
    Text button_text;
    [SerializeField]
    Image button_image;

    MidScript parent;
    int type;

    public void Init(MidScript parent, string text, int type)
    {
        Start();

        this.parent = parent;
        this.type = type;

        button_text.text = text;

        GetComponent<Button>().onClick.AsObservable().Subscribe(x => { parent.ChildButtonClicked(type); });
    }
}
