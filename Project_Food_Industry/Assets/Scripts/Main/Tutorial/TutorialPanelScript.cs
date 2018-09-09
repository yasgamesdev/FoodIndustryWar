using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class TutorialPanelScript : UIScript
{
    [SerializeField]
    Sprite[] sprite_array;
    [SerializeField]
    string[] string_array;
    [SerializeField]
    int select_code;
    [SerializeField]
    Button next_button;

    [SerializeField]
    Image panel_image;
    [SerializeField]
    Text panel_text;

    TutorialScript tutorial_script;

    int current = 0;

    public void Init(TutorialScript tutorial_script)
    {
        base.Start();

        this.tutorial_script = tutorial_script;

        SetUI();

        next_button.onClick.AsObservable().Subscribe(x =>
        {
            current++;

            if(current >= sprite_array.Length)
            {
                tutorial_script.SelectPanel(select_code);
                Destroy(gameObject);
            }
            else
            {
                SetUI();
            }
        });
    }

    void SetUI()
    {
        panel_image.sprite = sprite_array[current];
        panel_text.text = string_array[current];
    }
}
