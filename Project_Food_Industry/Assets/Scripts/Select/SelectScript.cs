using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using Food_Industry;
using System.Linq;

public class SelectScript : MonoBehaviour
{
    [SerializeField]
    Dropdown difficulty_dropdown, color_dropdown, cpu_dropdown;
    [SerializeField]
    InputField seed_inputfield;
    [SerializeField]
    Button next_button, cancel_button;

    void Start()
    {
        color_dropdown.options.Clear();
        foreach(CompanyColor color in ColorGenerator.GetAllColor())
        {
            color_dropdown.options.Add(new Dropdown.OptionData($"<color={color.GetColorNameForTag()}>{color.color_name}</color>"));
        }
        color_dropdown.captionText.text = color_dropdown.options[0].text;

        cpu_dropdown.options.Clear();
        for(int i=0; i<ColorGenerator.GetAllColor().Count; i++)
        {
            cpu_dropdown.options.Add(new Dropdown.OptionData($"{i}"));
        }
        cpu_dropdown.value = MEnv.default_cpu_num;

        next_button.onClick.AsObservable().Subscribe(x =>
        {
            int seed;
            if (int.TryParse(seed_inputfield.text, out seed))
            {
                CompanyColor color = ColorGenerator.GetAllColor()[color_dropdown.value];
                FIS fis = new FIS(difficulty_dropdown.value, seed, color, cpu_dropdown.value);
                FISLoader.SetInstance(fis);
                SceneManager.LoadScene("Calculate");
            }
        });

        cancel_button.onClick.AsObservable().Subscribe(x =>
        {
            SceneManager.LoadScene("Title");
        });
    }
}
