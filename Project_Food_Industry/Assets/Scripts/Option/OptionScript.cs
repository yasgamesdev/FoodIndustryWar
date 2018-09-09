using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class OptionScript : MonoBehaviour
{
    [SerializeField]
    Button back_button;
    [SerializeField]
    Dropdown drop_down;

    void Start()
    {
        drop_down.options.Clear();

        foreach(string language in Lang.GetLanguages())
        {
            drop_down.options.Add(new Dropdown.OptionData(language));
        }

        for(int i=0; i<drop_down.options.Count; i++)
        {
            if(drop_down.options[i].text == Lang.GetSelectLanguage())
            {
                drop_down.captionText.text = drop_down.options[i].text;
                drop_down.value = i;
                break;
            }
        }

        drop_down.onValueChanged.AsObservable().Subscribe(x => {
            string select = drop_down.options[drop_down.value].text;
            Lang.SetLanguage(select);
        });

        back_button.onClick.AsObservable().Subscribe(x => {
            SceneManager.LoadScene("Title");
        });
    }
}
