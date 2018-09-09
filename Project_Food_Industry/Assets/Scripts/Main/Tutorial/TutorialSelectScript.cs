using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class TutorialSelectScript : UIScript
{
    [SerializeField]
    Button ok_button, skip_button;

    TutorialScript tutorial_script;

    public void Init(TutorialScript tutorial_script)
    {
        base.Start();

        this.tutorial_script = tutorial_script;

        ok_button.onClick.AsObservable().Subscribe(x =>
        {
            tutorial_script.SelectOk();
            Destroy(gameObject);
        });

        skip_button.onClick.AsObservable().Subscribe(x =>
        {
            tutorial_script.SelectSkip();
            Destroy(gameObject);
        });
    }
}
